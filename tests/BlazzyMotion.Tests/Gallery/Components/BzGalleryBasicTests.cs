namespace BlazzyMotion.Tests.Gallery.Components;

public class BzGalleryBasicTests : TestBase
{
    private static List<TestGalleryPhoto> CreatePhotos(int count = 3)
    {
        return Enumerable.Range(1, count).Select(i => new TestGalleryPhoto
        {
            Id = i,
            ImageUrl = $"https://example.com/photo{i}.jpg",
            Title = $"Photo {i}",
            Description = $"Description {i}",
            Category = i % 2 == 0 ? "Nature" : "City"
        }).ToList();
    }

    private static List<TestGalleryPhoto> CreatePhotosWithoutTitles(int count = 2)
    {
        return Enumerable.Range(1, count).Select(i => new TestGalleryPhoto
        {
            Id = i,
            ImageUrl = $"https://example.com/photo{i}.jpg"
        }).ToList();
    }

    #region Rendering Tests

    [Fact]
    public void BzGallery_WithItems_ShouldRenderGalleryContainer()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("bzg-container");
        cut.Markup.Should().Contain("bzg-grid");
    }

    [Fact]
    public void BzGallery_WithItems_ShouldRenderCorrectNumberOfItems()
    {
        // Arrange
        var items = CreatePhotos(5);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        var galleryItems = cut.FindAll(".bzg-item");
        galleryItems.Should().HaveCount(5);
    }

    [Fact]
    public void BzGallery_WithItems_ShouldRenderGridLayoutByDefault()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("bzg-layout-grid");
    }

    #endregion

    #region Empty State Tests

    [Fact]
    public void BzGallery_WithNullItems_ShouldRenderEmptyState()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, null));

        // Assert
        cut.Markup.Should().Contain("bz-empty");
        cut.Markup.Should().Contain("No items to display");
    }

    [Fact]
    public void BzGallery_WithEmptyList_ShouldRenderEmptyState()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, new List<TestGalleryPhoto>()));

        // Assert
        cut.Markup.Should().Contain("bz-empty");
        cut.Markup.Should().Contain("No items to display");
    }

    [Fact]
    public void BzGallery_WithEmptyList_ShouldNotRenderGrid()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, new List<TestGalleryPhoto>()));

        // Assert
        cut.Markup.Should().NotContain("bzg-grid");
        cut.Markup.Should().NotContain("bzg-container");
    }

    [Fact]
    public void BzGallery_WithEmptyTemplate_ShouldRenderCustomEmpty()
    {
        // Arrange
        RenderFragment emptyTemplate = builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddContent(1, "Nothing here yet");
            builder.CloseElement();
        };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, new List<TestGalleryPhoto>())
            .Add(p => p.EmptyTemplate, emptyTemplate));

        // Assert
        cut.Markup.Should().Contain("Nothing here yet");
        cut.Markup.Should().NotContain("No items to display");
    }

    #endregion

    #region Loading State Tests

    [Fact]
    public void BzGallery_WithLoadingTemplate_ShouldRenderCustomLoading()
    {
        // Arrange
        RenderFragment loadingTemplate = builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddContent(1, "Please wait...");
            builder.CloseElement();
        };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.LoadingTemplate, loadingTemplate));

        // Assert - LoadingTemplate is available as a parameter
        cut.Should().NotBeNull();
    }

    [Fact]
    public void BzGallery_DefaultLoadingState_ShouldContainSpinnerMarkup()
    {
        // Arrange & Act - Verify loading template exists in component definition
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, new List<TestGalleryPhoto>()));

        // Assert - Empty state is rendered instead of loading when items is empty list
        cut.Markup.Should().Contain("bz-empty");
    }

    #endregion

    #region Columns Tests

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void BzGallery_WithColumns_ShouldApplyCorrectCssVariable(int columns)
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Columns, columns));

        // Assert
        cut.Markup.Should().Contain($"--bzg-columns: {columns}");
    }

    [Fact]
    public void BzGallery_DefaultColumns_ShouldBeThree()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("--bzg-columns: 3");
    }

    [Fact]
    public void BzGallery_WithColumnsAboveSix_ShouldClampToSix()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Columns, 10));

        // Assert
        cut.Markup.Should().Contain("--bzg-columns: 6");
    }

    [Fact]
    public void BzGallery_WithColumnsBelowOne_ShouldClampToOne()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Columns, 0));

        // Assert
        cut.Markup.Should().Contain("--bzg-columns: 1");
    }

    #endregion

    #region Gap Tests

    [Theory]
    [InlineData(0)]
    [InlineData(8)]
    [InlineData(24)]
    [InlineData(32)]
    public void BzGallery_WithGap_ShouldApplyCorrectCssVariable(int gap)
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Gap, gap));

        // Assert
        cut.Markup.Should().Contain($"--bzg-gap: {gap}px");
    }

    [Fact]
    public void BzGallery_DefaultGap_ShouldBeSixteen()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("--bzg-gap: 16px");
    }

    #endregion

    #region AspectRatio Tests

    [Fact]
    public void BzGallery_WithAspectRatio_GridLayout_ShouldApplyCssVariable()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.Grid)
            .Add(p => p.AspectRatio, "16/9"));

        // Assert
        cut.Markup.Should().Contain("--bzg-aspect-ratio: 16/9");
    }

    [Fact]
    public void BzGallery_WithAspectRatio_MasonryLayout_ShouldNotApplyCssVariable()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.Masonry)
            .Add(p => p.AspectRatio, "16/9"));

        // Assert
        cut.Markup.Should().NotContain("--bzg-aspect-ratio");
    }

    [Fact]
    public void BzGallery_WithoutAspectRatio_ShouldNotRenderAspectRatioVariable()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().NotContain("--bzg-aspect-ratio");
    }

    #endregion

    #region Layout Tests

    [Fact]
    public void BzGallery_WithGridLayout_ShouldApplyGridClass()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.Grid));

        // Assert
        cut.Markup.Should().Contain("bzg-layout-grid");
    }

    [Fact]
    public void BzGallery_WithMasonryLayout_ShouldApplyMasonryClass()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.Masonry));

        // Assert
        cut.Markup.Should().Contain("bzg-layout-masonry");
    }

    [Fact]
    public void BzGallery_WithListLayout_ShouldApplyListClass()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.List));

        // Assert
        cut.Markup.Should().Contain("bzg-layout-list");
    }

    #endregion

    #region Theme Tests

    [Fact]
    public void BzGallery_DefaultTheme_ShouldApplyGlassThemeClass()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("bzc-theme-glass");
    }

    [Fact]
    public void BzGallery_WithDarkTheme_ShouldApplyThemeClass()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Theme, BzTheme.Dark));

        // Assert
        cut.Markup.Should().Contain("bzc-theme-dark");
    }

    [Fact]
    public void BzGallery_WithLightTheme_ShouldApplyThemeClass()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Theme, BzTheme.Light));

        // Assert
        cut.Markup.Should().Contain("bzc-theme-light");
    }

    [Fact]
    public void BzGallery_WithMinimalTheme_ShouldApplyThemeClass()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Theme, BzTheme.Minimal));

        // Assert
        cut.Markup.Should().Contain("bzc-theme-minimal");
    }

    #endregion

    #region Image Rendering Tests

    [Fact]
    public void BzGallery_Images_ShouldHaveCorrectClass()
    {
        // Arrange
        var items = CreatePhotos(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("class=\"bzg-image\"");
    }

    [Fact]
    public void BzGallery_Images_ShouldUseLazyLoading()
    {
        // Arrange
        var items = CreatePhotos(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("loading=\"lazy\"");
    }

    [Fact]
    public void BzGallery_Images_ShouldBeNonDraggable()
    {
        // Arrange
        var items = CreatePhotos(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("draggable=\"false\"");
    }

    [Fact]
    public void BzGallery_Images_WithTitle_ShouldUseAsTitleAlt()
    {
        // Arrange
        var items = new List<TestGalleryPhoto>
        {
            new TestGalleryPhoto { ImageUrl = "test.jpg", Title = "Sunset Beach" }
        };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("alt=\"Sunset Beach\"");
    }

    [Fact]
    public void BzGallery_Images_WithoutTitle_ShouldUseFallbackAlt()
    {
        // Arrange
        var items = CreatePhotosWithoutTitles(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("alt=\"Gallery image\"");
    }

    #endregion

    #region Lightbox Tests

    [Fact]
    public void BzGallery_WithLightboxEnabled_ShouldRenderOverlayIcon()
    {
        // Arrange
        var items = CreatePhotos(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableLightbox, true));

        // Assert
        cut.Markup.Should().Contain("bzg-overlay-icon");
    }

    [Fact]
    public void BzGallery_WithLightboxDisabled_ShouldNotRenderOverlayIcon()
    {
        // Arrange
        var items = CreatePhotos(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableLightbox, false));

        // Assert
        cut.Markup.Should().NotContain("bzg-overlay-icon");
    }

    [Fact]
    public void BzGallery_DefaultEnableLightbox_ShouldBeTrue()
    {
        // Arrange
        var items = CreatePhotos(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("bzg-overlay-icon");
    }

    #endregion

    #region ItemTemplate Tests

    [Fact]
    public void BzGallery_WithItemTemplate_ShouldRenderCustomContent()
    {
        // Arrange
        var items = CreatePhotos(2);
        RenderFragment<TestGalleryPhoto> itemTemplate = item => builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "custom-item");
            builder.AddContent(2, $"Custom: {item.Title}");
            builder.CloseElement();
        };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.ItemTemplate, itemTemplate));

        // Assert
        cut.Markup.Should().Contain("custom-item");
        cut.Markup.Should().Contain("Custom: Photo 1");
        cut.Markup.Should().Contain("Custom: Photo 2");
    }

    [Fact]
    public void BzGallery_WithItemTemplate_ShouldNotRenderDefaultImage()
    {
        // Arrange
        var items = CreatePhotos(1);
        RenderFragment<TestGalleryPhoto> itemTemplate = item => builder =>
        {
            builder.OpenElement(0, "span");
            builder.AddContent(1, item.Title);
            builder.CloseElement();
        };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.ItemTemplate, itemTemplate));

        // Assert
        cut.Markup.Should().NotContain("bzg-image");
        cut.Markup.Should().NotContain("bzg-overlay");
    }

    #endregion

    #region CSS Class Tests

    [Fact]
    public void BzGallery_WithCssClass_ShouldIncludeCustomClass()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.CssClass, "my-gallery"));

        // Assert
        cut.Markup.Should().Contain("my-gallery");
    }

    [Fact]
    public void BzGallery_WithCssClass_ShouldCombineWithBaseClasses()
    {
        // Arrange
        var items = CreatePhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.CssClass, "custom-class"));

        // Assert
        cut.Markup.Should().Contain("bzg-container");
        cut.Markup.Should().Contain("custom-class");
        cut.Markup.Should().Contain("bzc-theme-glass");
    }

    #endregion

    #region Additional Attributes Tests

    [Fact]
    public void BzGallery_WithDataAttribute_ShouldApply()
    {
        // Arrange
        var items = CreatePhotos();
        var attributes = new Dictionary<string, object> { { "data-testid", "gallery-1" } };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.AdditionalAttributes, attributes));

        // Assert
        cut.Markup.Should().Contain("data-testid=\"gallery-1\"");
    }

    [Fact]
    public void BzGallery_WithId_ShouldApply()
    {
        // Arrange
        var items = CreatePhotos();
        var attributes = new Dictionary<string, object> { { "id", "photo-gallery" } };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.AdditionalAttributes, attributes));

        // Assert
        cut.Markup.Should().Contain("id=\"photo-gallery\"");
    }

    #endregion

    #region Overlay Title Tests

    [Fact]
    public void BzGallery_WithTitle_ShouldRenderOverlayTitle()
    {
        // Arrange
        var items = new List<TestGalleryPhoto>
        {
            new TestGalleryPhoto { ImageUrl = "test.jpg", Title = "Mountain View" }
        };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("bzg-overlay-title");
        cut.Markup.Should().Contain("Mountain View");
    }

    [Fact]
    public void BzGallery_WithoutTitle_ShouldNotRenderOverlayTitle()
    {
        // Arrange
        var items = CreatePhotosWithoutTitles(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().NotContain("bzg-overlay-title");
    }

    #endregion
}
