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
/// A modern Bento Grid component with glassmorphism design and staggered animations.
/// </summary>
/// <typeparam name="TItem">The type of items to display in the grid</typeparam>
/// <remarks>
/// <para>
/// BzBento provides a flexible grid layout with:
/// <list type="bullet">
/// <item>Zero-config setup with [BzBentoItem] attribute</item>
/// <item>Glassmorphism visual themes (Glass, Dark, Light, Minimal)</item>
/// <item>Staggered entrance animations</item>
/// <item>Composition mode for embedding other components (like BzCarousel)</item>
/// <item>Auto-layout patterns for dynamic grid arrangements</item>
/// </list>
/// </para>
/// <para>
/// <strong>Usage Modes:</strong>
/// <list type="number">
/// <item>
/// <strong>Items Mode (Zero Config):</strong> Just provide Items and optional Theme
/// <code>
/// &lt;BzBento Items="dashboardItems" Theme="BzTheme.Glass" /&gt;
/// </code>
/// </item>
/// <item>
/// <strong>Composition Mode:</strong> Custom layout with helper components
/// <code>
/// &lt;BzBento Theme="BzTheme.Glass"&gt;
///     &lt;BzBentoCard Image="hero.jpg" Title="Featured" ColSpan="2" RowSpan="2" /&gt;
///     &lt;BzBentoMetric Value="1,234" Label="Users" /&gt;
///     &lt;BzBentoFeature IconText="🚀" Label="Fast" /&gt;
///     &lt;BzBentoItem ColSpan="2" RowSpan="2"&gt;
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
    /// Used in Items mode (zero-config).
    /// </summary>
    [Parameter]
    public IEnumerable<TItem>? Items { get; set; }

    /// <summary>
    /// Child content for composition mode.
    /// Use BzBentoCard, BzBentoMetric, BzBentoFeature, BzBentoQuote, or BzBentoItem components.
    /// </summary>
    /// <remarks>
    /// When ChildContent is provided, Items parameter is ignored.
    /// This allows embedding any content including other BlazzyMotion components.
    /// </remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Custom template for rendering each item in Items mode.
    /// Overrides the auto-generated template from [BzBentoItem] attribute.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    #endregion

    #region Parameters - Layout

    /// <summary>
    /// Number of columns in the grid (1-12).
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
    /// Delay between each item's animation in milliseconds (0-1000).
    /// </summary>
    /// <remarks>
    /// Only applies when AnimationEnabled is true.
    /// Default: 50
    /// </remarks>
    [Parameter]
    public int StaggerDelay { get; set; } = 50;

    #endregion

    #region Parameters - Auto-Layout

    /// <summary>
    /// Enable automatic layout pattern assignment.
    /// </summary>
    /// <remarks>
    /// When enabled, items automatically receive ColSpan and RowSpan values
    /// based on the selected <see cref="Pattern"/>, creating visually dynamic
    /// Bento grids without manual configuration.
    /// Explicit ColSpan/RowSpan values in your model will override auto-layout.
    /// Default: true
    /// </remarks>
    [Parameter]
    public bool AutoLayout { get; set; } = true;

    /// <summary>
    /// Layout pattern to use when AutoLayout is enabled.
    /// </summary>
    /// <remarks>
    /// Only applies when <see cref="AutoLayout"/> is true.
    /// See <see cref="BentoLayoutPattern"/> for available patterns:
    /// <list type="bullet">
    /// <item><strong>Dynamic:</strong> Varied mix with hero (2x2), tall, wide, and normal cards</item>
    /// <item><strong>Featured:</strong> Large hero at start, rest smaller</item>
    /// <item><strong>Balanced:</strong> Uniform appearance with wide and tall cards, no 2x2</item>
    /// </list>
    /// Default: Dynamic
    /// </remarks>
    [Parameter]
    public BentoLayoutPattern Pattern { get; set; } = BentoLayoutPattern.Dynamic;

    #endregion

    #region Parameters - Events

    /// <summary>
    /// Callback invoked when an item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> OnItemSelected { get; set; }

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
    /// Called when parameters are set. Validates, maps items and prepares rendering.
    /// </summary>
    protected override async Task OnParametersSetAsync()
    {
        // Validate parameters first
        ValidateParameters();

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

        // First render - initialize JS interop for Items mode
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
        // Composition mode with animations
        else if (firstRender && !_initialized && ChildContent != null && AnimationEnabled)
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
    }

    #endregion

    #region JS Invokable Methods

    /// <summary>
    /// Called from JavaScript when the Bento grid is initialized.
    /// </summary>
    /// <param name="itemCount">Number of items in the grid</param>
    [JSInvokable]
    [ExcludeFromCodeCoverage(Justification = "JS callback")]
    public Task OnBentoInitializedFromJS(int itemCount)
    {
        // Optional: Handle initialization complete
        return Task.CompletedTask;
    }

    #endregion

    #region Private Methods - Validation

    /// <summary>
    /// Validates component parameters.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when parameters are invalid</exception>
    private void ValidateParameters()
    {
        if (Columns < 1 || Columns > 12)
            throw new ArgumentOutOfRangeException(nameof(Columns),
                "Columns must be between 1 and 12.");

        if (Gap < 0)
            throw new ArgumentOutOfRangeException(nameof(Gap),
                "Gap cannot be negative.");

        if (StaggerDelay < 0 || StaggerDelay > 1000)
            throw new ArgumentOutOfRangeException(nameof(StaggerDelay),
                "StaggerDelay must be between 0 and 1000ms.");
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
            StaggerDelay = StaggerDelay
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

    #endregion

    #region Private Methods - Item Handling

    /// <summary>
    /// Gets the mapped BzItem for a given source item.
    /// </summary>
    private BzItem GetMappedItem(TItem item, int index)
    {
        BzItem bzItem;

        if (_useRegistryMapping && index < _mappedItems.Count)
        {
            bzItem = _mappedItems[index];
        }
        else
        {
            bzItem = new BzItem
            {
                Title = item?.ToString(),
                OriginalItem = item
            };
        }

        // Apply auto-layout pattern
        return ApplyAutoLayout(bzItem, index);
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

    /// <summary>
    /// Handles keyboard navigation for accessibility.
    /// </summary>
    private async Task HandleKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e, TItem item)
    {
        if (e.Key == "Enter")
        {
            await HandleItemClick(item);
        }
    }

    #endregion

    #region Private Methods - Auto-Layout

    /// <summary>
    /// Applies auto-layout pattern to an item if AutoLayout is enabled.
    /// </summary>
    /// <param name="item">The BzItem to apply pattern to</param>
    /// <param name="index">The item's index in the collection</param>
    /// <returns>BzItem with ColSpan and RowSpan applied</returns>
    private BzItem ApplyAutoLayout(BzItem item, int index)
    {
        if (!AutoLayout) return item;

        // Respect explicit spans set by user
        if (item.ColSpan > 1 || item.RowSpan > 1) return item;

        return Pattern switch
        {
            BentoLayoutPattern.Dynamic => ApplyDynamicPattern(item, index),
            BentoLayoutPattern.Featured => ApplyFeaturedPattern(item, index),
            BentoLayoutPattern.Balanced => ApplyBalancedPattern(item, index),
            _ => item
        };
    }

    /// <summary>
    /// Applies Dynamic pattern: varied mix of hero, tall, wide, and normal cards.
    /// </summary>
    /// <remarks>
    /// Pattern optimized for 4-column grid (repeating cycle of 8):
    /// Row 1-2: [0] 2x2 Hero + [1] 1x2 Tall + [2] 1x1 + [3] 1x1 = 4 cols
    /// Row 3:   [4] 2x1 Wide + [5] 1x1 + [6] 1x1 = 4 cols
    /// Row 4:   [7] 2x1 Wide + [8] 1x1 + [9] 1x1 = 4 cols (cycle continues)
    /// Uses grid-auto-flow: dense for optimal filling.
    /// </remarks>
    private static BzItem ApplyDynamicPattern(BzItem item, int index)
    {
        var patterns = new[]
        {
            (Col: 2, Row: 2), // 0 - Hero (takes 2x2)
            (Col: 1, Row: 2), // 1 - Tall (fills beside hero)
            (Col: 1, Row: 1), // 2 - Normal (top right of hero)
            (Col: 1, Row: 1), // 3 - Normal (below item 2)
            (Col: 2, Row: 1), // 4 - Wide
            (Col: 1, Row: 1), // 5 - Normal
            (Col: 1, Row: 1), // 6 - Normal
            (Col: 2, Row: 1), // 7 - Wide (starts new cycle feel)
        };

        var pattern = patterns[index % patterns.Length];
        var newItem = CloneBzItem(item);
        newItem.ColSpan = pattern.Col;
        newItem.RowSpan = pattern.Row;
        return newItem;
    }

    /// <summary>
    /// Applies Featured pattern: one large hero (2x2) at start, rest smaller.
    /// </summary>
    /// <remarks>
    /// First item is always 2x2 hero.
    /// Every 3rd item after is 1x2 tall, others are 1x1.
    /// </remarks>
    private static BzItem ApplyFeaturedPattern(BzItem item, int index)
    {
        var newItem = CloneBzItem(item);

        if (index == 0)
        {
            newItem.ColSpan = 2;
            newItem.RowSpan = 2;
        }
        else if (index % 3 == 0)
        {
            newItem.ColSpan = 1;
            newItem.RowSpan = 2;
        }
        else
        {
            newItem.ColSpan = 1;
            newItem.RowSpan = 1;
        }

        return newItem;
    }

    /// <summary>
    /// Applies Balanced pattern: mostly uniform with some wide and tall cards.
    /// </summary>
    /// <remarks>
    /// Pattern (repeating cycle of 6):
    /// [0] 2x1 Wide, [1-2] 1x1 Normal, [3] 1x2 Tall,
    /// [4] 2x1 Wide, [5] 1x1 Normal
    /// No 2x2 hero cards for more uniform appearance.
    /// </remarks>
    private static BzItem ApplyBalancedPattern(BzItem item, int index)
    {
        var patterns = new[]
        {
            (Col: 2, Row: 1), // 0 - Wide
            (Col: 1, Row: 1), // 1 - Normal
            (Col: 1, Row: 1), // 2 - Normal
            (Col: 1, Row: 2), // 3 - Tall
            (Col: 2, Row: 1), // 4 - Wide
            (Col: 1, Row: 1), // 5 - Normal
        };

        var pattern = patterns[index % patterns.Length];
        var newItem = CloneBzItem(item);
        newItem.ColSpan = pattern.Col;
        newItem.RowSpan = pattern.Row;
        return newItem;
    }

    /// <summary>
    /// Creates a shallow clone of a BzItem for pattern modification.
    /// </summary>
    private static BzItem CloneBzItem(BzItem item)
    {
        return new BzItem
        {
            ImageUrl = item.ImageUrl,
            Title = item.Title,
            Description = item.Description,
            OriginalItem = item.OriginalItem,
            ColSpan = item.ColSpan,
            RowSpan = item.RowSpan,
            Order = item.Order
        };
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
