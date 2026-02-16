using BlazzyMotion.Core.Abstractions;
using BlazzyMotion.Core.Models;
using BlazzyMotion.Core.Services;
using BlazzyMotion.Marquee.Models;
using BlazzyMotion.Marquee.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazzyMotion.Marquee.Components;

public partial class BzMarquee<TItem> : BzComponentBase where TItem : class
{
    [Inject]
    private IJSRuntime JsRuntime { get; set; } = default!;

    #region Parameters

    [Parameter]
    public IEnumerable<TItem>? Items { get; set; }

    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    public BzDirection Direction { get; set; } = BzDirection.Left;

    [Parameter]
    public int Speed { get; set; } = 50;

    [Parameter]
    public int Gap { get; set; } = 40;

    [Parameter]
    public bool PauseOnHover { get; set; } = true;

    [Parameter]
    public bool ShowGradientEdges { get; set; } = true;

    [Parameter]
    public bool FullWidth { get; set; }

    [Parameter]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    [Parameter]
    public RenderFragment? LoadingTemplate { get; set; }

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

    // Previous parameter values for JS re-init detection
    private BzDirection _prevDirection;
    private int _prevSpeed;

    #endregion

    #region Computed Properties

    private bool IsLoading => !_isInitialized && Items != null && MappedItems == null;
    private bool IsEmpty => MappedItems is null or { Count: 0 };
    private bool IsReverse => Direction is BzDirection.Right;

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
        _prevSpeed != Speed;

    private void SnapshotParameters()
    {
        _prevDirection = Direction;
        _prevSpeed = Speed;
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
            Reverse = IsReverse
        };

        await _jsInterop.InitializeAsync(_marqueeRef, options, _dotNetRef);
    }

    #endregion

    #region JS Callbacks

    [JSInvokable]
    public async Task OnMarqueeInitializedFromJS()
    {
        if (IsDisposed) return;
        await InvokeAsync(StateHasChanged);
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
        if (!string.IsNullOrWhiteSpace(CssClass)) classes.Add(CssClass);

        return string.Join(" ", classes);
    }

    private string GetAriaLabel()
    {
        if (!string.IsNullOrWhiteSpace(Text))
            return "Scrolling text marquee";

        var count = MappedItems?.Count ?? 0;
        return $"Scrolling content marquee with {count} items";
    }

    private static string GetInitials(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "?";
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[^1][0]}"
            : $"{parts[0][0]}";
    }

    private string GetContainerStyle() => $"--bzm-gap: {Gap}px";

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
