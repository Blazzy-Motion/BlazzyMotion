using BlazzyMotion.Gallery.Models;
using Microsoft.AspNetCore.Components.Web;

namespace BlazzyMotion.Tests.Gallery.Components;

/// <summary>
/// Advanced tests for BzGallery: keyboard navigation, JSInvokable callbacks (early-exit paths),
/// ARIA labels, accessibility attributes, layout rendering, and disposal edge cases.
/// All tests are fast and do NOT depend on JS interop or reflection.
/// </summary>
public class BzGalleryAdvancedTests : TestBase
{
  private static List<TestGalleryPhoto> CreatePhotos(int count = 3) =>
      Enumerable.Range(1, count).Select(i => new TestGalleryPhoto
      {
        Id = i,
        ImageUrl = $"https://example.com/photo{i}.jpg",
        Title = $"Photo {i}",
        Description = $"Description {i}",
        Category = i % 2 == 0 ? "Nature" : "City"
      }).ToList();

  private static List<TestGalleryPhoto> CreatePhotosWithCategories() => new()
    {
        new() { Id = 1, ImageUrl = "https://example.com/1.jpg", Title = "Nature 1", Description = "Landscape", Category = "Nature" },
        new() { Id = 2, ImageUrl = "https://example.com/2.jpg", Title = "City 1", Description = "Urban", Category = "City" },
        new() { Id = 3, ImageUrl = "https://example.com/3.jpg", Title = "Nature 2", Description = "Landscape", Category = "Nature" },
        new() { Id = 4, ImageUrl = "https://example.com/4.jpg", Title = "City 2", Description = "Urban", Category = "City" },
        new() { Id = 5, ImageUrl = "https://example.com/5.jpg", Title = "Abstract 1", Description = "Art", Category = "Abstract" },
    };

  #region Keyboard Navigation (EnableLightbox=false)

  [Fact]
  public void OnItemKeyDown_Enter_WithLightboxDisabled_ShouldFireOnItemSelected()
  {
    TestGalleryPhoto? selectedItem = null;
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, false)
        .Add(p => p.OnItemSelected, EventCallback.Factory.Create<TestGalleryPhoto>(this, item => selectedItem = item)));

    var secondItem = cut.FindAll(".bzg-item")[1];
    secondItem.KeyDown(new KeyboardEventArgs { Key = "Enter" });

    selectedItem.Should().NotBeNull();
    selectedItem!.Id.Should().Be(2);
  }

  [Fact]
  public void OnItemKeyDown_Space_WithLightboxDisabled_ShouldFireOnItemSelected()
  {
    TestGalleryPhoto? selectedItem = null;
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, false)
        .Add(p => p.OnItemSelected, EventCallback.Factory.Create<TestGalleryPhoto>(this, item => selectedItem = item)));

    var firstItem = cut.FindAll(".bzg-item")[0];
    firstItem.KeyDown(new KeyboardEventArgs { Key = " " });

    selectedItem.Should().NotBeNull();
    selectedItem!.Id.Should().Be(1);
  }

  [Fact]
  public void OnItemKeyDown_Tab_ShouldNotTriggerAnyAction()
  {
    TestGalleryPhoto? selectedItem = null;
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, false)
        .Add(p => p.OnItemSelected, EventCallback.Factory.Create<TestGalleryPhoto>(this, item => selectedItem = item)));

    var firstItem = cut.FindAll(".bzg-item")[0];
    firstItem.KeyDown(new KeyboardEventArgs { Key = "Tab" });

    selectedItem.Should().BeNull();
  }

  [Fact]
  public void OnItemKeyDown_Escape_ShouldNotTriggerAnyAction()
  {
    TestGalleryPhoto? selectedItem = null;
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, false)
        .Add(p => p.OnItemSelected, EventCallback.Factory.Create<TestGalleryPhoto>(this, item => selectedItem = item)));

    var firstItem = cut.FindAll(".bzg-item")[0];
    firstItem.KeyDown(new KeyboardEventArgs { Key = "Escape" });

    selectedItem.Should().BeNull();
  }

  [Fact]
  public void OnItemKeyDown_Enter_OnLastItem_ShouldFireWithCorrectItem()
  {
    TestGalleryPhoto? selectedItem = null;
    var items = CreatePhotos(5);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, false)
        .Add(p => p.OnItemSelected, EventCallback.Factory.Create<TestGalleryPhoto>(this, item => selectedItem = item)));

    var lastItem = cut.FindAll(".bzg-item")[4];
    lastItem.KeyDown(new KeyboardEventArgs { Key = "Enter" });

    selectedItem.Should().NotBeNull();
    selectedItem!.Id.Should().Be(5);
  }

  [Fact]
  public void OnItemKeyDown_WithNoOnItemSelected_ShouldNotThrow()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, false));

    var firstItem = cut.FindAll(".bzg-item")[0];

    var action = () => firstItem.KeyDown(new KeyboardEventArgs { Key = "Enter" });
    action.Should().NotThrow();
  }

  #endregion

  #region JSInvokable Early-Return Paths

  [Fact]
  public async Task SwipePrev_WithClosedLightbox_ShouldDoNothing()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, true));

    await cut.InvokeAsync(() => cut.Instance.SwipePrev());

    cut.Markup.Should().NotContain("bzg-lightbox-open");
  }

  [Fact]
  public async Task SwipeNext_WithClosedLightbox_ShouldDoNothing()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, true));

    await cut.InvokeAsync(() => cut.Instance.SwipeNext());

    cut.Markup.Should().NotContain("bzg-lightbox-open");
  }

  [Fact]
  public async Task SwipePrev_AfterDispose_ShouldNotThrow()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, true));

    await cut.Instance.DisposeAsync();

    var action = async () => await cut.Instance.SwipePrev();
    await action.Should().NotThrowAsync();
  }

  [Fact]
  public async Task SwipeNext_AfterDispose_ShouldNotThrow()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, true));

    await cut.Instance.DisposeAsync();

    var action = async () => await cut.Instance.SwipeNext();
    await action.Should().NotThrowAsync();
  }

  [Fact]
  public async Task OnGalleryInitializedFromJS_ShouldNotThrow()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    var action = async () => await cut.InvokeAsync(() => cut.Instance.OnGalleryInitializedFromJS(3));
    await action.Should().NotThrowAsync();
  }

  [Fact]
  public async Task OnGalleryInitializedFromJS_AfterDispose_ShouldNotThrow()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    await cut.Instance.DisposeAsync();

    var action = async () => await cut.Instance.OnGalleryInitializedFromJS(5);
    await action.Should().NotThrowAsync();
  }

  [Fact]
  public async Task OnGalleryInitializedFromJS_WithZeroItems_ShouldNotThrow()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    var action = async () => await cut.InvokeAsync(() => cut.Instance.OnGalleryInitializedFromJS(0));
    await action.Should().NotThrowAsync();
  }

  #endregion

  #region ARIA Label Tests

  [Fact]
  public void ItemWithTitle_LightboxEnabled_ShouldHaveCorrectAriaLabel()
  {
    var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "test.jpg", Title = "Beautiful Sunset" }
        };

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, true));

    var item = cut.Find(".bzg-item");
    item.GetAttribute("aria-label").Should().Contain("Beautiful Sunset");
    item.GetAttribute("aria-label").Should().Contain("press Enter to open lightbox");
  }

  [Fact]
  public void ItemWithoutTitle_ShouldHaveFallbackAriaLabel()
  {
    var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "test.jpg" }
        };

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, true));

    var item = cut.Find(".bzg-item");
    item.GetAttribute("aria-label").Should().Contain("Gallery image 1");
  }

  [Fact]
  public void ItemWithLightboxDisabled_ShouldNotMentionLightboxInAriaLabel()
  {
    var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "test.jpg", Title = "Sunset" }
        };

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, false));

    var item = cut.Find(".bzg-item");
    item.GetAttribute("aria-label").Should().Contain("Sunset");
    item.GetAttribute("aria-label").Should().NotContain("lightbox");
  }

  [Fact]
  public void MultipleItems_ShouldHaveSequentialAriaLabels()
  {
    var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "1.jpg" },
            new() { Id = 2, ImageUrl = "2.jpg" },
            new() { Id = 3, ImageUrl = "3.jpg" }
        };

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, false));

    var galleryItems = cut.FindAll(".bzg-item");
    galleryItems[0].GetAttribute("aria-label").Should().Contain("Gallery image 1");
    galleryItems[1].GetAttribute("aria-label").Should().Contain("Gallery image 2");
    galleryItems[2].GetAttribute("aria-label").Should().Contain("Gallery image 3");
  }

  #endregion

  #region Gallery Item Accessibility

  [Fact]
  public void Items_ShouldHaveButtonRole()
  {
    var items = CreatePhotos(2);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    var galleryItems = cut.FindAll(".bzg-item");
    foreach (var item in galleryItems)
    {
      item.GetAttribute("role").Should().Be("button");
    }
  }

  [Fact]
  public void Items_ShouldHaveTabindex()
  {
    var items = CreatePhotos(2);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    var galleryItems = cut.FindAll(".bzg-item");
    foreach (var item in galleryItems)
    {
      item.GetAttribute("tabindex").Should().Be("0");
    }
  }

  [Fact]
  public void Items_ShouldHaveDataCategoryAttribute()
  {
    var items = CreatePhotosWithCategories();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableFilter, true)
        .Add(p => p.CategorySelector, p => p.Category!));

    var galleryItems = cut.FindAll(".bzg-item");
    galleryItems.Should().HaveCount(5);
  }

  #endregion

  #region Animation CSS Class Tests

  [Fact]
  public void AnimationEnabled_ShouldNotHaveNoAnimationClass()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.AnimationEnabled, true));

    cut.Markup.Should().NotContain("bzg-no-animation");
  }

  [Fact]
  public void AnimationDisabled_GridClassShouldStillRender()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.AnimationEnabled, false));

    cut.Markup.Should().Contain("bzg-grid");
  }

  #endregion

  #region Layout Classes

  [Fact]
  public void InvalidLayout_ShouldDefaultToGrid()
  {
    var items = CreatePhotos(1);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Layout, (BzGalleryLayout)99));

    cut.Find(".bzg-grid").ClassList.Should().Contain("bzg-layout-grid");
  }

  [Fact]
  public void MasonryLayout_ShouldHaveMasonryClass()
  {
    var items = CreatePhotos(1);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Layout, BzGalleryLayout.Masonry));

    cut.Find(".bzg-grid").ClassList.Should().Contain("bzg-layout-masonry");
  }

  [Fact]
  public void ListLayout_ShouldHaveListClass()
  {
    var items = CreatePhotos(1);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Layout, BzGalleryLayout.List));

    cut.Find(".bzg-grid").ClassList.Should().Contain("bzg-layout-list");
  }

  #endregion

  #region List Layout Description Overlay

  [Fact]
  public void ListLayout_WithDescription_ShouldRenderOverlayDescription()
  {
    var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "test.jpg", Title = "Photo", Description = "A beautiful sunset" }
        };

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Layout, BzGalleryLayout.List));

    cut.Markup.Should().Contain("bzg-overlay-desc");
    cut.Markup.Should().Contain("A beautiful sunset");
  }

  [Fact]
  public void GridLayout_WithDescription_ShouldNotRenderOverlayDescription()
  {
    var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "test.jpg", Title = "Photo", Description = "A beautiful sunset" }
        };

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Layout, BzGalleryLayout.Grid));

    cut.Markup.Should().NotContain("bzg-overlay-desc");
  }

  [Fact]
  public void ListLayout_WithTitleAndDescription_ShouldRenderListInfo()
  {
    var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "test.jpg", Title = "My Title", Description = "My Desc" }
        };

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Layout, BzGalleryLayout.List));

    cut.Markup.Should().Contain("bzg-list-info");
    cut.Markup.Should().Contain("bzg-list-title");
    cut.Markup.Should().Contain("bzg-list-desc");
  }

  [Fact]
  public void ListLayout_WithOnlyTitle_ShouldRenderListInfoWithoutDesc()
  {
    var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "test.jpg", Title = "My Title" }
        };

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Layout, BzGalleryLayout.List));

    cut.Markup.Should().Contain("bzg-list-info");
    cut.Markup.Should().Contain("bzg-list-title");
    cut.Markup.Should().NotContain("bzg-list-desc");
  }

  [Fact]
  public void MasonryLayout_WithDescription_ShouldNotRenderOverlayDescription()
  {
    var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "test.jpg", Title = "Photo", Description = "A beautiful sunset" }
        };

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Layout, BzGalleryLayout.Masonry));

    cut.Markup.Should().NotContain("bzg-overlay-desc");
  }

  #endregion

  #region Filter Bar ARIA (Rendering Only)

  [Fact]
  public void FilterBar_ShouldHaveToolbarRole()
  {
    var items = CreatePhotosWithCategories();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableFilter, true)
        .Add(p => p.CategorySelector, p => p.Category!));

    var filterBar = cut.Find(".bzg-filter-bar");
    filterBar.GetAttribute("role").Should().Be("toolbar");
    filterBar.GetAttribute("aria-label").Should().Be("Filter by category");
  }

  [Fact]
  public void FilterBar_AllButton_ShouldBeActiveByDefault()
  {
    var items = CreatePhotosWithCategories();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableFilter, true)
        .Add(p => p.CategorySelector, p => p.Category!));

    var allButton = cut.FindAll(".bzg-filter-btn").First(b => b.TextContent.Trim() == "All");
    allButton.GetAttribute("aria-pressed").Should().Be("true");
    allButton.ClassList.Should().Contain("bzg-filter-active");
  }

  [Fact]
  public void FilterBar_CategoryButtons_ShouldNotBeActiveByDefault()
  {
    var items = CreatePhotosWithCategories();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableFilter, true)
        .Add(p => p.CategorySelector, p => p.Category!));

    var categoryButtons = cut.FindAll(".bzg-filter-btn").Where(b => b.TextContent.Trim() != "All").ToList();
    foreach (var btn in categoryButtons)
    {
      btn.GetAttribute("aria-pressed").Should().Be("false");
    }
  }

  [Fact]
  public void FilterBar_ShouldRenderSortedCategories()
  {
    var items = CreatePhotosWithCategories();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableFilter, true)
        .Add(p => p.CategorySelector, p => p.Category!));

    var buttons = cut.FindAll(".bzg-filter-btn").Select(b => b.TextContent.Trim()).ToList();
    buttons.Should().ContainInOrder("All", "Abstract", "City", "Nature");
  }

  [Fact]
  public void FilterBar_AllItemsVisibleByDefault()
  {
    var items = CreatePhotosWithCategories();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableFilter, true)
        .Add(p => p.CategorySelector, p => p.Category!));

    cut.FindAll(".bzg-item").Should().HaveCount(5);
    cut.FindAll(".bzg-item-hidden").Should().BeEmpty();
  }

  #endregion

  #region Grid Style Variables

  [Fact]
  public void GridStyle_ShouldContainColumnsVariable()
  {
    var items = CreatePhotos(1);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Columns, 4));

    var grid = cut.Find(".bzg-grid");
    grid.GetAttribute("style").Should().Contain("--bzg-columns: 4");
  }

  [Fact]
  public void GridStyle_ShouldContainGapVariable()
  {
    var items = CreatePhotos(1);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Gap, 24));

    var grid = cut.Find(".bzg-grid");
    grid.GetAttribute("style").Should().Contain("--bzg-gap: 24px");
  }

  [Fact]
  public void GridStyle_WithAspectRatio_ShouldContainAspectRatioVariable()
  {
    var items = CreatePhotos(1);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Layout, BzGalleryLayout.Grid)
        .Add(p => p.AspectRatio, "16/9"));

    var grid = cut.Find(".bzg-grid");
    grid.GetAttribute("style").Should().Contain("--bzg-aspect-ratio: 16/9");
  }

  [Fact]
  public void GridStyle_MasonryLayout_ShouldNotContainAspectRatio()
  {
    var items = CreatePhotos(1);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Layout, BzGalleryLayout.Masonry)
        .Add(p => p.AspectRatio, "16/9"));

    var grid = cut.Find(".bzg-grid");
    grid.GetAttribute("style").Should().NotContain("--bzg-aspect-ratio");
  }

  [Fact]
  public void GridStyle_ColumnsClamped_ShouldClampToMax6()
  {
    var items = CreatePhotos(1);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Columns, 10));

    var grid = cut.Find(".bzg-grid");
    grid.GetAttribute("style").Should().Contain("--bzg-columns: 6");
  }

  [Fact]
  public void GridStyle_ColumnsClamped_ShouldClampToMin1()
  {
    var items = CreatePhotos(1);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.Columns, -5));

    var grid = cut.Find(".bzg-grid");
    grid.GetAttribute("style").Should().Contain("--bzg-columns: 1");
  }

  #endregion

  #region Lightbox Overlay Icon

  [Fact]
  public void LightboxEnabled_ShouldRenderOverlayIcon()
  {
    var items = CreatePhotos(1);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, true));

    cut.Markup.Should().Contain("bzg-overlay-icon");
  }

  [Fact]
  public void LightboxDisabled_ShouldNotRenderOverlayIcon()
  {
    var items = CreatePhotos(1);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.EnableLightbox, false));

    cut.Markup.Should().NotContain("bzg-overlay-icon");
  }

  #endregion

  #region Disposal Edge Cases

  [Fact]
  public async Task Dispose_WithoutLightboxOpen_ShouldNotThrow()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    var action = async () => await cut.Instance.DisposeAsync();
    await action.Should().NotThrowAsync();
  }

  [Fact]
  public async Task Dispose_Twice_ShouldNotThrow()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    await cut.Instance.DisposeAsync();

    var action = async () => await cut.Instance.DisposeAsync();
    await action.Should().NotThrowAsync();
  }

  [Fact]
  public async Task Dispose_WithNoItems_ShouldNotThrow()
  {
    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, new List<TestGalleryPhoto>()));

    var action = async () => await cut.Instance.DisposeAsync();
    await action.Should().NotThrowAsync();
  }

  #endregion

  #region Container Rendering

  [Fact]
  public void ContainerClass_ShouldContainBzgContainer()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    cut.Markup.Should().Contain("bzg-container");
  }

  [Fact]
  public void ContainerStyle_BeforeInit_ShouldBeHidden()
  {
    var items = CreatePhotos();

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    var container = cut.Find(".bzg-container");
    container.GetAttribute("style").Should().Contain("opacity:0");
  }

  #endregion

  #region Image Rendering

  [Fact]
  public void Items_ShouldRenderImagesWithLazyLoading()
  {
    var items = CreatePhotos(2);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    var images = cut.FindAll(".bzg-image");
    foreach (var img in images)
    {
      img.GetAttribute("loading").Should().Be("lazy");
      img.GetAttribute("draggable").Should().Be("false");
    }
  }

  [Fact]
  public void Items_WithTitle_ShouldRenderOverlayTitle()
  {
    var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "test.jpg", Title = "My Title" }
        };

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    cut.Markup.Should().Contain("bzg-overlay-title");
    cut.Markup.Should().Contain("My Title");
  }

  [Fact]
  public void Items_WithCorrectSrc_ShouldRenderImages()
  {
    var items = CreatePhotos(3);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    var images = cut.FindAll(".bzg-image");
    images[0].GetAttribute("src").Should().Be("https://example.com/photo1.jpg");
    images[1].GetAttribute("src").Should().Be("https://example.com/photo2.jpg");
    images[2].GetAttribute("src").Should().Be("https://example.com/photo3.jpg");
  }

  #endregion

  #region Empty & Edge Cases

  [Fact]
  public void NullItems_ShouldRenderEmptyState()
  {
    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, (IEnumerable<TestGalleryPhoto>?)null));

    cut.Markup.Should().Contain("bz-empty");
  }

  [Fact]
  public void EmptyItems_ShouldRenderEmptyState()
  {
    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, new List<TestGalleryPhoto>()));

    cut.Markup.Should().Contain("bz-empty");
  }

  [Fact]
  public void SingleItem_ShouldRenderOneGalleryItem()
  {
    var items = CreatePhotos(1);

    var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
        .Add(p => p.Items, items));

    cut.FindAll(".bzg-item").Should().HaveCount(1);
  }

  #endregion
}
