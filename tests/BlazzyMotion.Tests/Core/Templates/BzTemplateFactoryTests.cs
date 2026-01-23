namespace BlazzyMotion.Tests.Core.Templates;

/// <summary>
/// Tests for BzTemplateFactory static factory class.
/// </summary>
public class BzTemplateFactoryTests : TestContext
{
  #region CreateImage Tests

  [Fact]
  public void CreateImage_ReturnsRenderFragment()
  {
    // Act
    var template = BzTemplateFactory.CreateImage();

    // Assert
    template.Should().NotBeNull();
    template.Should().BeOfType<RenderFragment<BzItem>>();
  }

  [Fact]
  public void CreateImage_RendersNothing_WhenItemIsNull()
  {
    // Arrange
    var template = BzTemplateFactory.CreateImage();

    // Act
    var cut = Render(builder => template(null!)(builder));

    // Assert
    cut.Markup.Should().BeEmpty();
  }

  [Fact]
  public void CreateImage_RendersNothing_WhenHasImageIsFalse()
  {
    // Arrange
    var template = BzTemplateFactory.CreateImage();
    var item = new BzItem { ImageUrl = null };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Markup.Should().BeEmpty();
  }

  [Fact]
  public void CreateImage_RendersNothing_WhenImageUrlIsEmpty()
  {
    // Arrange
    var template = BzTemplateFactory.CreateImage();
    var item = new BzItem { ImageUrl = "" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Markup.Should().BeEmpty();
  }

  [Fact]
  public void CreateImage_RendersNothing_WhenImageUrlIsWhitespace()
  {
    // Arrange
    var template = BzTemplateFactory.CreateImage();
    var item = new BzItem { ImageUrl = "   " };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Markup.Should().BeEmpty();
  }

  [Fact]
  public void CreateImage_RendersImgElement_WhenHasImage()
  {
    // Arrange
    var template = BzTemplateFactory.CreateImage();
    var item = new BzItem { ImageUrl = "https://example.com/image.jpg" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("img").Should().NotBeNull();
  }

  [Fact]
  public void CreateImage_SetsSrcAttribute_ToImageUrl()
  {
    // Arrange
    var template = BzTemplateFactory.CreateImage();
    var item = new BzItem { ImageUrl = "https://example.com/image.jpg" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var img = cut.Find("img");
    img.GetAttribute("src").Should().Be("https://example.com/image.jpg");
  }

  [Fact]
  public void CreateImage_SetsAltAttribute_ToTitle_WhenHasTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateImage();
    var item = new BzItem { ImageUrl = "test.jpg", Title = "My Image" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var img = cut.Find("img");
    img.GetAttribute("alt").Should().Be("My Image");
  }

  [Fact]
  public void CreateImage_SetsTitleAttribute_ToTitle_WhenHasTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateImage();
    var item = new BzItem { ImageUrl = "test.jpg", Title = "My Image" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var img = cut.Find("img");
    img.GetAttribute("title").Should().Be("My Image");
  }

  [Fact]
  public void CreateImage_SetsAltToImage_WhenNoTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateImage();
    var item = new BzItem { ImageUrl = "test.jpg", Title = null };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var img = cut.Find("img");
    img.GetAttribute("alt").Should().Be("Image");
  }

  [Fact]
  public void CreateImage_SetsLoadingLazy()
  {
    // Arrange
    var template = BzTemplateFactory.CreateImage();
    var item = new BzItem { ImageUrl = "test.jpg" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var img = cut.Find("img");
    img.GetAttribute("loading").Should().Be("lazy");
  }

  #endregion

  #region CreateFallback Tests

  [Fact]
  public void CreateFallback_ReturnsRenderFragment()
  {
    // Act
    var template = BzTemplateFactory.CreateFallback();

    // Assert
    template.Should().NotBeNull();
    template.Should().BeOfType<RenderFragment<BzItem>>();
  }

  [Fact]
  public void CreateFallback_RendersDivElement()
  {
    // Arrange
    var template = BzTemplateFactory.CreateFallback();
    var item = new BzItem { Title = "Test" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("div").Should().NotBeNull();
  }

  [Fact]
  public void CreateFallback_SetsFallbackClass()
  {
    // Arrange
    var template = BzTemplateFactory.CreateFallback();
    var item = new BzItem { Title = "Test" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var div = cut.Find("div");
    div.ClassList.Should().Contain("bzc-fallback-item");
  }

  [Fact]
  public void CreateFallback_RendersDisplayTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateFallback();
    var item = new BzItem { Title = "My Title" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Markup.Should().Contain("My Title");
  }

  [Fact]
  public void CreateFallback_RendersItem_WhenItemIsNull()
  {
    // Arrange
    var template = BzTemplateFactory.CreateFallback();

    // Act
    var cut = Render(builder => template(null!)(builder));

    // Assert
    cut.Markup.Should().Contain("Item");
  }

  [Fact]
  public void CreateFallback_RendersItem_WhenTitleIsNull()
  {
    // Arrange
    var template = BzTemplateFactory.CreateFallback();
    var item = new BzItem { Title = null };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Markup.Should().Contain("Item");
  }

  #endregion

  #region CreateGenericFallback Tests

  [Fact]
  public void CreateGenericFallback_ReturnsRenderFragment()
  {
    // Act
    var template = BzTemplateFactory.CreateGenericFallback<TestItem>();

    // Assert
    template.Should().NotBeNull();
  }

  [Fact]
  public void CreateGenericFallback_RendersDivElement()
  {
    // Arrange
    var template = BzTemplateFactory.CreateGenericFallback<TestItem>();
    var item = new TestItem { Name = "Test" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("div").Should().NotBeNull();
  }

  [Fact]
  public void CreateGenericFallback_SetsFallbackClass()
  {
    // Arrange
    var template = BzTemplateFactory.CreateGenericFallback<TestItem>();
    var item = new TestItem { Name = "Test" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var div = cut.Find("div");
    div.ClassList.Should().Contain("bz-fallback-item");
  }

  [Fact]
  public void CreateGenericFallback_RendersToStringResult()
  {
    // Arrange
    var template = BzTemplateFactory.CreateGenericFallback<TestItem>();
    var item = new TestItem { Name = "Custom Name" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Markup.Should().Contain("TestItem: Custom Name");
  }

  [Fact]
  public void CreateGenericFallback_RendersNull_WhenItemIsNull()
  {
    // Arrange
    var template = BzTemplateFactory.CreateGenericFallback<TestItem>();

    // Act
    var cut = Render(builder => template(null!)(builder));

    // Assert
    cut.Markup.Should().Contain("[null]");
  }

  #endregion

  #region SelectTemplate Tests

  [Fact]
  public void SelectTemplate_ReturnsFallback_WhenItemIsNull()
  {
    // Act
    var template = BzTemplateFactory.SelectTemplate(null);

    // Assert
    template.Should().NotBeNull();
    // Verify it's the fallback template by rendering
    var cut = Render(builder => template(new BzItem())(builder));
    cut.Find("div.bzc-fallback-item").Should().NotBeNull();
  }

  [Fact]
  public void SelectTemplate_ReturnsFallback_WhenHasImageIsFalse()
  {
    // Arrange
    var item = new BzItem { ImageUrl = null };

    // Act
    var template = BzTemplateFactory.SelectTemplate(item);

    // Assert
    var cut = Render(builder => template(item)(builder));
    cut.Find("div.bzc-fallback-item").Should().NotBeNull();
  }

  [Fact]
  public void SelectTemplate_ReturnsImage_WhenHasImageIsTrue()
  {
    // Arrange
    var item = new BzItem { ImageUrl = "test.jpg" };

    // Act
    var template = BzTemplateFactory.SelectTemplate(item);

    // Assert
    var cut = Render(builder => template(item)(builder));
    cut.Find("img").Should().NotBeNull();
  }

  #endregion

  #region Edge Cases

  [Fact]
  public void CreateImage_HandlesSpecialCharactersInUrl()
  {
    // Arrange
    var template = BzTemplateFactory.CreateImage();
    var item = new BzItem { ImageUrl = "https://example.com/image?id=123&size=large" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var img = cut.Find("img");
    img.GetAttribute("src").Should().Contain("id=123");
  }

  [Fact]
  public void CreateImage_HandlesUnicodeInTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateImage();
    var item = new BzItem { ImageUrl = "test.jpg", Title = "日本語タイトル" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var img = cut.Find("img");
    img.GetAttribute("alt").Should().Be("日本語タイトル");
  }

  [Fact]
  public void CreateFallback_HandlesUnicodeInTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateFallback();
    var item = new BzItem { Title = "中文标题" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Markup.Should().Contain("中文标题");
  }

  #endregion

  #region CreateGenericFallback Null Handling Tests

  [Fact]
  public void CreateGenericFallback_HandlesNullItemCorrectly()
  {
    // Arrange
    var template = BzTemplateFactory.CreateGenericFallback<TestItem>();

    // Act
    var cut = Render(builder => template(null!)(builder));

    // Assert
    cut.Markup.Should().Contain("[null]");
  }

  [Fact]
  public void CreateGenericFallback_HandlesItemWithNullToString()
  {
    // Arrange
    var template = BzTemplateFactory.CreateGenericFallback<TestItem>();
    var item = new TestItem { Name = null };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Markup.Should().Contain("TestItem:");
  }

  #endregion

  #region CreateFallback Null Handling Tests

  [Fact]
  public void CreateFallback_HandlesNullItemCorrectly()
  {
    // Arrange
    var template = BzTemplateFactory.CreateFallback();

    // Act
    var cut = Render(builder => template(null!)(builder));

    // Assert
    cut.Markup.Should().Contain("Item");
  }

  [Fact]
  public void CreateFallback_HandlesItemWithNullTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateFallback();
    var item = new BzItem { Title = null };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Markup.Should().Contain("Item");
  }

  [Fact]
  public void CreateFallback_HandlesItemWithEmptyTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateFallback();
    var item = new BzItem { Title = "" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Markup.Should().Contain("Item");
  }

  #endregion

  #region SelectTemplate Edge Cases

  [Fact]
  public void SelectTemplate_ReturnsFallback_WhenImageUrlIsWhitespace()
  {
    // Arrange
    var item = new BzItem { ImageUrl = "   " };

    // Act
    var template = BzTemplateFactory.SelectTemplate(item);

    // Assert
    var cut = Render(builder => template(item)(builder));
    cut.Find("div").GetAttribute("class").Should().Contain("bzc-fallback-item");
  }

  #endregion

  #region Test Models

  private class TestItem
  {
    public string? Name { get; set; }

    public override string ToString() => $"TestItem: {Name}";
  }

  #endregion
}
