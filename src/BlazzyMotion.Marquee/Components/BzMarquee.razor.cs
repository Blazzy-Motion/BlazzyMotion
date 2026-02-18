using BlazzyMotion.Core.Abstractions;
using BlazzyMotion.Core.Models;
using BlazzyMotion.Core.Services;
using BlazzyMotion.Marquee.Models;
using BlazzyMotion.Marquee.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazzyMotion.Marquee.Components;

/// <summary>
/// A CSS-driven infinite scrolling marquee for Blazor with logo bars, testimonials and text tickers.
/// </summary>
/// <typeparam name="TItem">The type of items to display</typeparam>
public partial class BzMarquee<TItem> : BzComponentBase where TItem : class
{
    [Inject]
    private IJSRuntime JsRuntime { get; set; } = default!;

    #region Parameters

    /// <summary>
    /// Collection of items to display in the marquee.
    /// </summary>
    [Parameter]
    public IEnumerable<TItem>? Items { get; set; }

    /// <summary>
    /// Plain text content for text ticker mode. When set, Items are ignored.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Scroll direction of the marquee.
    /// </summary>
    [Parameter]
    public BzDirection Direction { get; set; } = BzDirection.Left;

    /// <summary>
    /// Animation speed in pixels per second.
    /// </summary>
    [Parameter]
    public int Speed { get; set; } = 50;

    /// <summary>
    /// Gap between items in pixels.
    /// </summary>
    [Parameter]
    public int Gap { get; set; } = 40;

    /// <summary>
    /// Whether to pause animation on mouse hover.
    /// </summary>
    [Parameter]
    public bool PauseOnHover { get; set; } = true;

    /// <summary>
    /// Whether to show gradient fade on edges.
    /// </summary>
    [Parameter]
    public bool ShowGradientEdges { get; set; } = true;

    /// <summary>
    /// Whether the marquee spans full viewport width.
    /// </summary>
    [Parameter]
    public bool FullWidth { get; set; }

    /// <summary>
    /// Number of rows to display (1-10).
    /// </summary>
    [Parameter]
    public int Rows { get; set; } = 1;

    /// <summary>
    /// Whether adjacent rows scroll in opposite directions.
    /// </summary>
    [Parameter]
    public bool AlternateDirection { get; set; } = true;

    /// <summary>
    /// Speed variation factor between rows (0.0-0.5). Deterministic, SSR-safe.
    /// </summary>
    [Parameter]
    public double SpeedVariation { get; set; }

    /// <summary>
    /// Whether to enable staggered entrance animation on first appearance.
    /// </summary>
    [Parameter]
    public bool StaggerEntrance { get; set; } = true;

    /// <summary>
    /// Delay between each item's entrance animation in milliseconds.
    /// </summary>
    [Parameter]
    public int StaggerDelay { get; set; } = 60;

    /// <summary>
    /// Callback when a marquee item is clicked. Optional — items are not clickable unless this is set.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> OnItemClick { get; set; }

    /// <summary>
    /// Custom template for rendering each item.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    /// <summary>
    /// Custom loading template.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingTemplate { get; set; }

    /// <summary>
    /// Custom empty state template.
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyTemplate { get; set; }

    #endregion

    #region Private Fields

    private ElementReference _marqueeRef;
    private BzMarqueeJsInterop? _jsInterop;
    private DotNetObjectReference<BzMarquee<TItem>>? _dotNetRef;
    private IReadOnlyList<BzItem>? MappedItems;
    private bool _isInitialized;
    private bool _needsReInit;
    private bool _isPausedByKeyboard;
    private string _srAnnouncement = string.Empty;

    private BzDirection _prevDirection;
    private int _prevSpeed;
    private int _prevRows;
    private double _prevSpeedVariation;
    private bool _prevAlternateDirection;
    private bool _prevStaggerEntrance;
    private int _prevStaggerDelay;

    #endregion

    #region Computed Properties

    private bool IsLoading => !_isInitialized && Items != null && MappedItems == null;
    private bool IsEmpty => MappedItems is null or { Count: 0 };
    private bool IsReverse => Direction is BzDirection.Right;
    private int EffectiveRows => Math.Clamp(Rows, 1, 10);
    private double EffectiveSpeedVariation => Math.Clamp(SpeedVariation, 0.0, 0.5);

    #endregion

    #region Lifecycle

    protected override void OnParametersSet()
    {
        if (Items != null)
        {
            MappedItems = BzRegistry.ToBzItems(Items);
        }

        if (_isInitialized && HasParametersChanged())
        {
            _needsReInit = true;
        }

        SnapshotParameters();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !IsDisposed)
        {
            _jsInterop = new BzMarqueeJsInterop(JsRuntime);
            _dotNetRef = DotNetObjectReference.Create(this);

            if (!IsEmpty || !string.IsNullOrWhiteSpace(Text))
            {
                await InitializeMarqueeAsync();
            }

            _isInitialized = true;
            StateHasChanged();
        }
        else if (_needsReInit && !IsDisposed)
        {
            _needsReInit = false;
            await InitializeMarqueeAsync();
        }
    }

    private bool HasParametersChanged() =>
        _prevDirection != Direction ||
        _prevSpeed != Speed ||
        _prevRows != Rows ||
        Math.Abs(_prevSpeedVariation - SpeedVariation) > 0.001 ||
        _prevAlternateDirection != AlternateDirection ||
        _prevStaggerEntrance != StaggerEntrance ||
        _prevStaggerDelay != StaggerDelay;

    private void SnapshotParameters()
    {
        _prevDirection = Direction;
        _prevSpeed = Speed;
        _prevRows = Rows;
        _prevSpeedVariation = SpeedVariation;
        _prevAlternateDirection = AlternateDirection;
        _prevStaggerEntrance = StaggerEntrance;
        _prevStaggerDelay = StaggerDelay;
    }

    private async Task InitializeMarqueeAsync()
    {
        if (_jsInterop is null || IsDisposed) return;

        var options = new BzMarqueeOptions
        {
            Direction = Direction.ToString().ToLowerInvariant(),
            Speed = Speed,
            Gap = Gap,
            PauseOnHover = PauseOnHover,
            ShowGradientEdges = ShowGradientEdges,
            FullWidth = FullWidth,
            Reverse = IsReverse,
            StaggerEntrance = StaggerEntrance,
            StaggerDelay = StaggerDelay
        };

        await _jsInterop.InitializeAsync(_marqueeRef, options, _dotNetRef);
    }

    #endregion

    #region JS Callbacks

    /// <summary>
    /// Called from JS when marquee initialization completes.
    /// </summary>
    [JSInvokable]
    public async Task OnMarqueeInitializedFromJS()
    {
        if (IsDisposed) return;
        await InvokeAsync(StateHasChanged);
    }

    #endregion

    #region Keyboard Handling

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key is not (" " or "Enter")) return;

        _isPausedByKeyboard = !_isPausedByKeyboard;
        _srAnnouncement = _isPausedByKeyboard ? "Marquee paused" : "Marquee playing";

        if (_jsInterop is not null && !IsDisposed)
        {
            if (_isPausedByKeyboard)
                await _jsInterop.PauseAsync(_marqueeRef);
            else
                await _jsInterop.ResumeAsync(_marqueeRef);
        }
    }

    private async Task HandleItemClick(TItem item)
    {
        if (OnItemClick.HasDelegate)
            await OnItemClick.InvokeAsync(item);
    }

    #endregion

    #region Row Helpers

    private BzDirection GetRowDirection(int rowIndex)
    {
        if (!AlternateDirection || rowIndex % 2 == 0)
            return Direction;

        return Direction == BzDirection.Left ? BzDirection.Right : BzDirection.Left;
    }

    private int GetRowSpeed(int rowIndex)
    {
        if (EffectiveSpeedVariation <= 0.0 || EffectiveRows <= 1)
            return Speed;

        var factor = rowIndex % 2 == 0
            ? 1.0 + (EffectiveSpeedVariation * (rowIndex + 1) / EffectiveRows)
            : 1.0 - (EffectiveSpeedVariation * (rowIndex + 1) / EffectiveRows);

        return Math.Max(10, (int)(Speed * factor));
    }

    private IReadOnlyList<BzItem> GetRowItems(int rowIndex)
    {
        if (MappedItems is null or { Count: 0 })
            return Array.Empty<BzItem>();

        var offset = (MappedItems.Count / EffectiveRows) * rowIndex;
        var result = new List<BzItem>(MappedItems.Count);
        for (var i = 0; i < MappedItems.Count; i++)
        {
            result.Add(MappedItems[(i + offset) % MappedItems.Count]);
        }

        return result;
    }

    #endregion

    #region CSS Helpers

    private string GetContainerClass()
    {
        var classes = new List<string> { "bzm-container", ThemeClass };

        if (_isInitialized) classes.Add("bzm-ready");
        if (FullWidth) classes.Add("bzm-full-width");
        if (PauseOnHover) classes.Add("bzm-pause-hover");
        if (ShowGradientEdges) classes.Add("bzm-has-gradient");
        if (!string.IsNullOrWhiteSpace(Text)) classes.Add("bzm-ticker");
        if (_isPausedByKeyboard) classes.Add("bzm-paused");
        if (EffectiveRows > 1) classes.Add("bzm-multirow");
        if (StaggerEntrance) classes.Add("bzm-stagger");
        if (OnItemClick.HasDelegate) classes.Add("bzm-clickable");
        if (!string.IsNullOrWhiteSpace(CssClass)) classes.Add(CssClass);

        return string.Join(" ", classes);
    }

    private string GetAriaLabel()
    {
        string baseLabel;

        if (!string.IsNullOrWhiteSpace(Text))
        {
            baseLabel = "Scrolling text marquee";
        }
        else
        {
            var count = MappedItems?.Count ?? 0;
            var rowInfo = EffectiveRows > 1 ? $" in {EffectiveRows} rows" : "";
            baseLabel = $"Scrolling content marquee with {count} items{rowInfo}";
        }

        return _isPausedByKeyboard
            ? $"{baseLabel}, paused. Press Space to resume"
            : $"{baseLabel}. Press Space to pause";
    }

    private static string GetInitials(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "?";
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[^1][0]}"
            : $"{parts[0][0]}";
    }

    private string GetContainerStyle()
    {
        var style = $"--bzm-gap: {Gap}px";
        if (EffectiveRows > 1)
            style += $"; --bzm-row-gap: {Math.Max(4, Gap / 3)}px";

        if (!_isInitialized)
            style += "; opacity:0; visibility:hidden";

        return style;
    }

    #endregion

    #region Disposal

    protected override async ValueTask DisposeAsyncCore()
    {
        if (_jsInterop != null)
        {
            await _jsInterop.DisposeAsync();
            _jsInterop = null;
        }

        _dotNetRef?.Dispose();
        _dotNetRef = null;
    }

    #endregion
}
