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
/// <item>Staggered entrance animations</item>
/// <item>Intersection Observer for performance</item>
/// </list>
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
public class BzBentoJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    private ElementReference? _element;
    private bool _initialized;
    private bool _coreStylesLoaded;

    /// <summary>
    /// Initializes a new instance of the BzBentoJsInterop class.
    /// </summary>
    /// <param name="jsRuntime">The Blazor JS runtime</param>
    public BzBentoJsInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/BlazzyMotion.Core/js/blazzy-core.js").AsTask());
    }

    /// <summary>
    /// Initializes the Bento Grid with the specified options.
    /// </summary>
    /// <param name="element">The grid's root element reference</param>
    /// <param name="options">Bento configuration options</param>
    /// <param name="dotNetRef">Reference to the Blazor component for callbacks</param>
    public async ValueTask InitializeAsync<TComponent>(
        ElementReference element,
        BzBentoOptions options,
        DotNetObjectReference<TComponent>? dotNetRef = null) where TComponent : class
    {
        _element = element;
        var module = await _moduleTask.Value;

        if (!_coreStylesLoaded)
        {
            await module.InvokeVoidAsync("ensureCoreStylesLoaded");
            _coreStylesLoaded = true;
        }

        var optionsJson = JsonSerializer.Serialize(options, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

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
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }

    /// <summary>
    /// Destroys the Bento Grid instance and cleans up resources.
    /// </summary>
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
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }

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
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
