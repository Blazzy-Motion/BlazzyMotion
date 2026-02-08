namespace BlazzyMotion.Tests.Gallery.Components;

/// <summary>
/// Comprehensive tests for the BzGalleryLightbox component.
/// </summary>
public class BzGalleryLightboxTests : TestBase
{
    /// <summary>
    /// Creates a list of BzItem instances for multi-item test scenarios.
    /// </summary>
    private static IReadOnlyList<BzItem> CreateItems(int count = 3)
    {
        return Enumerable.Range(1, count).Select(i => new BzItem
        {
            ImageUrl = $"https://example.com/photo{i}.jpg",
            Title = $"Photo {i}",
            Description = $"Description for photo {i}"
        }).ToList().AsReadOnly();
    }

    /// <summary>
    /// Creates a single-item list for single-item test scenarios.
    /// </summary>
    private static IReadOnlyList<BzItem> CreateSingleItem()
    {
        return new List<BzItem>
        {
            new BzItem
            {
                ImageUrl = "https://example.com/single.jpg",
                Title = "Single Photo",
                Description = "Only photo in the gallery"
            }
        }.AsReadOnly();
    }

    #region Visibility Tests

    [Fact]
    public void Lightbox_WhenIsOpenFalse_ShouldRenderNothing()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, false)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        cut.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public void Lightbox_WhenIsOpenTrue_ShouldRenderDialog()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var lightbox = cut.Find(".bzg-lightbox");
        lightbox.Should().NotBeNull();
        lightbox.ClassList.Should().Contain("bzg-lightbox-open");
    }

    [Fact]
    public void Lightbox_WhenIsOpenTrue_ShouldHaveDialogRole()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var lightbox = cut.Find(".bzg-lightbox");
        lightbox.GetAttribute("role").Should().Be("dialog");
    }

    [Fact]
    public void Lightbox_WhenIsOpenTrue_ShouldHaveAriaModalTrue()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var lightbox = cut.Find(".bzg-lightbox");
        lightbox.GetAttribute("aria-modal").Should().Be("true");
    }

    [Fact]
    public void Lightbox_WhenIsOpenTrue_ShouldHaveAriaLabelImageLightbox()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var lightbox = cut.Find(".bzg-lightbox");
        lightbox.GetAttribute("aria-label").Should().Be("Image lightbox");
    }

    [Fact]
    public void Lightbox_WhenIsOpenTrue_ShouldHaveTabindexZero()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var lightbox = cut.Find(".bzg-lightbox");
        lightbox.GetAttribute("tabindex").Should().Be("0");
    }

    #endregion

    #region Structural Elements Tests

    [Fact]
    public void Lightbox_WhenOpen_ShouldRenderBackdrop()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        cut.Find(".bzg-lightbox-backdrop").Should().NotBeNull();
    }

    [Fact]
    public void Lightbox_WhenOpen_ShouldRenderCloseButton()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var closeButton = cut.Find(".bzg-lightbox-close");
        closeButton.Should().NotBeNull();
        closeButton.GetAttribute("aria-label").Should().Be("Close lightbox");
    }

    [Fact]
    public void Lightbox_WhenOpen_ShouldRenderContentArea()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        cut.Find(".bzg-lightbox-content").Should().NotBeNull();
    }

    #endregion

    #region Image Rendering Tests

    [Fact]
    public void Lightbox_ShouldRenderCurrentImage()
    {
        // Arrange
        var items = CreateItems();

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var img = cut.Find(".bzg-lightbox-image");
        img.GetAttribute("src").Should().Be("https://example.com/photo1.jpg");
        img.GetAttribute("loading").Should().Be("eager");
    }

    [Fact]
    public void Lightbox_ImageWithTitle_ShouldUseAsTitleAlt()
    {
        // Arrange
        var items = CreateItems();

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var img = cut.Find(".bzg-lightbox-image");
        img.GetAttribute("alt").Should().Be("Photo 1");
    }

    [Fact]
    public void Lightbox_ImageWithoutTitle_ShouldUseFallbackAlt()
    {
        // Arrange
        var items = new List<BzItem>
        {
            new BzItem { ImageUrl = "notitle.jpg" }
        }.AsReadOnly();

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var img = cut.Find(".bzg-lightbox-image");
        img.GetAttribute("alt").Should().Be("Gallery image");
    }

    [Fact]
    public void Lightbox_AtIndex1_ShouldRenderSecondImage()
    {
        // Arrange
        var items = CreateItems();

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 1));

        // Assert
        var img = cut.Find(".bzg-lightbox-image");
        img.GetAttribute("src").Should().Be("https://example.com/photo2.jpg");
        img.GetAttribute("alt").Should().Be("Photo 2");
    }

    #endregion

    #region Caption Tests

    [Fact]
    public void Lightbox_ItemWithTitleAndDescription_ShouldRenderCaption()
    {
        // Arrange
        var items = CreateItems();

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        cut.Find(".bzg-lightbox-caption").Should().NotBeNull();
        cut.Find(".bzg-lightbox-title").TextContent.Should().Be("Photo 1");
        cut.Find(".bzg-lightbox-description").TextContent.Should().Be("Description for photo 1");
    }

    [Fact]
    public void Lightbox_ItemWithTitleOnly_ShouldRenderCaptionWithoutDescription()
    {
        // Arrange
        var items = new List<BzItem>
        {
            new BzItem { ImageUrl = "test.jpg", Title = "Title Only" }
        }.AsReadOnly();

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        cut.Find(".bzg-lightbox-caption").Should().NotBeNull();
        cut.Find(".bzg-lightbox-title").TextContent.Should().Be("Title Only");
        cut.FindAll(".bzg-lightbox-description").Should().BeEmpty();
    }

    [Fact]
    public void Lightbox_ItemWithDescriptionOnly_ShouldRenderCaptionWithoutTitle()
    {
        // Arrange
        var items = new List<BzItem>
        {
            new BzItem { ImageUrl = "test.jpg", Description = "Desc Only" }
        }.AsReadOnly();

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        cut.Find(".bzg-lightbox-caption").Should().NotBeNull();
        cut.FindAll(".bzg-lightbox-title").Should().BeEmpty();
        cut.Find(".bzg-lightbox-description").TextContent.Should().Be("Desc Only");
    }

    [Fact]
    public void Lightbox_ItemWithNoTitleOrDescription_ShouldNotRenderCaption()
    {
        // Arrange
        var items = new List<BzItem>
        {
            new BzItem { ImageUrl = "test.jpg" }
        }.AsReadOnly();

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        cut.FindAll(".bzg-lightbox-caption").Should().BeEmpty();
    }

    #endregion

    #region Multi-Item Navigation UI Tests

    [Fact]
    public void Lightbox_MultipleItems_ShouldRenderCounter()
    {
        // Arrange
        var items = CreateItems(5);

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var counter = cut.Find(".bzg-lightbox-counter");
        counter.TextContent.Trim().Should().Be("1 / 5");
    }

    [Fact]
    public void Lightbox_MultipleItems_ShouldRenderPrevButton()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var prevButton = cut.Find(".bzg-lightbox-prev");
        prevButton.Should().NotBeNull();
        prevButton.GetAttribute("aria-label").Should().Be("Previous image");
    }

    [Fact]
    public void Lightbox_MultipleItems_ShouldRenderNextButton()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var nextButton = cut.Find(".bzg-lightbox-next");
        nextButton.Should().NotBeNull();
        nextButton.GetAttribute("aria-label").Should().Be("Next image");
    }

    [Fact]
    public void Lightbox_MultipleItems_ShouldRenderThumbnails()
    {
        // Arrange
        var items = CreateItems(3);

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var thumbs = cut.FindAll(".bzg-lightbox-thumb");
        thumbs.Should().HaveCount(3);
    }

    [Fact]
    public void Lightbox_MultipleItems_ActiveThumbnailShouldHaveActiveClass()
    {
        // Arrange
        var items = CreateItems(3);

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 1));

        // Assert
        var thumbs = cut.FindAll(".bzg-lightbox-thumb");
        thumbs[0].ClassList.Should().NotContain("bzg-lightbox-thumb-active");
        thumbs[1].ClassList.Should().Contain("bzg-lightbox-thumb-active");
        thumbs[2].ClassList.Should().NotContain("bzg-lightbox-thumb-active");
    }

    [Fact]
    public void Lightbox_MultipleItems_ThumbnailsShouldHaveAriaLabels()
    {
        // Arrange
        var items = CreateItems(3);

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var thumbs = cut.FindAll(".bzg-lightbox-thumb");
        thumbs[0].GetAttribute("aria-label").Should().Be("Go to image 1");
        thumbs[1].GetAttribute("aria-label").Should().Be("Go to image 2");
        thumbs[2].GetAttribute("aria-label").Should().Be("Go to image 3");
    }

    [Fact]
    public void Lightbox_MultipleItems_CounterShouldReflectCurrentIndex()
    {
        // Arrange
        var items = CreateItems(5);

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 3));

        // Assert
        var counter = cut.Find(".bzg-lightbox-counter");
        counter.TextContent.Trim().Should().Be("4 / 5");
    }

    #endregion

    #region Single-Item Tests

    [Fact]
    public void Lightbox_SingleItem_ShouldNotRenderCounter()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateSingleItem())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        cut.FindAll(".bzg-lightbox-counter").Should().BeEmpty();
    }

    [Fact]
    public void Lightbox_SingleItem_ShouldNotRenderNavButtons()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateSingleItem())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        cut.FindAll(".bzg-lightbox-prev").Should().BeEmpty();
        cut.FindAll(".bzg-lightbox-next").Should().BeEmpty();
    }

    [Fact]
    public void Lightbox_SingleItem_ShouldNotRenderThumbnails()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateSingleItem())
            .Add(p => p.CurrentIndex, 0));

        // Assert
        cut.FindAll(".bzg-lightbox-thumbs").Should().BeEmpty();
    }

    #endregion

    #region GoNext Navigation Tests

    [Fact]
    public void GoNext_ShouldAdvanceToNextImage()
    {
        // Arrange
        var indexChanged = -1;
        var items = CreateItems(3);

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0)
            .Add(p => p.OnIndexChanged, EventCallback.Factory.Create<int>(this, idx => indexChanged = idx)));

        // Act
        cut.Find(".bzg-lightbox-next").Click();

        // Assert
        indexChanged.Should().Be(1);
    }

    [Fact]
    public void GoNext_AtLastIndex_ShouldWrapToFirst()
    {
        // Arrange
        var indexChanged = -1;
        var items = CreateItems(3);

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 2)
            .Add(p => p.OnIndexChanged, EventCallback.Factory.Create<int>(this, idx => indexChanged = idx)));

        // Act
        cut.Find(".bzg-lightbox-next").Click();

        // Assert
        indexChanged.Should().Be(0);
    }

    #endregion

    #region GoPrev Navigation Tests

    [Fact]
    public void GoPrev_ShouldGoToPreviousImage()
    {
        // Arrange
        var indexChanged = -1;
        var items = CreateItems(3);

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 2)
            .Add(p => p.OnIndexChanged, EventCallback.Factory.Create<int>(this, idx => indexChanged = idx)));

        // Act
        cut.Find(".bzg-lightbox-prev").Click();

        // Assert
        indexChanged.Should().Be(1);
    }

    [Fact]
    public void GoPrev_AtFirstIndex_ShouldWrapToLast()
    {
        // Arrange
        var indexChanged = -1;
        var items = CreateItems(3);

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0)
            .Add(p => p.OnIndexChanged, EventCallback.Factory.Create<int>(this, idx => indexChanged = idx)));

        // Act
        cut.Find(".bzg-lightbox-prev").Click();

        // Assert
        indexChanged.Should().Be(2);
    }

    #endregion

    #region GoToIndex (Thumbnail) Navigation Tests

    [Fact]
    public void GoToIndex_ClickThumbnail_ShouldNavigateToThatIndex()
    {
        // Arrange
        var indexChanged = -1;
        var items = CreateItems(4);

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0)
            .Add(p => p.OnIndexChanged, EventCallback.Factory.Create<int>(this, idx => indexChanged = idx)));

        // Act
        var thumbs = cut.FindAll(".bzg-lightbox-thumb");
        thumbs[2].Click();

        // Assert
        indexChanged.Should().Be(2);
    }

    [Fact]
    public void GoToIndex_ClickCurrentThumbnail_ShouldNotInvokeCallback()
    {
        // Arrange
        var callbackInvoked = false;
        var items = CreateItems(3);

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 1)
            .Add(p => p.OnIndexChanged, EventCallback.Factory.Create<int>(this, _ => callbackInvoked = true)));

        // Act
        var thumbs = cut.FindAll(".bzg-lightbox-thumb");
        thumbs[1].Click();

        // Assert
        callbackInvoked.Should().BeFalse();
    }

    #endregion

    #region Close Tests

    [Fact]
    public void CloseButton_Click_ShouldInvokeOnClose()
    {
        // Arrange
        var closeCalled = false;

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0)
            .Add(p => p.OnClose, EventCallback.Factory.Create(this, () => closeCalled = true)));

        // Act
        cut.Find(".bzg-lightbox-close").Click();

        // Assert
        closeCalled.Should().BeTrue();
    }

    [Fact]
    public void BackdropClick_ShouldInvokeOnClose()
    {
        // Arrange
        var closeCalled = false;

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0)
            .Add(p => p.OnClose, EventCallback.Factory.Create(this, () => closeCalled = true)));

        // Act - clicking the root .bzg-lightbox div triggers OnBackdropClick
        cut.Find(".bzg-lightbox").Click();

        // Assert
        closeCalled.Should().BeTrue();
    }

    #endregion

    #region Keyboard Navigation Tests

    [Fact]
    public void KeyDown_Escape_ShouldInvokeOnClose()
    {
        // Arrange
        var closeCalled = false;

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0)
            .Add(p => p.OnClose, EventCallback.Factory.Create(this, () => closeCalled = true)));

        // Act
        cut.Find(".bzg-lightbox").KeyDown(new KeyboardEventArgs { Key = "Escape" });

        // Assert
        closeCalled.Should().BeTrue();
    }

    [Fact]
    public void KeyDown_ArrowRight_ShouldGoNext()
    {
        // Arrange
        var indexChanged = -1;
        var items = CreateItems(3);

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0)
            .Add(p => p.OnIndexChanged, EventCallback.Factory.Create<int>(this, idx => indexChanged = idx)));

        // Act
        cut.Find(".bzg-lightbox").KeyDown(new KeyboardEventArgs { Key = "ArrowRight" });

        // Assert
        indexChanged.Should().Be(1);
    }

    [Fact]
    public void KeyDown_ArrowLeft_ShouldGoPrev()
    {
        // Arrange
        var indexChanged = -1;
        var items = CreateItems(3);

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 2)
            .Add(p => p.OnIndexChanged, EventCallback.Factory.Create<int>(this, idx => indexChanged = idx)));

        // Act
        cut.Find(".bzg-lightbox").KeyDown(new KeyboardEventArgs { Key = "ArrowLeft" });

        // Assert
        indexChanged.Should().Be(1);
    }

    [Fact]
    public void KeyDown_ArrowRight_AtLastIndex_ShouldWrapToFirst()
    {
        // Arrange
        var indexChanged = -1;
        var items = CreateItems(3);

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 2)
            .Add(p => p.OnIndexChanged, EventCallback.Factory.Create<int>(this, idx => indexChanged = idx)));

        // Act
        cut.Find(".bzg-lightbox").KeyDown(new KeyboardEventArgs { Key = "ArrowRight" });

        // Assert
        indexChanged.Should().Be(0);
    }

    [Fact]
    public void KeyDown_ArrowLeft_AtFirstIndex_ShouldWrapToLast()
    {
        // Arrange
        var indexChanged = -1;
        var items = CreateItems(3);

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0)
            .Add(p => p.OnIndexChanged, EventCallback.Factory.Create<int>(this, idx => indexChanged = idx)));

        // Act
        cut.Find(".bzg-lightbox").KeyDown(new KeyboardEventArgs { Key = "ArrowLeft" });

        // Assert
        indexChanged.Should().Be(2);
    }

    [Fact]
    public void KeyDown_UnhandledKey_ShouldNotInvokeAnyCallback()
    {
        // Arrange
        var closeCalled = false;
        var indexChanged = false;

        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, CreateItems())
            .Add(p => p.CurrentIndex, 0)
            .Add(p => p.OnClose, EventCallback.Factory.Create(this, () => closeCalled = true))
            .Add(p => p.OnIndexChanged, EventCallback.Factory.Create<int>(this, _ => indexChanged = true)));

        // Act
        cut.Find(".bzg-lightbox").KeyDown(new KeyboardEventArgs { Key = "Enter" });

        // Assert
        closeCalled.Should().BeFalse();
        indexChanged.Should().BeFalse();
    }

    #endregion

    #region Null / Empty Items Tests

    [Fact]
    public void Lightbox_WithNullItems_WhenClosed_ShouldRenderNothing()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, false)
            .Add(p => p.Items, null)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        cut.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public void Lightbox_WithNullItems_WhenOpen_ShouldRenderDialogWithoutImage()
    {
        // Arrange & Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, null)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        cut.Find(".bzg-lightbox").Should().NotBeNull();
        cut.FindAll(".bzg-lightbox-image").Should().BeEmpty();
        cut.FindAll(".bzg-lightbox-counter").Should().BeEmpty();
        cut.FindAll(".bzg-lightbox-prev").Should().BeEmpty();
        cut.FindAll(".bzg-lightbox-next").Should().BeEmpty();
    }

    #endregion

    #region Thumbnail Image Tests

    [Fact]
    public void Lightbox_Thumbnails_ShouldHaveCorrectImageSrc()
    {
        // Arrange
        var items = CreateItems(3);

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var thumbs = cut.FindAll(".bzg-lightbox-thumb img");
        thumbs[0].GetAttribute("src").Should().Be("https://example.com/photo1.jpg");
        thumbs[1].GetAttribute("src").Should().Be("https://example.com/photo2.jpg");
        thumbs[2].GetAttribute("src").Should().Be("https://example.com/photo3.jpg");
    }

    [Fact]
    public void Lightbox_ThumbnailImages_ShouldUseLazyLoading()
    {
        // Arrange
        var items = CreateItems(3);

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var thumbImgs = cut.FindAll(".bzg-lightbox-thumb img");
        foreach (var img in thumbImgs)
        {
            img.GetAttribute("loading").Should().Be("lazy");
        }
    }

    #endregion

    #region Caption H3 Element Tests

    [Fact]
    public void Lightbox_CaptionTitle_ShouldBeH3Element()
    {
        // Arrange
        var items = CreateItems();

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var title = cut.Find(".bzg-lightbox-title");
        title.TagName.Should().BeEquivalentTo("h3");
    }

    [Fact]
    public void Lightbox_CaptionDescription_ShouldBePElement()
    {
        // Arrange
        var items = CreateItems();

        // Act
        var cut = RenderComponent<BzGalleryLightbox>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Items, items)
            .Add(p => p.CurrentIndex, 0));

        // Assert
        var desc = cut.Find(".bzg-lightbox-description");
        desc.TagName.Should().BeEquivalentTo("p");
    }

    #endregion
}
