using BlazzyMotion.Bento.Components;
using BlazzyMotion.Tests.Helpers;
using BlazzyMotion.Core.Models;

namespace BlazzyMotion.Tests.Bento.Components;

/// <summary>
/// Tests for BzBentoCard component.
/// </summary>
public class BzBentoCardTests : TestBase
{
  #region Rendering Tests

  [Fact]
  public void BzBentoCard_WithoutItem_ShouldRenderEmptyCard()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoCard<TestBentoItem>>();

    // Assert
    cut.Markup.Should().Contain("bzb-card");
  }

  [Fact]
  public void BzBentoCard_WithDirectParameters_ShouldRenderContent()
  {
    // Arrange & Act - Use direct parameters instead of Item mapping
    var cut = RenderComponent<BzBentoCard<TestBentoItem>>(parameters => parameters
        .Add(p => p.Image, "test.jpg")
        .Add(p => p.Title, "Test Title")
        .Add(p => p.Description, "Test Description"));

    // Assert
    cut.Markup.Should().Contain("test.jpg");
    cut.Markup.Should().Contain("Test Title");
    cut.Markup.Should().Contain("Test Description");
  }

  [Fact]
  public void BzBentoCard_WithImageOverride_ShouldUseOverrideValue()
  {
    // Arrange
    var item = new TestBentoItem { ImageUrl = "original.jpg" };

    // Act
    var cut = RenderComponent<BzBentoCard<TestBentoItem>>(parameters => parameters
        .Add(p => p.Item, item)
        .Add(p => p.Image, "override.jpg"));

    // Assert
    cut.Markup.Should().Contain("override.jpg");
    cut.Markup.Should().NotContain("original.jpg");
  }

  [Fact]
  public void BzBentoCard_WithTitleOverride_ShouldUseOverrideValue()
  {
    // Arrange
    var item = new TestBentoItem { Title = "Original Title" };

    // Act
    var cut = RenderComponent<BzBentoCard<TestBentoItem>>(parameters => parameters
        .Add(p => p.Item, item)
        .Add(p => p.Title, "Override Title"));

    // Assert
    cut.Markup.Should().Contain("Override Title");
    cut.Markup.Should().NotContain("Original Title");
  }

  [Fact]
  public void BzBentoCard_WithDescriptionOverride_ShouldUseOverrideValue()
  {
    // Arrange
    var item = new TestBentoItem { Description = "Original Description" };

    // Act
    var cut = RenderComponent<BzBentoCard<TestBentoItem>>(parameters => parameters
        .Add(p => p.Item, item)
        .Add(p => p.Description, "Override Description"));

    // Assert
    cut.Markup.Should().Contain("Override Description");
    cut.Markup.Should().NotContain("Original Description");
  }

  #endregion

  #region Direct Parameter Tests

  [Fact]
  public void BzBentoCard_WithDirectParameters_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoCard<object>>(parameters => parameters
        .Add(p => p.Image, "direct.jpg")
        .Add(p => p.Title, "Direct Title")
        .Add(p => p.Description, "Direct Description"));

    // Assert
    cut.Markup.Should().Contain("direct.jpg");
    cut.Markup.Should().Contain("Direct Title");
    cut.Markup.Should().Contain("Direct Description");
  }

  [Fact]
  public void BzBentoCard_WithImageOnly_ShouldRenderImage()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoCard<object>>(parameters => parameters
        .Add(p => p.Image, "image-only.jpg"));

    // Assert
    cut.Markup.Should().Contain("image-only.jpg");
  }

  [Fact]
  public void BzBentoCard_WithTitleOnly_ShouldRenderTitle()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoCard<object>>(parameters => parameters
        .Add(p => p.Title, "Title Only"));

    // Assert
    cut.Markup.Should().Contain("Title Only");
  }

  #endregion

  #region Layout Tests

  [Fact]
  public void BzBentoCard_WithColSpan_ShouldApplySpanClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoCard<object>>(parameters => parameters
        .Add(p => p.ColSpan, 2)
        .Add(p => p.Title, "Test"));

    // Assert
    cut.Markup.Should().Contain("bzb-col-2");
  }

  [Fact]
  public void BzBentoCard_WithRowSpan_ShouldApplySpanClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoCard<object>>(parameters => parameters
        .Add(p => p.RowSpan, 2)
        .Add(p => p.Title, "Test"));

    // Assert
    cut.Markup.Should().Contain("bzb-row-2");
  }

  [Fact]
  public void BzBentoCard_WithOrder_ShouldApplyOrderStyle()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoCard<object>>(parameters => parameters
        .Add(p => p.Order, 3)
        .Add(p => p.Title, "Test"));

    // Assert
    cut.Markup.Should().Contain("order: 3");
  }

  #endregion

  #region CSS Class Tests

  [Fact]
  public void BzBentoCard_ShouldHaveBaseCssClasses()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoCard<object>>(parameters => parameters
        .Add(p => p.Title, "Test"));

    // Assert
    cut.Markup.Should().Contain("bzb-item");
    cut.Markup.Should().Contain("bzb-card");
  }

  [Fact]
  public void BzBentoCard_WithCssClass_ShouldIncludeCustomClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoCard<object>>(parameters => parameters
        .Add(p => p.CssClass, "custom-card")
        .Add(p => p.Title, "Test"));

    // Assert
    cut.Markup.Should().Contain("custom-card");
  }

  #endregion

  #region Empty State Tests

  [Fact]
  public void BzBentoCard_WithNullValues_ShouldHandleGracefully()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoCard<TestBentoItem>>(parameters => parameters
        .Add(p => p.Item, new TestBentoItem()));

    // Assert
    cut.Markup.Should().Contain("bzb-card");
  }

  [Fact]
  public void BzBentoCard_WithEmptyStrings_ShouldHandleGracefully()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoCard<object>>(parameters => parameters
        .Add(p => p.Image, string.Empty)
        .Add(p => p.Title, string.Empty)
        .Add(p => p.Description, string.Empty));

    // Assert
    cut.Markup.Should().Contain("bzb-card");
  }

  #endregion

  #region Event Tests

  [Fact]
  public void BzBentoCard_WithOnClick_ShouldInvokeCallback()
  {
    // Arrange
    var clicked = false;
    var cut = RenderComponent<BzBentoCard<object>>(parameters => parameters
        .Add(p => p.Title, "Test")
        .Add(p => p.OnClick, EventCallback.Factory.Create(this, () => clicked = true)));

    // Act
    cut.Find(".bzb-card-container").Click();

    // Assert
    clicked.Should().BeTrue();
  }

  #endregion

  #region Additional Attributes Tests

  [Fact]
  public void BzBentoCard_WithDataAttribute_ShouldApply()
  {
    // Arrange
    var attributes = new Dictionary<string, object> { { "data-test-id", "card-1" } };

    // Act
    var cut = RenderComponent<BzBentoCard<object>>(parameters => parameters
        .Add(p => p.Title, "Test")
        .Add(p => p.AdditionalAttributes, attributes));
  }

  #endregion
}
