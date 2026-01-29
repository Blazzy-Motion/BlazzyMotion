# BlazzyMotion.Bento

A modern Bento Grid component for Blazor with **Composition Mode** for building rich dashboard layouts. Combine metrics, features, cards, quotes, and embed other components like BzCarousel.

[![NuGet](https://img.shields.io/nuget/v/BlazzyMotion.Bento.svg)](https://www.nuget.org/packages/BlazzyMotion.Bento/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/BlazzyMotion.Bento.svg)](https://www.nuget.org/packages/BlazzyMotion.Bento/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.txt)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)

## Table of Contents

- [Features](#features)
- [Live Demo](#live-demo)
- [Perfect For](#perfect-for)
- [Quick Start](#quick-start)
- [Composition Mode](#composition-mode)
- [Built-in Components](#built-in-components)
- [Items Mode](#items-mode)
- [API Reference](#api-reference)
- [Themes](#themes)
- [Responsive Behavior](#responsive-behavior)
- [CSS Customization](#css-customization)
- [How It Works](#how-it-works)
- [Performance](#performance)
- [Troubleshooting](#troubleshooting)
- [Browser Support](#browser-support)
- [Contributing](#contributing)
- [License](#license)
- [Author](#author)

## Features

- **Composition Mode** - Build complex layouts with metrics, features, cards, and quotes
- **Embed Any Component** - Nest BzCarousel, charts, or any Blazor component inside grid cells
- **5 Built-in Components** - BzBentoItem, BzBentoCard, BzBentoMetric, BzBentoFeature, BzBentoQuote
- **Flexible Grid Spanning** - ColSpan and RowSpan (1-4) for each item
- **Staggered Animations** - Intersection Observer-powered entrance animations
- **Multiple Themes** - Glass, Dark, Light, and Minimal themes
- **CSS Grid Powered** - Native CSS Grid with `grid-auto-flow: dense` for gap-free layouts
- **Responsive Design** - Automatic column reduction on tablet and mobile
- **Items Mode** - Simple image gallery with zero configuration (for basic use cases)

## Live Demo

Experience BlazzyMotion.Bento in action: **[View Live Demo](https://blazzymotion.com/bento)**

## Perfect For

- **SaaS Dashboards** - Display metrics, KPIs, and status indicators
- **Landing Pages** - Apple-style feature showcases with mixed content
- **Admin Panels** - Combine charts, stats, and data tables in one view
- **Portfolio Sites** - Showcase projects with varying sizes and emphasis
- **Product Pages** - Mix product images, reviews, and specifications

## Quick Start

### Installation

```bash
dotnet add package BlazzyMotion.Bento
```

Or via Package Manager Console:

```powershell
Install-Package BlazzyMotion.Bento
```

### Basic Composition Mode

The primary way to use BzBento is **Composition Mode** - building layouts with built-in components:

```razor
@using BlazzyMotion.Bento.Components
@using BlazzyMotion.Core.Models

<BzBento TItem="object" Theme="BzTheme.Glass" Columns="4">

    <BzBentoCard ColSpan="2" RowSpan="2"
                 Image="images/hero.jpg"
                 Title="Featured Product"
                 Description="Our flagship offering" />

    <BzBentoMetric Value="12,847" Label="Active Users" Trend="+23%" />
    <BzBentoMetric Value="99.9%" Label="Uptime" />

    <BzBentoFeature ColSpan="2"
                    IconText="⚡"
                    Label="Lightning Fast"
                    Description="Built for performance" />

    <BzBentoQuote Text="This component saved us weeks of development!"
                  Author="Jane Doe"
                  Role="Tech Lead" />

</BzBento>
```

## Composition Mode

Composition Mode is the **recommended approach** for BzBento. It gives you complete control over the layout by combining built-in components.

### Dashboard Example

Build interactive dashboards by combining metrics, features, and embedded components:

```razor
<BzBento TItem="object" Theme="BzTheme.Glass" Columns="4">

    <!-- Hero card spanning 2x2 -->
    <BzBentoItem ColSpan="2" RowSpan="2">
        <BzCarousel TItem="Company"
                    Items="companies"
                    OnItemSelected="OnCompanyChanged" />
    </BzBentoItem>

    <!-- Dynamic metrics that update based on carousel selection -->
    <BzBentoMetric Value="@company?.Revenue" Label="Revenue" Trend="+23%" />
    <BzBentoMetric Value="@company?.Users" Label="Users" Trend="+12%" />
    <BzBentoMetric Value="99.9%" Label="Uptime" />
    <BzBentoMetric Value="24ms" Label="Response Time" />

    <!-- Feature spanning 2 columns -->
    <BzBentoFeature ColSpan="2"
                    IconText="📊"
                    Label="Q4 Report Ready"
                    Description="Revenue exceeded targets by 15%" />

    <!-- Testimonial -->
    <BzBentoQuote ColSpan="2"
                  Text="The best Blazor component library we've used."
                  Author="John Smith"
                  Role="CTO at TechCorp" />

</BzBento>

@code {
    private List<Company> companies = new();
    private Company? company;

    private void OnCompanyChanged(Company c) => company = c;
}
```

### Embedding Components

Use `BzBentoItem` to embed any Blazor component inside the grid:

```razor
<BzBento TItem="object" Columns="3">

    <!-- Embed a carousel -->
    <BzBentoItem ColSpan="2" RowSpan="2">
        <BzCarousel TItem="Product" Items="products" />
    </BzBentoItem>

    <!-- Embed a chart component -->
    <BzBentoItem ColSpan="1" RowSpan="2">
        <MyChartComponent Data="chartData" />
    </BzBentoItem>

    <!-- Embed any custom content -->
    <BzBentoItem ColSpan="3">
        <div class="custom-footer">
            <p>Custom HTML content goes here</p>
        </div>
    </BzBentoItem>

</BzBento>
```

## Built-in Components

All components support `ColSpan` (1-4), `RowSpan` (1-4), and `OnClick` for grid spanning and interaction.

### BzBentoItem

Base container for embedding any content:

```razor
<BzBentoItem ColSpan="2" RowSpan="2">
    <BzCarousel TItem="Product" Items="products" />
</BzBentoItem>
```

### BzBentoCard

Image card with overlay text:

```razor
<BzBentoCard ColSpan="2" RowSpan="2"
             Image="images/hero.jpg"
             Title="Featured"
             Description="Our flagship product" />
```

| Parameter     | Type      | Description                    |
| ------------- | --------- | ------------------------------ |
| `Image`       | `string?` | Image URL                      |
| `Title`       | `string?` | Card title (overlay)           |
| `Description` | `string?` | Card description (overlay)     |
| `Item`        | `TItem?`  | Auto-map via [BzImage] attrs   |

### BzBentoMetric

KPI/statistic display with trend indicator:

```razor
<BzBentoMetric Value="12,847" Label="Active Users" Trend="+23%" IconText="👥" />
```

| Parameter  | Type              | Required | Description                              |
| ---------- | ----------------- | -------- | ---------------------------------------- |
| `Value`    | `string`          | Yes      | The metric value (e.g., "1,234", "$48K") |
| `Label`    | `string`          | Yes      | Description of the metric                |
| `Trend`    | `string?`         | No       | Trend indicator (+green, -red)           |
| `IconText` | `string?`         | No       | Emoji icon (e.g., "📈")                  |
| `Icon`     | `RenderFragment?` | No       | Custom icon component                    |

### BzBentoFeature

Feature showcase with icon:

```razor
<BzBentoFeature ColSpan="2"
                IconText="⚡"
                Label="Lightning Fast"
                Description="Built for performance" />
```

| Parameter     | Type              | Required | Description             |
| ------------- | ----------------- | -------- | ----------------------- |
| `Label`       | `string`          | Yes      | Feature name            |
| `Description` | `string?`         | No       | Feature description     |
| `IconText`    | `string?`         | No       | Emoji icon              |
| `Icon`        | `RenderFragment?` | No       | Custom icon (SVG, etc.) |

### BzBentoQuote

Testimonial/quote display:

```razor
<BzBentoQuote ColSpan="2"
              Text="This component saved us weeks!"
              Author="Jane Doe"
              Role="Tech Lead"
              Avatar="images/jane.jpg" />
```

| Parameter | Type      | Description                        |
| --------- | --------- | ---------------------------------- |
| `Text`    | `string?` | Quote text                         |
| `Author`  | `string?` | Author name                        |
| `Role`    | `string?` | Author's role/title                |
| `Avatar`  | `string?` | Author's avatar URL                |
| `Item`    | `TItem?`  | Auto-map via [BzDescription] attrs |

## Items Mode

For simple **uniform image grids**, you can use Items Mode with the `Items` parameter. Mark your model with `[BzImage]` and `[BzTitle]` attributes, and the Source Generator creates the template automatically.

```razor
<BzBento TItem="Product" Items="products" Columns="4" OnItemSelected="HandleClick" />
```

See [Composition Mode](#composition-mode) for full layout control with metrics, features, and embedded components.

## API Reference

### BzBento Parameters

| Parameter          | Type                     | Default | Description                   |
| ------------------ | ------------------------ | ------- | ----------------------------- |
| `Items`            | `IEnumerable<TItem>?`    | `null`  | Collection for Items mode     |
| `ChildContent`     | `RenderFragment?`        | `null`  | Content for Composition mode  |
| `Columns`          | `int`                    | `4`     | Grid columns (1-12)           |
| `Gap`              | `int`                    | `16`    | Gap between items in pixels   |
| `Theme`            | `BzTheme`                | `Glass` | Visual theme                  |
| `AnimationEnabled` | `bool`                   | `true`  | Enable entrance animations    |
| `StaggerDelay`     | `int`                    | `50`    | Animation delay per item (ms) |
| `OnItemSelected`   | `EventCallback<TItem>`   | -       | Item click callback           |
| `ItemTemplate`     | `RenderFragment<TItem>?` | `null`  | Custom item template          |
| `LoadingTemplate`  | `RenderFragment?`        | `null`  | Custom loading state          |
| `EmptyTemplate`    | `RenderFragment?`        | `null`  | Custom empty state            |

## Themes

```razor
<BzBento Items="items" Theme="BzTheme.Glass" />   <!-- Glassmorphism (default) -->
<BzBento Items="items" Theme="BzTheme.Dark" />    <!-- Solid dark -->
<BzBento Items="items" Theme="BzTheme.Light" />   <!-- Clean light -->
<BzBento Items="items" Theme="BzTheme.Minimal" /> <!-- No container -->
```

## Responsive Behavior

| Breakpoint       | Columns       | Gap           |
| ---------------- | ------------- | ------------- |
| Desktop (>991px) | As configured | As configured |
| Tablet (<=991px) | 2             | 12px          |
| Mobile (<=600px) | 1             | 8px           |

## CSS Customization

Override CSS variables for custom styling:

```css
.my-bento {
  --bzb-columns: 3;
  --bzb-gap: 24px;
  --bzb-card-radius: 12px;
  --bzb-bg: rgba(100, 50, 200, 0.1);
}
```

```razor
<BzBento Items="items" CssClass="my-bento" />
```

## How It Works

### Source Generator Magic

When you mark a property with `[BzImage]`, the BlazzyMotion Source Generator automatically creates a registration function during compilation:

```csharp
// Auto-generated at compile-time
internal static class BzMappingRegistration_Product
{
    [ModuleInitializer]
    internal static void Register()
    {
        BzRegistry.Register<Product>(item => new BzItem
        {
            ImageUrl = item.ImageUrl,
            Title = item.Name,
            OriginalItem = item
        });
    }
}
```

The `[ModuleInitializer]` attribute ensures registration runs automatically at application startup - **zero reflection, zero configuration**.

### Composition Mode Rendering

In Composition Mode, BzBento uses CSS Grid with `grid-auto-flow: dense` to automatically fill gaps:

1. Each `BzBentoItem`, `BzBentoCard`, etc. becomes a grid item
2. `ColSpan` and `RowSpan` control `grid-column` and `grid-row` CSS properties
3. The `dense` algorithm places items efficiently without leaving gaps

## Performance

- **Zero Runtime Overhead** - Mapping functions generated at compile-time
- **Zero Reflection** - Uses `[ModuleInitializer]` for automatic registration
- **GPU Accelerated** - Animations use `will-change: transform, opacity`
- **Intersection Observer** - Only animates items when they enter viewport
- **CSS Grid Native** - Browser-optimized layout engine

## Troubleshooting

**Template Not Generated:**

- Ensure `[BzImage]` is on a `public string` property
- Rebuild the project
- Add `@using BlazzyMotion.Core.Attributes`

**Grid Items Not Visible:**

- Check that `Items` is not null or empty
- Verify image URLs are accessible

**Animations Not Working:**

- Verify `AnimationEnabled="true"`
- Check browser Intersection Observer support

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

MIT License - see [LICENSE](LICENSE.txt) for details.

## Author

- GitHub: [@nenad0707](https://github.com/nenad0707)
- LinkedIn: [Nenad Ristic](https://www.linkedin.com/in/nenad-risti%C4%87-27459958/)

---

**Part of the [BlazzyMotion](https://github.com/Blazzy-Motion/BlazzyMotion) component ecosystem.**
