using System.Reflection;
using Bunit.JSInterop;

namespace BlazzyMotion.Tests.Core.Services;

/// <summary>
/// Tests for BzJsInteropBase abstract class.
/// Tests constructor validation and basic properties.
/// Note: Async JS interop tests are covered by component integration tests.
/// </summary>
public class BzJsInteropBaseTests : TestContext
{
  public BzJsInteropBaseTests()
  {
    JSInterop.Mode = JSRuntimeMode.Loose;
  }

  #region Constructor Validation Tests

  [Fact]
  public void Constructor_ThrowsArgumentNullException_WhenJsRuntimeIsNull()
  {
    // Act
    var act = () => new TestBzJsInterop(null!, "./_content/Test/test.js");

    // Assert
    act.Should().Throw<ArgumentNullException>()
        .WithParameterName("jsRuntime");
  }

  [Fact]
  public void Constructor_ThrowsArgumentException_WhenModulePathIsNull()
  {
    // Arrange
    var jsRuntime = JSInterop.JSRuntime;

    // Act
    var act = () => new TestBzJsInterop(jsRuntime, null!);

    // Assert
    act.Should().Throw<ArgumentException>()
        .WithParameterName("modulePath");
  }

  [Fact]
  public void Constructor_ThrowsArgumentException_WhenModulePathIsEmpty()
  {
    // Arrange
    var jsRuntime = JSInterop.JSRuntime;

    // Act
    var act = () => new TestBzJsInterop(jsRuntime, "");

    // Assert
    act.Should().Throw<ArgumentException>()
        .WithParameterName("modulePath");
  }

  [Fact]
  public void Constructor_ThrowsArgumentException_WhenModulePathIsWhitespace()
  {
    // Arrange
    var jsRuntime = JSInterop.JSRuntime;

    // Act
    var act = () => new TestBzJsInterop(jsRuntime, "   ");

    // Assert
    act.Should().Throw<ArgumentException>()
        .WithParameterName("modulePath");
  }

  [Fact]
  public void Constructor_CreatesInstance_WithValidParameters()
  {
    // Arrange
    var jsRuntime = JSInterop.JSRuntime;

    // Act
    var interop = new TestBzJsInterop(jsRuntime, "./_content/Test/test.js");

    // Assert
    interop.Should().NotBeNull();
  }

  #endregion

  #region IsModuleLoaded Tests

  [Fact]
  public void IsModuleLoaded_ReturnsFalse_BeforeModuleAccessed()
  {
    // Arrange
    var jsRuntime = JSInterop.JSRuntime;
    var interop = new TestBzJsInterop(jsRuntime, "./_content/Test/test.js");

    // Assert
    interop.TestIsModuleLoaded.Should().BeFalse();
  }

  #endregion

  #region IsDisposed Tests

  [Fact]
  public void IsDisposed_ReturnsFalse_Initially()
  {
    // Arrange
    var jsRuntime = JSInterop.JSRuntime;
    var interop = new TestBzJsInterop(jsRuntime, "./_content/Test/test.js");

    // Assert
    interop.TestIsDisposed.Should().BeFalse();
  }

  #endregion

  #region SetElement Tests

  [Fact]
  public void SetElement_SetsElementRefProperty()
  {
    // Arrange
    var jsRuntime = JSInterop.JSRuntime;
    var interop = new TestBzJsInterop(jsRuntime, "./_content/Test/test.js");
    var element = new ElementReference("test-id");

    // Act
    interop.TestSetElement(element);

    // Assert
    interop.TestElementRef.Should().Be(element);
  }

  [Fact]
  public void SetElement_CanBeCalledMultipleTimes()
  {
    // Arrange
    var jsRuntime = JSInterop.JSRuntime;
    var interop = new TestBzJsInterop(jsRuntime, "./_content/Test/test.js");
    var element1 = new ElementReference("test-id-1");
    var element2 = new ElementReference("test-id-2");

    // Act
    interop.TestSetElement(element1);
    interop.TestSetElement(element2);

    // Assert
    interop.TestElementRef.Should().Be(element2);
  }

  #endregion

  #region BzJsInteropBase Class Tests

  [Fact]
  public void BzJsInteropBase_ImplementsIAsyncDisposable()
  {
    // Assert
    typeof(BzJsInteropBase).Should().Implement<IAsyncDisposable>();
  }

  [Fact]
  public void BzJsInteropBase_IsAbstract()
  {
    // Assert
    typeof(BzJsInteropBase).IsAbstract.Should().BeTrue();
  }

  [Fact]
  public void BzJsInteropBase_HasProtectedConstructor()
  {
    // Assert
    var constructors = typeof(BzJsInteropBase).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
    constructors.Should().HaveCount(1);
    constructors[0].IsFamily.Should().BeTrue();
  }

  #endregion

  #region Test Implementation

  /// <summary>
  /// Concrete implementation of BzJsInteropBase for testing.
  /// </summary>
  private class TestBzJsInterop : BzJsInteropBase
  {
    public TestBzJsInterop(IJSRuntime jsRuntime, string modulePath)
        : base(jsRuntime, modulePath)
    {
    }

    public bool TestIsModuleLoaded => IsModuleLoaded;
    public bool TestIsDisposed => IsDisposed;
    public ElementReference? TestElementRef => ElementRef;
    public void TestSetElement(ElementReference element) => SetElement(element);

    protected override ValueTask DisposeAsyncCore()
    {
      return ValueTask.CompletedTask;
    }
  }

  #endregion
}
