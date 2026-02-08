namespace BlazzyMotion.Tests.Gallery.Services;

/// <summary>
/// Tests for BzGalleryJsInterop service.
/// </summary>
public class BzGalleryJsInteropTests : TestBase
{
    [Fact]
    public void Constructor_WithValidJsRuntime_ShouldCreateInstance()
    {
        var service = new BzGalleryJsInterop(JSInterop.JSRuntime);
        service.Should().NotBeNull();
    }

    [Fact]
    public void Service_ShouldImplementIAsyncDisposable()
    {
        var service = new BzGalleryJsInterop(JSInterop.JSRuntime);
        service.Should().BeAssignableTo<IAsyncDisposable>();
    }

    [Fact]
    public void Service_ShouldInheritFromBzJsInteropBase()
    {
        var service = new BzGalleryJsInterop(JSInterop.JSRuntime);
        service.Should().BeAssignableTo<BzJsInteropBase>();
    }
}
