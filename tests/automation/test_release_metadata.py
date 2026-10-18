"""Exercise publication metadata, NuGet visibility and production deployment boundaries."""

from contextlib import contextmanager
import importlib.util
import io
import os
from pathlib import Path
import subprocess
import sys
import json
import yaml
import tempfile
import unittest
from unittest.mock import patch
from urllib.error import HTTPError, URLError

ROOT = Path(__file__).resolve().parents[2]
SPEC = importlib.util.spec_from_file_location("release_metadata", ROOT / "scripts/release_metadata.py")
release = importlib.util.module_from_spec(SPEC)
SPEC.loader.exec_module(release)


class ReleaseMetadataTests(unittest.TestCase):
    """Protect the source of truth and prevent accidental publication or production refresh."""

    def setUp(self):
        """Load the current central metadata without embedding package numbers in tests."""
        self.versions = release.read_versions(ROOT / "Directory.Build.props")
        self.selection = {"PUBLISH_CAROUSEL": "true"}

    def test_blank_inputs_use_all_central_versions(self):
        """Default dispatch values resolve without manual version entry."""
        resolved = release.resolve_versions(self.versions, self.selection)
        self.assertEqual(len(resolved), 5)
        self.assertEqual(resolved["CAROUSEL_VERSION"], self.versions["Carousel"])

    def test_matching_override_is_accepted(self):
        """Retain manual forms when their versions agree with source metadata."""
        environment = self.selection | {"CAROUSEL_VERSION": self.versions["Carousel"]}
        self.assertEqual(release.resolve_versions(self.versions, environment)["CAROUSEL_VERSION"], self.versions["Carousel"])

    def test_conflicting_override_fails_before_publication(self):
        """Reject version drift even if the conflicting component is not selected."""
        with self.assertRaisesRegex(ValueError, "update central metadata"):
            release.resolve_versions(self.versions, self.selection | {"CORE_VERSION": "99.0.0"})

    def test_empty_and_invalid_selections_fail(self):
        """No-op or malformed publication requests cannot produce a successful refresh."""
        for environment in ({}, {"PUBLISH_CORE": "yes"}):
            with self.subTest(environment=environment), self.assertRaises(ValueError):
                release.selected_components(environment)

    def test_partial_selection_does_not_include_other_packages(self):
        """Visibility checks concern only the packages actually selected for publication."""
        self.assertEqual(release.selected_components(self.selection), ["Carousel"])

    def test_missing_duplicate_and_unsafe_versions_fail(self):
        """Reject ambiguous MSBuild values and unsafe endpoint or environment-file content."""
        original = (ROOT / "Directory.Build.props").read_text()
        for replacement in ("", "$(Unresolved)", "1.2.3\nEVIL=value", "1.2.3/../../other"):
            with self.subTest(replacement=replacement), tempfile.TemporaryDirectory() as directory:
                props = Path(directory) / "props.xml"
                props.write_text(original.replace(self.versions["Core"], replacement))
                with self.assertRaises(ValueError):
                    release.read_versions(props)
        with tempfile.TemporaryDirectory() as directory:
            props = Path(directory) / "props.xml"
            props.write_text(original.replace("<PropertyGroup>", "<PropertyGroup><BlazzyCoreVersion>1.0.0</BlazzyCoreVersion>", 1))
            with self.assertRaises(ValueError):
                release.read_versions(props)

    @contextmanager
    def manifest(self, package="BlazzyMotion.Carousel", version=None):
        """Provide a namespaced NuGet manifest without making network calls."""
        version = version or self.versions["Carousel"]
        with io.BytesIO(f'<package xmlns="urn:test"><metadata><id>{package}</id><version>{version}</version></metadata></package>'.encode()) as response:
            yield response

    def test_visible_manifest_uses_official_endpoint_and_tls_defaults(self):
        """Verify exact package identity, version, endpoint and a finite request timeout."""
        with patch.object(release, "urlopen", return_value=self.manifest()) as opener:
            release.confirm_visible(self.versions, ["Carousel"], opener=opener)
        url = opener.call_args.args[0]
        self.assertEqual(url, f'https://api.nuget.org/v3-flatcontainer/blazzymotion.carousel/{self.versions["Carousel"].lower()}/blazzymotion.carousel.nuspec')
        self.assertEqual(opener.call_args.kwargs, {"timeout": 20})

    def test_transient_failures_are_retried(self):
        """Handle indexing delays, rate limiting and network failures before declaring success."""
        for error in (HTTPError("url", 404, "pending", {}, None), HTTPError("url", 429, "limited", {}, None), HTTPError("url", 503, "unavailable", {}, None), URLError("temporary")):
            with self.subTest(error=error), patch.object(release, "urlopen", side_effect=[error, self.manifest()]) as opener, patch.object(release.time, "sleep") as sleep:
                release.confirm_visible(self.versions, ["Carousel"], attempts=2, delay=1, opener=opener, sleep=sleep)
                sleep.assert_called_once_with(1)

    def test_disabled_availability_checks_are_rejected(self):
        """A zero-attempt check must never report successful visibility."""
        with self.assertRaises(ValueError):
            release.confirm_visible(self.versions, ["Carousel"], attempts=0)

    def test_timeout_and_permanent_errors_fail_closed(self):
        """Do not refresh production when publication visibility cannot be established."""
        with patch.object(release, "urlopen", side_effect=URLError("offline")) as opener:
            with self.assertRaises(RuntimeError):
                release.confirm_visible(self.versions, ["Carousel"], attempts=1, opener=opener)
        with patch.object(release, "urlopen", side_effect=HTTPError("url", 403, "denied", {}, None)) as opener:
            with self.assertRaises(HTTPError):
                release.confirm_visible(self.versions, ["Carousel"], opener=opener)

    def test_wrong_manifest_never_counts_as_published(self):
        """Reject a response for a different version or package, even when HTTP succeeds."""
        for response in (self.manifest(package="Other.Package"), self.manifest(version="99.0.0"), io.BytesIO(b"<package/>")):
            with self.subTest(response=response), patch.object(release, "urlopen", return_value=response) as opener:
                with self.assertRaises(ValueError):
                    release.confirm_visible(self.versions, ["Carousel"], opener=opener)

    def test_resolve_cli_writes_validated_github_environment(self):
        """Exercise the actual dispatch command rather than a duplicate implementation."""
        with tempfile.TemporaryDirectory() as directory:
            output = Path(directory) / "env"
            with patch.dict(os.environ, self.selection | {"GITHUB_ENV": str(output)}, clear=True), patch.object(sys, "argv", ["release_metadata.py", "resolve", "--props", str(ROOT / "Directory.Build.props")]):
                release.main()
            self.assertIn(f'CAROUSEL_VERSION={self.versions["Carousel"]}\n', output.read_text())

    def test_refresh_cli_skips_newer_main_version_metadata(self):
        """Deploy current main only while its package metadata still describes the successful publication."""
        with tempfile.TemporaryDirectory() as directory:
            published = Path(directory) / "published.props"
            published.write_text((ROOT / "Directory.Build.props").read_text())
            for matches in (True, False):
                with self.subTest(matches=matches):
                    if not matches:
                        published.write_text(published.read_text().replace(self.versions["Core"], "99.0.0"))
                    output = Path(directory) / str(matches)
                    with patch.dict(os.environ, {"GITHUB_OUTPUT": str(output)}), patch.object(sys, "argv", ["release_metadata.py", "refresh", "--props", str(ROOT / "Directory.Build.props"), "--published-props", str(published)]):
                        release.main()
                    self.assertEqual(output.read_text(), f"refresh={str(matches).lower()}\n")

    def test_visibility_cli_uses_selected_versions(self):
        """The workflow command delegates to the checked source metadata and selected packages."""
        with patch.dict(os.environ, self.selection, clear=True), patch.object(release, "confirm_visible") as confirm, patch.object(sys, "argv", ["release_metadata.py", "visible", "--props", str(ROOT / "Directory.Build.props")]):
            release.main()
        confirm.assert_called_once_with(self.versions, ["Carousel"])


class PublicationWorkflowTests(unittest.TestCase):
    """Evaluate actual job conditions with GitHub-compatible Boolean expressions."""

    def test_only_successful_manual_main_publication_refreshes_production(self):
        """Failed, skipped, PR and feature-branch runs cannot deploy production."""
        workflow = yaml.safe_load((ROOT / ".github/workflows/publish.yml").read_text())
        job = workflow["jobs"]["refresh_demo"]
        expression = job["if"].removeprefix("${{").removesuffix("}}").strip()
        for event, ref, result, expected in (
            ("workflow_dispatch", "refs/heads/main", "success", True),
            ("workflow_dispatch", "refs/heads/feature/test", "success", False),
            ("workflow_dispatch", "refs/tags/v1", "success", False),
            ("pull_request", "refs/heads/main", "success", False),
            ("workflow_dispatch", "refs/heads/main", "failure", False),
            ("workflow_dispatch", "refs/heads/main", "cancelled", False),
            ("workflow_dispatch", "refs/heads/main", "skipped", False),
        ):
            with self.subTest(event=event, ref=ref, result=result):
                context = {"github": {"event_name": event, "ref": ref}, "needs": {"publish": {"result": result}}}
                script = "const vm=require('node:vm');console.log(vm.runInNewContext(process.argv[1],JSON.parse(process.argv[2])));"
                actual = subprocess.check_output(["node", "-e", script, expression, json.dumps(context)], text=True).strip()
                self.assertEqual(actual, str(expected).lower())
        self.assertEqual(job["needs"], "publish")
        self.assertEqual(job["permissions"], {"contents": "read"})
        self.assertEqual(job["steps"][0]["with"]["ref"], "main")
        self.assertFalse(job["steps"][0]["with"]["persist-credentials"])
        self.assertIn("metadata.outputs.refresh", job["steps"][-1]["if"])

    def test_stable_tag_selection_uses_resolved_versions(self):
        """Blank dispatch version fields cannot turn a preview into a stable tag."""
        workflow = yaml.safe_load((ROOT / ".github/workflows/publish.yml").read_text())
        tags = next(step for step in workflow["jobs"]["publish"]["steps"] if step["name"] == "Create Git Tags")
        expression = tags["if"].removeprefix("${{").removesuffix("}}").strip()
        for version, expected in (("1.2.3-preview.1", False), ("1.2.3", True)):
            inputs = {f"publish_{component.lower()}": component == "Carousel" for component in release.COMPONENTS}
            environment = {f"{component.upper()}_VERSION": version for component in release.COMPONENTS}
            script = "const vm=require('node:vm');const ctx=JSON.parse(process.argv[2]);ctx.contains=(value,part)=>value.includes(part);console.log(vm.runInNewContext(process.argv[1],ctx));"
            actual = subprocess.check_output(["node", "-e", script, expression, json.dumps({"inputs": inputs, "env": environment})], text=True).strip()
            self.assertEqual(actual, str(expected).lower())


if __name__ == "__main__":
    unittest.main()
