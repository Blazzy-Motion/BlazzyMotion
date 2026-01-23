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
  public void BzBento_WithoutItemsOrChildContent_ShouldRenderEmptyGrid()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>();

    // Assert
    cut.Markup.Should().Contain("bzb-grid");
  }

  #endregion

  #region Columns Tests

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(4)]
  [InlineData(6)]
  public void BzBento_WithColumns_ShouldApplyCorrectCssVariable(int columns)
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Columns, columns));

    // Assert
    cut.Markup.Should().Contain($"--bzb-columns: {columns}");
  }

  [Fact]
  public void BzBento_DefaultColumns_ShouldBe4()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>();

    // Assert
    cut.Markup.Should().Contain("--bzb-columns: 4");
  }

  #endregion

  #region Gap Tests

  [Theory]
  [InlineData(0)]
  [InlineData(8)]
  [InlineData(16)]
  [InlineData(24)]
  public void BzBento_WithGap_ShouldApplyCorrectCssVariable(int gap)
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Gap, gap));

    // Assert
    cut.Markup.Should().Contain($"--bzb-gap: {gap}px");
  }

  [Fact]
  public void BzBento_DefaultGap_ShouldBe16()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>();

    // Assert
    cut.Markup.Should().Contain("--bzb-gap: 16px");
  }

  #endregion

  #region Theme Tests

  [Fact]
  public void BzBento_WithGlassTheme_ShouldApplyThemeClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Theme, BzTheme.Glass));

    // Assert
    cut.Markup.Should().Contain("bzb-theme-glass");
  }

  [Fact]
  public void BzBento_WithDarkTheme_ShouldApplyThemeClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Theme, BzTheme.Dark));

    // Assert
    cut.Markup.Should().Contain("bzb-theme-dark");
  }

  [Fact]
  public void BzBento_WithLightTheme_ShouldApplyThemeClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Theme, BzTheme.Light));

    // Assert
    cut.Markup.Should().Contain("bzb-theme-light");
  }

  [Fact]
  public void BzBento_WithMinimalTheme_ShouldApplyThemeClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.Theme, BzTheme.Minimal));

    // Assert
    cut.Markup.Should().Contain("bzb-theme-minimal");
  }

  #endregion

  #region AnimationEnabled Tests

  [Fact]
  public void BzBento_WithAnimationEnabled_ShouldApplyAnimationClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.AnimationEnabled, true));

    // Assert
    cut.Markup.Should().Contain("bzb-animated");
  }

  [Fact]
  public void BzBento_WithAnimationDisabled_ShouldNotApplyAnimationClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.AnimationEnabled, false));

    // Assert
    cut.Markup.Should().NotContain("bzb-animated");
  }

  #endregion

  #region AutoLayout Tests

  [Fact]
  public void BzBento_WithAutoLayoutEnabled_ShouldWork()
  {
    // Arrange
    var items = new List<TestBentoItem>
        {
            new TestBentoItem { Title = "Item 1" },
            new TestBentoItem { Title = "Item 2" },
            new TestBentoItem { Title = "Item 3" }
        };

    // Act
    var cut = RenderComponent<BzBento<TestBentoItem>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.AutoLayout, true));

    // Assert
    cut.Markup.Should().Contain("bzb-grid");
  }

  [Fact]
  public void BzBento_WithLayoutPattern_ShouldWork()
  {
    // Arrange
    var items = new List<TestBentoItem>
        {
            new TestBentoItem { Title = "Item 1" }
        };

    // Act
    var cut = RenderComponent<BzBento<TestBentoItem>>(parameters => parameters
        .Add(p => p.Items, items)
        .Add(p => p.AutoLayout, true)
        .Add(p => p.Pattern, BentoLayoutPattern.Featured));

    // Assert
    cut.Markup.Should().Contain("bzb-grid");
  }

  #endregion

  #region CSS Class Tests

  [Fact]
  public void BzBento_ShouldHaveBaseCssClasses()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>();

    // Assert
    cut.Markup.Should().Contain("bzb");
    cut.Markup.Should().Contain("bzb-grid");
  }

  [Fact]
  public void BzBento_WithCssClass_ShouldIncludeCustomClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<object>>(parameters => parameters
        .Add(p => p.CssClass, "custom-bento"));

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
        .Add(p => p.Options, options));

    // Assert
    cut.Markup.Should().Contain("--bzb-columns: 3");
    cut.Markup.Should().Contain("--bzb-gap: 24px");
  }

  #endregion

  #region Empty State Tests

  [Fact]
  public void BzBento_WithNullItems_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<TestBentoItem>>(parameters => parameters
        .Add(p => p.Items, null));

    // Assert
    cut.Markup.Should().Contain("bzb-grid");
  }

  [Fact]
  public void BzBento_WithEmptyItems_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBento<TestBentoItem>>(parameters => parameters
        .Add(p => p.Items, new List<TestBentoItem>()));

    // Assert
    cut.Markup.Should().Contain("bzb-grid");
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
        .Add(p => p.AdditionalAttributes, attributes));

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
        .Add(p => p.AdditionalAttributes, attributes));
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
}
