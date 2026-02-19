namespace BlazzyMotion.Tests.Marquee.Services;

public class BzMarqueeJsInteropTests : TestBase
{
    [Fact]
    public void Constructor_WithValidJsRuntime_ShouldCreateInstance()
    {
        var service = new BzMarqueeJsInterop(JSInterop.JSRuntime);
        service.Should().NotBeNull();
    }

    [Fact]
    public void Service_ShouldImplementIAsyncDisposable()
    {
        var service = new BzMarqueeJsInterop(JSInterop.JSRuntime);
        service.Should().BeAssignableTo<IAsyncDisposable>();
    }

    [Fact]
    public void Service_ShouldInheritFromBzJsInteropBase()
    {
        var service = new BzMarqueeJsInterop(JSInterop.JSRuntime);
        service.Should().BeAssignableTo<BzJsInteropBase>();
    }
}
