# BlazzyMotion.Bento

A modern Bento Grid component for Blazor with glassmorphism design, staggered animations, and automatic layout patterns.

[![NuGet](https://img.shields.io/nuget/v/BlazzyMotion.Bento.svg)](https://www.nuget.org/packages/BlazzyMotion.Bento/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/BlazzyMotion.Bento.svg)](https://www.nuget.org/packages/BlazzyMotion.Bento/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.txt)

## Table of Contents

- [BlazzyMotion.Bento](#blazzymotionbento)
  - [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Live Demo](#live-demo)
  - [Quick Start](#quick-start)
    - [Installation](#installation)
    - [Basic Usage](#basic-usage)
  - [Usage Modes](#usage-modes)
    - [Static Mode (Data-Driven)](#static-mode-data-driven)
    - [Composition Mode](#composition-mode)
    - [Interactive Mode](#interactive-mode)
    - [Paginated Mode](#paginated-mode)
  - [Layout Patterns](#layout-patterns)
    - [Dynamic Pattern (Default)](#dynamic-pattern-default)
    - [Featured Pattern](#featured-pattern)
    - [Balanced Pattern](#balanced-pattern)
    - [Manual Layout](#manual-layout)
  - [Built-in Components](#built-in-components)
    - [BzBentoItem](#bzbentoitem)
    - [BzBentoCard](#bzbentocard)
    - [BzBentoMetric](#zzbentometric)
    - [BzBentoFeature](#bzbentofeature)
    - [BzBentoQuote](#zzbentoquote)
  - [API Reference](#api-reference)
    - [Component Parameters](#component-parameters)
      - [Data Parameters](#data-parameters)
      - [Layout Parameters](#layout-parameters)
      - [Animation Parameters](#animation-parameters)
      - [Pagination Parameters](#pagination-parameters)
      - [Event Parameters](#event-parameters)
      - [Template Parameters](#template-parameters)
      - [Advanced Parameters](#advanced-parameters)
  - [Themes](#themes)
    - [Glass Theme (Default)](#glass-theme-default)
    - [Dark Theme](#dark-theme)
    - [Light Theme](#light-theme)
    - [Minimal Theme](#minimal-theme)
  - [CSS Variables](#css-variables)
  - [Responsive Design](#responsive-design)
  - [Examples](#examples)
    - [Dashboard Layout](#dashboard-layout)
    - [Product Showcase](#product-showcase)
    - [Metrics Dashboard](#metrics-dashboard)
  - [Browser Support](#browser-support)
  - [License](#license)

## Features

- **Zero Configuration**: Use Source Generators to automatically create item templates from your data models
- **Multiple Layout Patterns**: Dynamic, Featured, and Balanced patterns for automatic visual variety
- **Composition Mode**: Embed any content including BzCarousel and other BlazzyMotion components
- **Built-in Components**: BzBentoCard, BzBentoMetric, BzBentoFeature, and BzBentoQuote for rapid development
- **Staggered Animations**: Intersection Observer-powered entrance animations at 60 FPS
- **Multiple Themes**: Glass, Dark, Light, and Minimal themes with consistent BlazzyMotion design
- **Pagination Support**: Swipeable pages with touch gestures for large datasets
- **Responsive Design**: Automatic column reduction on tablet and mobile
- **CSS Grid Powered**: Native CSS Grid with dense auto-flow for optimal item placement

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

Mark your data model with `[BzImage]` and optional `[BzTitle]` attributes:

```csharp
using BlazzyMotion.Core.Attributes;

public class DashboardItem
{
    [BzImage]
    public string ImageUrl { get; set; } = "";

    [BzTitle]
    public string Title { get; set; } = "";

    [BzDescription]
    public string Description { get; set; } = "";
}
```

#### 2. Use the Component

```razor
@page "/dashboard"
@using BlazzyMotion.Bento.Components
@using BlazzyMotion.Core.Models

<BzBento TItem="DashboardItem"
         Items="@items"
         Theme="BzTheme.Glass" />

@code {
    private List<DashboardItem> items = new()
    {
        new() { Title = "Analytics", ImageUrl = "images/analytics.jpg" },
        new() { Title = "Reports", ImageUrl = "images/reports.jpg" },
        new() { Title = "Settings", ImageUrl = "images/settings.jpg" },
        // ... more items
    };
}
```

The Source Generator automatically creates the template at compile-time.

## Usage Modes

BzBento supports four distinct usage modes to fit different scenarios.

### Static Mode (Data-Driven)

The simplest mode - just provide a collection of items:

```razor
<BzBento TItem="DashboardItem"
         Items="@items"
         Theme="BzTheme.Glass"
         Columns="4"
         Gap="16"
         OnItemSelected="@HandleItemClick" />
```

Items are automatically rendered with layouts based on the selected `Pattern`.

### Composition Mode

For complete control, use ChildContent with built-in Bento components:

```razor
<BzBento TItem="object" Items="null" Theme="BzTheme.Glass" Columns="4" Gap="16">

    <BzBentoCard ColSpan="2" RowSpan="2"
                 Image="images/hero.jpg"
                 Title="Featured"
                 Description="Hero card with 2x2 span" />

    <BzBentoMetric Value="1,234" Label="Users" IconText="👥" />

    <BzBentoMetric Value="$45K" Label="Revenue" IconText="💰" Trend="+12%" />

    <BzBentoFeature ColSpan="2"
                    Label="New Feature"
                    Description="Announcing our latest release"
                    IconText="🚀" />

</BzBento>
```

### Interactive Mode

Combine BzBento with BzCarousel for dynamic dashboards:

```razor
<BzBento TItem="object" Items="null" Theme="BzTheme.Glass" Columns="4" Gap="16">

    @* Carousel in 2x2 cell *@
    <BzBentoItem ColSpan="2" RowSpan="2">
        <BzCarousel TItem="Company"
                    Items="@companies"
                    Theme="BzTheme.Glass"
                    OnItemSelected="@OnCompanyChanged" />
    </BzBentoItem>

    @* Dynamic metrics that update when carousel selection changes *@
    <BzBentoMetric Value="@activeCompany.Revenue" Label="Revenue" IconText="💰" />
    <BzBentoMetric Value="@activeCompany.Users" Label="Users" IconText="👥" />
    <BzBentoMetric Value="@activeCompany.Growth" Label="Growth" IconText="📈" />

</BzBento>

@code {
    private Company activeCompany = companies.First();

    private void OnCompanyChanged(Company company) => activeCompany = company;
}
```

### Paginated Mode

For large datasets, enable pagination with swipe support:

```razor
<BzBento TItem="Product"
         Items="@products"
         Theme="BzTheme.Glass"
         Paginated="true"
         ItemsPerPage="6"
         OnPageChanged="@HandlePageChange" />

@code {
    private void HandlePageChange(int pageIndex)
    {
        Console.WriteLine($"Page changed to: {pageIndex}");
    }
}
```

## Layout Patterns

When `AutoLayout="true"` (default), items automatically receive ColSpan and RowSpan values based on the selected pattern.

### Dynamic Pattern (Default)

Creates visual interest with varied card sizes:

```razor
<BzBento Items="@items" Pattern="BentoLayoutPattern.Dynamic" />
```

| Position | Size | Description |
|----------|------|-------------|
| Item 0 | 2x2 | Hero card |
| Item 1 | 1x2 | Tall card |
| Item 2-3 | 1x1 | Normal cards |
| Item 4 | 2x1 | Wide card |
| Item 5-6 | 1x1 | Normal cards |
| Item 7 | 2x1 | Wide card |

Pattern repeats for additional items.

### Featured Pattern

One prominent hero card with supporting content:

```razor
<BzBento Items="@items" Pattern="BentoLayoutPattern.Featured" />
```

| Position | Size | Description |
|----------|------|-------------|
| Item 0 | 2x2 | Hero (featured) |
| Every 3rd | 1x2 | Tall accent |
| Others | 1x1 | Normal cards |

Best for: Blog posts, product showcases, content feeds.

### Balanced Pattern

More uniform appearance without large hero cards:

```razor
<BzBento Items="@items" Pattern="BentoLayoutPattern.Balanced" />
```

| Position | Size | Description |
|----------|------|-------------|
| Items 0, 4 | 2x1 | Wide cards |
| Item 3 | 1x2 | Tall card |
| Others | 1x1 | Normal cards |

Best for: Metric dashboards, stat displays, uniform content.

### Manual Layout

Disable auto-layout for complete control:

```razor
<BzBento Items="@items" AutoLayout="false" />
```

Or set explicit spans in your model:

```csharp
public class DashboardItem
{
    [BzImage]
    public string ImageUrl { get; set; } = "";

    public int ColSpan { get; set; } = 1;
    public int RowSpan { get; set; } = 1;
}
```

## Built-in Components

BzBento includes several pre-styled components for common Bento grid use cases.

### BzBentoItem

Base container for custom content:

```razor
<BzBentoItem ColSpan="2" RowSpan="1" Style="padding: 20px;">
    <h3>Custom Content</h3>
    <p>Any HTML or Blazor components</p>
</BzBentoItem>
```

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ColSpan` | `int` | `1` | Number of columns to span (1-4) |
| `RowSpan` | `int` | `1` | Number of rows to span (1-4) |
| `Style` | `string?` | `null` | Additional inline styles |
| `ChildContent` | `RenderFragment` | - | Content to render |

### BzBentoCard

Image card with title and description:

```razor
<BzBentoCard TItem="object"
             ColSpan="2" RowSpan="2"
             Image="images/featured.jpg"
             Title="Featured Item"
             Description="Optional description text" />
```

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Image` | `string` | - | Image URL |
| `Title` | `string?` | `null` | Card title |
| `Description` | `string?` | `null` | Card description |
| `ColSpan` | `int` | `1` | Column span |
| `RowSpan` | `int` | `1` | Row span |

### BzBentoMetric

Metric/statistic display with icon and trend:

```razor
<BzBentoMetric Value="1,234"
               Label="Active Users"
               IconText="👥"
               Trend="+12%" />
```

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Value` | `string` | - | Main metric value |
| `Label` | `string` | - | Metric label |
| `IconText` | `string?` | `null` | Emoji or text icon |
| `Trend` | `string?` | `null` | Trend indicator (+12%, -5%, etc.) |

### BzBentoFeature

Feature highlight with icon and description:

```razor
<BzBentoFeature ColSpan="2"
                Label="New Feature"
                Description="Detailed description of the feature"
                IconText="🚀" />
```

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Label` | `string` | - | Feature title |
| `Description` | `string` | - | Feature description |
| `IconText` | `string?` | `null` | Emoji or text icon |
| `ColSpan` | `int` | `1` | Column span (often 2 for wide display) |

### BzBentoQuote

Testimonial or quote display:

```razor
<BzBentoQuote ColSpan="2"
              Quote="This component is amazing!"
              Author="Happy Developer"
              Role="Senior Engineer" />
```

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Quote` | `string` | - | Quote text |
| `Author` | `string` | - | Author name |
| `Role` | `string?` | `null` | Author role/title |
| `ColSpan` | `int` | `1` | Column span |

## API Reference

### Component Parameters

#### Data Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Items` | `IEnumerable<TItem>?` | `null` | Collection of items to display |
| `ChildContent` | `RenderFragment?` | `null` | Child content for composition mode |
| `ItemTemplate` | `RenderFragment<TItem>?` | `null` | Custom template for rendering items |

#### Layout Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Columns` | `int` | `4` | Number of grid columns |
| `Gap` | `int` | `16` | Gap between items in pixels |
| `AutoLayout` | `bool` | `true` | Enable automatic layout pattern |
| `Pattern` | `BentoLayoutPattern` | `Dynamic` | Layout pattern when AutoLayout is enabled |

#### Animation Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `AnimationEnabled` | `bool` | `true` | Enable staggered entrance animations |
| `StaggerDelay` | `int` | `50` | Delay between each item's animation (ms) |

#### Pagination Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Paginated` | `bool` | `false` | Enable paginated mode |
| `ItemsPerPage` | `int` | `6` | Items per page when paginated |

#### Event Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `OnItemSelected` | `EventCallback<TItem>` | Fired when an item is clicked |
| `OnPageChanged` | `EventCallback<int>` | Fired when page changes (paginated mode) |

#### Template Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `LoadingTemplate` | `RenderFragment?` | `null` | Custom loading state |
| `EmptyTemplate` | `RenderFragment?` | `null` | Custom empty state |

#### Advanced Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Options` | `BzBentoOptions?` | `null` | Advanced configuration (overrides individual parameters) |
| `Theme` | `BzTheme` | `Glass` | Visual theme |
| `CssClass` | `string?` | `null` | Additional CSS classes |

## Themes

BzBento supports the same four themes as all BlazzyMotion components.

### Glass Theme (Default)

Glassmorphism design with backdrop blur and subtle borders:

```razor
<BzBento Items="@items" Theme="BzTheme.Glass" />
```

### Dark Theme

Dark background with light text, no transparency:

```razor
<BzBento Items="@items" Theme="BzTheme.Dark" />
```

### Light Theme

Light background with dark text for bright interfaces:

```razor
<BzBento Items="@items" Theme="BzTheme.Light" />
```

### Minimal Theme

Clean, minimal styling with subtle accents:

```razor
<BzBento Items="@items" Theme="BzTheme.Minimal" />
```

## CSS Variables

Customize BzBento appearance with CSS variables:

```css
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
```

## Responsive Design

BzBento automatically adjusts based on screen width:

| Breakpoint | Columns | Gap |
|------------|---------|-----|
| Desktop (>991px) | As configured | As configured |
| Tablet (≤991px) | 2 | 12px |
| Mobile (≤600px) | 1 | 8px |

## Examples

### Dashboard Layout

```razor
<BzBento TItem="object" Items="null" Theme="BzTheme.Glass" Columns="4" Gap="16">
    <BzBentoMetric Value="$124K" Label="Revenue" IconText="💰" Trend="+23%" />
    <BzBentoMetric Value="8,432" Label="Users" IconText="👥" Trend="+12%" />
    <BzBentoMetric Value="99.9%" Label="Uptime" IconText="✓" />
    <BzBentoMetric Value="24ms" Label="Response" IconText="⚡" />

    <BzBentoFeature ColSpan="2"
                    Label="Q4 Report"
                    Description="Revenue exceeded targets by 15%"
                    IconText="📊" />

    <BzBentoCard ColSpan="2" RowSpan="2"
                 Image="images/chart.png"
                 Title="Growth Chart"
                 Description="Monthly trends" />
</BzBento>
```

### Product Showcase

```razor
<BzBento TItem="Product"
         Items="@products"
         Theme="BzTheme.Glass"
         Pattern="BentoLayoutPattern.Featured"
         OnItemSelected="@ViewProduct" />
```

### Metrics Dashboard

```razor
<BzBento TItem="Metric"
         Items="@metrics"
         Theme="BzTheme.Dark"
         Pattern="BentoLayoutPattern.Balanced"
         AutoLayout="true"
         Columns="4" />
```

## Browser Support

| Browser | Version | Status |
|---------|---------|--------|
| Chrome | 88+ | ✅ Full support |
| Firefox | 78+ | ✅ Full support |
| Safari | 14+ | ✅ Full support |
| Edge | 88+ | ✅ Full support |

**Requirements:**
- CSS Grid support
- Intersection Observer API (for animations)
- Backdrop-filter support (for Glass theme, graceful fallback on unsupported browsers)

## License

MIT License - free to use in personal and commercial projects.

---

**Part of the [BlazzyMotion](https://blazzymotion.com) component ecosystem.**
