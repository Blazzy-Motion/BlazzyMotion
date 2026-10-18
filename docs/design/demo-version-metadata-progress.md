# Demo version metadata

Branch: `feat/demo-version-metadata`. Base: `a366480`.

## Implemented

- Generate five component version attributes from the same central MSBuild properties used by their projects.
- Read those attributes in Demo AppVersions instead of maintaining duplicate version strings.
- Mark prerelease builds as preview in the component directory.
- Display and copy an installation command pinned to the demonstrated version; preserve generic commands for unrelated packages.
- Add tests against actual library assembly versions, prerelease/build metadata parsing, clipboard consistency and browser rendering.

## Validation checkpoint

Release solution build passed. All 1,117 tests passed. A synthetic `BlazzyCarouselVersion=1.5.0-preview.999` build passed all seven version tests; no package was published. A subsequent default Release publish restored central versions and completed successfully. Published-asset browser checks are in progress.

## Scope and remaining work

Library source, central version values and GitHub workflows remain unchanged. This implementation removes manual Demo version duplication at build time. It does not claim to discover the newest published NuGet package.

Automatically rebuilding production after a successful main-branch package publication and validating publication overrides against central source metadata are separate workflow changes awaiting explicit authorization. Feature-branch publications must not trigger production deployment. Keep publication manual and do not publish packages or merge this branch without the owner's concrete authorization.
