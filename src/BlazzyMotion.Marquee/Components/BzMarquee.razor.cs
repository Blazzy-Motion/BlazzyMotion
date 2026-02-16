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
    public int Height { get; set; } = 300;

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

    #endregion

    #region Computed Properties

    private bool IsLoading => !_isInitialized && Items != null && MappedItems == null;
    private bool IsEmpty => MappedItems is null or { Count: 0 };
    private bool IsVertical => Direction is BzDirection.Up or BzDirection.Down;
    private bool IsReverse => Direction is BzDirection.Right or BzDirection.Down;

    #endregion

    #region Lifecycle

    protected override void OnParametersSet()
    {
        if (Items != null)
        {
            MappedItems = BzRegistry.ToBzItems(Items);
        }
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
        }
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
            Vertical = IsVertical,
            Reverse = IsReverse,
            ContainerHeight = Height
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

        if (IsVertical) classes.Add("bzm-vertical");
        if (FullWidth) classes.Add("bzm-full-width");
        if (PauseOnHover) classes.Add("bzm-pause-hover");
        if (ShowGradientEdges) classes.Add("bzm-has-gradient");
        if (!string.IsNullOrWhiteSpace(Text)) classes.Add("bzm-ticker");
        if (!string.IsNullOrWhiteSpace(CssClass)) classes.Add(CssClass);

        return string.Join(" ", classes);
    }

    private string GetContainerStyle()
    {
        var styles = new List<string>();

        if (!_isInitialized)
            styles.Add("opacity:0; visibility:hidden");

        styles.Add($"--bzm-gap: {Gap}px");

        if (IsVertical)
            styles.Add($"--bzm-container-height: {Height}px");

        return string.Join("; ", styles);
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
