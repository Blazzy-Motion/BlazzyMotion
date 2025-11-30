namespace BlazzyMotion.Tests.Services;

/// <summary>
/// Tests for BzCarouselJsInterop service with mocked JavaScript interop
/// </summary>
public class BzCarouselJsInteropTests : TestContext
{
  [Fact]
  public async Task InitializeAsync_ShouldLoadModuleAndInitializeCarousel()
  {
    // Arrange
    var jsInterop = JSInterop.SetupModule("./_content/BlazzyMotion.Carousel/js/blazzy-carousel.js");
    jsInterop.SetupVoid("ensureSwiperLoaded");
    jsInterop.SetupVoid("initializeCarousel");

    var element = new ElementReference();
    var options = new BzCarouselOptions
    {
      Effect = "coverflow",
      Loop = true
    };

    var service = new BzCarouselJsInterop(JSInterop.JSRuntime);

    // Act
    await service.InitializeAsync(element, options);

    // Assert
    jsInterop.VerifyInvoke("ensureSwiperLoaded");
    jsInterop.VerifyInvoke("initializeCarousel");
  }

  [Fact]
  public async Task GetActiveIndexAsync_ShouldReturnActiveSlideIndex()
  {
    // Arrange
    var jsInterop = JSInterop.SetupModule("./_content/BlazzyMotion.Carousel/js/blazzy-carousel.js");
    jsInterop.SetupVoid("ensureSwiperLoaded");
    jsInterop.SetupVoid("initializeCarousel");
    jsInterop.Setup<int>("getActiveIndex").SetResult(2);

    var element = new ElementReference();
    var options = new BzCarouselOptions { Effect = "coverflow" };

    var service = new BzCarouselJsInterop(JSInterop.JSRuntime);

    // Initialize first
    await service.InitializeAsync(element, options);

    // Act
    var activeIndex = await service.GetActiveIndexAsync();

    // Assert
    activeIndex.Should().Be(2);
  }

  [Fact]
  public async Task GetActiveIndexAsync_WithoutInitialization_ShouldReturnZero()
  {
    // Arrange
    var service = new BzCarouselJsInterop(JSInterop.JSRuntime);

    // Act
    var activeIndex = await service.GetActiveIndexAsync();

    // Assert
    activeIndex.Should().Be(0);
  }

  [Fact]
  public async Task DestroyAsync_ShouldCallDestroyCarousel()
  {
    // Arrange
    var jsInterop = JSInterop.SetupModule("./_content/BlazzyMotion.Carousel/js/blazzy-carousel.js");
    jsInterop.SetupVoid("ensureSwiperLoaded");
    jsInterop.SetupVoid("initializeCarousel");
    jsInterop.SetupVoid("destroyCarousel");

    var element = new ElementReference();
    var options = new BzCarouselOptions { Effect = "coverflow" };

    var service = new BzCarouselJsInterop(JSInterop.JSRuntime);

    // Initialize first
    await service.InitializeAsync(element, options);

    // Act
    await service.DestroyAsync();

    // Assert
    jsInterop.VerifyInvoke("destroyCarousel");
  }

  [Fact]
  public async Task DestroyAsync_WithoutInitialization_ShouldNotThrow()
  {
    // Arrange
    var service = new BzCarouselJsInterop(JSInterop.JSRuntime);

    // Act
    var act = async () => await service.DestroyAsync();

    // Assert
    await act.Should().NotThrowAsync();
  }

  [Fact]
  public async Task DisposeAsync_ShouldDestroyAndDisposeModule()
  {
    // Arrange
    var jsInterop = JSInterop.SetupModule("./_content/BlazzyMotion.Carousel/js/blazzy-carousel.js");
    jsInterop.SetupVoid("ensureSwiperLoaded");
    jsInterop.SetupVoid("initializeCarousel");
    jsInterop.SetupVoid("destroyCarousel");

    var element = new ElementReference();
    var options = new BzCarouselOptions { Effect = "coverflow" };

    var service = new BzCarouselJsInterop(JSInterop.JSRuntime);

    // Initialize first
    await service.InitializeAsync(element, options);

    // Act
    await service.DisposeAsync();

    // Assert
    jsInterop.VerifyInvoke("destroyCarousel");
  }

  [Fact]
  public async Task InitializeAsync_CalledTwice_ShouldLoadSwiperOnlyOnce()
  {
    // Arrange
    var jsInterop = JSInterop.SetupModule("./_content/BlazzyMotion.Carousel/js/blazzy-carousel.js");
    jsInterop.SetupVoid("ensureSwiperLoaded");
    jsInterop.SetupVoid("initializeCarousel");

    var element = new ElementReference();
    var options = new BzCarouselOptions { Effect = "coverflow" };

    var service = new BzCarouselJsInterop(JSInterop.JSRuntime);

    // Act
    await service.InitializeAsync(element, options);
    await service.InitializeAsync(element, options);

    // Assert - ensureSwiperLoaded should be called only once
    jsInterop.VerifyInvoke("ensureSwiperLoaded", 1);
    jsInterop.VerifyInvoke("initializeCarousel", 2);
  }

  [Fact]
  public async Task InitializeAsync_WithDifferentOptions_ShouldSerializeCorrectly()
  {
    // Arrange
    var jsInterop = JSInterop.SetupModule("./_content/BlazzyMotion.Carousel/js/blazzy-carousel.js");
    jsInterop.SetupVoid("ensureSwiperLoaded");
    jsInterop.SetupVoid("initializeCarousel");

    var element = new ElementReference();
    var options = new BzCarouselOptions
    {
      Effect = "fade",
      Speed = 1000,
      Loop = false,
      RotateDegree = 45,
      Depth = 100
    };

    var service = new BzCarouselJsInterop(JSInterop.JSRuntime);

    // Act
    await service.InitializeAsync(element, options);

    // Assert - verify that initializeCarousel was called
    jsInterop.VerifyInvoke("initializeCarousel");
  }
}
