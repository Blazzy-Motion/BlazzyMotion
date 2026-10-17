using System.Text.Json;

namespace BlazzyMotion.Tests.Carousel.Components;

/// <summary>Regression coverage for optional presentation and accessible navigation.</summary>
public class BzCarouselRefreshTests : TestBase
{
    /// <summary>Completes JS setup so configuration assertions exercise initialization.</summary>
    public BzCarouselRefreshTests()
    {
        CoreModule.SetupVoid("ensureSwiperLoaded", _ => true).SetVoidResult();
        CoreModule.SetupVoid("initializeCarousel", _ => true).SetVoidResult();
        CoreModule.SetupVoid("destroyCarousel", _ => true).SetVoidResult();
    }

    /// <summary>Cards must reach JS even when automatic mode would choose simple slides.</summary>
    [Fact]
    public void Cards_WithTwoItems_SendsCardsConfiguration()
    {
        var cut = RenderComponent<BzCarousel<TestMovie>>(p => p
            .Add(x => x.Items, new[] { new TestMovie(), new TestMovie() })
            .Add(x => x.Variant, BzCarouselVariant.Cards));
        var call = CoreModule.Invocations["initializeCarousel"].Last();
        using var options = JsonDocument.Parse((string)call.Arguments[1]!);
        options.RootElement.GetProperty("effect").GetString().Should().Be("cards");
        options.RootElement.GetProperty("slidesPerView").GetString().Should().Be("1");
        options.RootElement.GetProperty("loop").GetBoolean().Should().BeFalse();
    }

    /// <summary>Existing small collections retain their simple non-looping presentation.</summary>
    [Fact]
    public void Default_WithTwoItems_PreservesSimpleMode()
    {
        RenderComponent<BzCarousel<TestMovie>>(p => p
            .Add(x => x.Items, new[] { new TestMovie(), new TestMovie() }));
        using var options = JsonDocument.Parse((string)CoreModule.Invocations["initializeCarousel"].Last().Arguments[1]!);
        options.RootElement.GetProperty("effect").GetString().Should().Be("slide");
        options.RootElement.GetProperty("loop").GetBoolean().Should().BeFalse();
    }

    /// <summary>Consumers can name the region and opt out of visible navigation.</summary>
    [Fact]
    public void Navigation_CanBeHidden_AndRegionCanBeNamed()
    {
        var cut = RenderComponent<BzCarousel<TestMovie>>(p => p
            .Add(x => x.Items, new[] { new TestMovie(), new TestMovie() })
            .Add(x => x.ShowNavigation, false)
            .Add(x => x.AdditionalAttributes, new Dictionary<string, object> { ["aria-label"] = "Featured products" }));
        cut.Find("[role=region]").GetAttribute("aria-label").Should().Be("Featured products");
        cut.FindAll(".bzc-navigation").Should().BeEmpty();
    }

    /// <summary>Keyboard selection invokes the original item callback exactly once.</summary>
    [Fact]
    public void SelectionButton_UsesExistingCallback()
    {
        var movie = new TestMovie { Title = "Selected" };
        var selected = new List<TestMovie>();
        var cut = RenderComponent<BzCarousel<TestMovie>>(p => p
            .Add(x => x.Items, new[] { movie })
            .Add(x => x.OnItemSelected, (TestMovie item) => selected.Add(item)));
        cut.Find(".bzc-select").Click();
        selected.Should().ContainSingle().Which.Should().BeSameAs(movie);
    }
    /// <summary>Showing navigation after initialization must bind the newly rendered controls.</summary>
    [Fact]
    public void EnablingNavigation_ReinitializesTheRenderedControls()
    {
        var cut = RenderComponent<BzCarousel<TestMovie>>(p => p
            .Add(x => x.Items, new[] { new TestMovie(), new TestMovie() })
            .Add(x => x.ShowNavigation, false));
        var initializations = CoreModule.Invocations["initializeCarousel"].Count;
        cut.SetParametersAndRender(p => p.Add(x => x.ShowNavigation, true));
        cut.FindAll(".bzc-navigation button").Should().HaveCount(2);
        CoreModule.Invocations["initializeCarousel"].Count.Should().Be(initializations + 1);
    }

    /// <summary>Cards keeps safe loop boundaries and honors consumer timing options.</summary>
    /// <param name="count">Number of consumer items.</param>
    /// <param name="loop">Whether the collection can loop using the existing minimum.</param>
    [Theory]
    [InlineData(1, false)]
    [InlineData(2, false)]
    [InlineData(4, true)]
    [InlineData(6, true)]
    public void Cards_PreservesLoopThreshold_AndCustomTiming(int count, bool loop)
    {
        RenderComponent<BzCarousel<TestMovie>>(p => p
            .Add(x => x.Items, Enumerable.Range(0, count).Select(_ => new TestMovie()).ToArray())
            .Add(x => x.Variant, BzCarouselVariant.Cards)
            .Add(x => x.Options, new BzCarouselOptions { Speed = 650, Effect = "fade" }));
        using var options = JsonDocument.Parse((string)CoreModule.Invocations["initializeCarousel"].Last().Arguments[1]!);
        options.RootElement.GetProperty("effect").GetString().Should().Be("cards");
        options.RootElement.GetProperty("speed").GetInt32().Should().Be(650);
        options.RootElement.GetProperty("loop").GetBoolean().Should().Be(loop);
    }

    /// <summary>Navigation is only rendered for collections that can actually advance.</summary>
    /// <param name="count">Number of consumer items.</param>
    /// <param name="buttons">Expected number of navigation controls.</param>
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(2, 2)]
    public void Navigation_ReflectsCollectionSize(int count, int buttons)
    {
        var cut = RenderComponent<BzCarousel<TestMovie>>(p => p
            .Add(x => x.Items, Enumerable.Range(0, count).Select(_ => new TestMovie()).ToArray()));
        cut.FindAll(".bzc-navigation button").Should().HaveCount(buttons);
    }

    /// <summary>Dots identify original items rather than loop copies and retain an arrow opt-in.</summary>
    [Fact]
    public void Navigation_DefaultsToNamedDots_AndSupportsArrows()
    {
        var cut = RenderComponent<BzCarousel<TestMovie>>(p => p
            .Add(x => x.Items, new[] { new TestMovie(), new TestMovie(), new TestMovie() }));
        cut.FindAll(".bzc-dot").Should().HaveCount(3);
        cut.Find("[data-bzc-index='2']").GetAttribute("aria-label").Should().Be("Go to slide 3");
        cut.SetParametersAndRender(p => p.Add(x => x.NavigationStyle, BzCarouselNavigation.Arrows));
        cut.FindAll(".bzc-dot").Should().BeEmpty();
        cut.Find(".bzc-previous").Should().NotBeNull();
        cut.Find(".bzc-next").Should().NotBeNull();
    }

}
