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

## Compact playground revision

Move the component resource navigation beneath the title, description and installation command. Place the version beside the title, remove repeated introductory headings and reduce vertical spacing. Share one example shell across all four components: its title/Code/Copy toolbar precedes the live example, and its source opens underneath without replacing the component. Focus the requested code heading on open; Close and Escape restore the toolbar. Keep clipboard feedback independent of preview state and reset it when settings change. The previous isolated Gallery List accessibility finding remains outside this Demo-only scope.

Release publish and all 1,117 .NET tests passed. Targeted checks passed at 320, 390 and 1440 px, including toolbar copying with hidden source, clipboard denial/recovery, native clipboard contents, focus visibility below the sticky header, Close/Escape restoration, unchanged preview dimensions/instances, compact local navigation and source-content WCAG scans. The 12 copied source/model files match the separately compiled stable consumer byte-for-byte. Desktop/mobile screenshots were reviewed; at 390 px the first Gallery image moves from approximately y=1174 to y=917 relative to the preceding preview. The complete browser suite and remote checks are in progress. No library source, package versions, stable release notes or main deployment changes are included.

## Density and alignment revision

Align Carousel toolbars and source panels with the existing responsive component width token. Preserve the desktop 800 × 450 px frame and package styles. Reduce preview/source spacing from 20 to 12 px, trim repeated section/stage padding and remove excess selected-title margins. Keep primary Gallery/Marquee controls visible, and group all secondary settings in native Advanced options disclosures that preserve values on close. Hide aspect-ratio controls from layout and keyboard navigation outside Grid mode. Rename the Gallery example Photo collection. Enforce native hidden visibility against the legacy flex rule and strengthen Demo label/selected-option contrast. Footer design and library packages remain unchanged. Release publish and all 1,117 .NET tests passed. Targeted checks passed at 320, 390 and 1440 px, including advanced-option state retention, keyboard input, expanded-controls accessibility, source spacing/alignment and unchanged preview state. Browser measurements match main at 390 px (350 × 300), 980 px (750 × 350) and 1440 px (800 × 450). The first Gallery image at 390 px moves from approximately y=917 to y=758 compared with the preceding preview. Desktop/mobile screenshots were reviewed. Full local browser and remote checks are in progress.
