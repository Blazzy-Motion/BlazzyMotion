using BlazzyMotion.Bento.Components;
using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Tests.Bento.Components;

/// <summary>
/// Tests for BzBentoFeature component.
/// </summary>
public class BzBentoFeatureTests : TestBase
{
  #region Rendering Tests

  [Fact]
  public void BzBentoFeature_WithLabel_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test Feature"));

    // Assert
    cut.Markup.Should().Contain("bzb-feature");
    cut.Markup.Should().Contain("Test Feature");
  }

  [Fact]
  public void BzBentoFeature_WithIconText_ShouldRenderIcon()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.IconText, "🚀")
        .Add(p => p.Label, "Fast"));

    // Assert
    cut.Markup.Should().Contain("🚀");
  }

  [Fact]
  public void BzBentoFeature_WithDescription_ShouldRenderDescription()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Feature")
        .Add(p => p.Description, "Feature description"));

    // Assert
    cut.Markup.Should().Contain("Feature description");
  }

  [Fact]
  public void BzBentoFeature_WithIconRenderFragment_ShouldRenderCustomIcon()
  {
    // Arrange
    RenderFragment icon = builder =>
    {
      builder.OpenElement(0, "i");
      builder.AddAttribute(1, "class", "bi bi-star");
      builder.CloseElement();
    };

    // Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Icon, icon)
        .Add(p => p.Label, "Star Feature"));

    // Assert
    cut.Markup.Should().Contain("bi-star");
  }

  #endregion

  #region Label Tests

  [Fact]
  public void BzBentoFeature_WithEmptyLabel_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, string.Empty));

    // Assert
    cut.Markup.Should().Contain("bzb-feature");
  }

  [Fact]
  public void BzBentoFeature_WithLongLabel_ShouldRender()
  {
    // Arrange
    var longLabel = "This is a very long feature label that might wrap";

    // Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, longLabel));

    // Assert
    cut.Markup.Should().Contain(longLabel);
  }

  #endregion

  #region Icon Tests

  [Fact]
  public void BzBentoFeature_WithoutIcon_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "No Icon Feature"));

    // Assert
    cut.Markup.Should().Contain("No Icon Feature");
  }

  [Fact]
  public void BzBentoFeature_WithEmojiIcon_ShouldRenderEmoji()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.IconText, "⚡")
        .Add(p => p.Label, "Lightning"));

    // Assert
    cut.Markup.Should().Contain("⚡");
  }

  [Fact]
  public void BzBentoFeature_WithTextIcon_ShouldRenderText()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.IconText, "NEW")
        .Add(p => p.Label, "New Feature"));

    // Assert
    cut.Markup.Should().Contain("NEW");
  }

  #endregion

  #region Description Tests

  [Fact]
  public void BzBentoFeature_WithoutDescription_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Feature"));

    // Assert
    cut.Markup.Should().Contain("Feature");
  }

  [Fact]
  public void BzBentoFeature_WithLongDescription_ShouldRender()
  {
    // Arrange
    var longDesc = "This is a detailed description of the feature that provides more context";

    // Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Feature")
        .Add(p => p.Description, longDesc));

    // Assert
    cut.Markup.Should().Contain(longDesc);
  }

  #endregion

  #region Layout Tests

  [Fact]
  public void BzBentoFeature_WithColSpan_ShouldApplySpanClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Feature")
        .Add(p => p.ColSpan, 2));

    // Assert
    cut.Markup.Should().Contain("bzb-col-2");
  }

  [Fact]
  public void BzBentoFeature_WithRowSpan_ShouldApplySpanClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Feature")
        .Add(p => p.RowSpan, 2));

    // Assert
    cut.Markup.Should().Contain("bzb-row-2");
  }

  #endregion

  #region CSS Tests

  [Fact]
  public void BzBentoFeature_ShouldHaveBaseCssClasses()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Feature"));

    // Assert
    cut.Markup.Should().Contain("bzb-item");
    cut.Markup.Should().Contain("bzb-feature");
  }

  [Fact]
  public void BzBentoFeature_WithCssClass_ShouldIncludeCustomClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Feature")
        .Add(p => p.CssClass, "custom-feature"));

    // Assert
    cut.Markup.Should().Contain("custom-feature");
  }

  #endregion

  #region Event Tests

  [Fact]
  public void BzBentoFeature_WithOnClick_ShouldInvokeCallback()
  {
    // Arrange
    var clicked = false;
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Clickable")
        .Add(p => p.OnClick, EventCallback.Factory.Create(this, () => clicked = true)));

    // Act
    cut.Find(".bzb-feature").Click();

    // Assert
    clicked.Should().BeTrue();
  }

  #endregion

  #region Combined Tests

  [Fact]
  public void BzBentoFeature_WithAllProperties_ShouldRenderCorrectly()
  {
    // Arrange
    RenderFragment icon = builder =>
    {
      builder.OpenElement(0, "i");
      builder.AddAttribute(1, "class", "test-icon");
      builder.CloseElement();
    };

    // Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Icon, icon)
        .Add(p => p.Label, "Complete Feature")
        .Add(p => p.Description, "Full description")
        .Add(p => p.ColSpan, 2)
        .Add(p => p.RowSpan, 1)
        .Add(p => p.Order, 5)
        .Add(p => p.CssClass, "custom"));

    // Assert
    cut.Markup.Should().Contain("test-icon");
    cut.Markup.Should().Contain("Complete Feature");
    cut.Markup.Should().Contain("Full description");
    cut.Markup.Should().Contain("bzb-col-2");
    cut.Markup.Should().Contain("order: 5");
    cut.Markup.Should().Contain("custom");
  }

  #endregion
}
