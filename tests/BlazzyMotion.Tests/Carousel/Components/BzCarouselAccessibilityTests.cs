namespace BlazzyMotion.Tests.Carousel.Components;

/// <summary>Protects the accessible shell without changing legacy selection behavior.</summary>
public class BzCarouselAccessibilityTests : TestBase
{
    /// <summary>Consumers can name the region using the existing attribute contract.</summary>
    [Fact]
    public void RegionPreservesConsumerLabelAndProvidesKeyboardInstructions()
    {
        var cut = RenderComponent<BzCarousel<TestMovie>>(p => p
            .Add(c => c.Items, new[] { new TestMovie() })
            .AddUnmatched("aria-label", "Featured movies"));
        cut.Find("[role=region]").GetAttribute("aria-label").Should().Be("Featured movies");
        cut.Find("[role=region]").GetAttribute("tabindex").Should().Be("0");
        cut.Find(".bzc-sr-only").TextContent.Should().Contain("Arrow");
        cut.FindAll("[data-bzc-direction]").Should().BeEmpty();
    }

    /// <summary>Loading does not attempt to initialize a nonexistent carousel element.</summary>
    [Fact]
    public void LoadingDoesNotInitializeSwiper()
    {
        var cut = RenderComponent<BzCarousel<TestMovie>>(p => p.Add(c => c.Items, null));
        cut.Find(".bzc-loading").Should().NotBeNull();
        CoreModule.Invocations.Should().NotContain(call => call.Identifier == "initializeCarousel");
    }

    /// <summary>Selection remains opt-in and receives the original consumer item.</summary>
    [Fact]
    public void SlideRetainsClickCallbackAndOriginalItem()
    {
        var movie = new TestMovie { Title = "Original" };
        TestMovie? selected = null;
        var cut = RenderComponent<BzCarousel<TestMovie>>(p => p
            .Add(c => c.Items, new[] { movie })
            .Add(c => c.OnItemSelected, item => selected = item));
        cut.Find(".swiper-slide").GetAttribute("data-bzc-selectable").Should().Be("true");
        cut.Find(".swiper-slide").Click();
        selected.Should().BeSameAs(movie);
        cut.Find(".swiper-container").ClassList.Should().Contain("bzc-pending");
        cut.Find("[role=status]").GetAttribute("aria-live").Should().Be("polite");
    }
}
