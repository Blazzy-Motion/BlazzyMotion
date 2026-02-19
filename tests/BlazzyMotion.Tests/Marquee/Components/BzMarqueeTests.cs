namespace BlazzyMotion.Tests.Marquee.Components;

public class BzMarqueeTests : TestBase
{
    private static List<TestMarqueeItem> CreateItems(int count = 4)
    {
        return Enumerable.Range(1, count).Select(i => new TestMarqueeItem
        {
            Id = i,
            ImageUrl = $"https://example.com/logo{i}.png",
            Name = $"Company {i}"
        }).ToList();
    }

    private static List<TestMarqueeItem> CreateTestimonials(int count = 3)
    {
        return Enumerable.Range(1, count).Select(i => new TestMarqueeItem
        {
            Id = i,
            ImageUrl = $"https://example.com/avatar{i}.jpg",
            Name = $"Person {i}",
            Quote = $"This is a great product review number {i}."
        }).ToList();
    }

    #region Rendering Tests

    [Fact]
    public void BzMarquee_WithItems_ShouldRenderContainer()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("bzm-container");
    }

    [Fact]
    public void BzMarquee_WithItems_ShouldRenderOneRowByDefault()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var rows = cut.FindAll(".bzm-row");
        rows.Should().HaveCount(1);
    }

    [Fact]
    public void BzMarquee_WithNullItems_ShouldRenderEmptyState()
    {
        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>();

        cut.Markup.Should().Contain("bz-empty");
    }

    [Fact]
    public void BzMarquee_WithTextOnly_ShouldRenderTextItem()
    {
        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Text, "Breaking news ticker"));

        cut.Markup.Should().Contain("bzm-text-item");
        cut.Markup.Should().Contain("Breaking news ticker");
    }

    [Fact]
    public void BzMarquee_WithItems_ShouldRenderCorrectItemCount()
    {
        var items = CreateItems(5);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var marqueeItems = cut.FindAll(".bzm-item");
        marqueeItems.Should().HaveCount(5);
    }

    #endregion

    #region Multi-Row Tests

    [Fact]
    public void BzMarquee_WithRows3_ShouldRender3Rows()
    {
        var items = CreateItems(6);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 3));

        var rows = cut.FindAll(".bzm-row");
        rows.Should().HaveCount(3);
    }

    [Fact]
    public void BzMarquee_WithRows2_ShouldAddMultirowClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2));

        cut.Markup.Should().Contain("bzm-multirow");
    }

    [Fact]
    public void BzMarquee_WithRows1_ShouldNotAddMultirowClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 1));

        cut.Markup.Should().NotContain("bzm-multirow");
    }

    [Fact]
    public void BzMarquee_WithAlternateDirection_ShouldAlternateRowDirections()
    {
        var items = CreateItems(6);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2)
            .Add(x => x.AlternateDirection, true)
            .Add(x => x.Direction, BzDirection.Left));

        var rows = cut.FindAll(".bzm-row");
        rows[0].GetAttribute("data-direction").Should().Be("left");
        rows[1].GetAttribute("data-direction").Should().Be("right");
    }

    [Fact]
    public void BzMarquee_WithAlternateDirectionFalse_ShouldUseSameDirection()
    {
        var items = CreateItems(6);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2)
            .Add(x => x.AlternateDirection, false)
            .Add(x => x.Direction, BzDirection.Left));

        var rows = cut.FindAll(".bzm-row");
        rows[0].GetAttribute("data-direction").Should().Be("left");
        rows[1].GetAttribute("data-direction").Should().Be("left");
    }

    [Fact]
    public void BzMarquee_WithSpeedVariation_ShouldHaveDifferentSpeeds()
    {
        var items = CreateItems(6);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2)
            .Add(x => x.Speed, 50)
            .Add(x => x.SpeedVariation, 0.3));

        var rows = cut.FindAll(".bzm-row");
        var speed0 = rows[0].GetAttribute("data-speed");
        var speed1 = rows[1].GetAttribute("data-speed");
        speed0.Should().NotBe(speed1);
    }

    [Fact]
    public void BzMarquee_WithRowGap_ShouldSetCssVariable()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 2)
            .Add(x => x.Gap, 30));

        cut.Markup.Should().Contain("--bzm-row-gap:");
    }

    #endregion

    #region Accessibility Tests

    [Fact]
    public void BzMarquee_ShouldHaveMarqueeRole()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var container = cut.Find(".bzm-container");
        container.GetAttribute("role").Should().Be("marquee");
    }

    [Fact]
    public void BzMarquee_ShouldHaveAriaLabel()
    {
        var items = CreateItems(4);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var container = cut.Find(".bzm-container");
        container.GetAttribute("aria-label").Should().Contain("4 items");
    }

    [Fact]
    public void BzMarquee_ShouldHaveTabindex()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var container = cut.Find(".bzm-container");
        container.GetAttribute("tabindex").Should().Be("0");
    }

    [Fact]
    public void BzMarquee_ShouldHaveAriaLiveOff()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var container = cut.Find(".bzm-container");
        container.GetAttribute("aria-live").Should().Be("off");
    }

    [Fact]
    public void BzMarquee_ShouldHaveScreenReaderAnnouncementSpan()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var srSpan = cut.Find(".bzm-sr-only");
        srSpan.Should().NotBeNull();
        srSpan.GetAttribute("aria-live").Should().Be("polite");
        srSpan.GetAttribute("role").Should().Be("status");
    }

    [Fact]
    public void BzMarquee_WithText_ShouldHaveTextAriaLabel()
    {
        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Text, "Test ticker"));

        var container = cut.Find(".bzm-container");
        container.GetAttribute("aria-label").Should().Contain("text marquee");
    }

    [Fact]
    public void BzMarquee_WithMultipleRows_ShouldIncludeRowCountInAriaLabel()
    {
        var items = CreateItems(6);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 3));

        var container = cut.Find(".bzm-container");
        container.GetAttribute("aria-label").Should().Contain("3 rows");
    }

    #endregion

    #region Keyboard Interaction Tests

    [Fact]
    public void BzMarquee_SpaceKey_ShouldAddPausedClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var container = cut.Find(".bzm-container");
        container.KeyDown(new KeyboardEventArgs { Key = " " });

        cut.Markup.Should().Contain("bzm-paused");
    }

    [Fact]
    public void BzMarquee_EnterKey_ShouldAddPausedClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var container = cut.Find(".bzm-container");
        container.KeyDown(new KeyboardEventArgs { Key = "Enter" });

        cut.Markup.Should().Contain("bzm-paused");
    }

    [Fact]
    public void BzMarquee_SpaceKeyTwice_ShouldRemovePausedClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var container = cut.Find(".bzm-container");
        container.KeyDown(new KeyboardEventArgs { Key = " " });
        container.KeyDown(new KeyboardEventArgs { Key = " " });

        cut.Markup.Should().NotContain("bzm-paused");
    }

    [Fact]
    public void BzMarquee_SpaceKey_ShouldUpdateScreenReaderAnnouncement()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var container = cut.Find(".bzm-container");
        container.KeyDown(new KeyboardEventArgs { Key = " " });

        var srSpan = cut.Find(".bzm-sr-only");
        srSpan.TextContent.Should().Contain("paused");
    }

    [Fact]
    public void BzMarquee_SpaceKey_ShouldUpdateAriaLabel()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var container = cut.Find(".bzm-container");
        container.KeyDown(new KeyboardEventArgs { Key = " " });

        container.GetAttribute("aria-label").Should().Contain("paused");
    }

    [Fact]
    public void BzMarquee_ArrowKey_ShouldNotTogglePause()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var container = cut.Find(".bzm-container");
        container.KeyDown(new KeyboardEventArgs { Key = "ArrowLeft" });

        cut.Markup.Should().NotContain("bzm-paused");
    }

    #endregion

    #region Theme Tests

    [Theory]
    [InlineData(BzTheme.Glass, "bzc-theme-glass")]
    [InlineData(BzTheme.Dark, "bzc-theme-dark")]
    [InlineData(BzTheme.Light, "bzc-theme-light")]
    [InlineData(BzTheme.Minimal, "bzc-theme-minimal")]
    public void BzMarquee_WithTheme_ShouldApplyThemeClass(BzTheme theme, string expectedClass)
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Theme, theme));

        cut.Markup.Should().Contain(expectedClass);
    }

    #endregion

    #region CSS Class Tests

    [Fact]
    public void BzMarquee_WithPauseOnHover_ShouldAddPauseHoverClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.PauseOnHover, true));

        cut.Markup.Should().Contain("bzm-pause-hover");
    }

    [Fact]
    public void BzMarquee_WithShowGradients_ShouldAddGradientClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.ShowGradientEdges, true));

        cut.Markup.Should().Contain("bzm-has-gradient");
        cut.Markup.Should().Contain("bzm-gradient-start");
        cut.Markup.Should().Contain("bzm-gradient-end");
    }

    [Fact]
    public void BzMarquee_WithoutGradients_ShouldNotRenderGradientElements()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.ShowGradientEdges, false));

        cut.Markup.Should().NotContain("bzm-gradient-start");
        cut.Markup.Should().NotContain("bzm-gradient-end");
    }

    [Fact]
    public void BzMarquee_WithFullWidth_ShouldAddFullWidthClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.FullWidth, true));

        cut.Markup.Should().Contain("bzm-full-width");
    }

    [Fact]
    public void BzMarquee_WithText_ShouldAddTickerClass()
    {
        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Text, "Test"));

        cut.Markup.Should().Contain("bzm-ticker");
    }

    [Fact]
    public void BzMarquee_ShouldSetGapCssVariable()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Gap, 60));

        cut.Markup.Should().Contain("--bzm-gap: 60px");
    }

    #endregion

    #region Testimonial Auto-Detection Tests

    [Fact]
    public void BzMarquee_WithTestimonials_ShouldRenderTestimonialCards()
    {
        var items = CreateTestimonials();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("bzm-testimonial-card");
        cut.Markup.Should().Contain("bzm-testimonial-text");
    }

    [Fact]
    public void BzMarquee_WithTestimonialsWithoutImage_ShouldRenderInitials()
    {
        var items = new List<TestMarqueeItem>
        {
            new() { Id = 1, Name = "John Doe", Quote = "Great product!" }
        };

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("bzm-testimonial-initials");
        cut.Markup.Should().Contain("JD");
    }

    #endregion

    #region Stagger Entrance Tests

    [Fact]
    public void BzMarquee_WithStaggerEntranceDefault_ShouldAddStaggerClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("bzm-stagger");
    }

    [Fact]
    public void BzMarquee_WithStaggerEntranceFalse_ShouldNotAddStaggerClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.StaggerEntrance, false));

        cut.Markup.Should().NotContain("bzm-stagger");
    }

    [Fact]
    public void BzMarquee_WithStaggerEntrance_ItemsShouldBeRendered()
    {
        var items = CreateItems(5);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.StaggerEntrance, true));

        var marqueeItems = cut.FindAll(".bzm-item");
        marqueeItems.Should().HaveCount(5);
    }

    [Fact]
    public void BzMarquee_WithText_AndStaggerEntrance_ShouldAddBothClasses()
    {
        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Text, "Breaking news")
            .Add(x => x.StaggerEntrance, true));

        cut.Markup.Should().Contain("bzm-stagger");
        cut.Markup.Should().Contain("bzm-ticker");
    }

    [Fact]
    public void BzMarquee_StaggerDelay_ShouldAcceptCustomValue()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.StaggerDelay, 100));

        cut.Markup.Should().Contain("bzm-stagger");
    }

    #endregion

    #region FOUC Protection Tests

    [Fact]
    public void BzMarquee_BeforeInit_ShouldHaveHiddenInlineStyle()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("opacity:0");
        cut.Markup.Should().Contain("visibility:hidden");
    }

    [Fact]
    public void BzMarquee_BeforeInit_ShouldNotHaveReadyClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().NotContain("bzm-ready");
    }

    [Fact]
    public void BzMarquee_WithGap30AndRows2_ShouldSetRowGapVariable()
    {
        var items = CreateItems(6);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Gap, 30)
            .Add(x => x.Rows, 2));

        cut.Markup.Should().Contain("--bzm-row-gap: 10px");
    }

    [Fact]
    public void BzMarquee_WithRows1_ShouldNotSetRowGapVariable()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Gap, 30)
            .Add(x => x.Rows, 1));

        cut.Markup.Should().NotContain("--bzm-row-gap:");
    }

    #endregion

    #region GetInitials Tests

    [Fact]
    public void BzMarquee_TestimonialWithTwoWordName_ShouldShowCorrectInitials()
    {
        var items = new List<TestMarqueeItem>
        {
            new() { Id = 1, Name = "Sarah Chen", Quote = "Great!" }
        };

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("SC");
    }

    [Fact]
    public void BzMarquee_TestimonialWithSingleName_ShouldShowOneInitial()
    {
        var items = new List<TestMarqueeItem>
        {
            new() { Id = 1, Name = "Madonna", Quote = "Amazing product!" }
        };

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("bzm-testimonial-initials");
        cut.Markup.Should().Contain("M");
    }

    [Fact]
    public void BzMarquee_TestimonialWithThreeWordName_ShouldUseFirstAndLast()
    {
        var items = new List<TestMarqueeItem>
        {
            new() { Id = 1, Name = "Mary Jane Watson", Quote = "Superb!" }
        };

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("MW");
    }

    #endregion

    #region Parameter Clamping Tests

    [Fact]
    public void BzMarquee_WithRows0_ShouldClampTo1()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 0));

        var rows = cut.FindAll(".bzm-row");
        rows.Should().HaveCount(1);
    }

    [Fact]
    public void BzMarquee_WithRows15_ShouldClampTo10()
    {
        var items = CreateItems(20);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 15));

        var rows = cut.FindAll(".bzm-row");
        rows.Should().HaveCount(10);
    }

    [Fact]
    public void BzMarquee_WithNegativeRows_ShouldClampTo1()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, -5));

        var rows = cut.FindAll(".bzm-row");
        rows.Should().HaveCount(1);
    }

    #endregion

    #region RenderFragment Tests

    [Fact]
    public void BzMarquee_WithItemTemplate_ShouldRenderCustomContent()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.ItemTemplate, item =>
                builder =>
                {
                    builder.OpenElement(0, "div");
                    builder.AddAttribute(1, "class", "custom-item");
                    builder.AddContent(2, item.Name);
                    builder.CloseElement();
                }));

        cut.Markup.Should().Contain("custom-item");
        cut.Markup.Should().Contain("Company 1");
    }

    [Fact]
    public void BzMarquee_WithEmptyTemplate_ShouldRenderCustomEmptyState()
    {
        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.EmptyTemplate, builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "custom-empty");
                builder.AddContent(2, "Nothing here");
                builder.CloseElement();
            }));

        cut.Markup.Should().Contain("custom-empty");
        cut.Markup.Should().Contain("Nothing here");
    }

    [Fact]
    public void BzMarquee_WithDefaultEmptyState_ShouldShowDefaultMessage()
    {
        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>();

        cut.Markup.Should().Contain("bz-empty");
        cut.Markup.Should().Contain("No items to display");
    }

    #endregion

    #region Logo Rendering Tests

    [Fact]
    public void BzMarquee_WithImageOnlyItems_ShouldRenderLogoImages()
    {
        var items = new List<TestMarqueeItem>
        {
            new() { Id = 1, ImageUrl = "https://example.com/logo1.png" },
            new() { Id = 2, ImageUrl = "https://example.com/logo2.png" }
        };

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("bzm-logo-item");
        cut.Markup.Should().Contain("bzm-logo-image");
    }

    [Fact]
    public void BzMarquee_LogoWithTitle_ShouldUseAsAlt()
    {
        var items = new List<TestMarqueeItem>
        {
            new() { Id = 1, ImageUrl = "https://example.com/logo.png", Name = "Acme Corp" }
        };

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("alt=\"Acme Corp\"");
    }

    [Fact]
    public void BzMarquee_LogoWithoutTitle_ShouldUseFallbackAlt()
    {
        var items = new List<TestMarqueeItem>
        {
            new() { Id = 1, ImageUrl = "https://example.com/logo.png" }
        };

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("alt=\"Logo\"");
    }

    [Fact]
    public void BzMarquee_LogoImages_ShouldHaveLazyLoading()
    {
        var items = CreateItems(1);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var images = cut.FindAll("img.bzm-logo-image");
        images.Should().NotBeEmpty();
        images[0].GetAttribute("loading").Should().Be("lazy");
    }

    [Fact]
    public void BzMarquee_LogoImages_ShouldHaveDraggableFalse()
    {
        var items = CreateItems(1);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var images = cut.FindAll("img.bzm-logo-image");
        images.Should().NotBeEmpty();
        images[0].GetAttribute("draggable").Should().Be("false");
    }

    #endregion

    #region Testimonial Rendering Tests

    [Fact]
    public void BzMarquee_TestimonialWithAvatar_ShouldRenderAvatarImage()
    {
        var items = new List<TestMarqueeItem>
        {
            new() { Id = 1, ImageUrl = "https://example.com/avatar.jpg", Name = "Jane Doe", Quote = "Great!" }
        };

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("bzm-testimonial-avatar");
        cut.Markup.Should().NotContain("bzm-testimonial-initials");
    }

    [Fact]
    public void BzMarquee_TestimonialAvatarImage_ShouldHaveLazyLoading()
    {
        var items = new List<TestMarqueeItem>
        {
            new() { Id = 1, ImageUrl = "https://example.com/avatar.jpg", Name = "Jane Doe", Quote = "Great!" }
        };

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var avatar = cut.Find("img.bzm-testimonial-avatar");
        avatar.GetAttribute("loading").Should().Be("lazy");
        avatar.GetAttribute("draggable").Should().Be("false");
    }

    [Fact]
    public void BzMarquee_TestimonialQuote_ShouldRenderInBlockquote()
    {
        var items = new List<TestMarqueeItem>
        {
            new() { Id = 1, Name = "Test User", Quote = "Amazing product review" }
        };

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("bzm-testimonial-text");
        cut.Markup.Should().Contain("Amazing product review");
    }

    [Fact]
    public void BzMarquee_TestimonialName_ShouldBeRendered()
    {
        var items = new List<TestMarqueeItem>
        {
            new() { Id = 1, Name = "Sarah Chen", Quote = "Excellent!" }
        };

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("bzm-testimonial-name");
        cut.Markup.Should().Contain("Sarah Chen");
    }

    #endregion

    #region Custom CSS Class Tests

    [Fact]
    public void BzMarquee_WithCustomCssClass_ShouldAddToContainer()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.CssClass, "my-custom-marquee"));

        cut.Markup.Should().Contain("my-custom-marquee");
    }

    [Fact]
    public void BzMarquee_WithDirection_ShouldSetDataAttribute()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Direction, BzDirection.Right));

        var row = cut.Find(".bzm-row");
        row.GetAttribute("data-direction").Should().Be("right");
    }

    [Fact]
    public void BzMarquee_WithSpeed_ShouldSetDataAttribute()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Speed, 80));

        var row = cut.Find(".bzm-row");
        row.GetAttribute("data-speed").Should().Be("80");
    }

    #endregion

    #region Edge Case Tests

    [Fact]
    public void BzMarquee_SingleItem_ShouldRenderOneItem()
    {
        var items = CreateItems(1);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.FindAll(".bzm-item").Should().HaveCount(1);
    }

    [Fact]
    public void BzMarquee_EmptyList_ShouldShowEmptyState()
    {
        var items = new List<TestMarqueeItem>();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("bz-empty");
    }

    [Fact]
    public void BzMarquee_WithTextAndItems_TextShouldTakePrecedence()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Text, "Ticker text"));

        cut.Markup.Should().Contain("bzm-text-item");
        cut.Markup.Should().Contain("Ticker text");
        cut.Markup.Should().NotContain("bzm-logo-item");
    }

    [Fact]
    public void BzMarquee_RowDataAttribute_ShouldBeZeroIndexed()
    {
        var items = CreateItems(6);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Rows, 3));

        var rows = cut.FindAll(".bzm-row");
        rows[0].GetAttribute("data-row").Should().Be("0");
        rows[1].GetAttribute("data-row").Should().Be("1");
        rows[2].GetAttribute("data-row").Should().Be("2");
    }

    [Fact]
    public void BzMarquee_ContentGroup_ShouldNotBeAriaHidden()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var content = cut.Find(".bzm-content");
        content.GetAttribute("aria-hidden").Should().Be("false");
    }

    #endregion
}
