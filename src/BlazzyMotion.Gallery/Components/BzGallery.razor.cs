using BlazzyMotion.Core.Abstractions;
using BlazzyMotion.Core.Models;
using BlazzyMotion.Core.Services;
using BlazzyMotion.Gallery.Models;
using BlazzyMotion.Gallery.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazzyMotion.Gallery.Components;

/// <summary>
/// A premium image gallery component for Blazor with Grid, Masonry, and List layouts.
/// </summary>
/// <typeparam name="TItem">The type of items to display</typeparam>
public partial class BzGallery<TItem> : BzComponentBase where TItem : class
{
    #region Injected Services

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = default!;

    #endregion

    #region Parameters

    /// <summary>
    /// Collection of items to display in the gallery.
    /// </summary>
    [Parameter]
    public IEnumerable<TItem>? Items { get; set; }

    /// <summary>
    /// Layout mode for the gallery.
    /// </summary>
    [Parameter]
    public BzGalleryLayout Layout { get; set; } = BzGalleryLayout.Grid;

    /// <summary>
    /// Number of columns for Grid and Masonry layouts (1-6).
    /// </summary>
    [Parameter]
    public int Columns { get; set; } = 3;

    /// <summary>
    /// Gap between items in pixels.
    /// </summary>
    [Parameter]
    public int Gap { get; set; } = 16;

    /// <summary>
    /// Image aspect ratio for Grid mode (e.g. "4/3", "1/1", "16/9").
    /// Only applies to Grid layout. Masonry preserves original aspect ratios.
    /// </summary>
    [Parameter]
    public string? AspectRatio { get; set; }

    /// <summary>
    /// Whether the fullscreen lightbox is enabled.
    /// </summary>
    [Parameter]
    public bool EnableLightbox { get; set; } = true;

    /// <summary>
    /// Whether the category filter bar is shown.
    /// </summary>
    [Parameter]
    public bool EnableFilter { get; set; }

    /// <summary>
    /// Function to extract category from the original item.
    /// Required when EnableFilter is true.
    /// </summary>
    [Parameter]
    public Func<TItem, string>? CategorySelector { get; set; }

    /// <summary>
    /// Whether to enable staggered entry animations.
    /// </summary>
    [Parameter]
    public bool AnimationEnabled { get; set; } = true;

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

    /// <summary>
    /// Callback when an item is clicked (if lightbox is disabled).
    /// </summary>
    [Parameter]
    public EventCallback<TItem> OnItemSelected { get; set; }

    #endregion

    #region Private Fields

    private ElementReference _galleryRef;
    private BzGalleryJsInterop? _jsInterop;
    private DotNetObjectReference<BzGallery<TItem>>? _dotNetRef;
    private List<BzItem>? MappedItems;
    private bool _isInitialized;

    private bool _lightboxOpen;
    private bool _lightboxJustOpened;
    private int _lightboxIndex;

    private string? _activeCategory;
    private List<string> _categories = new();
    private Dictionary<BzItem, string?> _itemCategories = new();

    #endregion

    #region Computed Properties

    private bool IsLoading => !_isInitialized && Items != null && MappedItems == null;
    private bool IsEmpty => MappedItems is null or { Count: 0 };

    private string LayoutClass => Layout switch
    {
        BzGalleryLayout.Grid => "grid",
        BzGalleryLayout.Masonry => "masonry",
        BzGalleryLayout.List => "list",
        _ => "grid"
    };

    private List<BzItem>? FilteredItems
    {
        get
        {
            if (MappedItems == null) return null;
            if (string.IsNullOrEmpty(_activeCategory)) return MappedItems;

            return MappedItems
                .Where(item => _itemCategories.TryGetValue(item, out var cat) && cat == _activeCategory)
                .ToList();
        }
    }

    private bool HasNoFilterResults =>
        !string.IsNullOrEmpty(_activeCategory) && FilteredItems is { Count: 0 };

    #endregion

    #region Lifecycle

    protected override void OnParametersSet()
    {
        if (!EnableFilter)
        {
            _activeCategory = null;
        }

        if (Items != null)
        {
            var itemsList = Items.ToList();
            MappedItems = BzRegistry.ToBzItems(itemsList).ToList();

            if (EnableFilter && CategorySelector != null && MappedItems.Count > 0)
            {
                _itemCategories.Clear();
                _categories.Clear();

                var categorySet = new HashSet<string>();

                for (int i = 0; i < MappedItems.Count && i < itemsList.Count; i++)
                {
                    var category = CategorySelector(itemsList[i]);

                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        _itemCategories[MappedItems[i]] = category;
                        categorySet.Add(category);
                    }
                }

                _categories = categorySet.OrderBy(c => c).ToList();
            }
            else if (EnableFilter && CategorySelector == null)
            {
                _itemCategories.Clear();
                _categories.Clear();

                var categorySet = new HashSet<string>();

                foreach (var item in MappedItems)
                {
                    var category = item.Description;
                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        _itemCategories[item] = category;
                        categorySet.Add(category);
                    }
                }

                _categories = categorySet.OrderBy(c => c).ToList();
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !IsDisposed)
        {
            _jsInterop = new BzGalleryJsInterop(JsRuntime);
            _dotNetRef = DotNetObjectReference.Create(this);

            if (!IsEmpty)
            {
                await InitializeGalleryAsync();
            }

            _isInitialized = true;
        }

        if (_lightboxJustOpened && !IsDisposed && _jsInterop != null)
        {
            _lightboxJustOpened = false;
            await _jsInterop.FocusLightboxAsync();
        }
    }

    private async Task InitializeGalleryAsync()
    {
        if (_jsInterop is null || IsDisposed) return;

        var options = new BzGalleryOptions
        {
            Layout = LayoutClass,
            Columns = Math.Clamp(Columns, 1, 6),
            Gap = Gap,
            EnableLightbox = EnableLightbox,
            AnimationEnabled = AnimationEnabled,
            AspectRatio = AspectRatio
        };

        await _jsInterop.InitializeAsync(_galleryRef, options, _dotNetRef);
    }

    #endregion

    #region JS Callbacks

    [JSInvokable]
    public async Task OnGalleryInitializedFromJS(int itemCount)
    {
        if (IsDisposed) return;
        await InvokeStateHasChangedAsync();
    }

    #endregion

    #region Lightbox

    private async Task OnItemClick(int index)
    {
        if (MappedItems == null || index >= MappedItems.Count) return;

        var item = MappedItems[index];

        // Ignore clicks on hidden (filtered out) items
        if (!string.IsNullOrEmpty(_activeCategory) && GetItemCategory(item) != _activeCategory)
            return;

        if (EnableLightbox)
        {
            // Map MappedItems index to FilteredItems index for lightbox navigation
            var filteredIndex = FilteredItems?.IndexOf(item) ?? -1;
            if (filteredIndex < 0) return;

            _lightboxIndex = filteredIndex;
            _lightboxOpen = true;
            _lightboxJustOpened = true;
            if (_jsInterop != null) await _jsInterop.LockBodyScrollAsync();
            StateHasChanged();
        }
        else if (OnItemSelected.HasDelegate)
        {
            var original = item.GetOriginal<TItem>();
            if (original != null)
            {
                await OnItemSelected.InvokeAsync(original);
            }
        }
    }

    private async Task CloseLightbox()
    {
        _lightboxOpen = false;
        if (_jsInterop != null) await _jsInterop.UnlockBodyScrollAsync();
        StateHasChanged();
    }

    private void OnLightboxIndexChanged(int newIndex)
    {
        _lightboxIndex = newIndex;
        StateHasChanged();
    }

    #endregion

    #region Filtering

    private async Task SetFilter(string? category)
    {
        // Lock grid height before re-render to prevent layout jump
        if (_jsInterop != null)
        {
            await _jsInterop.PrepareFilterAsync(_galleryRef);
        }

        _activeCategory = category;
        StateHasChanged();

        if (_jsInterop != null)
        {
            await _jsInterop.FilterAsync(_galleryRef, category);

            if (Layout == BzGalleryLayout.Masonry)
            {
                await _jsInterop.RecalculateMasonryAsync(_galleryRef);
            }
        }
    }

    private string? GetItemCategory(BzItem item)
    {
        return _itemCategories.TryGetValue(item, out var cat) ? cat : null;
    }

    private string GetItemVisibilityClass(BzItem item)
    {
        if (string.IsNullOrEmpty(_activeCategory)) return "";
        var cat = GetItemCategory(item);
        return cat == _activeCategory ? "" : "bzg-item-hidden";
    }

    #endregion

    #region CSS Helpers

    private string GetContainerClass()
    {
        var baseClass = _isInitialized ? "bzg-container bzg-ready" : "bzg-container";
        return GetCombinedClass(baseClass);
    }

    private string GetContainerStyle()
    {
        if (!_isInitialized)
        {
            return "opacity:0; visibility:hidden";
        }

        return string.Empty;
    }

    private string GetGridClass()
    {
        var classes = $"bzg-grid bzg-layout-{LayoutClass}";
        if (_isInitialized && !AnimationEnabled)
        {
            classes += " bzg-no-animation";
        }

        return classes;
    }

    private string GetItemClass(BzItem item)
    {
        var classes = _isInitialized ? "bzg-item bzg-animate" : "bzg-item";
        var visibility = GetItemVisibilityClass(item);
        if (!string.IsNullOrEmpty(visibility))
        {
            classes += $" {visibility}";
        }

        return classes;
    }

    private string GetGridStyle()
    {
        var styles = new List<string>
        {
            $"--bzg-columns: {Math.Clamp(Columns, 1, 6)}",
            $"--bzg-gap: {Gap}px"
        };

        if (!string.IsNullOrWhiteSpace(AspectRatio) && Layout == BzGalleryLayout.Grid)
        {
            styles.Add($"--bzg-aspect-ratio: {AspectRatio}");
        }

        return string.Join("; ", styles);
    }

    #endregion

    #region Disposal

    protected override async ValueTask DisposeAsyncCore()
    {
        if (_lightboxOpen && _jsInterop != null)
        {
            await _jsInterop.UnlockBodyScrollAsync();
        }

        _lightboxOpen = false;

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
