using BlazzyMotion.Bento.Components;
using BlazzyMotion.Tests.Helpers;

namespace BlazzyMotion.Tests.Bento.Components;

/// <summary>
/// Tests for BzBentoQuote component.
/// </summary>
public class BzBentoQuoteTests : TestBase
{
  #region Rendering Tests

  [Fact]
  public void BzBentoQuote_WithAllParameters_ShouldRenderContent()
  {
    // Arrange & Act - Use direct parameters
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Amazing product!")
        .Add(p => p.Author, "John Doe")
        .Add(p => p.Avatar, "avatar.jpg"));

    // Assert
    cut.Markup.Should().Contain("Amazing product!");
    cut.Markup.Should().Contain("John Doe");
    cut.Markup.Should().Contain("avatar.jpg");
  }

  [Fact]
  public void BzBentoQuote_WithDirectParameters_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Great service!")
        .Add(p => p.Author, "Jane Smith")
        .Add(p => p.Role, "CEO, TechCorp")
        .Add(p => p.Avatar, "jane.jpg"));

    // Assert
    cut.Markup.Should().Contain("Great service!");
    cut.Markup.Should().Contain("Jane Smith");
    cut.Markup.Should().Contain("CEO, TechCorp");
    cut.Markup.Should().Contain("jane.jpg");
  }

  [Fact]
  public void BzBentoQuote_WithTextOverride_ShouldUseOverrideValue()
  {
    // Arrange
    var item = new TestBentoItem { Description = "Original quote" };

    // Act
    var cut = RenderComponent<BzBentoQuote<TestBentoItem>>(parameters => parameters
        .Add(p => p.Item, item)
        .Add(p => p.Text, "Override quote"));

    // Assert
    cut.Markup.Should().Contain("Override quote");
    cut.Markup.Should().NotContain("Original quote");
  }

  [Fact]
  public void BzBentoQuote_WithAuthorOverride_ShouldUseOverrideValue()
  {
    // Arrange
    var item = new TestBentoItem { Title = "Original Author" };

    // Act
    var cut = RenderComponent<BzBentoQuote<TestBentoItem>>(parameters => parameters
        .Add(p => p.Item, item)
        .Add(p => p.Author, "Override Author"));

    // Assert
    cut.Markup.Should().Contain("Override Author");
    cut.Markup.Should().NotContain("Original Author");
  }

  [Fact]
  public void BzBentoQuote_WithAvatarOverride_ShouldUseOverrideValue()
  {
    // Arrange
    var item = new TestBentoItem { ImageUrl = "original.jpg" };

    // Act
    var cut = RenderComponent<BzBentoQuote<TestBentoItem>>(parameters => parameters
        .Add(p => p.Item, item)
        .Add(p => p.Avatar, "override.jpg"));

    // Assert
    cut.Markup.Should().Contain("override.jpg");
    cut.Markup.Should().NotContain("original.jpg");
  }

  #endregion

  #region Text Tests

  [Fact]
  public void BzBentoQuote_WithShortText_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Great!")
        .Add(p => p.Author, "User"));

    // Assert
    cut.Markup.Should().Contain("Great!");
  }

  [Fact]
  public void BzBentoQuote_WithLongText_ShouldRender()
  {
    // Arrange
    var longText = "This is a very long testimonial that spans multiple lines and provides detailed feedback about the product or service.";

    // Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, longText)
        .Add(p => p.Author, "Customer"));

    // Assert
    cut.Markup.Should().Contain(longText);
  }

  [Fact]
  public void BzBentoQuote_WithoutText_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Author, "Someone"));

    // Assert
    cut.Markup.Should().Contain("bzb-quote");
  }

  #endregion

  #region Author Tests

  [Fact]
  public void BzBentoQuote_WithAuthor_ShouldRenderAuthor()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Quote")
        .Add(p => p.Author, "John Doe"));

    // Assert
    cut.Markup.Should().Contain("John Doe");
  }

  [Fact]
  public void BzBentoQuote_WithoutAuthor_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Anonymous quote"));

    // Assert
    cut.Markup.Should().Contain("Anonymous quote");
  }

  #endregion

  #region Role Tests

  [Fact]
  public void BzBentoQuote_WithRole_ShouldRenderRole()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Quote")
        .Add(p => p.Author, "John")
        .Add(p => p.Role, "CEO, Company"));

    // Assert
    cut.Markup.Should().Contain("CEO, Company");
  }

  [Fact]
  public void BzBentoQuote_WithoutRole_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Quote")
        .Add(p => p.Author, "John"));

    // Assert
    cut.Markup.Should().Contain("John");
  }

  [Fact]
  public void BzBentoQuote_WithRoleButNoAuthor_ShouldRenderRole()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Quote")
        .Add(p => p.Role, "Developer"));

    // Assert
    cut.Markup.Should().Contain("Developer");
  }

  #endregion

  #region Avatar Tests

  [Fact]
  public void BzBentoQuote_WithAvatar_ShouldRenderAvatar()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Quote")
        .Add(p => p.Author, "John")
        .Add(p => p.Avatar, "john.jpg"));

    // Assert
    cut.Markup.Should().Contain("john.jpg");
  }

  [Fact]
  public void BzBentoQuote_WithoutAvatar_ShouldRender()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Quote")
        .Add(p => p.Author, "John"));

    // Assert
    cut.Markup.Should().Contain("John");
  }

  #endregion

  #region Layout Tests

  [Fact]
  public void BzBentoQuote_WithColSpan_ShouldApplySpanClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Quote")
        .Add(p => p.ColSpan, 2));

    // Assert
    cut.Markup.Should().Contain("bzb-col-2");
  }

  [Fact]
  public void BzBentoQuote_WithRowSpan_ShouldApplySpanClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Quote")
        .Add(p => p.RowSpan, 2));

    // Assert
    cut.Markup.Should().Contain("bzb-row-2");
  }

  #endregion

  #region CSS Tests

  [Fact]
  public void BzBentoQuote_ShouldHaveBaseCssClasses()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Quote"));

    // Assert
    cut.Markup.Should().Contain("bzb-item");
    cut.Markup.Should().Contain("bzb-quote");
  }

  [Fact]
  public void BzBentoQuote_WithCssClass_ShouldIncludeCustomClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Quote")
        .Add(p => p.CssClass, "custom-quote"));

    // Assert
    cut.Markup.Should().Contain("custom-quote");
  }

  #endregion

  #region Empty State Tests

  [Fact]
  public void BzBentoQuote_WithNullItem_ShouldHandleGracefully()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<TestBentoItem>>();

    // Assert
    cut.Markup.Should().Contain("bzb-quote");
  }

  [Fact]
  public void BzBentoQuote_WithEmptyStrings_ShouldHandleGracefully()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, string.Empty)
        .Add(p => p.Author, string.Empty));

    // Assert
    cut.Markup.Should().Contain("bzb-quote");
  }

  #endregion

  #region Event Tests

  [Fact]
  public void BzBentoQuote_WithOnClick_ShouldInvokeCallback()
  {
    // Arrange
    var clicked = false;
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Clickable Quote")
        .Add(p => p.OnClick, EventCallback.Factory.Create(this, () => clicked = true)));

    // Act
    cut.Find(".bzb-quote").Click();

    // Assert
    clicked.Should().BeTrue();
  }

  #endregion

  #region Combined Tests

  [Fact]
  public void BzBentoQuote_WithAllProperties_ShouldRenderCorrectly()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoQuote<object>>(parameters => parameters
        .Add(p => p.Text, "Excellent product and service!")
        .Add(p => p.Author, "Jane Smith")
        .Add(p => p.Role, "CTO, TechStart")
        .Add(p => p.Avatar, "jane-avatar.jpg")
        .Add(p => p.ColSpan, 2)
        .Add(p => p.RowSpan, 1)
        .Add(p => p.Order, 1)
        .Add(p => p.CssClass, "testimonial"));

    // Assert
    cut.Markup.Should().Contain("Excellent product and service!");
    cut.Markup.Should().Contain("Jane Smith");
    cut.Markup.Should().Contain("CTO, TechStart");
    cut.Markup.Should().Contain("jane-avatar.jpg");
    cut.Markup.Should().Contain("bzb-col-2");
    cut.Markup.Should().Contain("order: 1");
    cut.Markup.Should().Contain("testimonial");
  }

  #endregion
}
