# Catalog preview checkpoint

Branch: `feat/demo-component-previews`. Base: `1baca3c`.

## Implemented

- Add local screenshots of the real Carousel, composite Bento dashboard, Gallery grid and Marquee demonstrations to the shared component directory.
- Keep all four examples static in the catalog: do not instantiate extra animated libraries. Load images lazily, reserve the frame and intrinsic ratio, and keep adjacent component names/descriptions as the accessible content.
- Preserve central version labels and existing demo/documentation routes.
- Scope a compact introduction to Home/Carousel, preserving installation, primary actions and View Demo.
- Provide a repeatable browser capture script and screenshot maintenance instructions.

## Current evidence

Release build, all 1,117 .NET tests and default publish passed. Targeted browser checks passed at 320, 390 and 1440 px, including loaded previews, the 260 KiB budget, no extra component instances, no horizontal overflow and View Demo navigation. The live-demo section moves from approximately 802 to 604 CSS pixels at 1440 px, and from 745 to 550 at 390 px. The full browser suite and remote PR checks remain to be completed. Desktop/mobile screenshots were visually reviewed.

Library sources, package versions and publication workflows remain unchanged. No main merge or NuGet publication is included. The owner reviews the successful Azure PR preview before merging.
