# BlazzyMotion v1.5.0 - Carousel Themes and Accessibility

This release promotes **BlazzyMotion.Carousel 1.5.0** and **BlazzyMotion.Core 1.4.0** to stable. It retains the original Coverflow composition while refining the four themes, improving keyboard navigation and making mapped-image loading more resilient.

## Improvements

- **IMPROVED:** Glass edge lighting and active-artwork depth, with distinct Dark, Light and Minimal surface treatments.
- **PRESERVED:** Original responsive artwork dimensions and full-height Swiper slide wrappers, including custom templates.
- **ADDED:** Local arrow-key navigation, keyboard-focused Previous/Next controls, slide semantics, focus management and polite slide announcements.
- **ADDED:** Respect for reduced-motion preferences, including changes made while the component is running.
- **IMPROVED:** Initial mapped-image priority, asynchronous decoding, loading/error placeholders and a visible first-artwork fallback when Swiper cannot load.
- **IMPROVED:** Active-index notifications occur once per actual consumer-index change rather than repeating during Swiper loop repairs.

## Bug Fixes

- **FIXED:** Ancestor attribute updates no longer realign an in-progress touch gesture in nested grid layouts.
- **PRESERVED:** Actual container-size observation, route reentry and disposal behavior.
- **FIXED:** A loading collection does not initialize Swiper before item markup exists.

## Migration Guide

Existing Carousel 1.4.x parameters, attributes and model mappings remain supported. Both `BlazzyMotion.Carousel.Attributes` and `BlazzyMotion.Core.Attributes` continue to work. Custom templates retain ownership of their image loading and accessible content.

Carousel 1.5.0 requires **Core 1.4.0 or later**, which NuGet installs transitively. If your application explicitly references an older Core version, update that reference too, including any entry in `Directory.Packages.props`, to avoid a downgrade conflict. Updating Core alone does not upgrade Carousel. Earlier Carousel 1.4.1 usage was validated against the new Core.

Consumers should not rely on duplicate callbacks for an unchanged active index. Shared Core theme token values and static asset paths remain unchanged.

Bento 1.0.2, Gallery 1.0.0 and Marquee 1.0.0 are not republished. Their existing Core dependency ranges remain valid; applications using those packages alone can explicitly update Core if they want the shared runtime update.

## Installation

```bash
dotnet add package BlazzyMotion.Carousel --version 1.5.0
```

For an explicitly pinned Core reference:

```bash
dotnet add package BlazzyMotion.Core --version 1.4.0
```

## Validation

Validation includes the Release solution suite, browser checks on desktop/mobile viewports, reduced motion, cold and repeated gestures, actual container resizing, image lifecycle behavior, API baselines and separate consumers using packed artifacts. Package consumers cover legacy/Core model attributes, released companion components and an older Carousel with the new Core.

Automated accessibility checks do not certify complete WCAG conformance or every screen-reader/browser combination. Performance checks do not guarantee 60 FPS on physical devices.

## Links

- **NuGet (Carousel):** [BlazzyMotion.Carousel 1.5.0](https://www.nuget.org/packages/BlazzyMotion.Carousel/1.5.0)
- **NuGet (Core):** [BlazzyMotion.Core 1.4.0](https://www.nuget.org/packages/BlazzyMotion.Core/1.4.0)
- **Live Demo:** [blazzymotion.com](https://blazzymotion.com/)
