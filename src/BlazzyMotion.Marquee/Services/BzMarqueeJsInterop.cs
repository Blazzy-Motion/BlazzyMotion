using BlazzyMotion.Core.Services;
using BlazzyMotion.Marquee.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazzyMotion.Marquee.Services;

/// <summary>
/// JavaScript interop service for BzMarquee component.
/// </summary>
/// <remarks>
/// <para>
/// Handles communication with blazzy-marquee.js module.
/// Inherits lazy module loading and safe invoke methods from <see cref="BzJsInteropBase"/>.
/// </para>
/// <para>
/// <strong>JS Functions:</strong>
/// <list type="bullet">
/// <item><c>initializeMarquee</c> - Sets up infinite scroll, cloning and stagger entrance</item>
/// <item><c>pauseMarquee</c> - Pauses marquee animation</item>
/// <item><c>resumeMarquee</c> - Resumes marquee animation</item>
/// <item><c>destroyMarquee</c> - Cleans up instance</item>
/// </list>
/// </para>
/// </remarks>
public sealed class BzMarqueeJsInterop : BzJsInteropBase
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public BzMarqueeJsInterop(IJSRuntime jsRuntime)
        : base(jsRuntime, "./_content/BlazzyMotion.Marquee/js/blazzy-marquee.js")
    {
    }

    /// <summary>
    /// Initializes the marquee component with infinite scroll and stagger entrance.
    /// </summary>
    public async ValueTask InitializeAsync<TComponent>(
        ElementReference element,
        BzMarqueeOptions options,
        DotNetObjectReference<TComponent>? dotNetRef = null) where TComponent : class
    {
        if (IsDisposed) return;

        SetElement(element);

        var module = await GetModuleAsync();
        var optionsJson = JsonSerializer.Serialize(options, _jsonOptions);

        await module.InvokeVoidAsync("initializeMarquee", element, optionsJson, dotNetRef);
    }

    /// <summary>
    /// Pauses the marquee animation.
    /// </summary>
    public async ValueTask PauseAsync(ElementReference element)
    {
        await SafeInvokeVoidAsync("pauseMarquee", element);
    }

    /// <summary>
    /// Resumes the marquee animation.
    /// </summary>
    public async ValueTask ResumeAsync(ElementReference element)
    {
        await SafeInvokeVoidAsync("resumeMarquee", element);
    }

    /// <summary>
    /// Destroys the marquee instance and cleans up resources.
    /// </summary>
    public async ValueTask DestroyAsync(ElementReference element)
    {
        await SafeInvokeVoidAsync("destroyMarquee", element);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        if (ElementRef.HasValue)
        {
            await SafeInvokeVoidAsync("destroyMarquee", ElementRef.Value);
        }
    }
}
