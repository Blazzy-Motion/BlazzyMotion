# Demo design refresh

Branch: `feat/demo-design-refresh`. Base: `050fb6d`.

## Scope

Refresh the entire Demo presentation while preserving existing component libraries, public routes, documentation, API reference, examples and startup splash animation. Use a neutral editorial design with restrained brand accents. Do not publish packages or merge to main during visual review.

## Progress

- [x] Inspect existing layouts, navigation, component demos and documentation.
- [x] Add shared navigation and component discovery across all pages.
- [x] Refine typography, spacing, documentation surfaces and demo controls.
- [x] Preserve and verify startup animation and reduced motion behavior.
- [x] Run Release build and desktop/mobile navigation and component regressions.
- [x] Publish branch and Azure PR preview for owner review.

## Follow-up direction

Keep each component's interactive examples together on its existing page. Link documentation and API clearly. The Gallery already has a playground; extend controls only when they map to supported public parameters. Keep site presentation separate from component themes. A complete independent light/dark site theme needs a later audit of hard-coded documentation colors; do not claim it exists before that work is complete.

## Resume

Read this file and `/workspace/AGENTS.md`, inspect `git status`, and continue on the existing branch. Keep progress and validation results current. All new code comments, documentation and commits must be in English.

## Validation checkpoint

Release solution build passed (one existing CS1998 warning). All 1,110 C# tests passed. Desktop/mobile visual review performed locally. The full existing component browser suite passed, including nested gestures, route reentry, four themes, image lifecycle and missing-Swiper fallback. New site checks passed all 16 routes at 1440px and 390px, including navigation, component discovery and selected theme state. Final Demo rebuild passed. No library source, package versions or public component behavior changed.

## Preview

PR #9: https://github.com/Blazzy-Motion/BlazzyMotion/pull/9. Use the PR checks for the current Azure deployment and Quality Gate status. Do not merge without owner visual approval.

## Remaining scope

This pass refreshes presentation across all existing pages, rather than replacing documentation content. Independent site light mode, richer per-component playground controls, a documentation table of contents remain follow-up work. The responsive native-modal documentation drawer has now been implemented and is being validated. Existing component theme controls and Gallery playground are preserved. No claim of complete site accessibility conformance or measured device FPS is made.

## Verified deployment checkpoint — 2026-10-18

Implementation commit `b776779` passed Azure deployment, the complete Quality workflow and SonarCloud Quality Gate.

- Azure run: https://github.com/Blazzy-Motion/BlazzyMotion/actions/runs/39079743534
- Quality run: https://github.com/Blazzy-Motion/BlazzyMotion/actions/runs/39079743535
- Preview URL reported by the successful Azure deployment: https://nice-sea-02b1e9803-9.westeurope.1.azurestaticapps.net

Direct requests to the preview domain from this cloud instance are currently denied by its network policy (CONNECT 403). The current allowlist includes preview domains #6 and #7, but not #9. Production domain access succeeds. The preview address is verified from Azure's deployment annotation; live browser validation of this deployment remains pending network access. Local browser and route checks passed. Owner visual review remains required before merge. This checkpoint only changes this progress document.

## Follow-up implementation — 2026-10-18

- Replace the cramped documentation rail on widths up to 1024px with native modal navigation; preserve a 248px neutral rail on larger screens.
- Support Escape, browser-managed focus restoration, inert background controls and scroll locking while the menu is open.
- Add component-specific Demo / Documentation / API links and real API anchors on all four documentation pages.
- Reduce Carousel section spacing and correct anchor offsets without changing component geometry.
- Add an independent Page / Dark / Light / Tinted host background selector to the Carousel demo; preserve the running Swiper instance when changing it.
- Release solution build and 1,110 tests passed. Existing component browser regressions and expanded navigation tests are running. Native dialogs may permit focus to browser chrome; tests must reject background page focus rather than reject normal browser behavior.

No production merge or NuGet publication is authorized for this visual iteration.
