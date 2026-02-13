# BlazzyMotion

A collection of modern, high-performance UI components for Blazor with zero-configuration support through Source Generators.

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.txt)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)

## Live Demo

Experience BlazzyMotion components in action: **[View Live Demo](https://blazzymotion.com/)**

## Components

| Package                                                      | Description                                              | NuGet                                                                                                                       |
| ------------------------------------------------------------ | -------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------- |
| [BlazzyMotion.Carousel](src/BlazzyMotion.Carousel/README.md) | 3D coverflow carousel powered by Swiper.js               | [![NuGet](https://img.shields.io/nuget/v/BlazzyMotion.Carousel.svg)](https://www.nuget.org/packages/BlazzyMotion.Carousel/) |
| [BlazzyMotion.Gallery](src/BlazzyMotion.Gallery/README.md)   | Photo gallery with lightbox, filtering, and masonry grid | [![NuGet](https://img.shields.io/nuget/v/BlazzyMotion.Gallery.svg)](https://www.nuget.org/packages/BlazzyMotion.Gallery/)   |
| [BlazzyMotion.Bento](src/BlazzyMotion.Bento/README.md)       | Bento Grid with Composition Mode for dashboards          | [![NuGet](https://img.shields.io/nuget/v/BlazzyMotion.Bento.svg)](https://www.nuget.org/packages/BlazzyMotion.Bento/)       |
| BlazzyMotion.Core                                            | Shared infrastructure (attributes, themes, registry)     | [![NuGet](https://img.shields.io/nuget/v/BlazzyMotion.Core.svg)](https://www.nuget.org/packages/BlazzyMotion.Core/)         |

## Key Features

- **Zero Configuration** - Source Generators create item templates from your models at compile-time
- **Type-Safe** - Full IntelliSense support with strongly-typed generic components
- **Multiple Themes** - Glass, Dark, Light, and Minimal themes included
- **Performance Optimized** - No runtime reflection, compiled delegates, template caching
- **Responsive Design** - Built-in adaptive behavior for all screen sizes

## Quick Start

### Installation

```bash
# For 3D Carousel
dotnet add package BlazzyMotion.Carousel

# For Photo Gallery
dotnet add package BlazzyMotion.Gallery

# For Bento Grid
dotnet add package BlazzyMotion.Bento
```

### Define Your Model

```csharp
using BlazzyMotion.Core.Attributes;

public class Movie
{
    [BzImage]
    public string ImageUrl { get; set; } = "";

    [BzTitle]
    public string Title { get; set; } = "";
}
```

### Use the Components

**Carousel:**

```razor
@using BlazzyMotion.Carousel.Components
@using BlazzyMotion.Core.Models

<BzCarousel TItem="Movie"
            Items="movies"
            Theme="BzTheme.Glass" />
```

**Bento Grid (Composition Mode):**

```razor
@using BlazzyMotion.Bento.Components
@using BlazzyMotion.Core.Models

<BzBento TItem="object" Theme="BzTheme.Glass" Columns="4">

    <BzBentoCard ColSpan="2" RowSpan="2"
                 Image="images/hero.jpg"
                 Title="Featured" />

    <BzBentoMetric Value="1,234" Label="Users" Trend="+12%" />
    <BzBentoMetric Value="99.9%" Label="Uptime" />

    <BzBentoFeature ColSpan="2" IconText="⚡" Label="Fast" Description="Built for speed" />

</BzBento>
```

**Note:** Use Composition Mode to build dashboards with metrics, cards, and embedded components. See [Bento README](src/BlazzyMotion.Bento/README.md) for full documentation.

**Gallery:**

```razor
@using BlazzyMotion.Gallery.Components
@using BlazzyMotion.Core.Models

<BzGallery TItem="Photo"
           Items="photos"
           Theme="BzTheme.Glass"
           Layout="BzGalleryLayout.Masonry"
           EnableLightbox="true"
           EnableFilter="true" />
```

**Note:** Gallery supports grid, masonry, and columns layouts with built-in lightbox, category filtering, and full keyboard/screen reader accessibility. See [Gallery README](src/BlazzyMotion.Gallery/README.md) for full documentation.

## How It Works

BlazzyMotion uses Source Generators to automatically create mapping functions at compile-time. When you mark a property with `[BzImage]`, the generator creates optimized code that runs at application startup via `[ModuleInitializer]`. This means:

- Zero runtime overhead
- Zero reflection
- Full type safety
- Automatic template generation

## Browser Support

| Browser | Version |
| ------- | ------- |
| Chrome  | 88+     |
| Firefox | 78+     |
| Safari  | 14+     |
| Edge    | 88+     |

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

```bash
git clone https://github.com/Blazzy-Motion/BlazzyMotion.git
cd BlazzyMotion
dotnet build
dotnet test
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE.txt) file for details.

## Author

- GitHub: [@nenad0707](https://github.com/nenad0707)
- LinkedIn: [Nenad Ristic](https://www.linkedin.com/in/nenad-risti%C4%87-27459958/)

## Support

If you find BlazzyMotion useful, please consider giving it a star on GitHub.

For questions or support, please open an issue on GitHub.
