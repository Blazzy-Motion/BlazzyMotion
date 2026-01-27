# BlazzyMotion.Bento

A modern Bento Grid component for Blazor with zero-configuration support through Source Generators and glassmorphism design.

[![NuGet](https://img.shields.io/nuget/v/BlazzyMotion.Bento.svg)](https://www.nuget.org/packages/BlazzyMotion.Bento/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/BlazzyMotion.Bento.svg)](https://www.nuget.org/packages/BlazzyMotion.Bento/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.txt)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Blazzy-Motion_BlazzyMotion&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Blazzy-Motion_BlazzyMotion)

## Table of Contents

- [BlazzyMotion.Bento](#blazzymotionbento)
  - [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Live Demo](#live-demo)
  - [Quick Start](#quick-start)
    - [Installation](#installation)
    - [Basic Usage](#basic-usage)
      - [1. Define Your Model](#1-define-your-model)
      - [2. Use the Component](#2-use-the-component)
  - [How It Works](#how-it-works)
    - [Source Generator Magic](#source-generator-magic)
  - [Usage Modes](#usage-modes)
    - [Items Mode (Zero Config)](#items-mode-zero-config)
    - [Composition Mode](#composition-mode)
    - [Interactive Dashboard Mode](#interactive-dashboard-mode)
  - [Built-in Components](#built-in-components)
    - [BzBentoItem](#bzbentoitem)
    - [BzBentoCard](#bzbentocard)
    - [BzBentoMetric](#bzbentometric)
    - [BzBentoFeature](#bzbentofeature)
    - [BzBentoQuote](#bzbentoquote)
  - [API Reference](#api-reference)
    - [BzBento Component Parameters](#bzbento-component-parameters)
      - [Data Parameters](#data-parameters)
      - [Layout Parameters](#layout-parameters)
      - [Animation Parameters](#animation-parameters)
      - [Event Parameters](#event-parameters)
      - [Template Parameters](#template-parameters)
      - [Advanced Parameters](#advanced-parameters)
    - [Child Component Parameters](#child-component-parameters)
      - [BzBentoCard Parameters](#bzbentocard-parameters)
      - [BzBentoMetric Parameters](#bzbentometric-parameters)
      - [BzBentoFeature Parameters](#bzbentofeature-parameters)
      - [BzBentoQuote Parameters](#bzbentoquote-parameters)
      - [BzBentoItem Parameters](#bzbentoitem-parameters)
  - [Attributes](#attributes)
    - [BzImageAttribute](#bzimageattribute)
    - [BzTitleAttribute](#bztitleattribute)
    - [BzDescriptionAttribute](#bzdescriptionattribute)
  - [Themes](#themes)
    - [Glass Theme (Default)](#glass-theme-default)
    - [Dark Theme](#dark-theme)
    - [Light Theme](#light-theme)
    - [Minimal Theme](#minimal-theme)
  - [Advanced Usage](#advanced-usage)
    - [Custom Item Template](#custom-item-template)
    - [Handling Item Selection](#handling-item-selection)
    - [Custom Grid Spans](#custom-grid-spans)
    - [Icon Support](#icon-support)
    - [Advanced Configuration](#advanced-configuration)
    - [Custom Loading State](#custom-loading-state)
    - [Custom Empty State](#custom-empty-state)
  - [Customization](#customization)
    - [CSS Variables](#css-variables)
  - [Responsive Design](#responsive-design)
  - [Performance](#performance)
    - [Performance Characteristics](#performance-characteristics)
  - [Browser Support](#browser-support)
  - [Examples](#examples)
    - [Dashboard with Metrics](#dashboard-with-metrics)
    - [Product Showcase](#product-showcase)
    - [Company Data with Carousel](#company-data-with-carousel)
  - [Troubleshooting](#troubleshooting)
    - [Template Not Generated](#template-not-generated)
    - [Grid Items Not Visible](#grid-items-not-visible)
    - [Animations Not Working](#animations-not-working)
  - [Contributing](#contributing)
    - [Building from Source](#building-from-source)
    - [Running Tests](#running-tests)
  - [License](#license)
  - [Author](#author)
  - [Acknowledgments](#acknowledgments)
  - [Support](#support)

## Features

- **Zero Configuration**: Use Source Generators to automatically create item templates from your data models
- **Composition Mode**: Embed any content including BzCarousel and other BlazzyMotion components
- **Built-in Components**: BzBentoCard, BzBentoMetric, BzBentoFeature, and BzBentoQuote for rapid development
- **Staggered Animations**: Intersection Observer-powered entrance animations at 60 FPS
- **Multiple Themes**: Glass, Dark, Light, and Minimal themes with consistent BlazzyMotion design
- **CSS Grid Powered**: Native CSS Grid with flexible column and row spanning
- **Fully Customizable**: Extensive API for fine-tuning appearance and behavior
- **Type-Safe**: Strongly-typed generic component with full IntelliSense support
- **Responsive Design**: Automatic column reduction on tablet and mobile devices
- **Performance Optimized**: Template caching and incremental source generation for minimal overhead

## Live Demo

Experience BlazzyMotion.Bento in action: **[View Live Demo](https://blazzymotion.com/bento)**

## Quick Start

### Installation

Install the package via NuGet:

```bash
dotnet add package BlazzyMotion.Bento
```

Or via the Package Manager Console:

```powershell
Install-Package BlazzyMotion.Bento
```

### Basic Usage

#### 1. Define Your Model

Mark your data model with the `[BzImage]` attribute to specify which property contains the image URL:

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

#### 2. Use the Component

That's it! No need to define an `ItemTemplate`:

```razor
@page "/movies"
@using BlazzyMotion.Bento.Components
@using BlazzyMotion.Core.Models

<BzBento TItem="Movie"
         Items="movies"
         Theme="BzTheme.Glass"
         Columns="4" />

@code {
    private List<Movie> movies = new()
    {
        new Movie { ImageUrl = "images/movie1.jpg", Title = "Inception" },
        new Movie { ImageUrl = "images/movie2.jpg", Title = "Interstellar" },
        new Movie { ImageUrl = "images/movie3.jpg", Title = "The Dark Knight" }
    };
}
```

The Source Generator automatically creates the template for you at compile-time.

## How It Works

### Source Generator Magic

When you mark a property with `[BzImage]`, the BlazzyMotion Source Generator automatically creates a registration function during compilation:

```csharp
// Auto-generated at compile-time
internal static class BzMappingRegistration_Movie
{
    [ModuleInitializer]
    internal static void Register()
    {
        BzRegistry.Register<Movie>(item => new BzItem
        {
            ImageUrl = item.ImageUrl,
            Title = item.Title,
            OriginalItem = item
        });
    }
}
```

The `[ModuleInitializer]` attribute ensures this registration runs automatically at application startup. BzBento then uses the registered mapper from `BzRegistry` to render your items with zero reflection and zero configuration.

## Usage Modes

BzBento supports three distinct usage modes to fit different scenarios.

### Items Mode (Zero Config)

The simplest mode - just provide a collection of items:

```razor
<BzBento TItem="DashboardItem"
         Items="items"
         Theme="BzTheme.Glass"
         Columns="4"
         Gap="16"
         OnItemSelected="HandleItemClick" />
```

All items are rendered as uniform 1x1 grid cells using auto-generated templates.

### Composition Mode

For complete control over layout, use ChildContent with built-in Bento components:

```razor
<BzBento TItem="object" Theme="BzTheme.Glass" Columns="4" Gap="16">

    <BzBentoCard TItem="object"
                 ColSpan="2" RowSpan="2"
                 Image="images/hero.jpg"
                 Title="Featured"
                 Description="Hero card with 2x2 span" />

    <BzBentoMetric Value="1,234" Label="Users">
        <Icon>@Icons.Users</Icon>
    </BzBentoMetric>

    <BzBentoMetric Value="$45K" Label="Revenue" Trend="+12%">
        <Icon>@Icons.Revenue</Icon>
    </BzBentoMetric>

    <BzBentoFeature ColSpan="2"
                    Label="Lightning Fast"
                    Description="Built for performance">
        <Icon>@Icons.Zap</Icon>
    </BzBentoFeature>

    <BzBentoQuote TItem="object"
                  Text="Amazing UI components!"
                  Author="Dev Community"
                  Role="GitHub Stars"
                  Avatar="images/avatar.webp" />

</BzBento>
```

### Interactive Dashboard Mode

Combine BzBento with BzCarousel for dynamic dashboards that update in real-time:

```razor
<BzBento TItem="object" Theme="BzTheme.Glass" Columns="4" Gap="16">

    <!-- Carousel in 2x2 cell -->
    <BzBentoItem ColSpan="2" RowSpan="2">
        <BzCarousel TItem="CompanyData"
                    Items="companies"
                    Theme="BzTheme.Glass"
                    OnItemSelected="OnCompanyChanged" />
    </BzBentoItem>

    <!-- Dynamic metrics that update when carousel selection changes -->
    <BzBentoMetric Value="@(activeCompany?.Revenue ?? "$0")"
                   Label="Revenue"
                   Trend="@(activeCompany?.RevenueTrend ?? "")">
        <Icon>@Icons.Revenue</Icon>
    </BzBentoMetric>

    <BzBentoMetric Value="@(activeCompany?.Users ?? "0")"
                   Label="Users"
                   Trend="@(activeCompany?.UsersTrend ?? "")">
        <Icon>@Icons.Users</Icon>
    </BzBentoMetric>

    <BzBentoMetric Value="@(activeCompany?.Growth ?? "0%")"
                   Label="Growth"
                   Trend="@(activeCompany?.GrowthTrend ?? "")">
        <Icon>@Icons.Growth</Icon>
    </BzBentoMetric>

</BzBento>

@code {
    private CompanyData? activeCompany;

    private void OnCompanyChanged(CompanyData company)
    {
        activeCompany = company;
    }
}
```

## Built-in Components

BzBento includes five pre-styled components for common Bento grid use cases. All components inherit from `BzBentoItemBase` and support `ColSpan` and `RowSpan` parameters.

### BzBentoItem

Base container for custom content. Perfect for embedding any Blazor component.

```razor
<BzBentoItem ColSpan="2" RowSpan="2">
    <BzCarousel Items="movies" Theme="BzTheme.Glass" />
</BzBentoItem>
```

### BzBentoCard

Image card with title and description overlay. Supports both manual values and automatic mapping via `Item` parameter.

```razor
<!-- Manual values -->
<BzBentoCard TItem="object"
             ColSpan="2" RowSpan="2"
             Image="images/featured.jpg"
             Title="Featured Item"
             Description="Optional description" />

<!-- Auto-mapping from Item -->
<BzBentoCard Item="product" ColSpan="2" RowSpan="2" />
```

### BzBentoMetric

Metric/statistic display with optional icon and trend indicator. Trend values starting with `+` display in green, values starting with `-` display in red.

```razor
<BzBentoMetric Value="1,234" Label="Active Users" Trend="+12%">
    <Icon>
        <svg><!-- your icon --></svg>
    </Icon>
</BzBentoMetric>

<!-- Or use IconText for emoji -->
<BzBentoMetric Value="$48.5K" Label="Revenue" IconText="💰" />
```

### BzBentoFeature

Feature highlight with icon, label, and optional description. Supports any icon library through RenderFragment.

```razor
<BzBentoFeature ColSpan="2"
                Label="Real-time Updates"
                Description="Powered by SignalR">
    <Icon>
        <i class="bi bi-lightning-charge"></i>
    </Icon>
</BzBentoFeature>

<!-- Or use IconText for emoji -->
<BzBentoFeature Label="Secure" Description="Enterprise-grade" IconText="🔒" />
```

### BzBentoQuote

Testimonial or quote display with author info and optional avatar.

```razor
<!-- Manual values -->
<BzBentoQuote TItem="object"
              Text="This component library is fantastic!"
              Author="John Doe"
              Role="Senior Developer"
              Avatar="images/john.jpg"
              ColSpan="2" />

<!-- Auto-mapping from Item -->
<BzBentoQuote Item="testimonial" ColSpan="2" />
```

## API Reference

### BzBento Component Parameters

#### Data Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Items` | `IEnumerable<TItem>?` | `null` | Collection of items to display in Items mode |
| `ChildContent` | `RenderFragment?` | `null` | Child content for Composition mode (uses BzBentoCard, BzBentoMetric, etc.) |
| `ItemTemplate` | `RenderFragment<TItem>?` | `null` | Custom template for rendering items (overrides generated template) |

#### Layout Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Columns` | `int` | `4` | Number of grid columns (1-12). Responsive: Desktop uses this value, Tablet uses 2, Mobile uses 1 |
| `Gap` | `int` | `16` | Gap between items in pixels |

#### Animation Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `AnimationEnabled` | `bool` | `true` | Enable staggered entrance animations using Intersection Observer |
| `StaggerDelay` | `int` | `50` | Delay between each item's animation in milliseconds (0-1000) |

#### Event Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `OnItemSelected` | `EventCallback<TItem>` | Callback invoked when an item is clicked |

#### Template Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `LoadingTemplate` | `RenderFragment?` | `null` | Custom loading state template |
| `EmptyTemplate` | `RenderFragment?` | `null` | Custom empty state template |

#### Advanced Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Options` | `BzBentoOptions?` | `null` | Advanced configuration options (overrides individual parameters) |
| `Theme` | `BzTheme` | `Glass` | Visual theme: `Glass`, `Dark`, `Light`, or `Minimal` |
| `CssClass` | `string?` | `null` | Additional CSS classes for customization |
| `AdditionalAttributes` | `Dictionary<string, object>?` | `null` | HTML attributes to splat onto root element (id, aria-*, data-*) |

### Child Component Parameters

#### BzBentoCard Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Item` | `TItem?` | `null` | Item to auto-map via BzRegistry |
| `Image` | `string?` | `null` | Override image URL (or use [BzImage] from Item) |
| `Title` | `string?` | `null` | Override title (or use [BzTitle] from Item) |
| `Description` | `string?` | `null` | Override description (or use [BzDescription] from Item) |
| `ColSpan` | `int` | `1` | Number of columns to span (1-4) |
| `RowSpan` | `int` | `1` | Number of rows to span (1-4) |

#### BzBentoMetric Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Value` | `string` | **Required** | Main metric value to display (e.g., "1,234", "$48.5K", "99.9%") |
| `Label` | `string` | **Required** | Label describing the metric |
| `Trend` | `string?` | `null` | Trend indicator (e.g., "+12%", "-5%"). Positive values show green, negative show red |
| `Icon` | `RenderFragment?` | `null` | Icon content (supports any HTML/Blazor component) |
| `IconText` | `string?` | `null` | Simple text icon (emoji or text). Use Icon for complex icons |
| `ColSpan` | `int` | `1` | Number of columns to span (1-4) |
| `RowSpan` | `int` | `1` | Number of rows to span (1-4) |

#### BzBentoFeature Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Label` | `string` | **Required** | Feature name/title |
| `Description` | `string?` | `null` | Optional feature description |
| `Icon` | `RenderFragment?` | `null` | Icon content (supports Bootstrap Icons, Font Awesome, MudBlazor, etc.) |
| `IconText` | `string?` | `null` | Simple text icon (emoji or text). Use Icon for complex icons |
| `ColSpan` | `int` | `1` | Number of columns to span (1-4) |
| `RowSpan` | `int` | `1` | Number of rows to span (1-4) |

#### BzBentoQuote Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Item` | `TItem?` | `null` | Item to auto-map via BzRegistry |
| `Text` | `string?` | `null` | Override quote text (or use [BzDescription] from Item) |
| `Author` | `string?` | `null` | Override author name (or use [BzTitle] from Item) |
| `Role` | `string?` | `null` | Author's role or title (e.g., "CEO, TechCorp") |
| `Avatar` | `string?` | `null` | Override avatar URL (or use [BzImage] from Item) |
| `ColSpan` | `int` | `1` | Number of columns to span (1-4) |
| `RowSpan` | `int` | `1` | Number of rows to span (1-4) |

#### BzBentoItem Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ChildContent` | `RenderFragment?` | `null` | Content to render inside the grid item |
| `ColSpan` | `int` | `1` | Number of columns to span (1-4) |
| `RowSpan` | `int` | `1` | Number of rows to span (1-4) |

## Attributes

### BzImageAttribute

Marks a property as the image source for Bento items. The Source Generator creates a default template using this property.

**Requirements:**
- Property must be `public`
- Property must be of type `string`
- Only one property per class should have this attribute

```csharp
[BzImage]
public string ImageUrl { get; set; }
```

### BzTitleAttribute

Marks a property as the title for Bento items. Used for accessibility attributes and text overlays.

```csharp
[BzTitle]
public string Name { get; set; }
```

### BzDescriptionAttribute

Marks a property as the description for Bento items. Used for text overlays and quote text in BzBentoQuote.

```csharp
[BzDescription]
public string Description { get; set; }
```

## Themes

BlazzyMotion.Bento includes four professionally designed themes that match the BlazzyMotion design system:

### Glass Theme (Default)

Modern glassmorphism design with blur effect and transparency:

```razor
<BzBento Items="items" Theme="BzTheme.Glass" />
```

### Dark Theme

Solid dark background with subtle gradients:

```razor
<BzBento Items="items" Theme="BzTheme.Dark" />
```

### Light Theme

Clean light theme with soft shadows:

```razor
<BzBento Items="items" Theme="BzTheme.Light" />
```

### Minimal Theme

No background container, pure grid:

```razor
<BzBento Items="items" Theme="BzTheme.Minimal" />
```

## Advanced Usage

### Custom Item Template

If you need full control over item rendering, provide a custom template:

```razor
<BzBento Items="products">
    <ItemTemplate Context="product">
        <div class="custom-card">
            <img src="@product.ImageUrl" alt="@product.Name" />
            <h3>@product.Name</h3>
            <p class="price">$@product.Price</p>
        </div>
    </ItemTemplate>
</BzBento>
```

### Handling Item Selection

React to user clicks on Bento items:

```razor
<BzBento Items="movies" OnItemSelected="HandleMovieClick" />

@code {
    private void HandleMovieClick(Movie movie)
    {
        Console.WriteLine($"Selected: {movie.Title}");
        // Navigate, open modal, etc.
    }
}
```

### Custom Grid Spans

Control how many columns and rows each item occupies:

```razor
<BzBento TItem="object" Theme="BzTheme.Glass" Columns="4">
    <!-- Takes 2 columns, 2 rows -->
    <BzBentoCard TItem="object"
                 ColSpan="2" RowSpan="2"
                 Image="hero.jpg"
                 Title="Featured" />

    <!-- Takes 1 column, 1 row (default) -->
    <BzBentoMetric Value="100" Label="Score" />

    <!-- Takes 2 columns, 1 row -->
    <BzBentoFeature ColSpan="2"
                    Label="Wide Card"
                    Description="Spans 2 columns" />
</BzBento>
```

### Icon Support

Built-in components support both simple text icons and complex RenderFragment icons:

```razor
<!-- Simple emoji icon -->
<BzBentoMetric Value="99.9%" Label="Uptime" IconText="⚡" />

<!-- Bootstrap Icons -->
<BzBentoFeature Label="Secure">
    <Icon>
        <i class="bi bi-shield-check"></i>
    </Icon>
</BzBentoFeature>

<!-- Font Awesome -->
<BzBentoMetric Value="24/7" Label="Support">
    <Icon>
        <i class="fa-solid fa-headset"></i>
    </Icon>
</BzBentoMetric>

<!-- MudBlazor Icons -->
<BzBentoFeature Label="Settings">
    <Icon>
        <MudIcon Icon="@Icons.Material.Filled.Settings" />
    </Icon>
</BzBentoFeature>

<!-- Custom SVG -->
<BzBentoMetric Value="$124K" Label="Revenue">
    <Icon>
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor">
            <line x1="12" y1="1" x2="12" y2="23"></line>
            <path d="M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"></path>
        </svg>
    </Icon>
</BzBentoMetric>
```

### Advanced Configuration

Use `BzBentoOptions` for programmatic configuration:

```razor
<BzBento Items="items" Options="customOptions" />

@code {
    private BzBentoOptions customOptions = new()
    {
        Columns = 3,
        Gap = 20,
        AnimationEnabled = true,
        StaggerDelay = 75
    };
}
```

### Custom Loading State

Provide a custom loading template:

```razor
<BzBento Items="asyncItems">
    <LoadingTemplate>
        <div class="custom-loader">
            <span>Loading dashboard...</span>
        </div>
    </LoadingTemplate>
</BzBento>
```

### Custom Empty State

Handle empty data gracefully:

```razor
<BzBento Items="emptyList">
    <EmptyTemplate>
        <div class="no-data">
            <p>No items available at this time.</p>
        </div>
    </EmptyTemplate>
</BzBento>
```

## Customization

### CSS Variables

BlazzyMotion.Bento uses CSS custom properties for easy customization:

```css
:root {
  /* Grid Layout */
  --bzb-columns: 4;
  --bzb-gap: 16px;

  /* Colors */
  --bzb-bg: rgba(255, 255, 255, 0.03);
  --bzb-border: rgba(255, 255, 255, 0.08);
  --bzb-text-primary: rgba(255, 255, 255, 0.95);
  --bzb-text-secondary: rgba(255, 255, 255, 0.7);
  --bzb-text-muted: rgba(255, 255, 255, 0.5);

  /* Animation */
  --bzb-stagger-delay: 50ms;
  --bzb-transition-duration: 0.3s;

  /* Card Styling */
  --bzb-card-radius: 16px;
  --bzb-card-padding: 24px;
}
```

Override these in your app's CSS:

```css
.my-custom-bento {
  --bzb-columns: 3;
  --bzb-gap: 24px;
  --bzb-card-radius: 12px;
  --bzb-bg: rgba(100, 50, 200, 0.1);
}
```

```razor
<BzBento Items="items" CssClass="my-custom-bento" />
```

## Responsive Design

BlazzyMotion.Bento automatically adapts to different screen sizes:

| Breakpoint | Columns | Gap | Description |
|------------|---------|-----|-------------|
| Desktop (>991px) | As configured | As configured | Uses your specified values |
| Tablet (≤991px) | 2 | 12px | Automatically reduces to 2 columns |
| Mobile (≤600px) | 1 | 8px | Forces single column layout |

You can override responsive behavior via CSS variables in media queries.

## Performance

### Performance Characteristics

- **Zero Runtime Overhead**: Mapping functions are generated at compile-time
- **Zero Reflection**: Uses `[ModuleInitializer]` for automatic registration at app startup
- **Type Safety**: Full compile-time checking of property names and types
- **O(1) Lookup**: Dictionary-based mapper lookup per type
- **Compiled Delegates**: Mapping functions are compiled, not interpreted
- **Efficient Animations**: Intersection Observer API for performant entrance animations
- **CSS Grid Native**: Leverages browser-optimized CSS Grid layout engine

The entire system is designed for maximum performance with no runtime code generation or reflection.

## Browser Support

BlazzyMotion.Bento supports all modern browsers:

| Browser | Version | CSS Grid | Intersection Observer | Backdrop Filter |
|---------|---------|----------|----------------------|-----------------|
| Chrome | 88+ | ✓ | ✓ | ✓ |
| Firefox | 78+ | ✓ | ✓ | ✓ |
| Safari | 14+ | ✓ | ✓ | ✓ |
| Edge | 88+ | ✓ | ✓ | ✓ |

**Requirements:**
- CSS Grid support (for layout)
- Intersection Observer API (for animations, graceful fallback if unavailable)
- Backdrop-filter support (for Glass theme, graceful fallback on older browsers)

## Examples

### Dashboard with Metrics

```razor
<BzBento TItem="object" Theme="BzTheme.Glass" Columns="4" Gap="16">
    <BzBentoMetric Value="$124K" Label="Revenue" Trend="+23%">
        <Icon>@Icons.Revenue</Icon>
    </BzBentoMetric>

    <BzBentoMetric Value="8,432" Label="Users" Trend="+12%">
        <Icon>@Icons.Users</Icon>
    </BzBentoMetric>

    <BzBentoMetric Value="99.9%" Label="Uptime">
        <Icon>@Icons.Uptime</Icon>
    </BzBentoMetric>

    <BzBentoMetric Value="24ms" Label="Response">
        <Icon>@Icons.Performance</Icon>
    </BzBentoMetric>

    <BzBentoFeature ColSpan="2"
                    Label="Q4 Report"
                    Description="Revenue exceeded targets by 15%">
        <Icon>@Icons.Chart</Icon>
    </BzBentoFeature>

    <BzBentoCard TItem="object"
                 ColSpan="2" RowSpan="2"
                 Image="images/growth-chart.png"
                 Title="Growth Chart"
                 Description="Monthly trends" />
</BzBento>
```

### Product Showcase

```razor
@page "/products"
@using BlazzyMotion.Bento.Components
@using BlazzyMotion.Core.Models

<BzBento TItem="Product"
         Items="products"
         Theme="BzTheme.Glass"
         Columns="4"
         OnItemSelected="ViewProductDetails" />

@code {
    private List<Product> products = new()
    {
        new() { ImageUrl = "product1.jpg", Title = "Product 1" },
        new() { ImageUrl = "product2.jpg", Title = "Product 2" },
        new() { ImageUrl = "product3.jpg", Title = "Product 3" },
        // ... more products
    };

    private void ViewProductDetails(Product product)
    {
        Navigation.NavigateTo($"/product/{product.Id}");
    }
}
```

### Company Data with Carousel

```razor
@page "/companies"
@using BlazzyMotion.Bento.Components
@using BlazzyMotion.Carousel.Components
@using BlazzyMotion.Core.Models

<BzBento TItem="object" Theme="BzTheme.Glass" Columns="4" Gap="16">

    <!-- Company Carousel in 2x2 cell -->
    <BzBentoItem ColSpan="2" RowSpan="2">
        <BzCarousel TItem="CompanyData"
                    Items="companies"
                    Theme="BzTheme.Glass"
                    OnItemSelected="OnCompanyChanged" />
    </BzBentoItem>

    <!-- Company Name with dynamic logo -->
    <BzBentoMetric Value="@(activeCompany?.Name ?? "Select")"
                   Label="Company">
        <Icon>@GetCompanyLogo(activeCompany?.Name)</Icon>
    </BzBentoMetric>

    <!-- Dynamic Revenue -->
    <BzBentoMetric Value="@(activeCompany?.Revenue ?? "$0")"
                   Label="Revenue"
                   Trend="@(activeCompany?.RevenueTrend ?? "")">
        <Icon>@Icons.Revenue</Icon>
    </BzBentoMetric>

    <!-- Dynamic Users -->
    <BzBentoMetric Value="@(activeCompany?.Users ?? "0")"
                   Label="Users"
                   Trend="@(activeCompany?.UsersTrend ?? "")">
        <Icon>@Icons.Users</Icon>
    </BzBentoMetric>

    <!-- Dynamic Growth -->
    <BzBentoMetric Value="@(activeCompany?.Growth ?? "0%")"
                   Label="Growth"
                   Trend="@(activeCompany?.GrowthTrend ?? "")">
        <Icon>@Icons.Growth</Icon>
    </BzBentoMetric>

    <!-- Company Description -->
    <BzBentoFeature ColSpan="2"
                    Label="@(activeCompany?.Name ?? "Company")"
                    Description="@(activeCompany?.Description ?? "Swipe to see details")">
        <Icon>@GetCompanyLogo(activeCompany?.Name)</Icon>
    </BzBentoFeature>

    <!-- More metrics -->
    <BzBentoMetric Value="@(activeCompany?.Rating ?? "0")"
                   Label="Rating">
        <Icon>@Icons.Star</Icon>
    </BzBentoMetric>

    <BzBentoMetric Value="@(activeCompany?.MarketCap ?? "$0")"
                   Label="Market Cap">
        <Icon>@Icons.MarketCap</Icon>
    </BzBentoMetric>

</BzBento>

@code {
    private CompanyData? activeCompany;
    private List<CompanyData> companies = new()
    {
        new()
        {
            Name = "Microsoft",
            ImageUrl = "companies/microsoft.webp",
            Revenue = "$211B",
            RevenueTrend = "+12%",
            Users = "1.4B",
            UsersTrend = "+18%",
            Growth = "22%",
            GrowthTrend = "+5%",
            Rating = "4.6",
            MarketCap = "$2.8T",
            Description = "Cloud computing, AI, and enterprise software leader"
        },
        // ... more companies
    };

    protected override void OnInitialized()
    {
        activeCompany = companies.FirstOrDefault();
    }

    private void OnCompanyChanged(CompanyData company)
    {
        activeCompany = company;
    }

    private RenderFragment GetCompanyLogo(string? name)
    {
        // Return appropriate logo SVG or icon based on company name
        return builder => { /* SVG markup */ };
    }
}
```

## Troubleshooting

### Template Not Generated

**Problem**: Component shows fallback template instead of generated one.

**Solution**:

1. Ensure `[BzImage]` attribute is applied to a `public string` property
2. Rebuild the project to trigger Source Generator
3. Check build output for any BZC001 or BZC002 errors
4. Verify you have `@using BlazzyMotion.Core.Attributes` in your file or `_Imports.razor`

### Grid Items Not Visible

**Problem**: Grid container is present but items are not visible.

**Solution**:

1. Check that `Items` collection is not null or empty
2. Verify image URLs are valid and accessible
3. Inspect browser console for JavaScript errors
4. Ensure you have the correct `@using BlazzyMotion.Bento.Components` directive

### Animations Not Working

**Problem**: Items appear instantly without staggered animation.

**Solution**:

1. Check that `AnimationEnabled="true"` (it's true by default)
2. Verify your browser supports Intersection Observer API
3. Check browser console for JavaScript errors
4. Try adjusting `StaggerDelay` to a higher value (e.g., 100ms)

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

This project is licensed under the MIT License - see the [LICENSE](LICENSE.txt) file for details.

## Author

- GitHub: [@nenad0707](https://github.com/nenad0707)
- LinkedIn: [Nenad Ristic](https://www.linkedin.com/in/nenad-risti%C4%87-27459958/)

## Acknowledgments

- Built with CSS Grid for native, performant layouts
- Intersection Observer API for efficient entrance animations
- Inspired by modern dashboard designs and Bento UI patterns
- Thanks to the Blazor community for feedback and support

## Support

If you find BlazzyMotion.Bento useful, please consider:

- Giving it a star on GitHub
- Sharing it with other Blazor developers
- Reporting bugs or suggesting features via GitHub Issues

For questions or support, please open an issue on GitHub.

---

**Part of the [BlazzyMotion](https://blazzymotion.com) component ecosystem.**
