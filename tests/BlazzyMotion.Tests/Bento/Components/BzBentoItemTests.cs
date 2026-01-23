using BlazzyMotion.Bento.Components;
using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Tests.Bento.Components;

/// <summary>
/// Tests for BzBentoItem component.
/// </summary>
public class BzBentoItemTests : TestBase
{
  #region Rendering Tests

  [Fact]
  public void BzBentoItem_WithChildContent_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .AddChildContent("<span>Custom Content</span>"));

    // Assert
    cut.Markup.Should().Contain("bzb-item");
    cut.Markup.Should().Contain("Custom Content");
  }

  [Fact]
  public void BzBentoItem_WithoutChildContent_ShouldRenderEmpty()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoItem>();

    // Assert
    cut.Markup.Should().Contain("bzb-item");
  }

  [Fact]
  public void BzBentoItem_WithComplexChildContent_ShouldRenderNested()
  {
    // Arrange
    RenderFragment content = builder =>
    {
      builder.OpenElement(0, "div");
      builder.AddAttribute(1, "class", "nested-content");
      builder.OpenElement(2, "h3");
      builder.AddContent(3, "Title");
      builder.CloseElement();
      builder.OpenElement(4, "p");
      builder.AddContent(5, "Description");
      builder.CloseElement();
      builder.CloseElement();
    };

    // Act
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .Add(p => p.ChildContent, content));

    // Assert
    cut.Markup.Should().Contain("nested-content");
    cut.Markup.Should().Contain("<h3>Title</h3>");
    cut.Markup.Should().Contain("<p>Description</p>");
  }

  #endregion

  #region Layout Tests

  [Fact]
  public void BzBentoItem_WithColSpan_ShouldApplySpanClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .Add(p => p.ColSpan, 2)
        .AddChildContent("Content"));

    // Assert
    cut.Markup.Should().Contain("bzb-col-2");
  }

  [Fact]
  public void BzBentoItem_WithRowSpan_ShouldApplySpanClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .Add(p => p.RowSpan, 2)
        .AddChildContent("Content"));

    // Assert
    cut.Markup.Should().Contain("bzb-row-2");
  }

  [Fact]
  public void BzBentoItem_WithBothSpans_ShouldApplyBothClasses()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .Add(p => p.ColSpan, 2)
        .Add(p => p.RowSpan, 3)
        .AddChildContent("Content"));

    // Assert
    cut.Markup.Should().Contain("bzb-col-2");
    cut.Markup.Should().Contain("bzb-row-3");
  }

  [Fact]
  public void BzBentoItem_WithOrder_ShouldApplyOrderStyle()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .Add(p => p.Order, 5)
        .AddChildContent("Content"));

    // Assert
    cut.Markup.Should().Contain("order: 5");
  }

  [Fact]
  public void BzBentoItem_WithOrderZero_ShouldNotIncludeOrderInStyle()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .Add(p => p.Order, 0)
        .AddChildContent("Content"));

    // Assert
    cut.Markup.Should().NotContain("order:");
  }

  #endregion

  #region CSS Tests

  [Fact]
  public void BzBentoItem_ShouldHaveBaseCssClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .AddChildContent("Content"));

    // Assert
    cut.Markup.Should().Contain("bzb-item");
  }

  [Fact]
  public void BzBentoItem_WithCssClass_ShouldIncludeCustomClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .Add(p => p.CssClass, "custom-item")
        .AddChildContent("Content"));

    // Assert
    cut.Markup.Should().Contain("custom-item");
    cut.Markup.Should().Contain("bzb-item");
  }

  [Fact]
  public void BzBentoItem_WithMultipleCssClasses_ShouldIncludeAll()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .Add(p => p.CssClass, "class1 class2")
        .AddChildContent("Content"));

    // Assert
    cut.Markup.Should().Contain("class1");
    cut.Markup.Should().Contain("class2");
  }

  #endregion

  #region Event Tests

  [Fact]
  public void BzBentoItem_WithOnClick_ShouldInvokeCallback()
  {
    // Arrange
    var clicked = false;
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .Add(p => p.OnClick, EventCallback.Factory.Create(this, () => clicked = true))
        .AddChildContent("Clickable"));

    // Act
    cut.Find(".bzb-item").Click();

    // Assert
    clicked.Should().BeTrue();
  }

  [Fact]
  public void BzBentoItem_WithoutOnClick_ShouldNotThrowOnClick()
  {
    // Arrange
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .AddChildContent("Content"));

    // Act & Assert - should not throw
    cut.Find(".bzb-item").Click();
  }

  #endregion

  #region Additional Attributes Tests

  [Fact]
  public void BzBentoItem_WithAdditionalAttributes_ShouldRenderThem()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .AddUnmatched("data-testid", "bento-item-1")
        .AddUnmatched("aria-label", "Test item")
        .AddChildContent("Content"));

    // Assert
    cut.Markup.Should().Contain("data-testid=\"bento-item-1\"");
    cut.Markup.Should().Contain("aria-label=\"Test item\"");
  }

  #endregion

  #region Combined Tests

  [Fact]
  public void BzBentoItem_WithAllProperties_ShouldRenderCorrectly()
  {
    // Arrange
    var clicked = false;
    RenderFragment content = builder =>
    {
      builder.OpenElement(0, "span");
      builder.AddContent(1, "Full Test");
      builder.CloseElement();
    };

    // Act
    var cut = RenderComponent<BzBentoItem>(parameters => parameters
        .Add(p => p.ChildContent, content)
        .Add(p => p.ColSpan, 2)
        .Add(p => p.RowSpan, 2)
        .Add(p => p.Order, 3)
        .Add(p => p.CssClass, "featured")
        .Add(p => p.OnClick, EventCallback.Factory.Create(this, () => clicked = true))
        .AddUnmatched("data-testid", "featured-item"));

    // Assert
    cut.Markup.Should().Contain("Full Test");
    cut.Markup.Should().Contain("bzb-item");
    cut.Markup.Should().Contain("bzb-col-2");
    cut.Markup.Should().Contain("bzb-row-2");
    cut.Markup.Should().Contain("order: 3");
    cut.Markup.Should().Contain("featured");
    cut.Markup.Should().Contain("data-testid=\"featured-item\"");

    // Act - click
    cut.Find(".bzb-item").Click();

    // Assert
    clicked.Should().BeTrue();
  }

  #endregion
}
