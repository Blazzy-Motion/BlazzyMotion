using BlazzyMotion.Core.Models;
using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Core.Abstractions;

/// <summary>
/// Base class for all BlazzyMotion components.
/// </summary>
/// <remarks>
/// <para>
/// Provides shared functionality for all visual components in the BlazzyMotion library:
/// <list type="bullet">
/// <item>Theme management and CSS class generation</item>
/// <item>Common parameters (CssClass, Theme)</item>
/// <item>Lifecycle management</item>
/// <item>Shared utility methods</item>
/// </list>
/// </para>
/// <para>
/// <strong>Inheritance:</strong>
/// <code>
/// ComponentBase (Blazor)
///     └── BzComponentBase (BlazzyMotion)
///             ├── BzCarousel&lt;TItem&gt;
///             ├── BzGallery&lt;TItem&gt;
///             └── BzMasonry&lt;TItem&gt;
/// </code>
/// </para>
/// </remarks>
public abstract class BzComponentBase : ComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Visual theme for the component.
    /// </summary>
    /// <remarks>
    /// Applies CSS classes for consistent theming across all BlazzyMotion components.
    /// Default is <see cref="BzTheme.Glass"/> for the glassmorphism effect.
    /// </remarks>
    [Parameter]
    public BzTheme Theme { get; set; } = BzTheme.Glass;

    /// <summary>
    /// Additional CSS classes to apply to the component root element.
    /// </summary>
    /// <remarks>
    /// Use for custom styling without overriding the theme.
    /// Classes are appended to the existing theme classes.
    /// </remarks>
    /// <example>
    /// <code>
    /// &lt;BzCarousel Items="movies" CssClass="my-custom-carousel shadow-lg" /&gt;
    /// </code>
    /// </example>
    [Parameter]
    public string? CssClass { get; set; }

    /// <summary>
    /// Gets the CSS class for the current theme.
    /// </summary>
    /// <remarks>
    /// Maps <see cref="BzTheme"/> enum values to CSS class names.
    /// Uses the "bzc-theme-" prefix for carousel components.
    /// </remarks>
    protected string ThemeClass => Theme switch
    {
        BzTheme.Glass => "bzc-theme-glass",
        BzTheme.Dark => "bzc-theme-dark",
        BzTheme.Light => "bzc-theme-light",
        BzTheme.Minimal => "bzc-theme-minimal",
        _ => "bzc-theme-glass"
    };

    /// <summary>
    /// Combines theme class with custom CssClass.
    /// </summary>
    /// <param name="baseClass">The base CSS class for the component</param>
    /// <returns>Combined CSS class string</returns>
    /// <example>
    /// <code>
    /// // In derived component:
    /// &lt;div class="@GetCombinedClass("bzc-carousel-box")"&gt;
    /// // Output: "bzc-carousel-box bzc-theme-glass my-custom-class"
    /// </code>
    /// </example>
    protected string GetCombinedClass(string baseClass)
    {
        var classes = new List<string> { baseClass, ThemeClass };

        if (!string.IsNullOrWhiteSpace(CssClass))
        {
            classes.Add(CssClass);
        }

        return string.Join(" ", classes);
    }

    /// <summary>
    /// Tracks whether the component has been disposed.
    /// </summary>
    protected bool IsDisposed { get; private set; }

    /// <summary>
    /// Disposes component resources asynchronously.
    /// </summary>
    /// <remarks>
    /// Override <see cref="DisposeAsyncCore"/> in derived classes
    /// to add custom cleanup logic.
    /// </remarks>
    public async ValueTask DisposeAsync()
    {
        if (IsDisposed)
            return;

        await DisposeAsyncCore();

        IsDisposed = true;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Override this method to dispose resources in derived classes.
    /// </summary>
    /// <remarks>
    /// Called by <see cref="DisposeAsync"/>. Use for cleanup of:
    /// <list type="bullet">
    /// <item>JavaScript interop references</item>
    /// <item>Event subscriptions</item>
    /// <item>Unmanaged resources</item>
    /// </list>
    /// </remarks>
    protected virtual ValueTask DisposeAsyncCore()
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Safely invokes StateHasChanged on the UI thread.
    /// </summary>
    /// <remarks>
    /// Use when updating state from async callbacks or event handlers.
    /// </remarks>
    protected async Task InvokeStateHasChangedAsync()
    {
        if (IsDisposed)
            return;

        await InvokeAsync(StateHasChanged);
    }
}