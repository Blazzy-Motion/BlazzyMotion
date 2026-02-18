# BlazzyMotion.Marquee

A CSS-driven infinite scrolling marquee component for Blazor with logo bars, testimonials, and text tickers.

[![NuGet](https://img.shields.io/nuget/v/BlazzyMotion.Marquee.svg)](https://www.nuget.org/packages/BlazzyMotion.Marquee/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/BlazzyMotion.Marquee.svg)](https://www.nuget.org/packages/BlazzyMotion.Marquee/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.txt)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)

## Table of Contents

- [Features](#features)
- [Live Demo](#live-demo)
- [Quick Start](#quick-start)
- [Display Modes](#display-modes)
- [Multi-Row](#multi-row)
- [API Reference](#api-reference)
- [Themes](#themes)
- [CSS Customization](#css-customization)
- [How It Works](#how-it-works)
- [Performance](#performance)
- [Troubleshooting](#troubleshooting)
- [Browser Support](#browser-support)
- [Contributing](#contributing)
- [License](#license)
- [Author](#author)
- [Support](#support)

## Features

- **Zero Configuration** - Just add `[BzImage]` and `[BzTitle]` attributes to your model and the Source Generator handles the rest
- **3 Display Modes** - Logo Bar, Testimonials (auto-detected), and Text Ticker
- **Multi-Row Support** - Up to 10 rows with alternating directions and speed variation
- **Staggered Entrance** - Items fade in sequentially using IntersectionObserver
- **Pause on Hover + Keyboard** - Space/Enter to pause, mouse hover support
- **Multiple Themes** - Glass, Dark, Light, and Minimal themes included out of the box
- **Optional Item Click** - `OnItemClick` EventCallback for clickable items
- **Gradient Edge Overlays** - Smooth fade-out effect at container edges
- **CSS-Driven Animation** - Pure CSS transforms, no JavaScript animation loop

## Live Demo

Experience BlazzyMotion.Marquee in action: **[View Live Demo](https://blazzymotion.com/marquee)**

![BlazzyMotion.Marquee Demo](https://raw.githubusercontent.com/Blazzy-Motion/BlazzyMotion/main/docs/images/marquee.gif)

## Quick Start

### Installation

```bash
dotnet add package BlazzyMotion.Marquee
```

Or via Package Manager Console:

```powershell
Install-Package BlazzyMotion.Marquee
```

No CSS links or service registration needed — everything loads automatically.

### Basic Usage

#### Logo Bar (Zero Config)

Just mark your model with `[BzImage]` and the component auto-renders logos — no template needed:

```razor
@using BlazzyMotion.Marquee.Components
@using BlazzyMotion.Core.Models

<BzMarquee Items="brands" Theme="BzTheme.Glass" />

@code {
    private List<Brand> brands = new()
    {
        new() { LogoUrl = "/logos/github.svg", Name = "GitHub" },
        new() { LogoUrl = "/logos/azure.svg", Name = "Azure" },
        new() { LogoUrl = "/logos/aws.svg", Name = "AWS" }
    };

    public class Brand
    {
        [BzImage] public string LogoUrl { get; set; } = "";
        [BzTitle] public string Name { get; set; } = "";
    }
}
```

#### Logo Bar (Custom Template)

Use `ItemTemplate` for full control over logo rendering:

```razor
<BzMarquee Items="brands" Theme="BzTheme.Glass">
    <ItemTemplate Context="brand">
        <img src="@brand.LogoUrl" alt="@brand.Name" style="height: 40px;" />
    </ItemTemplate>
</BzMarquee>
```

#### Text Ticker

```razor
<BzMarquee Text="Breaking news: BlazzyMotion v1.0 released!"
           Theme="BzTheme.Minimal"
           Speed="40" />
```

#### Testimonials

Items with `[BzTitle]` + `[BzDescription]` auto-render as testimonial cards:

```razor
<BzMarquee Items="reviews" Theme="BzTheme.Dark" Speed="30" />

@code {
    private List<Review> reviews = new()
    {
        new() { Author = "Jane Doe", Quote = "Best Blazor library!", Avatar = "/avatars/jane.jpg" },
        new() { Author = "John Smith", Quote = "Easy to integrate, beautiful results." }
    };

    public class Review
    {
        [BzTitle] public string Author { get; set; } = "";
        [BzDescription] public string Quote { get; set; } = "";
        [BzImage] public string? Avatar { get; set; }
    }
}
```

## Display Modes

### Logo Bar Mode

When items have only `[BzImage]` (and optionally `[BzTitle]`), the component auto-renders logos with built-in styling. No template needed — zero configuration. Use `ItemTemplate` for custom rendering when you need full control.

### Testimonial Mode

When items have `Title` + `Description` mapped via attributes, the component auto-detects and renders testimonial cards with avatar, name, and blockquote. No template needed.

### Text Ticker Mode

Set the `Text` parameter for simple scrolling text. No items, model, or template required. Perfect for news tickers and announcements.

## Multi-Row

Display multiple scrolling rows with different directions and speeds:

```razor
<BzMarquee Items="brands"
           Rows="3"
           AlternateDirection="true"
           SpeedVariation="0.3"
           Theme="BzTheme.Glass" />
```

| Parameter            | Description                                        |
| -------------------- | -------------------------------------------------- |
| `Rows`               | Number of rows (1-10)                              |
| `AlternateDirection` | Even rows scroll in opposite direction             |
| `SpeedVariation`     | Speed varies per row (0.0-0.5) for parallax effect |

## API Reference

### Data Parameters

| Parameter         | Type                     | Default | Description                                      |
| ----------------- | ------------------------ | ------- | ------------------------------------------------ |
| `Items`           | `IEnumerable<TItem>?`    | `null`  | Collection of items to display                   |
| `Text`            | `string?`                | `null`  | Plain text for ticker mode (overrides Items)     |
| `ItemTemplate`    | `RenderFragment<TItem>?` | `null`  | Custom template for each item                    |
| `OnItemClick`     | `EventCallback<TItem>`   | -       | Click callback — items become clickable when set |
| `LoadingTemplate` | `RenderFragment?`        | `null`  | Custom loading state                             |
| `EmptyTemplate`   | `RenderFragment?`        | `null`  | Custom empty state                               |

### Appearance Parameters

| Parameter           | Type          | Default | Description                                  |
| ------------------- | ------------- | ------- | -------------------------------------------- |
| `Theme`             | `BzTheme`     | `Glass` | Visual theme: Glass, Dark, Light, or Minimal |
| `Direction`         | `BzDirection` | `Left`  | Scroll direction: Left or Right              |
| `Speed`             | `int`         | `50`    | Animation speed in pixels per second         |
| `Gap`               | `int`         | `40`    | Gap between items (px)                       |
| `Rows`              | `int`         | `1`     | Number of rows (1-10)                        |
| `ShowGradientEdges` | `bool`        | `true`  | Show gradient fade on edges                  |
| `FullWidth`         | `bool`        | `false` | Span full viewport width                     |
| `CssClass`          | `string?`     | `null`  | Additional CSS classes                       |

### Behavior Parameters

| Parameter            | Type     | Default | Description                                 |
| -------------------- | -------- | ------- | ------------------------------------------- |
| `PauseOnHover`       | `bool`   | `true`  | Pause animation on mouse hover              |
| `AlternateDirection` | `bool`   | `true`  | Adjacent rows scroll in opposite directions |
| `SpeedVariation`     | `double` | `0.0`   | Speed variation between rows (0.0-0.5)      |
| `StaggerEntrance`    | `bool`   | `true`  | Enable staggered entrance animation         |
| `StaggerDelay`       | `int`    | `60`    | Delay between entrance animations (ms)      |

## Themes

BlazzyMotion.Marquee includes four professionally designed themes:

### Glass Theme (Default)

Modern glassmorphism design with blur effect and transparency:

```razor
<BzMarquee Items="brands" Theme="BzTheme.Glass" />
```

### Dark Theme

Solid dark background with subtle gradient border:

```razor
<BzMarquee Items="brands" Theme="BzTheme.Dark" />
```

### Light Theme

Clean light theme with soft shadows:

```razor
<BzMarquee Items="brands" Theme="BzTheme.Light" />
```

### Minimal Theme

No background container, borderless design:

```razor
<BzMarquee Items="brands" Theme="BzTheme.Minimal" />
```

## CSS Customization

Override CSS variables for custom styling:

```css
.my-marquee {
    --bzm-speed: 30;
    --bzm-gap: 60px;
    --bzm-gradient-size: 120px;
    --bzm-logo-height: 50px;
    --bzm-logo-opacity: 0.8;
    --bzm-testimonial-width: 400px;
}
```

```razor
<BzMarquee Items="brands" CssClass="my-marquee" />
```

### Available CSS Variables

| Variable                    | Default  | Description                         |
| --------------------------- | -------- | ----------------------------------- |
| `--bzm-speed`               | `50`     | Animation speed (pixels per second) |
| `--bzm-gap`                 | `40px`   | Gap between items                   |
| `--bzm-container-padding`   | `24px`   | Container internal padding          |
| `--bzm-gradient-size`       | `80px`   | Gradient overlay width on edges     |
| `--bzm-logo-height`         | `40px`   | Logo image height                   |
| `--bzm-logo-opacity`        | `0.6`    | Logo opacity (hover restores to 1)  |
| `--bzm-testimonial-width`   | `350px`  | Testimonial card width              |
| `--bzm-testimonial-padding` | `24px`   | Testimonial card padding            |
| `--bzm-ticker-font-size`    | `1.1rem` | Ticker text font size               |

## How It Works

### Source Generator Magic

When you mark a property with `[BzImage]`, the BlazzyMotion Source Generator automatically creates a registration function during compilation:

```csharp
// Auto-generated at compile-time
internal static class BzMappingRegistration_Brand
{
    [ModuleInitializer]
    internal static void Register()
    {
        BzRegistry.Register<Brand>(item => new BzItem
        {
            ImageUrl = item.LogoUrl,
            Title = item.Name,
            OriginalItem = item
        });
    }
}
```

### Rendering Pipeline

1. Items are mapped via `BzRegistry.ToBzItems()` using the generated mapper
2. JavaScript module clones content for seamless infinite scroll
3. IntersectionObserver triggers stagger entrance animation
4. CSS `@keyframes` handles continuous scroll — no JS animation loop

## Performance

- **Zero Runtime Overhead** - Mapping functions generated at compile-time
- **Zero Reflection** - Uses `[ModuleInitializer]` for automatic registration
- **CSS-Only Animation** - Pure CSS transforms, no `requestAnimationFrame` loop
- **GPU Accelerated** - Uses `transform: translateX()` for compositor-layer animation
- **Lazy Loading** - Images use `loading="lazy"` for deferred loading

## Troubleshooting

**Template Not Generated:**

- Ensure `[BzImage]` or `[BzTitle]` is on a `public string` property
- Rebuild the project to trigger Source Generator
- Add `@using BlazzyMotion.Core.Attributes`

**Marquee Not Scrolling:**

- Verify content width exceeds container width (cloning needs enough content)
- Check that JavaScript interop loaded correctly

**Items Not Visible:**

- Check that `Items` is not null or empty
- Verify image URLs are accessible
- For testimonials, ensure model has both `[BzTitle]` and `[BzDescription]`

**Stagger Animation Not Working:**

- Verify `StaggerEntrance="true"` (default)
- Check browser IntersectionObserver support
- Ensure container is visible in viewport

## Browser Support

| Browser | Version |
| ------- | ------- |
| Chrome  | 88+     |
| Firefox | 78+     |
| Safari  | 14+     |
| Edge    | 88+     |

Requires CSS `animation` and `@keyframes` support. Backdrop-filter for Glass theme gracefully degrades on mobile.

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

### Building from Source

```bash
git clone https://github.com/Blazzy-Motion/BlazzyMotion.git
cd BlazzyMotion
dotnet build
```

### Running Tests

```bash
dotnet test
```

## License

MIT License - see [LICENSE](LICENSE.txt) for details.

## Author

- GitHub: [@nenad0707](https://github.com/nenad0707)
- LinkedIn: [Nenad Ristic](https://www.linkedin.com/in/nenad-risti%C4%87-27459958/)

## Support

If you find BlazzyMotion.Marquee useful, please consider:

- Giving it a star on GitHub
- Sharing it with other Blazor developers
- Reporting bugs or suggesting features via GitHub Issues

For questions or support, please open an issue on GitHub.

---

**Part of the [BlazzyMotion](https://github.com/Blazzy-Motion/BlazzyMotion) component ecosystem.**
