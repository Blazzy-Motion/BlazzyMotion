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
- [ ] Publish branch and Azure PR preview for owner review.

## Follow-up direction

Keep each component's interactive examples together on its existing page. Link documentation and API clearly. The Gallery already has a playground; extend controls only when they map to supported public parameters. Keep site presentation separate from component themes. A complete independent light/dark site theme needs a later audit of hard-coded documentation colors; do not claim it exists before that work is complete.

## Resume

Read this file and `/workspace/AGENTS.md`, inspect `git status`, and continue on the existing branch. Keep progress and validation results current. All new code comments, documentation and commits must be in English.

## Validation checkpoint

Release solution build passed (one existing CS1998 warning). All 1,110 C# tests passed. Desktop/mobile visual review performed locally. The full existing component browser suite passed, including nested gestures, route reentry, four themes, image lifecycle and missing-Swiper fallback. New site checks passed all 16 routes at 1440px and 390px, including navigation, component discovery and selected theme state. Final Demo rebuild passed. No library source, package versions or public component behavior changed.

## Preview

PR #9: https://github.com/Blazzy-Motion/BlazzyMotion/pull/9. Azure deployment and Quality Gate are running. Do not merge without owner visual approval.

## Remaining scope

This pass refreshes presentation across all existing pages, rather than replacing documentation content. Independent site light mode, richer per-component playground controls, a documentation table of contents and a complete focus-trapped mobile documentation drawer remain follow-up work. Existing component theme controls and Gallery playground are preserved. No claim of complete site accessibility conformance or measured device FPS is made.
