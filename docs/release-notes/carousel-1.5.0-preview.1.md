# BlazzyMotion.Carousel 1.5.0-preview.1 and BlazzyMotion.Core 1.4.0-preview.1

This prerelease is intended for evaluation before the stable release.

Carousel retains its original 3D Coverflow composition and responsive slide and
artwork dimensions while improving theme definition, keyboard navigation and
image-loading behavior.

## Carousel 1.5.0-preview.1

- Refine Glass with stationary edge lighting and a restrained active-artwork halo.
- Distinguish Dark, Light and Minimal through surface and shadow treatments.
- Preserve full-height Swiper slide wrappers, including custom item templates.
- Keep the first item or a mapped-image placeholder visible while Swiper loads or
  fails. Prioritize the initial mapped artwork and lazily load other mapped images.
- Handle mapped-image decoding, failed requests and source replacement independently.
- Add a named carousel region, local arrow-key navigation, keyboard-focused
  Previous/Next controls, active-slide focus management and polite announcements.
- Respect reduced-motion changes without changing the selected consumer index.
- Avoid repeated accessibility attribute and announcement writes when values match.

## Core 1.4.0-preview.1

- Update shared Carousel helpers for accessibility and mapped-image readiness.
- Forward active-index changes to Blazor once per actual consumer-index change,
  excluding repeated Swiper events and loop repairs for the same item.
- Preserve shared CSS tokens, public C# APIs, legacy attribute support, registry
  mapping, generated APIs and static asset paths.


## Upgrade

```sh
dotnet add package BlazzyMotion.Carousel --version 1.5.0-preview.1
```

Carousel requires Core 1.4.0-preview.1 or later, installed automatically. Update an explicit
older Core reference as well to avoid a package downgrade error. Existing
Carousel 1.4.x parameters, model mappings and selection options remain supported.
Click selection and `SelectOnScroll=false` continue to work; applications must
not depend on repeated active-index notifications for an unchanged item.

Theme appearance intentionally changes. Applications with custom CSS should
check their overrides against all four themes. Custom templates own their image
loading, alternative text and accessible interactive content.

## Validation and limits

Preview validation includes the solution test suite, Chromium desktop/mobile
regressions, reduced motion, scoped accessibility checks, released API baselines,
and separate consumers using packed artifacts and legacy/Core model attributes.

Performance diagnostics record animation callback intervals and long tasks at
normal speed and with CPU throttling. They confirm reduced redundant runtime
work, but do not establish a large displayed-FPS increase or guarantee 60 FPS on
physical devices. Automated accessibility checks do not certify full WCAG or
screen-reader interoperability. No Cards mode, persistent dots or counters are
introduced.
