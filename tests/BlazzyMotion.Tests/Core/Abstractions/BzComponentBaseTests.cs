using BlazzyMotion.Core.Abstractions;
using BlazzyMotion.Core.Models;

namespace BlazzyMotion.Tests.Core.Abstractions;

/// <summary>
/// Tests for BzComponentBase abstract base class.
/// Uses a concrete test implementation to test the base class behavior.
/// </summary>
public class BzComponentBaseTests : TestContext
{
  #region Theme Parameter Tests

  [Fact]
  public void Theme_DefaultValue_ShouldBeGlass()
  {
    // Act
    var cut = RenderComponent<TestBzComponent>();

    // Assert
    cut.Instance.Theme.Should().Be(BzTheme.Glass);
  }

  [Theory]
  [InlineData(BzTheme.Glass)]
  [InlineData(BzTheme.Dark)]
  [InlineData(BzTheme.Light)]
  [InlineData(BzTheme.Minimal)]
  public void Theme_CanBeSetToAllValues(BzTheme theme)
  {
    // Act
    var cut = RenderComponent<TestBzComponent>(p => p.Add(c => c.Theme, theme));

    // Assert
    cut.Instance.Theme.Should().Be(theme);
  }

  #endregion

  #region ThemeClass Tests

  [Theory]
  [InlineData(BzTheme.Glass, "bzc-theme-glass")]
  [InlineData(BzTheme.Dark, "bzc-theme-dark")]
  [InlineData(BzTheme.Light, "bzc-theme-light")]
  [InlineData(BzTheme.Minimal, "bzc-theme-minimal")]
  public void ThemeClass_ReturnsCorrectClass_ForEachTheme(BzTheme theme, string expectedClass)
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>(p => p.Add(c => c.Theme, theme));

    // Act
    var themeClass = cut.Instance.GetThemeClass();

    // Assert
    themeClass.Should().Be(expectedClass);
  }

  [Fact]
  public void ThemeClass_ReturnsGlass_ForUndefinedEnumValue()
  {
    // Arrange
    var invalidTheme = (BzTheme)999;
    var cut = RenderComponent<TestBzComponent>(p => p.Add(c => c.Theme, invalidTheme));

    // Act
    var themeClass = cut.Instance.GetThemeClass();

    // Assert
    themeClass.Should().Be("bzc-theme-glass");
  }

  #endregion

  #region CssClass Parameter Tests

  [Fact]
  public void CssClass_CanBeNull()
  {
    // Act
    var cut = RenderComponent<TestBzComponent>(p => p.Add(c => c.CssClass, null));

    // Assert
    cut.Instance.CssClass.Should().BeNull();
  }

  [Fact]
  public void CssClass_CanBeSetToCustomString()
  {
    // Act
    var cut = RenderComponent<TestBzComponent>(p => p.Add(c => c.CssClass, "my-custom-class"));

    // Assert
    cut.Instance.CssClass.Should().Be("my-custom-class");
  }

  #endregion

  #region AdditionalAttributes Tests

  [Fact]
  public void AdditionalAttributes_DefaultValue_ShouldBeNull()
  {
    // Act
    var cut = RenderComponent<TestBzComponent>();

    // Assert
    cut.Instance.AdditionalAttributes.Should().BeNull();
  }

  [Fact]
  public void AdditionalAttributes_CanBeSet()
  {
    // Arrange
    var attributes = new Dictionary<string, object>
        {
            { "id", "test-id" },
            { "data-testid", "my-test" }
        };

    // Act
    var cut = RenderComponent<TestBzComponent>(p => p
        .Add(c => c.AdditionalAttributes, attributes));

    // Assert
    cut.Instance.AdditionalAttributes.Should().NotBeNull();
    cut.Instance.AdditionalAttributes.Should().HaveCount(2);
  }

  [Fact]
  public void AdditionalAttributes_RendersIdAttribute()
  {
    // Arrange
    var attributes = new Dictionary<string, object>
        {
            { "id", "my-carousel" }
        };

    // Act
    var cut = RenderComponent<TestBzComponent>(p => p
        .Add(c => c.AdditionalAttributes, attributes));

    // Assert
    var element = cut.Find("div");
    element.GetAttribute("id").Should().Be("my-carousel");
  }

  [Fact]
  public void AdditionalAttributes_RendersDataAttributes()
  {
    // Arrange
    var attributes = new Dictionary<string, object>
        {
            { "data-testid", "carousel-test" },
            { "data-custom", "custom-value" }
        };

    // Act
    var cut = RenderComponent<TestBzComponent>(p => p
        .Add(c => c.AdditionalAttributes, attributes));

    // Assert
    var element = cut.Find("div");
    element.GetAttribute("data-testid").Should().Be("carousel-test");
    element.GetAttribute("data-custom").Should().Be("custom-value");
  }

  [Fact]
  public void AdditionalAttributes_RendersAriaAttributes()
  {
    // Arrange
    var attributes = new Dictionary<string, object>
        {
            { "aria-label", "Movie carousel" },
            { "aria-describedby", "carousel-description" }
        };

    // Act
    var cut = RenderComponent<TestBzComponent>(p => p
        .Add(c => c.AdditionalAttributes, attributes));

    // Assert
    var element = cut.Find("div");
    element.GetAttribute("aria-label").Should().Be("Movie carousel");
    element.GetAttribute("aria-describedby").Should().Be("carousel-description");
  }

  [Fact]
  public void AdditionalAttributes_RendersRoleAttribute()
  {
    // Arrange
    var attributes = new Dictionary<string, object>
        {
            { "role", "region" }
        };

    // Act
    var cut = RenderComponent<TestBzComponent>(p => p
        .Add(c => c.AdditionalAttributes, attributes));

    // Assert
    var element = cut.Find("div");
    element.GetAttribute("role").Should().Be("region");
  }

  [Fact]
  public void AdditionalAttributes_CanContainMultipleAttributes()
  {
    // Arrange
    var attributes = new Dictionary<string, object>
        {
            { "id", "main-carousel" },
            { "data-testid", "test-carousel" },
            { "aria-label", "Image gallery" },
            { "role", "region" },
            { "tabindex", "0" }
        };

    // Act
    var cut = RenderComponent<TestBzComponent>(p => p
        .Add(c => c.AdditionalAttributes, attributes));

    // Assert
    cut.Instance.AdditionalAttributes.Should().HaveCount(5);
    var element = cut.Find("div");
    element.GetAttribute("id").Should().Be("main-carousel");
    element.GetAttribute("tabindex").Should().Be("0");
  }

  [Fact]
  public void AdditionalAttributes_EmptyDictionary_ShouldNotThrow()
  {
    // Arrange
    var attributes = new Dictionary<string, object>();

    // Act
    var act = () => RenderComponent<TestBzComponent>(p => p
        .Add(c => c.AdditionalAttributes, attributes));

    // Assert
    act.Should().NotThrow();
  }

  [Fact]
  public void AdditionalAttributes_CapturesUnmatchedValues()
  {
    // This test verifies the [Parameter(CaptureUnmatchedValues = true)] behavior

    // Arrange & Act
    var cut = RenderComponent<TestBzComponent>(p => p
        .AddUnmatched("custom-attr", "custom-value")
        .AddUnmatched("another-attr", "another-value"));

    // Assert
    cut.Instance.AdditionalAttributes.Should().NotBeNull();
    cut.Instance.AdditionalAttributes!["custom-attr"].Should().Be("custom-value");
    cut.Instance.AdditionalAttributes!["another-attr"].Should().Be("another-value");
  }

  #endregion

  #region GetCombinedClass Tests

  [Fact]
  public void GetCombinedClass_IncludesBaseClass()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>();

    // Act
    var combined = cut.Instance.TestGetCombinedClass("base-class");

    // Assert
    combined.Should().Contain("base-class");
  }

  [Fact]
  public void GetCombinedClass_IncludesThemeClass()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>(p => p.Add(c => c.Theme, BzTheme.Dark));

    // Act
    var combined = cut.Instance.TestGetCombinedClass("base-class");

    // Assert
    combined.Should().Contain("bzc-theme-dark");
  }

  [Fact]
  public void GetCombinedClass_IncludesCssClass_WhenNotNull()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>(p => p.Add(c => c.CssClass, "custom-class"));

    // Act
    var combined = cut.Instance.TestGetCombinedClass("base-class");

    // Assert
    combined.Should().Contain("custom-class");
  }

  [Fact]
  public void GetCombinedClass_ExcludesCssClass_WhenNull()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>(p => p.Add(c => c.CssClass, null));

    // Act
    var combined = cut.Instance.TestGetCombinedClass("base-class");

    // Assert
    combined.Should().NotContain("null");
    combined.Split(' ').Should().HaveCount(2); // base + theme only
  }

  [Fact]
  public void GetCombinedClass_ExcludesCssClass_WhenEmpty()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>(p => p.Add(c => c.CssClass, ""));

    // Act
    var combined = cut.Instance.TestGetCombinedClass("base-class");

    // Assert
    combined.Split(' ', StringSplitOptions.RemoveEmptyEntries).Should().HaveCount(2);
  }

  [Fact]
  public void GetCombinedClass_ExcludesCssClass_WhenWhitespace()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>(p => p.Add(c => c.CssClass, "   "));

    // Act
    var combined = cut.Instance.TestGetCombinedClass("base-class");

    // Assert
    var classes = combined.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    classes.Should().HaveCount(2);
  }

  [Fact]
  public void GetCombinedClass_ClassesSeparatedBySpace()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>(p => p
        .Add(c => c.Theme, BzTheme.Glass)
        .Add(c => c.CssClass, "custom"));

    // Act
    var combined = cut.Instance.TestGetCombinedClass("base");

    // Assert
    combined.Should().Be("base bzc-theme-glass custom");
  }

  #endregion

  #region IsDisposed Tests

  [Fact]
  public void IsDisposed_InitiallyFalse()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>();

    // Assert
    cut.Instance.TestIsDisposed.Should().BeFalse();
  }

  [Fact]
  public async Task IsDisposed_TrueAfterDisposeAsyncCalled()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>();

    // Act
    await cut.Instance.DisposeAsync();

    // Assert
    cut.Instance.TestIsDisposed.Should().BeTrue();
  }

  #endregion

  #region DisposeAsync Tests

  [Fact]
  public async Task DisposeAsync_SetsIsDisposedToTrue()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>();

    // Act
    await cut.Instance.DisposeAsync();

    // Assert
    cut.Instance.TestIsDisposed.Should().BeTrue();
  }

  [Fact]
  public async Task DisposeAsync_CallsDisposeAsyncCore()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>();

    // Act
    await cut.Instance.DisposeAsync();

    // Assert
    cut.Instance.DisposeAsyncCoreCalled.Should().BeTrue();
  }

  [Fact]
  public async Task DisposeAsync_CanBeCalledMultipleTimes()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>();

    // Act
    await cut.Instance.DisposeAsync();
    await cut.Instance.DisposeAsync();
    await cut.Instance.DisposeAsync();

    // Assert - should not throw
    cut.Instance.TestIsDisposed.Should().BeTrue();
    cut.Instance.DisposeAsyncCoreCallCount.Should().Be(1); // Only called once
  }

  #endregion

  #region DisposeAsyncCore Tests

  [Fact]
  public async Task DisposeAsyncCore_CalledDuringDispose()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>();

    // Act
    await cut.Instance.DisposeAsync();

    // Assert
    cut.Instance.DisposeAsyncCoreCalled.Should().BeTrue();
  }

  #endregion

  #region InvokeStateHasChangedAsync Tests

  [Fact]
  public async Task InvokeStateHasChangedAsync_DoesNotThrow_WhenNotDisposed()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>();

    // Act
    var act = async () => await cut.Instance.TestInvokeStateHasChangedAsync();

    // Assert
    await act.Should().NotThrowAsync();
  }

  [Fact]
  public async Task InvokeStateHasChangedAsync_ReturnsImmediately_WhenDisposed()
  {
    // Arrange
    var cut = RenderComponent<TestBzComponent>();
    await cut.Instance.DisposeAsync();

    // Act
    var act = async () => await cut.Instance.TestInvokeStateHasChangedAsync();

    // Assert - should not throw, just return immediately
    await act.Should().NotThrowAsync();
  }

  #endregion

  #region Test Component

  /// <summary>
  /// Concrete implementation of BzComponentBase for testing.
  /// </summary>
  private class TestBzComponent : BzComponentBase
  {
    public bool DisposeAsyncCoreCalled { get; private set; }
    public int DisposeAsyncCoreCallCount { get; private set; }

    // Expose protected members for testing
    public bool TestIsDisposed => IsDisposed;
    public string GetThemeClass() => ThemeClass;
    public string TestGetCombinedClass(string baseClass) => GetCombinedClass(baseClass);
    public Task TestInvokeStateHasChangedAsync() => InvokeStateHasChangedAsync();

    protected override ValueTask DisposeAsyncCore()
    {
      DisposeAsyncCoreCalled = true;
      DisposeAsyncCoreCallCount++;
      return ValueTask.CompletedTask;
    }

    protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
    {
      builder.OpenElement(0, "div");
      builder.AddAttribute(1, "class", GetCombinedClass("test-component"));
      builder.AddMultipleAttributes(2, AdditionalAttributes);
      builder.CloseElement();
    }
  }

  #endregion
}
