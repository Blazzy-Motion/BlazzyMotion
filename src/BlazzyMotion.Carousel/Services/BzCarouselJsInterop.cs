using BlazzyMotion.Carousel.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace BlazzyMotion.Carousel.Services;

/// <summary>
/// JavaScript interop service for BzCarousel component.
/// </summary>
/// <remarks>
/// <para>
/// Provides communication between Blazor and the Swiper.js library
/// through the unified BlazzyMotion.Core JavaScript module.
/// </para>
/// <para>
/// <strong>Features:</strong>
/// <list type="bullet">
/// <item>Lazy module loading (loaded on first use)</item>
/// <item>Automatic Swiper library loading</item>
/// <item>Instance management per element</item>
/// <item>Proper disposal and cleanup</item>
/// </list>
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
public class BzCarouselJsInterop : IAsyncDisposable
{
    // ═══════════════════════════════════════════════════════════════════
    // PRIVATE FIELDS
    // ═══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Lazy-loaded JavaScript module reference.
    /// </summary>
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    /// <summary>
    /// Indicates whether Swiper library has been loaded.
    /// </summary>
    private bool _swiperLoaded;

    /// <summary>
    /// Reference to the carousel's root element.
    /// </summary>
    private ElementReference? _element;

    // CONSTRUCTOR

    /// <summary>
    /// Initializes a new instance of the BzCarouselJsInterop class.
    /// </summary>
    /// <param name="jsRuntime">The Blazor JS runtime</param>
    /// <remarks>
    /// The JavaScript module is loaded lazily on first use.
    /// Path points to BlazzyMotion.Core unified JS module.
    /// </remarks>
    public BzCarouselJsInterop(IJSRuntime jsRuntime)
    {
        // IMPORTANT: Path points to Core JS module
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/BlazzyMotion.Core/js/blazzy-core.js").AsTask());
    }

   // PUBLIC METHODS

    /// <summary>
    /// Initializes the carousel with the specified options.
    /// </summary>
    /// <param name="element">The carousel's root element reference</param>
    /// <param name="options">Carousel configuration options</param>
    /// <remarks>
    /// <para>
    /// This method:
    /// <list type="number">
    /// <item>Loads Swiper CSS and JS (if not already loaded)</item>
    /// <item>Creates a Swiper instance with coverflow effect</item>
    /// <item>Stores the instance for later control</item>
    /// </list>
    /// </para>
    /// </remarks>
    public async ValueTask InitializeAsync(ElementReference element, BzCarouselOptions options)
    {
        _element = element;
        var module = await _moduleTask.Value;

        // Ensure Swiper library is loaded
        if (!_swiperLoaded)
        {
            await module.InvokeVoidAsync("ensureSwiperLoaded");
            _swiperLoaded = true;
        }

        // Serialize options to JSON for JS
        var optionsJson = JsonSerializer.Serialize(options, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        // Initialize carousel
        await module.InvokeVoidAsync("initializeCarousel", element, optionsJson);
    }

    /// <summary>
    /// Gets the currently active slide index.
    /// </summary>
    /// <returns>Active slide index (0-based)</returns>
    public async ValueTask<int> GetActiveIndexAsync()
    {
        if (_moduleTask.IsValueCreated && _element.HasValue)
        {
            var module = await _moduleTask.Value;
            return await module.InvokeAsync<int>("getActiveIndex", _element.Value);
        }
        return 0;
    }

    /// <summary>
    /// Destroys the Swiper instance and cleans up resources.
    /// </summary>
    /// <remarks>
    /// Should be called before reinitializing or when the component is disposed.
    /// </remarks>
    public async ValueTask DestroyAsync()
    {
        if (_moduleTask.IsValueCreated && _element.HasValue)
        {
            try
            {
                var module = await _moduleTask.Value;
                await module.InvokeVoidAsync("destroyCarousel", _element.Value);
            }
            catch (JSDisconnectedException)
            {
                // Circuit disconnected - expected during navigation
            }
            catch (ObjectDisposedException)
            {
                // Module already disposed
            }
        }
    }

    // DISPOSAL

    /// <summary>
    /// Disposes the JS interop resources.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await DestroyAsync();

        if (_moduleTask.IsValueCreated)
        {
            try
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Expected during navigation
            }
            catch (ObjectDisposedException)
            {
                // Already disposed
            }
        }
    }
}