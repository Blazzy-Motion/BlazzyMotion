# Carousel example prototype checkpoint

Branch: `feat/carousel-demo-code-example`. Base: `1baca3c`.

## Scope

- Give `/carousel` a dedicated demonstration-and-code page; preserve the existing landing page at `/`.
- Pair the real component with a complete theme-aware Razor snippet and supported Movie attribute model.
- Generate sample item declarations from the same list rendered by the preview, using public image URLs in copied code.
- Preserve central build-version metadata, stable installation commands, resource links, library defaults and package behavior.
- Keep the live preview full width at every viewport. Reveal code below it through a native, keyboard-accessible View code disclosure, closed by default.

## Validation checkpoint

Release solution build, all 1,117 .NET tests and default publish passed. Targeted browser checks passed at 320, 390 and 1440 px: clipboard contents match the displayed code, four themes retain the instance and keyboard-selected movie, the model disclosure opens and expanded-content accessibility passes. The exact copied files compiled in a separate consumer using the current projects with zero warnings/errors. The same files also compiled against the latest published stable Carousel 1.4.1 package with zero warnings/errors. The complete browser suite is in progress. Remote PR outcomes will be available on the associated checks. Desktop/mobile screenshots were visually reviewed.

No component library source, package version, publication workflow or main deployment is changed. This is one isolated page for owner review before applying any pattern elsewhere.

## Owner review revision

Replace the side-by-side prototype with a full-width preview and optional code beneath it. Opening and closing code must retain the Swiper instance, selected movie, theme and preview dimensions. The revised Release publish and all 1,117 .NET tests passed. Targeted browser checks passed at 320, 390 and 1440 px, including real clipboard copying, keyboard disclosure interaction, stable preview dimensions and expanded-content WCAG scans. Desktop and mobile screenshots were reviewed. The complete browser suite and remote checks are running for this revision.

## Shared examples revision

- Center the theme controls on `/carousel`, `/bento`, `/gallery` and `/marquee`. Center the Carousel preview-background control as well.
- Apply the compact installation header to Bento, Gallery and Marquee using existing build-version metadata.
- Preserve every live example and its controls. Add a collapsed native View code disclosure beneath each Bento/Gallery/Marquee example.
- Generate complete consumer snippets from the live datasets and selected settings, including the Bento company callback and Gallery selection handler. Include supported Core attribute models, public artwork URLs and culture-independent Marquee speed variation. Optional site-specific icons are omitted from copied examples.
- Validate centered controls, disclosure keyboard interaction, exact clipboard output, retained nested Carousel state, expanded-content accessibility and standalone compilation of all copied examples.

The expanded Release build/publish and all 1,117 .NET tests passed. Targeted source/control checks passed at 320, 390 and 1440 px. All seven copied examples compiled together in a separate Razor consumer against live-confirmed stable packages (Carousel 1.4.1, Bento 1.0.2, Gallery 1.0.0 and Marquee 1.0.0), with zero warnings/errors. Desktop/mobile screenshots were reviewed after scrolling through the live examples. The complete browser suite and remote checks are in progress. Library source, package versions and production deployment remain unchanged.

### Existing Gallery List accessibility finding

The expanded-controls check also exercises the pre-existing List layout. Axe reports `label-content-name-mismatch` on Gallery item buttons: `GetItemAriaLabel` includes the title and lightbox instruction, while List items display both title and description. This logic is unchanged in the Gallery library. It is recorded for a separate package fix; this Demo revision does not claim full List-layout WCAG conformance. The new disclosure accessibility checks cover their expanded source content; the existing route suite still audits whole pages in their default configurations.
