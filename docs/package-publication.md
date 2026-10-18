# Package publication and Demo version refresh

## One version source

`Directory.Build.props` defines Core, Carousel, Bento, Gallery and Marquee versions. Library builds and Demo assembly metadata use these same properties. Set approved release versions there once, review them on a feature branch, and merge the approved change before publishing from `main`.

In **Actions → Publish NuGet → Run workflow**, select the packages to publish. Leave version fields blank to use central metadata. An explicit value is accepted only when it matches the source metadata; a conflicting value fails before build, packing or publication. Selecting no packages also fails. Version selection remains a release decision; automation does not increment versions or convert previews to stable.

## Successful publication

Publication remains a manual `workflow_dispatch` action. It builds and tests the solution, packs selected packages, verifies their manifests and dependencies, and pushes Core before its dependents. No new workflow trigger publishes packages automatically.

After the pushes, the workflow waits for each selected version's manifest on the official NuGet flat-container endpoint. Temporary indexing delays and transient network errors are retried. Missing or incorrect manifests prevent the success-dependent Demo refresh. A failure at this stage can occur after packages have already been uploaded: inspect NuGet before retrying; publication is not transactional and the existing push command skips duplicate versions.

The existing stable-tag behavior uses the resolved central versions, so blank version inputs cannot accidentally create stable tags for previews. GitHub Release notes remain a separate stable-release step.

## Production Demo refresh

The `Refresh Production Demo` job runs only after successful manual publication from `main`. Feature-branch publications, failed jobs and cancelled runs do not deploy production.

The job checks out current `main`, verifies the publication commit is its ancestor and compares all five central versions with those used for the publication. If versions have changed meanwhile, it skips the outdated refresh and emits a warning. It never checks out an older release over newer production source. The next approved publication can refresh the new metadata.

If metadata still matches, Azure rebuilds and deploys the entire Demo site using the existing deployment secret. Production deployments from pushes and publication share a concurrency group. No credentials, package artifacts or remotely supplied build scripts are downloaded from another workflow.

Documentation, API and component-directory badges describe the component versions built into that Demo. They are not live queries for the newest NuGet package: an independently published version outside this workflow will not update them. Unversioned installation commands continue to resolve the latest stable package on NuGet, even if the Demo uses a preview.

## Validation

`python -m unittest discover -s tests/automation -v` exercises central metadata resolution, dispatch overrides, partial selections, real job conditions, stable-tag decisions, NuGet availability retries/failures and outdated-main detection. The Quality Gate installs pinned tooling from `scripts/requirements-ci.txt`, runs these checks with branch coverage and supplies the coverage report to SonarCloud. Run `actionlint .github/workflows/*.yml` for workflow validation.

Do not publish a package merely to test this automation. End-to-end confirmation of the post-publication production job belongs to the next explicitly authorized release. A successful pull-request Azure preview verifies the normal preview path but does not execute the publication-dependent job.
