# Carousel example prototype checkpoint

Branch: `feat/carousel-demo-code-example`. Base: `1baca3c`.

## Scope

- Give `/carousel` a dedicated demonstration-and-code page; preserve the existing landing page at `/`.
- Pair the real component with a complete theme-aware Razor snippet and supported Movie attribute model.
- Generate sample item declarations from the same list rendered by the preview, using public image URLs in copied code.
- Preserve central build-version metadata, stable installation commands, resource links, library defaults and package behavior.
- Stack code below the live preview on small screens and keep long code inside a keyboard-accessible scroll region.

## Validation checkpoint

Release solution build, all 1,117 .NET tests and default publish passed. Targeted browser checks passed at 320, 390 and 1440 px: clipboard contents match the displayed code, four themes retain the instance and keyboard-selected movie, the model disclosure opens and expanded-content accessibility passes. The exact copied files compiled in a separate consumer using the current projects with zero warnings/errors. The same files also compiled against the latest published stable Carousel 1.4.1 package with zero warnings/errors. The complete browser suite is in progress. Remote PR outcomes will be available on the associated checks. Desktop/mobile screenshots were visually reviewed.

No component library source, package version, publication workflow or main deployment is changed. This is one isolated page for owner review before applying any pattern elsewhere.
