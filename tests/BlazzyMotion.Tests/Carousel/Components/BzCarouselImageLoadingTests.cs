using BlazzyMotion.Core.Models;
using BlazzyMotion.Core.Services;

namespace BlazzyMotion.Tests.Carousel.Components;

/// <summary>Protects initial-image priority and existing template behavior.</summary>
public class BzCarouselImageLoadingTests : TestBase
{
    /// <summary>Registers a test-local mapper without altering shared TestMovie behavior.</summary>
    public BzCarouselImageLoadingTests()
    {
        BzRegistry.Register<LoadingMovie>(item => new BzItem
        {
            ImageUrl = item.ImageUrl,
            Title = item.Title,
            OriginalItem = item
        });
    }

    private sealed class LoadingMovie
    {
        public string? ImageUrl { get; set; }
        public string? Title { get; set; }
    }

    /// <summary>Only the requested initial image receives eager loading, including loop wrapping.</summary>
    /// <param name="initial">The requested initial index.</param>
    /// <param name="expected">The normalized index in the original collection.</param>
    /// <param name="count">The collection size, including automatic small-collection mode.</param>
    [Theory]
    [InlineData(0, 0, 8)]
    [InlineData(2, 2, 8)]
    [InlineData(10, 2, 8)]
    [InlineData(2, 0, 3)]
    public void InitialImageHasPriorityWhileOtherImagesStayLazy(int initial, int expected, int count)
    {
        var items = Enumerable.Range(0, count).Select(i => new LoadingMovie { ImageUrl = $"{i}.jpg", Title = $"Movie {i}" }).ToArray();
        var cut = RenderComponent<BzCarousel<LoadingMovie>>(p => p.Add(c => c.Items, items).Add(c => c.InitialSlide, initial));
        var eager = cut.Find("img[loading=eager]");
        eager.GetAttribute("src").Should().Be($"{expected}.jpg");
        eager.GetAttribute("alt").Should().Be($"Movie {expected}");
        eager.GetAttribute("fetchpriority").Should().Be("high");
        cut.FindAll("img[loading=lazy]").Should().HaveCount(count - 1);
        cut.FindAll(".bzc-media-placeholder").Should().HaveCount(count);
    }

    /// <summary>Advanced option precedence also controls which image receives priority.</summary>
    [Fact]
    public void AdvancedInitialSlideOverridesComponentImagePriority()
    {
        var items = Enumerable.Range(0, 8).Select(i => new LoadingMovie { ImageUrl = $"{i}.jpg" }).ToArray();
        var cut = RenderComponent<BzCarousel<LoadingMovie>>(p => p.Add(c => c.Items, items)
            .Add(c => c.InitialSlide, 2)
            .Add(c => c.Loop, false)
            .Add(c => c.Options, new BlazzyMotion.Carousel.Models.BzCarouselOptions { InitialSlide = 100 }));
        cut.Find("img[loading=eager]").GetAttribute("src").Should().Be("7.jpg");
    }

    /// <summary>Untitled or missing images retain their existing accessible fallback behavior.</summary>
    [Fact]
    public void MissingImageDoesNotProduceAStuckPlaceholder()
    {
        var cut = RenderComponent<BzCarousel<LoadingMovie>>(p => p.Add(c => c.Items, new[] { new LoadingMovie { ImageUrl = "poster.jpg" }, new LoadingMovie() }));
        cut.Find("img").GetAttribute("alt").Should().Be("Image");
        cut.Find("img").HasAttribute("title").Should().BeFalse();
        cut.FindAll(".bzc-media-placeholder").Should().ContainSingle();
    }

    /// <summary>Custom templates retain control over their loading strategy and markup.</summary>
    [Fact]
    public void CustomTemplateDoesNotReceiveMappedImageManagement()
    {
        var cut = RenderComponent<BzCarousel<LoadingMovie>>(p => p
            .Add(c => c.Items, new[] { new LoadingMovie { ImageUrl = "poster.jpg" } })
            .Add(c => c.ItemTemplate, item => builder =>
            {
                builder.OpenElement(0, "img");
                builder.AddAttribute(1, "src", item.ImageUrl);
                builder.AddAttribute(2, "loading", "lazy");
                builder.AddAttribute(3, "alt", "Custom image");
                builder.CloseElement();
            }));
        cut.Find("img").GetAttribute("loading").Should().Be("lazy");
        cut.FindAll("[data-bzc-media]").Should().BeEmpty();
        cut.FindAll(".bzc-media-placeholder").Should().BeEmpty();
    }
}
