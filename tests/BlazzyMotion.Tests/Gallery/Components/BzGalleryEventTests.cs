namespace BlazzyMotion.Tests.Gallery.Components;

/// <summary>
/// Tests for BzGallery events, callbacks, and disposal.
/// </summary>
public class BzGalleryEventTests : TestBase
{
    private static List<TestGalleryPhoto> CreateTestPhotos(int count = 3) =>
        Enumerable.Range(1, count).Select(i => new TestGalleryPhoto
        {
            Id = i,
            ImageUrl = $"https://example.com/photo{i}.jpg",
            Title = $"Photo {i}",
            Description = $"Description {i}",
            Category = "General"
        }).ToList();

    #region OnItemSelected Callback Tests

    [Fact]
    public void BzGallery_LightboxDisabledAndItemClicked_ShouldFireOnItemSelected()
    {
        // Arrange
        TestGalleryPhoto? selectedItem = null;
        var items = CreateTestPhotos();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableLightbox, false)
            .Add(p => p.OnItemSelected, EventCallback.Factory.Create<TestGalleryPhoto>(this, item => selectedItem = item)));

        // Act
        var firstItem = cut.FindAll(".bzg-item")[0];
        firstItem.Click();

        // Assert
        selectedItem.Should().NotBeNull();
        selectedItem!.Id.Should().Be(1);
        selectedItem.Title.Should().Be("Photo 1");
    }

    [Fact]
    public void BzGallery_LightboxDisabledAndSecondItemClicked_ShouldReturnCorrectItem()
    {
        // Arrange
        TestGalleryPhoto? selectedItem = null;
        var items = CreateTestPhotos();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableLightbox, false)
            .Add(p => p.OnItemSelected, EventCallback.Factory.Create<TestGalleryPhoto>(this, item => selectedItem = item)));

        // Act
        var secondItem = cut.FindAll(".bzg-item")[1];
        secondItem.Click();

        // Assert
        selectedItem.Should().NotBeNull();
        selectedItem!.Id.Should().Be(2);
    }

    [Fact]
    public void BzGallery_LightboxEnabledAndItemClicked_ShouldNotFireOnItemSelected()
    {
        // Arrange
        TestGalleryPhoto? selectedItem = null;
        var items = CreateTestPhotos();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableLightbox, true)
            .Add(p => p.OnItemSelected, EventCallback.Factory.Create<TestGalleryPhoto>(this, item => selectedItem = item)));

        // Act
        var firstItem = cut.FindAll(".bzg-item")[0];
        firstItem.Click();

        // Assert
        selectedItem.Should().BeNull();
    }

    [Fact]
    public void BzGallery_LightboxDisabledWithNoHandler_ShouldNotThrow()
    {
        // Arrange
        var items = CreateTestPhotos();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableLightbox, false));

        // Act & Assert
        var firstItem = cut.FindAll(".bzg-item")[0];
        var action = () => firstItem.Click();
        action.Should().NotThrow();
    }

    #endregion

    #region Lightbox Rendering Tests

    [Fact]
    public void BzGallery_LightboxEnabled_ShouldRenderLightboxComponent()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableLightbox, true));

        // Assert - when lightbox is enabled, the BzGalleryLightbox child component should be present
        // Before opening, it renders but IsOpen=false so the lightbox content is hidden
        cut.HasComponent<BzGalleryLightbox>().Should().BeTrue();
    }

    [Fact]
    public void BzGallery_LightboxDisabled_ShouldNotRenderLightboxComponent()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableLightbox, false));

        // Assert
        cut.HasComponent<BzGalleryLightbox>().Should().BeFalse();
    }

    [Fact]
    public void BzGallery_LightboxEnabledAndItemClicked_ShouldOpenLightbox()
    {
        // Arrange
        var items = CreateTestPhotos();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableLightbox, true));

        // Act
        var firstItem = cut.FindAll(".bzg-item")[0];
        firstItem.Click();

        // Assert
        cut.Markup.Should().Contain("bzg-lightbox-open");
    }

    [Fact]
    public void BzGallery_LightboxEnabled_ShouldShowOverlayIcon()
    {
        // Arrange
        var items = CreateTestPhotos();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableLightbox, true));

        // Assert
        cut.Markup.Should().Contain("bzg-overlay-icon");
    }

    #endregion

    #region Re-render Tests

    [Fact]
    public void BzGallery_SetParametersWithNewItems_ShouldUpdateRenderedItems()
    {
        // Arrange
        var items = CreateTestPhotos(2);

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        cut.FindAll(".bzg-item").Should().HaveCount(2);

        // Act
        var newItems = CreateTestPhotos(5);
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Items, newItems));

        // Assert
        cut.FindAll(".bzg-item").Should().HaveCount(5);
    }

    [Fact]
    public void BzGallery_SetParametersWithNewLayout_ShouldUpdateLayoutClass()
    {
        // Arrange
        var items = CreateTestPhotos();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.Grid));

        cut.Markup.Should().Contain("bzg-layout-grid");

        // Act
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Layout, BzGalleryLayout.Masonry));

        // Assert
        cut.Markup.Should().Contain("bzg-layout-masonry");
        cut.Markup.Should().NotContain("bzg-layout-grid");
    }

    [Fact]
    public void BzGallery_SetParametersWithEmptyItems_ShouldShowEmptyState()
    {
        // Arrange
        var items = CreateTestPhotos();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        cut.FindAll(".bzg-item").Should().HaveCount(3);

        // Act
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Items, new List<TestGalleryPhoto>()));

        // Assert
        cut.Markup.Should().Contain("bz-empty");
    }

    #endregion

    #region Disposal Tests

    [Fact]
    public void BzGallery_Dispose_ShouldNotThrow()
    {
        // Arrange
        var items = CreateTestPhotos();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Act & Assert
        var action = () => cut.Instance.DisposeAsync();
        action.Should().NotThrow();
    }

    [Fact]
    public void BzGallery_DisposeWithoutItems_ShouldNotThrow()
    {
        // Arrange
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, (IEnumerable<TestGalleryPhoto>?)null));

        // Act & Assert
        var action = () => cut.Instance.DisposeAsync();
        action.Should().NotThrow();
    }

    [Fact]
    public void BzGallery_DoublDispose_ShouldNotThrow()
    {
        // Arrange
        var items = CreateTestPhotos();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Act & Assert
        var action = () =>
        {
            cut.Instance.DisposeAsync().AsTask().GetAwaiter().GetResult();
            cut.Instance.DisposeAsync().AsTask().GetAwaiter().GetResult();
        };
        action.Should().NotThrow();
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void BzGallery_SingleItem_ShouldRenderOneItem()
    {
        // Arrange
        var items = CreateTestPhotos(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.FindAll(".bzg-item").Should().HaveCount(1);
    }

    [Fact]
    public void BzGallery_ItemWithNullImage_ShouldNotRenderImageWrapper()
    {
        // Arrange
        var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = null, Title = "No Image", Description = "test" }
        };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().NotContain("bzg-image-wrapper");
        cut.Markup.Should().NotContain("<img");
    }

    [Fact]
    public void BzGallery_ItemWithNullTitle_ShouldUseGalleryImageAsAlt()
    {
        // Arrange
        var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "https://example.com/1.jpg", Title = null, Description = "test" }
        };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("alt=\"Gallery image\"");
    }

    [Fact]
    public void BzGallery_ImageElements_ShouldHaveLazyLoading()
    {
        // Arrange
        var items = CreateTestPhotos(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        var images = cut.FindAll("img.bzg-image");
        images.Should().NotBeEmpty();
        images[0].GetAttribute("loading").Should().Be("lazy");
    }

    [Fact]
    public void BzGallery_ImageElements_ShouldHaveDraggableFalse()
    {
        // Arrange
        var items = CreateTestPhotos(1);

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        var images = cut.FindAll("img.bzg-image");
        images.Should().NotBeEmpty();
        images[0].GetAttribute("draggable").Should().Be("false");
    }

    [Fact]
    public void BzGallery_ItemWithTitle_ShouldUseAsTitleAlt()
    {
        // Arrange
        var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "https://example.com/1.jpg", Title = "Sunset Beach" }
        };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().Contain("alt=\"Sunset Beach\"");
    }

    #endregion
}
