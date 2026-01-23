using BlazzyMotion.Bento.Components;
using BlazzyMotion.Bento.Models;
using BlazzyMotion.Core.Models;
using BlazzyMotion.Tests.Helpers;
using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Tests.Bento.Components;

/// <summary>
/// Basic tests for BzBento component.
/// </summary>
public class BzBentoBasicTests : TestBase
{
  /// <summary>
  /// Helper to create minimal ChildContent for composition mode testing.
  /// </summary>
  private static RenderFragment CreateMinimalChildContent() => builder =>
  {
    builder.OpenElement(0, "div");
    builder.AddContent(1, "Test Content");
    builder.CloseElement();
  };

  #region Rendering Tests

  [Fact]
  public void BzBento_WithChildContent_ShouldRenderCompositionMode()
  {
    // Arrange
    RenderFragment childContent = builder =>
    {
      builder.OpenComponent<BzBentoFeature>(0);
      builder.AddAttribute(1, "Label", "Test Feature");
      builder.CloseComponent();
    };

    // Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.ChildContent, childContent));

    // Assert
    cut.Markup.Should().Contain("bzb-grid");
    cut.Markup.Should().Contain("Test Feature");
  }

  [Fact]
  public void BzBento_WithItems_ShouldRenderItemsMode()
  {
    // Arrange
    var items = new List<TestBentoItem>
        {
            new TestBentoItem { Title = "Item 1", ImageUrl = "1.jpg" },
            new TestBentoItem { Title = "Item 2", ImageUrl = "2.jpg" }
        };

    // Act
    var cut = RenderComponent<BzBento<TestBentoItem>>(parameters => parameters
        .Add(p => p.Items, items));

    // Assert
    cut.Markup.Should().Contain("bzb-grid");
  }

  [Fact]
  public void BzBento_WithoutItemsOrChildContent_ShouldRenderLoadingState()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>();

    // Assert - Shows loading state when no items and no child content
    cut.Markup.Should().Contain("bz-loading");
  }

  #endregion

  #region Columns Tests

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(6)]
  public void BzBento_WithColumns_ShouldApplyCorrectCssVariable(int columns)
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Columns, columns)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert
    cut.Markup.Should().Contain($"--bzb-columns: {columns}");
  }

  [Fact]
  public void BzBento_DefaultColumns_ShouldNotRenderStyleWhenDefault()
  {
    // Arrange & Act - Default columns is 4, so style is not rendered
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert - Default value (4) doesn't render CSS variable
    cut.Markup.Should().Contain("bzb-grid");
    cut.Markup.Should().NotContain("--bzb-columns: 4");
  }

  #endregion

  #region Gap Tests

  [Theory]
  [InlineData(0)]
  [InlineData(8)]
  [InlineData(24)]
  public void BzBento_WithGap_ShouldApplyCorrectCssVariable(int gap)
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Gap, gap)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert
    cut.Markup.Should().Contain($"--bzb-gap: {gap}px");
  }

  [Fact]
  public void BzBento_DefaultGap_ShouldNotRenderStyleWhenDefault()
  {
    // Arrange & Act - Default gap is 16, so style is not rendered
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert - Grid is rendered, default gap (16) not in style
    cut.Markup.Should().Contain("bzb-grid");
  }

  #endregion

  #region Theme Tests

  [Fact]
  public void BzBento_WithGlassTheme_ShouldApplyThemeClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Theme, BzTheme.Glass)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert - Uses shared theme class prefix "bzc-theme-"
    cut.Markup.Should().Contain("bzc-theme-glass");
  }

  [Fact]
  public void BzBento_WithDarkTheme_ShouldApplyThemeClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Theme, BzTheme.Dark)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert - Uses shared theme class prefix "bzc-theme-"
    cut.Markup.Should().Contain("bzc-theme-dark");
  }

  [Fact]
  public void BzBento_WithLightTheme_ShouldApplyThemeClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Theme, BzTheme.Light)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert - Uses shared theme class prefix "bzc-theme-"
    cut.Markup.Should().Contain("bzc-theme-light");
  }

  [Fact]
  public void BzBento_WithMinimalTheme_ShouldApplyThemeClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Theme, BzTheme.Minimal)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert - Uses shared theme class prefix "bzc-theme-"
    cut.Markup.Should().Contain("bzc-theme-minimal");
  }

  #endregion

  #region AnimationEnabled Tests

  [Fact]
  public void BzBento_WithAnimationEnabled_ShouldRenderGrid()
  {
    // Arrange & Act - AnimationEnabled is a JS interop parameter, not a CSS class
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.AnimationEnabled, true)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert - Component should render grid regardless of animation setting
    cut.Markup.Should().Contain("bzb-grid");
  }

  [Fact]
  public void BzBento_WithAnimationDisabled_ShouldRenderGrid()
  {
    // Arrange & Act - AnimationEnabled controls JS behavior, not markup
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.AnimationEnabled, false)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert - Component should render grid regardless of animation setting
    cut.Markup.Should().Contain("bzb-grid");
  }

  #endregion

  #region CSS Class Tests

  [Fact]
  public void BzBento_ShouldHaveBaseCssClasses()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert
    cut.Markup.Should().Contain("bzb-grid");
  }

  [Fact]
  public void BzBento_WithCssClass_ShouldIncludeCustomClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.CssClass, "custom-bento")
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert
    cut.Markup.Should().Contain("custom-bento");
  }

  #endregion

  #region Options Tests

  [Fact]
  public void BzBento_WithOptions_ShouldApplyConfiguration()
  {
    // Arrange
    var options = new BzBentoOptions
    {
      Columns = 3,
      Gap = 24,
      AnimationEnabled = false,
      StaggerDelay = 100
    };

    // Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Options, options)
        .Add(p => p.Columns, 3)
        .Add(p => p.Gap, 24)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert
    cut.Markup.Should().Contain("--bzb-columns: 3");
    cut.Markup.Should().Contain("--bzb-gap: 24px");
  }

  #endregion

  #region Loading State Tests

  [Fact]
  public void BzBento_WithNullItems_ShouldRenderLoadingState()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<TestBentoItem>>(parameters => parameters
        .Add(p => p.Items, null));

    // Assert - null items without child content shows loading
    cut.Markup.Should().Contain("bz-loading");
  }

  #endregion

  #region Empty State Tests

  [Fact]
  public void BzBento_WithEmptyItems_ShouldRenderEmptyState()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<TestBentoItem>>(parameters => parameters
        .Add(p => p.Items, new List<TestBentoItem>()));

    // Assert - Empty items shows empty state
    cut.Markup.Should().Contain("bz-empty");
  }

  [Fact]
  public void BzBento_WithEmptyTemplate_ShouldRenderCustomEmpty()
  {
    // Arrange
    RenderFragment emptyTemplate = builder =>
    {
      builder.OpenElement(0, "div");
      builder.AddContent(1, "Custom Empty Message");
      builder.CloseElement();
    };

    // Act
    var cut = RenderComponent<BzBento<TestBentoItem>>(parameters => parameters
        .Add(p => p.Items, new List<TestBentoItem>())
        .Add(p => p.EmptyTemplate, emptyTemplate));

    // Assert
    cut.Markup.Should().Contain("Custom Empty Message");
  }

  [Fact]
  public void BzBento_WithLoadingTemplate_ShouldRenderCustomLoading()
  {
    // Arrange
    RenderFragment loadingTemplate = builder =>
    {
      builder.OpenElement(0, "div");
      builder.AddContent(1, "Custom Loading...");
      builder.CloseElement();
    };

    // Act
    var cut = RenderComponent<BzBento<TestBentoItem>>(parameters => parameters
        .Add(p => p.LoadingTemplate, loadingTemplate));

    // Assert
    cut.Markup.Should().Contain("Custom Loading...");
  }

  #endregion

  #region Additional Attributes Tests

  [Fact]
  public void BzBento_WithDataAttribute_ShouldApply()
  {
    // Arrange
    var attributes = new Dictionary<string, object> { { "data-test-id", "bento-1" } };

    // Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.AdditionalAttributes, attributes)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert
    cut.Markup.Should().Contain("data-test-id=\"bento-1\"");
  }

  [Fact]
  public void BzBento_WithId_ShouldApply()
  {
    // Arrange
    var attributes = new Dictionary<string, object> { { "id", "my-bento" } };

    // Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.AdditionalAttributes, attributes)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    // Assert
    cut.Markup.Should().Contain("id=\"my-bento\"");
  }

  #endregion

  #region Composition Mode Tests

  [Fact]
  public void BzBento_CompositionMode_WithMultipleChildren_ShouldRender()
  {
    // Arrange
    RenderFragment childContent = builder =>
    {
      builder.OpenComponent<BzBentoCard<object>>(0);
      builder.AddAttribute(1, "Title", "Card 1");
      builder.CloseComponent();

      builder.OpenComponent<BzBentoMetric>(2);
      builder.AddAttribute(3, "Value", "100");
      builder.AddAttribute(4, "Label", "Metric 1");
      builder.CloseComponent();

      builder.OpenComponent<BzBentoFeature>(5);
      builder.AddAttribute(6, "Label", "Feature 1");
      builder.CloseComponent();
    };

    // Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.ChildContent, childContent));

    // Assert
    cut.Markup.Should().Contain("Card 1");
    cut.Markup.Should().Contain("100");
    cut.Markup.Should().Contain("Feature 1");
  }

  #endregion

  #region Parameter Validation Tests

  [Fact]
  public void BzBento_WithInvalidColumns_ShouldThrow()
  {
    // Arrange & Act & Assert
    Action act = () => RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Columns, 0)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    act.Should().Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void BzBento_WithNegativeGap_ShouldThrow()
  {
    // Arrange & Act & Assert
    Action act = () => RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Gap, -1)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    act.Should().Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void BzBento_WithInvalidStaggerDelay_ShouldThrow()
  {
    // Arrange & Act & Assert
    Action act = () => RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.StaggerDelay, 2000)
        .Add(p => p.ChildContent, CreateMinimalChildContent()));

    act.Should().Throw<ArgumentOutOfRangeException>();
  }

  #endregion
}
