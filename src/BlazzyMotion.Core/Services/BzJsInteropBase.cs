using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace BlazzyMotion.Core.Services;

/// <summary>
/// Base class for JavaScript interop in BlazzyMotion components.
/// </summary>
/// <remarks>
/// <para>
/// Provides common functionality for JS module loading and lifecycle management.
/// All component-specific JS interop classes should inherit from this base.
/// </para>
/// <para>
/// <strong>Features:</strong>
/// <list type="bullet">
/// <item>Lazy module loading (loaded on first use)</item>
/// <item>Automatic disposal of JS references</item>
/// <item>Thread-safe module access</item>
/// <item>Element reference tracking</item>
/// </list>
/// </para>
/// <para>
/// <strong>Inheritance:</strong>
/// <code>
/// BzJsInteropBase
///     ├── BzCarouselJsInterop
///     ├── BzGalleryJsInterop (future)
///     └── BzMasonryJsInterop (future)
/// </code>
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BzCarouselJsInterop : BzJsInteropBase
/// {
///     public BzCarouselJsInterop(IJSRuntime jsRuntime) 
///         : base(jsRuntime, "./_content/BlazzyMotion.Carousel/js/blazzy-carousel.js")
///     {
///     }
///     
///     public async ValueTask InitializeAsync(ElementReference element)
///     {
///         var module = await GetModuleAsync();
///         await module.InvokeVoidAsync("initialize", element);
///     }
/// }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public abstract class BzJsInteropBase : IAsyncDisposable
{
    // PRIVATE FIELDS

    /// <summary>
    /// Lazy-loaded JavaScript module reference.
    /// </summary>
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    /// <summary>
    /// Path to the JavaScript module file.
    /// </summary>
    private readonly string _modulePath;

    /// <summary>
    /// Reference to the component's root element.
    /// </summary>
    protected ElementReference? ElementRef { get; set; }

    /// <summary>
    /// Indicates whether the interop has been disposed.
    /// </summary>
    protected bool IsDisposed { get; private set; }

    // CONSTRUCTOR

    /// <summary>
    /// Initializes a new instance of the BzJsInteropBase class.
    /// </summary>
    /// <param name="jsRuntime">The Blazor JS runtime</param>
    /// <param name="modulePath">Path to the JS module (e.g., "./_content/Package/js/file.js")</param>
    /// <exception cref="ArgumentNullException">Thrown if jsRuntime is null</exception>
    /// <exception cref="ArgumentException">Thrown if modulePath is null or empty</exception>
    protected BzJsInteropBase(IJSRuntime jsRuntime, string modulePath)
    {
        if (jsRuntime is null)
            throw new ArgumentNullException(nameof(jsRuntime));

        if (string.IsNullOrWhiteSpace(modulePath))
            throw new ArgumentException("Module path cannot be null or empty.", nameof(modulePath));

        _modulePath = modulePath;

        // Lazy initialization - module is loaded only when first accessed
        _moduleTask = new Lazy<Task<IJSObjectReference>>(() =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", _modulePath).AsTask());
    }

    
    // PROTECTED METHODS
   

    /// <summary>
    /// Gets the JavaScript module reference, loading it if necessary.
    /// </summary>
    /// <returns>The loaded JS module reference</returns>
    /// <remarks>
    /// <para>
    /// The module is loaded lazily on first access and cached for subsequent calls.
    /// This ensures optimal performance by avoiding unnecessary module loads.
    /// </para>
    /// </remarks>
    /// <exception cref="ObjectDisposedException">Thrown if called after disposal</exception>
    protected async Task<IJSObjectReference> GetModuleAsync()
    {
        if (IsDisposed)
            throw new ObjectDisposedException(GetType().Name);

        return await _moduleTask.Value;
    }

    /// <summary>
    /// Checks if the module has been loaded.
    /// </summary>
    /// <returns>True if module is loaded</returns>
    protected bool IsModuleLoaded => _moduleTask.IsValueCreated;

    /// <summary>
    /// Sets the element reference for this interop instance.
    /// </summary>
    /// <param name="element">The component's root element reference</param>
    protected void SetElement(ElementReference element)
    {
        ElementRef = element;
    }

    // SAFE INVOKE METHODS

    /// <summary>
    /// Safely invokes a JavaScript function, handling disposal and errors.
    /// </summary>
    /// <param name="identifier">The JS function name</param>
    /// <param name="args">Arguments to pass to the function</param>
    /// <returns>True if invocation succeeded, false otherwise</returns>
    /// <remarks>
    /// Use this method for "fire and forget" JS calls where you don't need the result.
    /// Errors are logged but not thrown.
    /// </remarks>
    protected async ValueTask<bool> SafeInvokeVoidAsync(string identifier, params object?[] args)
    {
        if (IsDisposed || !IsModuleLoaded)
            return false;

        try
        {
            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync(identifier, args);
            return true;
        }
        catch (JSDisconnectedException)
        {
            // Circuit disconnected (Blazor Server) - expected during navigation
            return false;
        }
        catch (ObjectDisposedException)
        {
            // Module already disposed
            return false;
        }
        catch (Exception ex)
        {
            // Log error but don't throw
            Console.Error.WriteLine($"[{GetType().Name}] JS invoke error: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Safely invokes a JavaScript function and returns the result.
    /// </summary>
    /// <typeparam name="T">The expected return type</typeparam>
    /// <param name="identifier">The JS function name</param>
    /// <param name="args">Arguments to pass to the function</param>
    /// <returns>The result or default(T) if failed</returns>
    protected async ValueTask<T?> SafeInvokeAsync<T>(string identifier, params object?[] args)
    {
        if (IsDisposed || !IsModuleLoaded)
            return default;

        try
        {
            var module = await _moduleTask.Value;
            return await module.InvokeAsync<T>(identifier, args);
        }
        catch (JSDisconnectedException)
        {
            return default;
        }
        catch (ObjectDisposedException)
        {
            return default;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[{GetType().Name}] JS invoke error: {ex.Message}");
            return default;
        }
    }

    // DISPOSAL

    /// <summary>
    /// Disposes the JavaScript module and releases resources.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Override <see cref="DisposeAsyncCore"/> in derived classes to add
    /// custom cleanup logic (e.g., destroying Swiper instances).
    /// </para>
    /// </remarks>
    public async ValueTask DisposeAsync()
    {
        if (IsDisposed)
            return;

        IsDisposed = true;

        // Allow derived classes to clean up first
        await DisposeAsyncCore();

        // Dispose the JS module
        if (_moduleTask.IsValueCreated)
        {
            try
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Expected during navigation/circuit disconnect
            }
            catch (ObjectDisposedException)
            {
                // Already disposed
            }
            catch
            {
                // Ignore disposal errors
            }
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Override this method to add custom disposal logic in derived classes.
    /// </summary>
    /// <remarks>
    /// Called before the JS module is disposed. Use for:
    /// <list type="bullet">
    /// <item>Destroying library instances (e.g., Swiper.destroy())</item>
    /// <item>Removing event listeners</item>
    /// <item>Cleaning up DOM elements</item>
    /// </list>
    /// </remarks>
    protected virtual ValueTask DisposeAsyncCore()
    {
        return ValueTask.CompletedTask;
    }
}