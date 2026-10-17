# Demo design refresh

Branch: `feat/demo-design-refresh`. Base: `050fb6d`.

## Scope

Refresh the entire Demo presentation while preserving existing component libraries, public routes, documentation, API reference, examples and startup splash animation. Use a neutral editorial design with restrained brand accents. Do not publish packages or merge to main during visual review.

## Progress

- [x] Inspect existing layouts, navigation, component demos and documentation.
- [x] Add shared navigation and component discovery across all pages.
- [x] Refine typography, spacing, documentation surfaces and demo controls.
- [ ] Preserve and verify startup animation and reduced motion behavior.
- [ ] Run Release build and desktop/mobile navigation and component regressions.
- [ ] Publish branch and Azure PR preview for owner review.

## Follow-up direction

Keep each component's interactive examples together on its existing page. Link documentation and API clearly. The Gallery already has a playground; extend controls only when they map to supported public parameters. Keep site presentation separate from component themes. A complete independent light/dark site theme needs a later audit of hard-coded documentation colors; do not claim it exists before that work is complete.

## Resume

Read this file and `/workspace/AGENTS.md`, inspect `git status`, and continue on the existing branch. Keep progress and validation results current. All new code comments, documentation and commits must be in English.

## Validation checkpoint

Release solution build passed (one existing CS1998 warning). All 1,110 C# tests passed. Desktop/mobile visual review performed locally. Full component browser regression suite and the new 16-route desktop/mobile site checks are in progress. No library source, package versions or public component behavior changed.
