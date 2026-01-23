using BlazzyMotion.Bento.Components;
using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Tests.Bento.Components;

/// <summary>
/// Tests for BzBentoMetric component.
/// </summary>
public class BzBentoMetricTests : TestBase
{
  #region Rendering Tests

  [Fact]
  public void BzBentoMetric_WithValueAndLabel_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "1,234")
        .Add(p => p.Label, "Users"));

    // Assert
    cut.Markup.Should().Contain("bzb-metric");
    cut.Markup.Should().Contain("1,234");
    cut.Markup.Should().Contain("Users");
  }

  [Fact]
  public void BzBentoMetric_WithTrend_ShouldRenderTrend()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "$48.5K")
        .Add(p => p.Label, "Revenue")
        .Add(p => p.Trend, "+12.5%"));

    // Assert
    cut.Markup.Should().Contain("+12.5%");
  }

  [Fact]
  public void BzBentoMetric_WithIconText_ShouldRenderIcon()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "99.9%")
        .Add(p => p.Label, "Uptime")
        .Add(p => p.IconText, "⚡"));

    // Assert
    cut.Markup.Should().Contain("⚡");
  }

  [Fact]
  public void BzBentoMetric_WithIconRenderFragment_ShouldRenderCustomIcon()
  {
    // Arrange
    RenderFragment icon = builder =>
    {
      builder.OpenElement(0, "i");
      builder.AddAttribute(1, "class", "bi bi-star-fill");
      builder.CloseElement();
    };

    // Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "4.9")
        .Add(p => p.Label, "Rating")
        .Add(p => p.Icon, icon));

    // Assert
    cut.Markup.Should().Contain("bi-star-fill");
  }

  #endregion

  #region Value Tests

  [Fact]
  public void BzBentoMetric_WithNumericValue_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "1234")
        .Add(p => p.Label, "Count"));

    // Assert
    cut.Markup.Should().Contain("1234");
  }

  [Fact]
  public void BzBentoMetric_WithCurrencyValue_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "$1,234.56")
        .Add(p => p.Label, "Amount"));

    // Assert
    cut.Markup.Should().Contain("$1,234.56");
  }

  [Fact]
  public void BzBentoMetric_WithPercentageValue_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "99.9%")
        .Add(p => p.Label, "Success Rate"));

    // Assert
    cut.Markup.Should().Contain("99.9%");
  }

  [Fact]
  public void BzBentoMetric_WithEmptyValue_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, string.Empty)
        .Add(p => p.Label, "Empty"));

    // Assert
    cut.Markup.Should().Contain("bzb-metric");
  }

  #endregion

  #region Label Tests

  [Fact]
  public void BzBentoMetric_WithShortLabel_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "100")
        .Add(p => p.Label, "Total"));

    // Assert
    cut.Markup.Should().Contain("Total");
  }

  [Fact]
  public void BzBentoMetric_WithLongLabel_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "1000")
        .Add(p => p.Label, "Total Active Users This Month"));

    // Assert
    cut.Markup.Should().Contain("Total Active Users This Month");
  }

  #endregion

  #region Trend Tests

  [Fact]
  public void BzBentoMetric_WithPositiveTrend_ShouldHaveTrendUpClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "1000")
        .Add(p => p.Label, "Sales")
        .Add(p => p.Trend, "+15%"));

    // Assert
    cut.Markup.Should().Contain("bzb-metric-trend-up");
  }

  [Fact]
  public void BzBentoMetric_WithNegativeTrend_ShouldHaveTrendDownClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "800")
        .Add(p => p.Label, "Sales")
        .Add(p => p.Trend, "-5%"));

    // Assert
    cut.Markup.Should().Contain("bzb-metric-trend-down");
  }

  [Fact]
  public void BzBentoMetric_WithNeutralTrend_ShouldNotHaveTrendClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "1000")
        .Add(p => p.Label, "Sales")
        .Add(p => p.Trend, "0%"));

    // Assert
    cut.Markup.Should().NotContain("bzb-metric-trend-up");
    cut.Markup.Should().NotContain("bzb-metric-trend-down");
  }

  [Fact]
  public void BzBentoMetric_WithoutTrend_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "1000")
        .Add(p => p.Label, "Count"));

    // Assert
    cut.Markup.Should().NotContain("bzb-metric-trend");
  }

  #endregion

  #region Icon Tests

  [Fact]
  public void BzBentoMetric_WithoutIcon_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "100")
        .Add(p => p.Label, "Items"));

    // Assert
    cut.Markup.Should().Contain("100");
  }

  [Fact]
  public void BzBentoMetric_WithEmojiIcon_ShouldRenderEmoji()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "100")
        .Add(p => p.Label, "Points")
        .Add(p => p.IconText, "🎯"));

    // Assert
    cut.Markup.Should().Contain("🎯");
  }

  #endregion

  #region Layout Tests

  [Fact]
  public void BzBentoMetric_WithColSpan_ShouldApplySpanClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "100")
        .Add(p => p.Label, "Test")
        .Add(p => p.ColSpan, 2));

    // Assert
    cut.Markup.Should().Contain("bzb-col-2");
  }

  [Fact]
  public void BzBentoMetric_WithRowSpan_ShouldApplySpanClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "100")
        .Add(p => p.Label, "Test")
        .Add(p => p.RowSpan, 2));

    // Assert
    cut.Markup.Should().Contain("bzb-row-2");
  }

  #endregion

  #region CSS Tests

  [Fact]
  public void BzBentoMetric_ShouldHaveBaseCssClasses()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "100")
        .Add(p => p.Label, "Test"));

    // Assert
    cut.Markup.Should().Contain("bzb-item");
    cut.Markup.Should().Contain("bzb-metric");
  }

  [Fact]
  public void BzBentoMetric_WithCssClass_ShouldIncludeCustomClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "100")
        .Add(p => p.Label, "Test")
        .Add(p => p.CssClass, "custom-metric"));

    // Assert
    cut.Markup.Should().Contain("custom-metric");
  }

  #endregion

  #region Event Tests

  [Fact]
  public void BzBentoMetric_WithOnClick_ShouldInvokeCallback()
  {
    // Arrange
    var clicked = false;
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "100")
        .Add(p => p.Label, "Clickable")
        .Add(p => p.OnClick, EventCallback.Factory.Create(this, () => clicked = true)));

    // Act
    cut.Find(".bzb-metric").Click();

    // Assert
    clicked.Should().BeTrue();
  }

  #endregion

  #region Combined Tests

  [Fact]
  public void BzBentoMetric_WithAllProperties_ShouldRenderCorrectly()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoMetric>(parameters => parameters
        .Add(p => p.Value, "$48,500")
        .Add(p => p.Label, "Monthly Revenue")
        .Add(p => p.Trend, "+12.5%")
        .Add(p => p.IconText, "💰")
        .Add(p => p.ColSpan, 2)
        .Add(p => p.Order, 1)
        .Add(p => p.CssClass, "revenue-metric"));

    // Assert
    cut.Markup.Should().Contain("$48,500");
    cut.Markup.Should().Contain("Monthly Revenue");
    cut.Markup.Should().Contain("+12.5%");
    cut.Markup.Should().Contain("💰");
    cut.Markup.Should().Contain("bzb-col-2");
    cut.Markup.Should().Contain("revenue-metric");
    cut.Markup.Should().Contain("bzb-metric-trend-up");
  }

  #endregion
}
