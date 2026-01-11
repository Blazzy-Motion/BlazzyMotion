using BlazzyMotion.Bento.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace BlazzyMotion.Bento.Services;

/// <summary>
/// JavaScript interop service for BzBento component.
/// </summary>
/// <remarks>
/// <para>
/// Provides communication between Blazor and the BlazzyMotion.Core JavaScript module
/// for Bento Grid functionality including:
/// <list type="bullet">
/// <item>Staggered entrance animations (static mode)</item>
/// <item>Swiper-based pagination (paginated mode)</item>
/// <item>Intersection Observer for performance</item>
/// </list>
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
public class BzBentoJsInterop : IAsyncDisposable
{
    #region Private Fields

    /// <summary>
    /// Lazy-loaded JavaScript module reference.
    /// </summary>
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    /// <summary>
    /// Reference to the Bento grid's root element.
    /// </summary>
    private ElementReference? _element;

    /// <summary>
    /// Indicates whether this instance has been initialized.
    /// </summary>
    private bool _initialized;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the BzBentoJsInterop class.
    /// </summary>
    /// <param name="jsRuntime">The Blazor JS runtime</param>
    /// <remarks>
    /// The JavaScript module is loaded lazily on first use.
    /// Path points to BlazzyMotion.Core unified JS module.
    /// </remarks>
    public BzBentoJsInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/BlazzyMotion.Core/js/blazzy-core.js").AsTask());
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the Bento Grid with the specified options.
    /// </summary>
    /// <param name="element">The grid's root element reference</param>
    /// <param name="options">Bento configuration options</param>
    /// <param name="dotNetRef">Reference to the Blazor component for callbacks</param>
    /// <remarks>
    /// <para>
    /// This method:
    /// <list type="number">
    /// <item>Detects mode (static vs paginated)</item>
    /// <item>For static: Sets up Intersection Observer for animations</item>
    /// <item>For paginated: Initializes Swiper for page transitions</item>
    /// <item>Registers callbacks to Blazor</item>
    /// </list>
    /// </para>
    /// </remarks>
    public async ValueTask InitializeAsync<TComponent>(
        ElementReference element,
        BzBentoOptions options,
        DotNetObjectReference<TComponent>? dotNetRef = null) where TComponent : class
    {
        _element = element;
        var module = await _moduleTask.Value;

        // Ensure Swiper is loaded (needed for paginated mode, but also loads core CSS)
        await module.InvokeVoidAsync("ensureSwiperLoaded");

        // Serialize options to JSON for JS
        var optionsJson = JsonSerializer.Serialize(options, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        // Initialize Bento with optional .NET callback reference
        if (dotNetRef != null)
        {
            await module.InvokeVoidAsync("initializeBento", element, optionsJson, dotNetRef);
        }
        else
        {
            await module.InvokeVoidAsync("initializeBento", element, optionsJson, null);
        }

        _initialized = true;
    }

    /// <summary>
    /// Refreshes the Bento Grid, re-triggering animations.
    /// </summary>
    /// <remarks>
    /// Useful after dynamically adding/removing items.
    /// </remarks>
    public async ValueTask RefreshAsync()
    {
        if (_moduleTask.IsValueCreated && _element.HasValue && _initialized)
        {
            try
            {
                var module = await _moduleTask.Value;
                await module.InvokeVoidAsync("refreshBento", _element.Value);
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

    /// <summary>
    /// Navigates to a specific page (paginated mode only).
    /// </summary>
    /// <param name="pageIndex">Zero-based page index</param>
    /// <param name="speed">Transition speed in milliseconds</param>
    public async ValueTask SlideToAsync(int pageIndex, int speed = 300)
    {
        if (_moduleTask.IsValueCreated && _element.HasValue && _initialized)
        {
            try
            {
                var module = await _moduleTask.Value;
                await module.InvokeVoidAsync("bentoSlideTo", _element.Value, pageIndex, speed);
            }
            catch (JSDisconnectedException)
            {
                // Circuit disconnected
            }
            catch (ObjectDisposedException)
            {
                // Module disposed
            }
        }
    }

    /// <summary>
    /// Gets the current page index (paginated mode only).
    /// </summary>
    /// <returns>Zero-based index of the active page</returns>
    public async ValueTask<int> GetActiveIndexAsync()
    {
        if (_moduleTask.IsValueCreated && _element.HasValue && _initialized)
        {
            try
            {
                var module = await _moduleTask.Value;
                return await module.InvokeAsync<int>("getBentoActiveIndex", _element.Value);
            }
            catch (JSDisconnectedException)
            {
                // Circuit disconnected
            }
            catch (ObjectDisposedException)
            {
                // Module disposed
            }
        }
        return 0;
    }

    /// <summary>
    /// Destroys the Bento Grid instance and cleans up resources.
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
                await module.InvokeVoidAsync("destroyBento", _element.Value);
                _initialized = false;
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

    #endregion

    #region Disposal

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

    #endregion
}
