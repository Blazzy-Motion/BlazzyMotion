namespace BlazzyMotion.Tests.Marquee.Components;

public class BzMarqueeEventTests : TestBase
{
    private static List<TestMarqueeItem> CreateItems(int count = 4) =>
        Enumerable.Range(1, count).Select(i => new TestMarqueeItem
        {
            Id = i,
            ImageUrl = $"https://example.com/logo{i}.png",
            Name = $"Company {i}"
        }).ToList();

    private static List<TestMarqueeItem> CreateTestimonials(int count = 3) =>
        Enumerable.Range(1, count).Select(i => new TestMarqueeItem
        {
            Id = i,
            ImageUrl = $"https://example.com/avatar{i}.jpg",
            Name = $"Person {i}",
            Quote = $"This is a great product review number {i}."
        }).ToList();

    #region OnItemClick Callback Tests

    [Fact]
    public void BzMarquee_ItemClicked_ShouldFireOnItemClick()
    {
        TestMarqueeItem? clickedItem = null;
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.OnItemClick, EventCallback.Factory.Create<TestMarqueeItem>(this, item => clickedItem = item)));

        var firstItem = cut.FindAll(".bzm-item")[0];
        firstItem.Click();

        clickedItem.Should().NotBeNull();
        clickedItem!.Id.Should().Be(1);
        clickedItem.Name.Should().Be("Company 1");
    }

    [Fact]
    public void BzMarquee_SecondItemClicked_ShouldReturnCorrectItem()
    {
        TestMarqueeItem? clickedItem = null;
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.OnItemClick, EventCallback.Factory.Create<TestMarqueeItem>(this, item => clickedItem = item)));

        var secondItem = cut.FindAll(".bzm-item")[1];
        secondItem.Click();

        clickedItem.Should().NotBeNull();
        clickedItem!.Id.Should().Be(2);
    }

    [Fact]
    public void BzMarquee_WithNoClickHandler_ShouldNotThrow()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var firstItem = cut.FindAll(".bzm-item")[0];
        var action = () => firstItem.Click();
        action.Should().NotThrow();
    }

    [Fact]
    public void BzMarquee_WithClickHandler_ShouldAddClickableClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.OnItemClick, EventCallback.Factory.Create<TestMarqueeItem>(this, _ => { })));

        cut.Markup.Should().Contain("bzm-clickable");
    }

    [Fact]
    public void BzMarquee_WithoutClickHandler_ShouldNotAddClickableClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().NotContain("bzm-clickable");
    }

    [Fact]
    public void BzMarquee_WithClickHandler_ItemsShouldHaveButtonRole()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.OnItemClick, EventCallback.Factory.Create<TestMarqueeItem>(this, _ => { })));

        var firstItem = cut.FindAll(".bzm-item")[0];
        firstItem.GetAttribute("role").Should().Be("button");
    }

    [Fact]
    public void BzMarquee_WithoutClickHandler_ItemsShouldNotHaveButtonRole()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var firstItem = cut.FindAll(".bzm-item")[0];
        firstItem.GetAttribute("role").Should().BeNull();
    }

    #endregion

    #region Re-render Tests

    [Fact]
    public void BzMarquee_SetParametersWithNewItems_ShouldUpdateRenderedItems()
    {
        var items = CreateItems(2);

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.FindAll(".bzm-item").Should().HaveCount(2);

        var newItems = CreateItems(5);
        cut.SetParametersAndRender(p => p
            .Add(x => x.Items, newItems));

        cut.FindAll(".bzm-item").Should().HaveCount(5);
    }

    [Fact]
    public void BzMarquee_SetParametersWithEmptyItems_ShouldShowEmptyState()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.FindAll(".bzm-item").Should().HaveCount(4);

        cut.SetParametersAndRender(p => p
            .Add(x => x.Items, new List<TestMarqueeItem>()));

        cut.Markup.Should().Contain("bz-empty");
    }

    [Fact]
    public void BzMarquee_SetParametersWithNewTheme_ShouldUpdateThemeClass()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Theme, BzTheme.Glass));

        cut.Markup.Should().Contain("bzc-theme-glass");

        cut.SetParametersAndRender(p => p
            .Add(x => x.Items, items)
            .Add(x => x.Theme, BzTheme.Dark));

        cut.Markup.Should().Contain("bzc-theme-dark");
        cut.Markup.Should().NotContain("bzc-theme-glass");
    }

    [Fact]
    public void BzMarquee_SwitchFromItemsToText_ShouldRenderTextMode()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        cut.Markup.Should().Contain("bzm-item");

        cut.SetParametersAndRender(p => p
            .Add(x => x.Items, (IEnumerable<TestMarqueeItem>?)null)
            .Add(x => x.Text, "Breaking news ticker"));

        cut.Markup.Should().Contain("bzm-text-item");
        cut.Markup.Should().Contain("Breaking news ticker");
    }

    #endregion

    #region Disposal Tests

    [Fact]
    public void BzMarquee_Dispose_ShouldNotThrow()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var action = () => cut.Instance.DisposeAsync();
        action.Should().NotThrow();
    }

    [Fact]
    public void BzMarquee_DisposeWithoutItems_ShouldNotThrow()
    {
        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>();

        var action = () => cut.Instance.DisposeAsync();
        action.Should().NotThrow();
    }

    [Fact]
    public void BzMarquee_DoubleDispose_ShouldNotThrow()
    {
        var items = CreateItems();

        var cut = RenderComponent<BzMarquee<TestMarqueeItem>>(p => p
            .Add(x => x.Items, items));

        var action = () =>
        {
            cut.Instance.DisposeAsync().AsTask().GetAwaiter().GetResult();
            cut.Instance.DisposeAsync().AsTask().GetAwaiter().GetResult();
        };
        action.Should().NotThrow();
    }

    #endregion
}
