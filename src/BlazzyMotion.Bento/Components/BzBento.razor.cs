using BlazzyMotion.Bento.Models;
using BlazzyMotion.Bento.Services;
using BlazzyMotion.Core.Abstractions;
using BlazzyMotion.Core.Models;
using BlazzyMotion.Core.Services;
using BlazzyMotion.Core.Templates;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace BlazzyMotion.Bento.Components;

/// <summary>
/// A modern Bento Grid component with glassmorphism design, staggered animations, and pagination support.
/// </summary>
/// <typeparam name="TItem">The type of items to display in the grid</typeparam>
/// <remarks>
/// <para>
/// BzBento provides a flexible grid layout with:
/// <list type="bullet">
/// <item>Zero-config setup with [BzBentoItem] attribute</item>
/// <item>Glassmorphism visual themes (Glass, Dark, Light, Minimal)</item>
/// <item>Staggered entrance animations</item>
/// <item>Optional pagination with swipe support</item>
/// <item>Composition mode for embedding other components (like BzCarousel)</item>
/// </list>
/// </para>
/// <para>
/// <strong>Usage Modes:</strong>
/// <list type="number">
/// <item>
/// <strong>Zero Config:</strong> Just provide Items and optional Theme
/// <code>
/// &lt;BzBento Items="dashboardItems" Theme="BzTheme.Glass" /&gt;
/// </code>
/// </item>
/// <item>
/// <strong>Paginated:</strong> Automatic pagination with swipe
/// <code>
/// &lt;BzBento Items="items" Paginated="true" ItemsPerPage="6" /&gt;
/// </code>
/// </item>
/// <item>
/// <strong>Composition:</strong> Custom layout with BzBentoItem children
/// <code>
/// &lt;BzBento Theme="BzTheme.Glass"&gt;
///     &lt;BzBentoItem ColSpan="2"&gt;
///         &lt;BzCarousel Items="movies" /&gt;
///     &lt;/BzBentoItem&gt;
/// &lt;/BzBento&gt;
/// </code>
/// </item>
/// </list>
/// </para>
/// </remarks>
public partial class BzBento<TItem> : BzComponentBase where TItem : class
{
    #region Parameters - Data

    /// <summary>
    /// Collection of items to display in the grid.
    /// Used in zero-config and paginated modes.
    /// </summary>
    [Parameter]
    public IEnumerable<TItem>? Items { get; set; }

    /// <summary>
    /// Child content for composition mode.
    /// Use BzBentoItem components as children.
    /// </summary>
    /// <remarks>
    /// When ChildContent is provided, Items parameter is ignored.
    /// This allows embedding any content including other BlazzyMotion components.
    /// </remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Custom template for rendering each item.
    /// Overrides the auto-generated template from [BzBentoItem] attribute.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    #endregion

    #region Parameters - Layout

    /// <summary>
    /// Number of columns in the grid.
    /// </summary>
    /// <remarks>
    /// Responsive behavior is automatic:
    /// <list type="bullet">
    /// <item>Desktop: Uses this value</item>
    /// <item>Tablet (≤991px): 2 columns</item>
    /// <item>Mobile (≤600px): 1 column</item>
    /// </list>
    /// Default: 4
    /// </remarks>
    [Parameter]
    public int Columns { get; set; } = 4;

    /// <summary>
    /// Gap between items in pixels.
    /// </summary>
    /// <remarks>
    /// Default: 16
    /// </remarks>
    [Parameter]
    public int Gap { get; set; } = 16;

    #endregion

    #region Parameters - Animation

    /// <summary>
    /// Enable staggered entrance animations.
    /// </summary>
    /// <remarks>
    /// When enabled, items fade in one by one using Intersection Observer.
    /// Default: true
    /// </remarks>
    [Parameter]
    public bool AnimationEnabled { get; set; } = true;

    /// <summary>
    /// Delay between each item's animation in milliseconds.
    /// </summary>
    /// <remarks>
    /// Only applies when AnimationEnabled is true.
    /// Default: 50
    /// </remarks>
    [Parameter]
    public int StaggerDelay { get; set; } = 50;

    #endregion

    #region Parameters - Pagination

    /// <summary>
    /// Enable paginated mode with swipeable pages.
    /// </summary>
    /// <remarks>
    /// When enabled, items are grouped into pages with dot navigation.
    /// Touch/swipe gestures are supported on mobile.
    /// Default: false
    /// </remarks>
    [Parameter]
    public bool Paginated { get; set; } = false;

    /// <summary>
    /// Number of items per page when paginated.
    /// </summary>
    /// <remarks>
    /// Only applies when Paginated is true.
    /// Default: 6
    /// </remarks>
    [Parameter]
    public int ItemsPerPage { get; set; } = 6;

    #endregion

    #region Parameters - Events

    /// <summary>
    /// Callback invoked when an item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> OnItemSelected { get; set; }

    /// <summary>
    /// Callback invoked when the page changes (paginated mode).
    /// </summary>
    [Parameter]
    public EventCallback<int> OnPageChanged { get; set; }

    #endregion

    #region Parameters - Templates

    /// <summary>
    /// Custom loading state template.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingTemplate { get; set; }

    /// <summary>
    /// Custom empty state template.
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyTemplate { get; set; }

    #endregion

    #region Parameters - Advanced

    /// <summary>
    /// Advanced configuration options.
    /// Overrides individual parameters if provided.
    /// </summary>
    [Parameter]
    public BzBentoOptions? Options { get; set; }

    #endregion

    #region Injected Services

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    #endregion

    #region Private Fields

    private ElementReference _gridRef;
    private BzBentoJsInterop? _jsInterop;
    private bool _initialized;
    private DotNetObjectReference<BzBento<TItem>>? _dotNetRef;
    private IReadOnlyList<TItem> _itemsCache = Array.Empty<TItem>();
    private IReadOnlyList<BzItem> _mappedItems = Array.Empty<BzItem>();
    private bool _useRegistryMapping;

    #endregion

    #region Computed Properties

    private bool IsLoading => Items == null && ChildContent == null;
    private bool IsEmpty => Items != null && !Items.Any();
    private int ItemCount => Items?.Count() ?? 0;

    #endregion

    #region Lifecycle Methods

    /// <summary>
    /// Called when parameters are set. Maps items and detects changes.
    /// </summary>
    protected override async Task OnParametersSetAsync()
    {
        // Cache items for O(1) access
        _itemsCache = Items?.ToList() ?? new List<TItem>();

        // Determine rendering strategy
        if (ItemTemplate != null)
        {
            _useRegistryMapping = false;
            _mappedItems = Array.Empty<BzItem>();
        }
        else if (BzRegistry.HasMapper<TItem>())
        {
            _useRegistryMapping = true;
            _mappedItems = BzRegistry.ToBzItems(_itemsCache);
        }
        else
        {
            _useRegistryMapping = false;
            _mappedItems = Array.Empty<BzItem>();
        }

        await base.OnParametersSetAsync();
    }

    /// <summary>
    /// Called after render. Initializes JavaScript functionality.
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "JS initialization - cannot fully test with bUnit")]
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsDisposed) return;

        // First render - initialize JS interop
        if (firstRender && !_initialized && !IsEmpty && ChildContent == null)
        {
            try
            {
                _dotNetRef = DotNetObjectReference.Create(this);
                _jsInterop ??= new BzBentoJsInterop(JS);
                await _jsInterop.InitializeAsync(_gridRef, BuildOptions(), _dotNetRef);
                _initialized = true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[BzBento] Initialization error: {ex.Message}");
            }
        }
        else if (firstRender && !_initialized && ChildContent != null && AnimationEnabled)
        {
            // Composition mode with animations
            try
            {
                _dotNetRef = DotNetObjectReference.Create(this);
                _jsInterop ??= new BzBentoJsInterop(JS);
                await _jsInterop.InitializeAsync(_gridRef, BuildOptions(), _dotNetRef);
                _initialized = true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[BzBento] Initialization error: {ex.Message}");
            }
        }
    }

    #endregion

    #region JS Invokable Methods

    /// <summary>
    /// Called from JavaScript when the Bento grid is initialized.
    /// </summary>
    /// <param name="itemOrPageCount">Number of items (static) or pages (paginated)</param>
    [JSInvokable]
    [ExcludeFromCodeCoverage(Justification = "JS callback")]
    public Task OnBentoInitializedFromJS(int itemOrPageCount)
    {
        // Optional: Handle initialization complete
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called from JavaScript when the page changes (paginated mode).
    /// </summary>
    /// <param name="pageIndex">Zero-based page index</param>
    [JSInvokable]
    [ExcludeFromCodeCoverage(Justification = "JS callback")]
    public async Task OnBentoPageChangeFromJS(int pageIndex)
    {
        if (OnPageChanged.HasDelegate)
        {
            await OnPageChanged.InvokeAsync(pageIndex);
        }
    }

    #endregion

    #region Private Methods - Configuration

    /// <summary>
    /// Builds the options object for JS initialization.
    /// </summary>
    private BzBentoOptions BuildOptions()
    {
        if (Options != null) return Options;

        return new BzBentoOptions
        {
            Columns = Columns,
            Gap = Gap,
            AnimationEnabled = AnimationEnabled,
            StaggerDelay = StaggerDelay,
            Paginated = Paginated,
            ItemsPerPage = ItemsPerPage
        };
    }

    /// <summary>
    /// Generates inline style for the grid container.
    /// </summary>
    private string GetGridStyle()
    {
        var styles = new List<string>();

        if (Columns != 4)
            styles.Add($"--bzb-columns: {Columns}");

        if (Gap != 16)
            styles.Add($"--bzb-gap: {Gap}px");

        return string.Join("; ", styles);
    }

    /// <summary>
    /// Generates inline style for paginated page grid.
    /// </summary>
    private string GetPageGridStyle()
    {
        return $"display: grid; grid-template-columns: repeat({Columns}, 1fr); gap: {Gap}px; width: 100%;";
    }

    #endregion

    #region Private Methods - Item Handling

    /// <summary>
    /// Gets the mapped BzItem for a given source item.
    /// </summary>
    private BzItem GetMappedItem(TItem item, int index)
    {
        if (_useRegistryMapping && index < _mappedItems.Count)
        {
            return _mappedItems[index];
        }

        return new BzItem
        {
            Title = item?.ToString(),
            OriginalItem = item
        };
    }

    /// <summary>
    /// Generates CSS classes for column and row spans.
    /// </summary>
    private string GetSpanClasses(BzItem item)
    {
        var col = $"bzb-col-{item.ClampedColSpan}";
        var row = $"bzb-row-{item.ClampedRowSpan}";
        return $"{col} {row}";
    }

    /// <summary>
    /// Generates inline style for item order.
    /// </summary>
    private string GetItemStyle(BzItem item)
    {
        if (item.Order != 0)
            return $"order: {item.Order}";
        return string.Empty;
    }

    /// <summary>
    /// Gets the RenderFragment for rendering an item.
    /// </summary>
    private RenderFragment GetItemContent(TItem item, int index)
    {
        // Priority 1: Custom template
        if (ItemTemplate != null)
        {
            return ItemTemplate(item);
        }

        // Priority 2: Registry mapping with smart template selection
        if (_useRegistryMapping && index < _mappedItems.Count)
        {
            var bzItem = _mappedItems[index];
            return BzTemplateFactory.SelectBentoTemplate(bzItem)(bzItem);
        }

        // Priority 3: Generic fallback
        return BzTemplateFactory.CreateGenericFallback<TItem>()(item);
    }

    /// <summary>
    /// Handles item click events.
    /// </summary>
    private async Task HandleItemClick(TItem item)
    {
        if (OnItemSelected.HasDelegate)
        {
            await OnItemSelected.InvokeAsync(item);
        }
    }

    #endregion

    #region Private Methods - Pagination

    /// <summary>
    /// Groups items into pages for paginated mode.
    /// </summary>
    private IEnumerable<IEnumerable<(TItem Item, int Index)>> GetPages()
    {
        if (Items == null) yield break;

        var itemsWithIndex = Items.Select((item, index) => (Item: item, Index: index)).ToList();
        var pageCount = (int)Math.Ceiling((double)itemsWithIndex.Count / ItemsPerPage);

        for (int i = 0; i < pageCount; i++)
        {
            yield return itemsWithIndex
                .Skip(i * ItemsPerPage)
                .Take(ItemsPerPage);
        }
    }

    #endregion

    #region Disposal

    /// <summary>
    /// Disposes component resources.
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "JS disposal")]
    protected override async ValueTask DisposeAsyncCore()
    {
        _initialized = false;
        _dotNetRef?.Dispose();

        if (_jsInterop != null)
        {
            await _jsInterop.DisposeAsync();
        }
    }

    #endregion
}
