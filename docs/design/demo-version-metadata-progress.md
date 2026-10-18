# Demo version metadata

Branch: `feat/demo-version-metadata`. Base: `a366480`.

## Implemented

- Generate five component version attributes from the same central MSBuild properties used by their projects.
- Read those attributes in Demo AppVersions instead of maintaining duplicate version strings.
- Mark prerelease builds as preview in the component directory.
- Display and copy unversioned installation commands so NuGet resolves the latest stable release, independently of Demo build badges.
- Use one scroll surface for the mobile documentation drawer, including its footer, with touch coverage for Docs and API.
- Add tests against actual library assembly versions, prerelease/build metadata parsing, clipboard consistency and browser rendering.

## Validation checkpoint

Release solution build passed. All 1,117 tests passed. A synthetic `BlazzyCarouselVersion=1.5.0-preview.999` build passed all seven version tests; no package was published. A subsequent default Release publish restored central versions and completed successfully. Published-asset browser checks passed: all 16 routes at desktop/mobile widths with WCAG scans, keyboard navigation, component regressions, actual preview badges and installation commands. PR #10 provides the Azure preview and current remote check status.

## Scope and remaining work

Library source, central version values and GitHub workflows remain unchanged. This implementation removes manual Demo version duplication at build time. It does not claim to discover the newest published NuGet package.

Automatically rebuilding production after a successful main-branch package publication and validating publication overrides against central source metadata are separate workflow changes awaiting explicit authorization. Feature-branch publications must not trigger production deployment. Keep publication manual and do not publish packages or merge this branch without the owner's concrete authorization.

## Follow-up validation

Release build and all 1,117 tests passed after the follow-up changes. The default Release publish and full browser suite passed, including all 16 routes at desktop/mobile widths with WCAG scans, existing component regressions, stable installation commands, and real touch scrolling to the drawer footer on Quick Start and API at 390 × 650. This is Chromium emulation, not a physical-device confirmation. No library or package changes are included.

## Mobile document scroll correction

The legacy index.html selector `html:has(.bz-sidebar--open)` locked mobile document scrolling even when that sidebar was inside a closed native dialog. Remove that selector and retain the actual open-dialog body lock. Add touch document scrolling regressions on Quick Start and API before opening, after Escape, and after following a modal link. The regression fails against the preceding published build at the initial document scroll assertion. Release publish passed. All 16 routes passed desktop/mobile WCAG and navigation checks. The document-touch regression passes with the correction on both routes, including modal dismissal and link navigation; existing drawer scrolling and background-lock checks also pass. The automated test waits for the asynchronous dialog opening before sending Escape. Physical-device confirmation remains with the owner.
