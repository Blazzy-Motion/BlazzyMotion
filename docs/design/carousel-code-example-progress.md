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
