# BlazzyMotion.Gallery

A premium image gallery component for Blazor with Grid, Masonry, and List layouts.

[![NuGet](https://img.shields.io/nuget/v/BlazzyMotion.Gallery.svg)](https://www.nuget.org/packages/BlazzyMotion.Gallery/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/BlazzyMotion.Gallery.svg)](https://www.nuget.org/packages/BlazzyMotion.Gallery/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.txt)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)

## Table of Contents

- [Features](#features)
- [Live Demo](#live-demo)
- [Quick Start](#quick-start)
- [Layouts](#layouts)
- [API Reference](#api-reference)
- [Themes](#themes)
- [Category Filtering](#category-filtering)
- [Lightbox](#lightbox)
- [Responsive Behavior](#responsive-behavior)
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

- **Zero Configuration** - Just add `[BzImage]` attribute to your model and the Source Generator handles the rest
- **3 Layout Modes** - Grid (uniform), Masonry (Pinterest-style), and List (horizontal cards)
- **Fullscreen Lightbox** - Keyboard navigation, touch swipe, zoom-in animation
- **Category Filtering** - Animated filter bar with smooth show/hide transitions
- **Multiple Themes** - Glass, Dark, Light, and Minimal themes included out of the box
- **Staggered Animations** - Intersection Observer-powered entrance animations per item
- **Mobile Optimized** - Touch swipe in lightbox, no backdrop-filter on mobile for performance
- **Accessible** - Focus-visible states, keyboard navigation, `prefers-reduced-motion` support

## Live Demo

Experience BlazzyMotion.Gallery in action: **[View Live Demo](https://blazzymotion.com/gallery)**

## Quick Start

### Installation

```bash
dotnet add package BlazzyMotion.Gallery
```

Or via Package Manager Console:

```powershell
Install-Package BlazzyMotion.Gallery
```

No CSS links or service registration needed — everything loads automatically.

### Basic Usage

#### 1. Define Your Model

Mark your data model with `[BzImage]` to specify the image property:

```csharp
using BlazzyMotion.Core.Attributes;

public class Photo
{
    [BzImage]
    public string Url { get; set; } = "";

    [BzTitle]
    public string Caption { get; set; } = "";
}
```

#### 2. Use the Component

```razor
@using BlazzyMotion.Gallery.Components
@using BlazzyMotion.Core.Models

<BzGallery Items="photos" Theme="BzTheme.Glass" />

@code {
    private List<Photo> photos = new()
    {
        new Photo { Url = "/images/photo1.jpg", Caption = "Sunset" },
        new Photo { Url = "/images/photo2.jpg", Caption = "Mountains" },
        new Photo { Url = "/images/photo3.jpg", Caption = "Ocean" }
    };
}
```

The Source Generator automatically creates the template at compile-time.

## Layouts

### Grid Layout

Uniform grid with configurable aspect ratio. All images are cropped to the same dimensions.

```razor
<BzGallery Items="photos"
           Layout="BzGalleryLayout.Grid"
           Columns="3"
           AspectRatio="4/3" />
```

### Masonry Layout

Pinterest-style layout that preserves original image aspect ratios. Uses CSS `columns` for a pure-CSS solution.

```razor
<BzGallery Items="photos"
           Layout="BzGalleryLayout.Masonry"
           Columns="3"
           Theme="BzTheme.Glass" />
```

### List Layout

Horizontal cards with image on the left and text on the right. Stacks vertically on mobile.

```razor
<BzGallery Items="photos"
           Layout="BzGalleryLayout.List"
           Theme="BzTheme.Light" />
```

## API Reference

### Component Parameters

#### Data Parameters

| Parameter          | Type                     | Default | Description                                      |
| ------------------ | ------------------------ | ------- | ------------------------------------------------ |
| `Items`            | `IEnumerable<TItem>?`    | `null`  | Collection of items to display                   |
| `ItemTemplate`     | `RenderFragment<TItem>?` | `null`  | Custom template for rendering items              |
| `OnItemSelected`   | `EventCallback<TItem>`   | -       | Item click callback (when lightbox is disabled)  |

#### Appearance Parameters

| Parameter     | Type              | Default | Description                                    |
| ------------- | ----------------- | ------- | ---------------------------------------------- |
| `Layout`      | `BzGalleryLayout` | `Grid`  | Layout mode: Grid, Masonry, or List            |
| `Columns`     | `int`             | `3`     | Number of columns (1-6)                        |
| `Gap`         | `int`             | `16`    | Gap between items in pixels                    |
| `AspectRatio` | `string?`         | `"4/3"` | Image aspect ratio for Grid mode               |
| `Theme`       | `BzTheme`         | `Glass` | Visual theme: Glass, Dark, Light, or Minimal   |
| `CssClass`    | `string?`         | `null`  | Additional CSS classes for customization       |

#### Behavior Parameters

| Parameter          | Type                    | Default | Description                            |
| ------------------ | ----------------------- | ------- | -------------------------------------- |
| `EnableLightbox`   | `bool`                  | `true`  | Enable fullscreen lightbox on click    |
| `EnableFilter`     | `bool`                  | `false` | Show category filter bar               |
| `CategorySelector` | `Func<TItem, string>?`  | `null`  | Function to extract category from item |
| `AnimationEnabled` | `bool`                  | `true`  | Enable staggered entry animations      |

#### Template Parameters

| Parameter          | Type              | Default | Description            |
| ------------------ | ----------------- | ------- | ---------------------- |
| `LoadingTemplate`  | `RenderFragment?` | `null`  | Custom loading state   |
| `EmptyTemplate`    | `RenderFragment?` | `null`  | Custom empty state     |

## Themes

BlazzyMotion.Gallery includes four professionally designed themes:

### Glass Theme (Default)

Modern glassmorphism design with blur effect and transparency:

```razor
<BzGallery Items="photos" Theme="BzTheme.Glass" />
```

### Dark Theme

Solid dark background with subtle gradient border:

```razor
<BzGallery Items="photos" Theme="BzTheme.Dark" />
```

### Light Theme

Clean light theme with soft shadows:

```razor
<BzGallery Items="photos" Theme="BzTheme.Light" />
```

### Minimal Theme

No background container, borderless design:

```razor
<BzGallery Items="photos" Theme="BzTheme.Minimal" />
```

## Category Filtering

Enable the filter bar to let users browse by category. Provide a `CategorySelector` function to extract the category from each item:

```csharp
public class Photo
{
    [BzImage] public string Url { get; set; } = "";
    [BzTitle] public string Caption { get; set; } = "";
    public string Category { get; set; } = "";
}
```

```razor
<BzGallery Items="photos"
           EnableFilter="true"
           CategorySelector="@(p => p.Category)"
           Theme="BzTheme.Dark" />
```

When no `CategorySelector` is provided, the `[BzDescription]` attribute value is used as the category.

## Lightbox

The fullscreen lightbox is enabled by default. It supports:

- **Keyboard Navigation** - Left/Right arrows to navigate, Escape to close
- **Touch Swipe** - Swipe left/right on mobile to navigate
- **Zoom-in Animation** - Smooth scale animation when opening
- **Image Counter** - Shows current position (e.g., "3 / 12")
- **Caption Display** - Shows title and description below the image

### Keyboard Shortcuts

| Key   | Action         |
| ----- | -------------- |
| `←`   | Previous image |
| `→`   | Next image     |
| `Esc` | Close lightbox |

### Disabling Lightbox

Use `OnItemSelected` instead for custom click handling:

```razor
<BzGallery TItem="Photo"
           Items="photos"
           EnableLightbox="false"
           OnItemSelected="HandlePhotoClick" />

@code {
    private void HandlePhotoClick(Photo photo)
    {
        // Navigate, open modal, etc.
    }
}
```

## Responsive Behavior

| Breakpoint              | Columns       | Gap           |
| ----------------------- | ------------- | ------------- |
| Desktop (> 991px)       | As configured | As configured |
| Tablet (600px - 991px)  | 2             | 12px          |
| Small mobile (< 600px)  | 1             | 8px           |

On mobile devices:

- Backdrop-filter is disabled for performance
- Filter bar becomes horizontally scrollable
- List layout stacks vertically (image on top, text below)

## CSS Customization

Override CSS variables for custom styling:

```css
.my-gallery {
    --bzg-columns: 4;
    --bzg-gap: 20px;
    --bzg-aspect-ratio: 16/9;
    --bzg-item-radius: 16px;
    --bzg-max-width: 1400px;
}
```

```razor
<BzGallery Items="photos" CssClass="my-gallery" />
```

### Available CSS Variables

| Variable                  | Default       | Description                    |
| ------------------------- | ------------- | ------------------------------ |
| `--bzg-columns`           | `3`           | Number of grid columns         |
| `--bzg-gap`               | `16px`        | Gap between items              |
| `--bzg-aspect-ratio`      | `4/3`         | Image aspect ratio (Grid mode) |
| `--bzg-item-radius`       | `8px`         | Item border radius             |
| `--bzg-max-width`         | `1200px`      | Container max width            |
| `--bzg-border-radius`     | `12px`        | Container border radius        |
| `--bzg-filter-height`     | `44px`        | Filter button height           |
| `--bzg-filter-radius`     | `22px`        | Filter button border radius    |
| `--bzg-list-image-width`  | `300px`       | Image width in List layout     |

## How It Works

### Source Generator Magic

When you mark a property with `[BzImage]`, the BlazzyMotion Source Generator automatically creates a registration function during compilation:

```csharp
// Auto-generated at compile-time
internal static class BzMappingRegistration_Photo
{
    [ModuleInitializer]
    internal static void Register()
    {
        BzRegistry.Register<Photo>(item => new BzItem
        {
            ImageUrl = item.Url,
            Title = item.Caption,
            OriginalItem = item
        });
    }
}
```

The `[ModuleInitializer]` attribute ensures registration runs automatically at application startup - **zero reflection, zero configuration**.

### Rendering Pipeline

1. Items are mapped via `BzRegistry.ToBzItems()` using the generated mapper
2. JavaScript module initializes IntersectionObserver for staggered animations
3. Double `requestAnimationFrame` reveals the container (FOUC prevention)
4. Each item animates in as it enters the viewport

## Performance

- **Zero Runtime Overhead** - Mapping functions generated at compile-time
- **Zero Reflection** - Uses `[ModuleInitializer]` for automatic registration
- **GPU Accelerated** - Animations use `will-change: transform, opacity`
- **Intersection Observer** - Only animates items when they enter viewport
- **CSS-Only Masonry** - Uses CSS `columns` property, no JavaScript layout calculation
- **Lazy Loading** - Images use `loading="lazy"` for deferred loading

## Troubleshooting

**Template Not Generated:**

- Ensure `[BzImage]` is on a `public string` property
- Rebuild the project to trigger Source Generator
- Add `@using BlazzyMotion.Core.Attributes`

**Gallery Items Not Visible:**

- Check that `Items` is not null or empty
- Verify image URLs are accessible
- Ensure the container has the `bzg-ready` class after initialization

**Animations Not Working:**

- Verify `AnimationEnabled="true"`
- Check browser Intersection Observer support
- Check `prefers-reduced-motion` media query in browser settings

**Filter Bar Not Showing:**

- Set `EnableFilter="true"`
- Ensure `CategorySelector` is provided or items have `[BzDescription]`
- At least 2 categories are needed for the filter bar to appear

## Browser Support

| Browser | Version |
| ------- | ------- |
| Chrome  | 88+     |
| Firefox | 78+     |
| Safari  | 14+     |
| Edge    | 88+     |

Requires CSS `aspect-ratio` support for Grid layout. Backdrop-filter for Glass theme gracefully degrades on mobile.

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

If you find BlazzyMotion.Gallery useful, please consider:

- Giving it a star on GitHub
- Sharing it with other Blazor developers
- Reporting bugs or suggesting features via GitHub Issues

For questions or support, please open an issue on GitHub.

---

**Part of the [BlazzyMotion](https://github.com/Blazzy-Motion/BlazzyMotion) component ecosystem.**
