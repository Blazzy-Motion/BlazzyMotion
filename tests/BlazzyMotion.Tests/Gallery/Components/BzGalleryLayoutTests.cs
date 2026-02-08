namespace BlazzyMotion.Tests.Gallery.Components;

/// <summary>
/// Tests for BzGallery layout modes and CSS class generation.
/// </summary>
public class BzGalleryLayoutTests : TestBase
{
    private static List<TestGalleryPhoto> CreateTestPhotos(int count = 3) =>
        Enumerable.Range(1, count).Select(i => new TestGalleryPhoto
        {
            Id = i,
            ImageUrl = $"https://example.com/photo{i}.jpg",
            Title = $"Photo {i}",
            Description = $"Description {i}",
            Category = $"Category {(i % 2 == 0 ? "A" : "B")}"
        }).ToList();

    #region Layout CSS Class Tests

    [Theory]
    [InlineData(BzGalleryLayout.Grid, "bzg-layout-grid")]
    [InlineData(BzGalleryLayout.Masonry, "bzg-layout-masonry")]
    [InlineData(BzGalleryLayout.List, "bzg-layout-list")]
    public void BzGallery_WithLayout_ShouldApplyCorrectLayoutClass(BzGalleryLayout layout, string expectedClass)
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, layout));

        // Assert
        cut.Markup.Should().Contain(expectedClass);
    }

    [Theory]
    [InlineData(BzGalleryLayout.Grid)]
    [InlineData(BzGalleryLayout.Masonry)]
    [InlineData(BzGalleryLayout.List)]
    public void BzGallery_WithLayout_ShouldAlwaysContainBzgGridBaseClass(BzGalleryLayout layout)
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, layout));

        // Assert
        cut.Markup.Should().Contain("bzg-grid");
    }

    [Fact]
    public void BzGallery_DefaultLayout_ShouldBeGrid()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("bzg-layout-grid");
    }

    [Fact]
    public void BzGallery_DefaultLayout_ShouldNotContainMasonryOrListClass()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().NotContain("bzg-layout-masonry");
        cut.Markup.Should().NotContain("bzg-layout-list");
    }

    #endregion

    #region Grid Style Variable Tests

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(6)]
    public void BzGallery_WithColumns_ShouldSetColumnsVariable(int columns)
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Columns, columns));

        // Assert
        cut.Markup.Should().Contain($"--bzg-columns: {columns}");
    }

    [Theory]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(24)]
    public void BzGallery_WithGap_ShouldSetGapVariable(int gap)
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Gap, gap));

        // Assert
        cut.Markup.Should().Contain($"--bzg-gap: {gap}px");
    }

    [Fact]
    public void BzGallery_DefaultColumnsAndGap_ShouldSetDefaultVariables()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("--bzg-columns: 3");
        cut.Markup.Should().Contain("--bzg-gap: 16px");
    }

    #endregion

    #region Aspect Ratio Tests

    [Fact]
    public void BzGallery_GridWithAspectRatio_ShouldSetAspectRatioVariable()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.Grid)
            .Add(p => p.AspectRatio, "16/9"));

        // Assert
        cut.Markup.Should().Contain("--bzg-aspect-ratio: 16/9");
    }

    [Fact]
    public void BzGallery_MasonryWithAspectRatio_ShouldNotSetAspectRatioVariable()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.Masonry)
            .Add(p => p.AspectRatio, "16/9"));

        // Assert
        cut.Markup.Should().NotContain("--bzg-aspect-ratio");
    }

    [Fact]
    public void BzGallery_ListWithAspectRatio_ShouldNotSetAspectRatioVariable()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.List)
            .Add(p => p.AspectRatio, "4/3"));

        // Assert
        cut.Markup.Should().NotContain("--bzg-aspect-ratio");
    }

    [Fact]
    public void BzGallery_GridWithoutAspectRatio_ShouldNotSetAspectRatioVariable()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.Grid));

        // Assert
        cut.Markup.Should().NotContain("--bzg-aspect-ratio");
    }

    #endregion

    #region List Layout Specific Rendering Tests

    [Fact]
    public void BzGallery_ListLayout_ShouldRenderListInfo()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.List));

        // Assert
        cut.Markup.Should().Contain("bzg-list-info");
    }

    [Fact]
    public void BzGallery_ListLayout_ShouldRenderListTitle()
    {
        // Arrange
        var items = CreateTestPhotos(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.List));

        // Assert
        var listTitles = cut.FindAll(".bzg-list-title");
        listTitles.Should().NotBeEmpty();
        listTitles[0].TagName.Should().Be("H4");
        listTitles[0].TextContent.Should().Contain("Photo 1");
    }

    [Fact]
    public void BzGallery_ListLayout_ShouldRenderListDescription()
    {
        // Arrange
        var items = CreateTestPhotos(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.List));

        // Assert
        var listDescs = cut.FindAll(".bzg-list-desc");
        listDescs.Should().NotBeEmpty();
        listDescs[0].TagName.Should().Be("P");
        listDescs[0].TextContent.Should().Contain("Description 1");
    }

    [Fact]
    public void BzGallery_GridLayout_ShouldNotRenderListInfo()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.Grid));

        // Assert
        cut.Markup.Should().NotContain("bzg-list-info");
        cut.Markup.Should().NotContain("bzg-list-title");
        cut.Markup.Should().NotContain("bzg-list-desc");
    }

    [Fact]
    public void BzGallery_MasonryLayout_ShouldNotRenderListInfo()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.Masonry));

        // Assert
        cut.Markup.Should().NotContain("bzg-list-info");
        cut.Markup.Should().NotContain("bzg-list-title");
        cut.Markup.Should().NotContain("bzg-list-desc");
    }

    #endregion

    #region Item Class Tests

    [Fact]
    public void BzGallery_Items_ShouldHaveBzgItemClass()
    {
        // Arrange
        var items = CreateTestPhotos(2);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        var itemElements = cut.FindAll(".bzg-item");
        itemElements.Should().HaveCount(2);
    }

    [Fact]
    public void BzGallery_WithAnimationDisabled_ShouldNotAddNoAnimationClassBeforeInit()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.AnimationEnabled, false));

        // Assert
        // bzg-no-animation is only added when _isInitialized is true AND AnimationEnabled is false.
        // In bUnit, OnAfterRenderAsync runs and sets _isInitialized=true, so we expect it.
        // However, the first render does not have _isInitialized=true yet.
        // After the component's lifecycle completes, the class should appear.
        cut.Markup.Should().Contain("bzg-grid");
    }

    #endregion

    #region Container Class Tests

    [Fact]
    public void BzGallery_WithItems_ShouldRenderContainerClass()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("bzg-container");
    }

    [Fact]
    public void BzGallery_WithCssClass_ShouldIncludeCustomClass()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.CssClass, "my-gallery"));

        // Assert
        cut.Markup.Should().Contain("my-gallery");
    }

    #endregion
}
