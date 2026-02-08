using BlazzyMotion.Core.Services;
using BlazzyMotion.Gallery.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazzyMotion.Gallery.Services;

/// <summary>
/// JavaScript interop service for BzGallery component.
/// </summary>
/// <remarks>
/// <para>
/// Handles communication with blazzy-gallery.js module.
/// Inherits lazy module loading and safe invoke methods from <see cref="BzJsInteropBase"/>.
/// </para>
/// <para>
/// <strong>JS Functions:</strong>
/// <list type="bullet">
/// <item><c>initializeGallery</c> - Sets up grid/masonry + animations</item>
/// <item><c>openLightbox</c> - Opens fullscreen lightbox at index</item>
/// <item><c>closeLightbox</c> - Closes lightbox</item>
/// <item><c>filterGallery</c> - Filters items by category with animation</item>
/// <item><c>destroyGallery</c> - Cleans up instance</item>
/// </list>
/// </para>
/// </remarks>
public sealed class BzGalleryJsInterop : BzJsInteropBase
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public BzGalleryJsInterop(IJSRuntime jsRuntime)
        : base(jsRuntime, "./_content/BlazzyMotion.Gallery/js/blazzy-gallery.js")
    {
    }

    /// <summary>
    /// Initializes the gallery component.
    /// </summary>
    public async ValueTask InitializeAsync<TComponent>(
        ElementReference element,
        BzGalleryOptions options,
        DotNetObjectReference<TComponent>? dotNetRef = null) where TComponent : class
    {
        if (IsDisposed) return;

        SetElement(element);

        var module = await GetModuleAsync();
        var optionsJson = JsonSerializer.Serialize(options, _jsonOptions);

        await module.InvokeVoidAsync("initializeGallery", element, optionsJson, dotNetRef);
    }

    /// <summary>
    /// Opens the lightbox at the specified item index.
    /// </summary>
    public async ValueTask OpenLightboxAsync(ElementReference element, int index)
    {
        await SafeInvokeVoidAsync("openLightbox", element, index);
    }

    /// <summary>
    /// Closes the lightbox.
    /// </summary>
    public async ValueTask CloseLightboxAsync(ElementReference element)
    {
        await SafeInvokeVoidAsync("closeLightbox", element);
    }

    /// <summary>
    /// Locks grid height before filter re-render to prevent layout jump.
    /// </summary>
    public async ValueTask PrepareFilterAsync(ElementReference element)
    {
        await SafeInvokeVoidAsync("prepareFilter", element);
    }

    /// <summary>
    /// Filters gallery items by category.
    /// </summary>
    public async ValueTask FilterAsync(ElementReference element, string? category)
    {
        await SafeInvokeVoidAsync("filterGallery", element, category);
    }

    /// <summary>
    /// Recalculates masonry layout after filter or resize.
    /// </summary>
    public async ValueTask RecalculateMasonryAsync(ElementReference element)
    {
        await SafeInvokeVoidAsync("recalculateMasonry", element);
    }

    /// <summary>
    /// Focuses the lightbox element for keyboard navigation.
    /// </summary>
    public async ValueTask FocusLightboxAsync()
    {
        await SafeInvokeVoidAsync("focusLightbox");
    }

    /// <summary>
    /// Locks body scroll when lightbox opens.
    /// </summary>
    public async ValueTask LockBodyScrollAsync()
    {
        await SafeInvokeVoidAsync("lockBodyScroll");
    }

    /// <summary>
    /// Unlocks body scroll when lightbox closes.
    /// </summary>
    public async ValueTask UnlockBodyScrollAsync()
    {
        await SafeInvokeVoidAsync("unlockBodyScroll");
    }

    /// <summary>
    /// Destroys gallery instance.
    /// </summary>
    public async ValueTask DestroyAsync(ElementReference element)
    {
        await SafeInvokeVoidAsync("destroyGallery", element);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        if (ElementRef.HasValue)
        {
            await SafeInvokeVoidAsync("destroyGallery", ElementRef.Value);
        }
    }
}
