"""Keep manually authorized package publication and Demo builds on central versions."""

import argparse
import os
from pathlib import Path
import re
import time
from urllib.error import HTTPError, URLError
from urllib.request import urlopen
import xml.etree.ElementTree as ET

COMPONENTS = ("Core", "Carousel", "Bento", "Gallery", "Marquee")


def read_versions(path):
    """Read one literal, URL-safe NuGet version per component from central MSBuild metadata."""
    root = ET.parse(path).getroot()
    versions = {}
    for component in COMPONENTS:
        values = root.findall(f".//Blazzy{component}Version")
        if len(values) != 1 or not re.fullmatch(r"[0-9]+\.[0-9]+\.[0-9]+(?:-[0-9A-Za-z.-]+)?", values[0].text or ""):
            raise ValueError(f"Expected one literal package version for {component}")
        versions[component] = values[0].text
    return versions


def selected_components(environment):
    """Require an explicit Boolean selection and at least one package to publish."""
    selected = []
    for component in COMPONENTS:
        flag = environment.get(f"PUBLISH_{component.upper()}", "false")
        if flag not in ("true", "false"):
            raise ValueError(f"Invalid publication selection for {component}")
        if flag == "true":
            selected.append(component)
    if not selected:
        raise ValueError("Select at least one package to publish")
    return selected


def resolve_versions(versions, environment):
    """Use central versions and reject conflicting manual overrides before build or publication."""
    selected_components(environment)
    for component, version in versions.items():
        override = environment.get(f"{component.upper()}_VERSION", "").strip()
        if override and override != version:
            raise ValueError(f"{component}: requested {override}, but Directory.Build.props defines {version}; update central metadata first")
    return {f"{component.upper()}_VERSION": version for component, version in versions.items()}


def confirm_visible(versions, selected, attempts=30, delay=10, opener=urlopen, sleep=time.sleep):
    """Wait for selected versions on official NuGet endpoints; fail closed on invalid manifests or timeout."""
    for component in selected:
        package = f"BlazzyMotion.{component}".lower()
        version = versions[component].lower()
        url = f"https://api.nuget.org/v3-flatcontainer/{package}/{version}/{package}.nuspec"
        for attempt in range(attempts):
            try:
                with opener(url, timeout=20) as response:
                    metadata = ET.fromstring(response.read(1024 * 1024)).find("{*}metadata")
                if metadata is None or metadata.findtext("{*}id", "").lower() != package or metadata.findtext("{*}version", "").lower() != version:
                    raise ValueError(f"Unexpected NuGet manifest for {package} {version}")
                print(f"Available on NuGet: {package} {version}")
                break
            except (HTTPError, URLError) as error:
                if isinstance(error, HTTPError) and error.code not in (404, 408, 429) and error.code < 500:
                    raise
                if attempt + 1 == attempts:
                    raise RuntimeError(f"NuGet availability not confirmed: {package} {version}") from error
                sleep(delay)


def main():
    """Run workflow checks and write only validated version values or a refresh decision to GitHub files."""
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("command", choices=("resolve", "visible", "refresh"))
    parser.add_argument("--props", default="Directory.Build.props")
    parser.add_argument("--published-props")
    args = parser.parse_args()
    versions = read_versions(args.props)
    if args.command == "resolve":
        values = resolve_versions(versions, os.environ)
        with Path(os.environ["GITHUB_ENV"]).open("a") as output:
            output.writelines(f"{key}={value}\n" for key, value in values.items())
    elif args.command == "visible":
        confirm_visible(versions, selected_components(os.environ))
    else:
        matches = versions == read_versions(args.published_props)
        with Path(os.environ["GITHUB_OUTPUT"]).open("a") as output:
            output.write(f"refresh={str(matches).lower()}\n")
        if not matches:
            print("::warning::Main package versions have changed since publication; skip the outdated refresh. The next main deployment uses current metadata.")


if __name__ == "__main__":
    main()
