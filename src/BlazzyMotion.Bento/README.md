# BlazzyMotion.Bento

A modern Bento Grid component for Blazor with zero-configuration support through Source Generators and glassmorphism design.

[![NuGet](https://img.shields.io/nuget/v/BlazzyMotion.Bento.svg)](https://www.nuget.org/packages/BlazzyMotion.Bento/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/BlazzyMotion.Bento.svg)](https://www.nuget.org/packages/BlazzyMotion.Bento/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.txt)

## Features

- **Zero Configuration** - Source Generators create item templates from your models at compile-time
- **Composition Mode** - Embed any content including BzCarousel and other BlazzyMotion components
- **Built-in Components** - BzBentoCard, BzBentoMetric, BzBentoFeature, and BzBentoQuote
- **Staggered Animations** - Intersection Observer-powered entrance animations
- **Multiple Themes** - Glass, Dark, Light, and Minimal themes
- **CSS Grid Powered** - Native CSS Grid with flexible column and row spanning
- **Responsive Design** - Automatic column reduction on tablet and mobile devices

## Live Demo

Experience BlazzyMotion.Bento in action: **[View Live Demo](https://blazzymotion.com/bento)**

## Quick Start

### Installation

```bash
dotnet add package BlazzyMotion.Bento
```

### Basic Usage

**1. Define Your Model:**

```csharp
using BlazzyMotion.Core.Attributes;

public class Product
{
    [BzImage]
    public string ImageUrl { get; set; } = "";

    [BzTitle]
    public string Name { get; set; } = "";
}
```

**2. Use the Component:**

```razor
@using BlazzyMotion.Bento.Components
@using BlazzyMotion.Core.Models

<BzBento TItem="Product"
         Items="products"
         Theme="BzTheme.Glass"
         Columns="4" />
```

**Note:** `TItem` is required when using `EventCallback<TItem>` parameters like `OnItemSelected`.

The Source Generator automatically creates the template at compile-time.

## Usage Modes

### Items Mode

Provide a collection of items for uniform grid rendering:

```razor
<BzBento TItem="Product"
         Items="products"
         Theme="BzTheme.Glass"
         Columns="4"
         OnItemSelected="HandleClick" />
```

### Composition Mode

Use built-in components for complete layout control:

```razor
<BzBento TItem="object" Theme="BzTheme.Glass" Columns="4">

    <BzBentoCard TItem="object"
                 ColSpan="2" RowSpan="2"
                 Image="images/hero.jpg"
                 Title="Featured" />

    <BzBentoMetric Value="1,234" Label="Users" Trend="+12%" />

    <BzBentoFeature ColSpan="2" Label="Fast" Description="Built for speed" />

    <BzBentoQuote TItem="object"
                  Text="Amazing components!"
                  Author="Developer" />

</BzBento>
```

## Built-in Components

| Component        | Description                           | Key Parameters                          |
| ---------------- | ------------------------------------- | --------------------------------------- |
| `BzBentoItem`    | Base container for custom content     | `ColSpan`, `RowSpan`, `ChildContent`    |
| `BzBentoCard`    | Image card with title and description | `Image`, `Title`, `Description`, `Item` |
| `BzBentoMetric`  | Metric display with trend indicator   | `Value`, `Label`, `Trend`, `Icon`       |
| `BzBentoFeature` | Feature highlight with icon           | `Label`, `Description`, `Icon`          |
| `BzBentoQuote`   | Testimonial display                   | `Text`, `Author`, `Role`, `Avatar`      |

All components support `ColSpan` (1-4) and `RowSpan` (1-4) for grid spanning.

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

## Example: Dashboard

```razor
<BzBento TItem="object" Theme="BzTheme.Glass" Columns="4">

    <BzBentoItem ColSpan="2" RowSpan="2">
        <BzCarousel TItem="Company" Items="companies" OnItemSelected="OnCompanyChanged" />
    </BzBentoItem>

    <BzBentoMetric Value="@company?.Revenue" Label="Revenue" Trend="+23%" />
    <BzBentoMetric Value="@company?.Users" Label="Users" Trend="+12%" />
    <BzBentoMetric Value="99.9%" Label="Uptime" />
    <BzBentoMetric Value="24ms" Label="Response" />

    <BzBentoFeature ColSpan="2" Label="Q4 Report" Description="Revenue exceeded targets" />

</BzBento>

@code {
    private Company? company;
    private void OnCompanyChanged(Company c) => company = c;
}
```

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

## License

MIT License - see [LICENSE](LICENSE.txt) for details.

## Author

- GitHub: [@nenad0707](https://github.com/nenad0707)
- LinkedIn: [Nenad Ristic](https://www.linkedin.com/in/nenad-risti%C4%87-27459958/)

---

**Part of the [BlazzyMotion](https://github.com/Blazzy-Motion/BlazzyMotion) component ecosystem.**
