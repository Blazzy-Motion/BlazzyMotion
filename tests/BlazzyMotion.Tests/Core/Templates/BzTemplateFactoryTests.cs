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

  #region CreateBentoItem Tests

  [Fact]
  public void CreateBentoItem_ReturnsRenderFragment()
  {
    // Act
    var template = BzTemplateFactory.CreateBentoItem();

    // Assert
    template.Should().NotBeNull();
    template.Should().BeOfType<RenderFragment<BzItem>>();
  }

  [Fact]
  public void CreateBentoItem_RendersNothing_WhenItemIsNull()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoItem();

    // Act
    var cut = Render(builder => template(null!)(builder));

    // Assert
    cut.Markup.Should().BeEmpty();
  }

  [Fact]
  public void CreateBentoItem_RendersImage_WhenHasImage()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoItem();
    var item = new BzItem { ImageUrl = "test.jpg", Title = "Test" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var img = cut.Find("img");
    img.GetAttribute("src").Should().Be("test.jpg");
    img.ClassList.Should().Contain("bzb-item-image");
    img.GetAttribute("loading").Should().Be("lazy");
  }

  [Fact]
  public void CreateBentoItem_RendersTitle_WhenHasTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoItem();
    var item = new BzItem { Title = "Test Title" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var h4 = cut.Find("h4");
    h4.ClassList.Should().Contain("bzb-item-title");
    h4.TextContent.Should().Be("Test Title");
  }

  [Fact]
  public void CreateBentoItem_RendersDescription_WhenHasDescription()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoItem();
    var item = new BzItem { Description = "Test Description" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var p = cut.Find("p");
    p.ClassList.Should().Contain("bzb-item-description");
    p.TextContent.Should().Be("Test Description");
  }

  [Fact]
  public void CreateBentoItem_RendersAllElements_WhenAllPropertiesSet()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoItem();
    var item = new BzItem { ImageUrl = "img.jpg", Title = "Title", Description = "Desc" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("img.bzb-item-image").Should().NotBeNull();
    cut.Find("h4.bzb-item-title").Should().NotBeNull();
    cut.Find("p.bzb-item-description").Should().NotBeNull();
  }

  [Fact]
  public void CreateBentoItem_SetsAltToImage_WhenNoTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoItem();
    var item = new BzItem { ImageUrl = "test.jpg" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var img = cut.Find("img");
    img.GetAttribute("alt").Should().Be("Image");
  }

  #endregion

  #region CreateBentoCard Tests

  [Fact]
  public void CreateBentoCard_ReturnsRenderFragment()
  {
    // Act
    var template = BzTemplateFactory.CreateBentoCard();

    // Assert
    template.Should().NotBeNull();
    template.Should().BeOfType<RenderFragment<BzItem>>();
  }

  [Fact]
  public void CreateBentoCard_RendersNothing_WhenItemIsNull()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoCard();

    // Act
    var cut = Render(builder => template(null!)(builder));

    // Assert
    cut.Markup.Should().BeEmpty();
  }

  [Fact]
  public void CreateBentoCard_RendersCardContainer()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoCard();
    var item = new BzItem { ImageUrl = "test.jpg" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("div.bzb-card").Should().NotBeNull();
  }

  [Fact]
  public void CreateBentoCard_RendersImage_WhenHasImage()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoCard();
    var item = new BzItem { ImageUrl = "test.jpg", Title = "Card" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var img = cut.Find("img.bzb-card-image");
    img.GetAttribute("src").Should().Be("test.jpg");
    img.GetAttribute("loading").Should().Be("lazy");
  }

  [Fact]
  public void CreateBentoCard_RendersOverlay_WhenHasTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoCard();
    var item = new BzItem { Title = "Card Title" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("div.bzb-card-overlay").Should().NotBeNull();
    cut.Find("h4.bzb-card-title").TextContent.Should().Be("Card Title");
  }

  [Fact]
  public void CreateBentoCard_RendersOverlay_WhenHasDescription()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoCard();
    var item = new BzItem { Description = "Card Description" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("div.bzb-card-overlay").Should().NotBeNull();
    cut.Find("p.bzb-card-description").TextContent.Should().Be("Card Description");
  }

  [Fact]
  public void CreateBentoCard_DoesNotRenderOverlay_WhenNoTitleOrDescription()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoCard();
    var item = new BzItem { ImageUrl = "test.jpg" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.FindAll("div.bzb-card-overlay").Should().BeEmpty();
  }

  [Fact]
  public void CreateBentoCard_RendersAllElements_WhenAllPropertiesSet()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoCard();
    var item = new BzItem { ImageUrl = "img.jpg", Title = "Title", Description = "Desc" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("div.bzb-card").Should().NotBeNull();
    cut.Find("img.bzb-card-image").Should().NotBeNull();
    cut.Find("div.bzb-card-overlay").Should().NotBeNull();
    cut.Find("h4.bzb-card-title").Should().NotBeNull();
    cut.Find("p.bzb-card-description").Should().NotBeNull();
  }

  #endregion

  #region CreateBentoImageRich Tests

  [Fact]
  public void CreateBentoImageRich_ReturnsRenderFragment()
  {
    // Act
    var template = BzTemplateFactory.CreateBentoImageRich();

    // Assert
    template.Should().NotBeNull();
    template.Should().BeOfType<RenderFragment<BzItem>>();
  }

  [Fact]
  public void CreateBentoImageRich_RendersNothing_WhenItemIsNull()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoImageRich();

    // Act
    var cut = Render(builder => template(null!)(builder));

    // Assert
    cut.Markup.Should().BeEmpty();
  }

  [Fact]
  public void CreateBentoImageRich_RendersContainer()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoImageRich();
    var item = new BzItem { ImageUrl = "test.jpg" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("div.bzb-card-simple").Should().NotBeNull();
  }

  [Fact]
  public void CreateBentoImageRich_RendersImage_WhenHasImage()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoImageRich();
    var item = new BzItem { ImageUrl = "test.jpg", Title = "Rich Image" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var img = cut.Find("img.bzb-card-image");
    img.GetAttribute("src").Should().Be("test.jpg");
    img.GetAttribute("alt").Should().Be("Rich Image");
    img.GetAttribute("loading").Should().Be("lazy");
  }

  [Fact]
  public void CreateBentoImageRich_RendersOverlay_WhenHasTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoImageRich();
    var item = new BzItem { ImageUrl = "test.jpg", Title = "Overlay Title" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("div.bzb-card-simple-overlay").Should().NotBeNull();
    cut.Find("h5.bzb-card-simple-title").TextContent.Should().Be("Overlay Title");
  }

  [Fact]
  public void CreateBentoImageRich_DoesNotRenderOverlay_WhenNoTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoImageRich();
    var item = new BzItem { ImageUrl = "test.jpg" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.FindAll("div.bzb-card-simple-overlay").Should().BeEmpty();
  }

  [Fact]
  public void CreateBentoImageRich_SetsAltToImage_WhenNoTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoImageRich();
    var item = new BzItem { ImageUrl = "test.jpg" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    var img = cut.Find("img");
    img.GetAttribute("alt").Should().Be("Image");
  }

  #endregion

  #region CreateBentoStat Tests

  [Fact]
  public void CreateBentoStat_ReturnsRenderFragment()
  {
    // Act
    var template = BzTemplateFactory.CreateBentoStat();

    // Assert
    template.Should().NotBeNull();
    template.Should().BeOfType<RenderFragment<BzItem>>();
  }

  [Fact]
  public void CreateBentoStat_RendersNothing_WhenItemIsNull()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoStat();

    // Act
    var cut = Render(builder => template(null!)(builder));

    // Assert
    cut.Markup.Should().BeEmpty();
  }

  [Fact]
  public void CreateBentoStat_RendersStatContainer()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoStat();
    var item = new BzItem { Title = "Metric", Description = "100" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("div.bzb-stat").Should().NotBeNull();
  }

  [Fact]
  public void CreateBentoStat_RendersValue()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoStat();
    var item = new BzItem { Description = "1,234" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("span.bzb-stat-value").TextContent.Should().Be("1,234");
  }

  [Fact]
  public void CreateBentoStat_RendersLabel()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoStat();
    var item = new BzItem { Title = "Users" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("span.bzb-stat-label").TextContent.Should().Be("Users");
  }

  [Fact]
  public void CreateBentoStat_RendersDefaultValue_WhenNoDescription()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoStat();
    var item = new BzItem { Title = "Metric" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("span.bzb-stat-value").TextContent.Should().Be("-");
  }

  [Fact]
  public void CreateBentoStat_RendersDefaultLabel_WhenNoTitle()
  {
    // Arrange
    var template = BzTemplateFactory.CreateBentoStat();
    var item = new BzItem { Description = "50" };

    // Act
    var cut = Render(builder => template(item)(builder));

    // Assert
    cut.Find("span.bzb-stat-label").TextContent.Should().Be("Metric");
  }

  #endregion

  #region SelectBentoTemplate Tests

  [Fact]
  public void SelectBentoTemplate_ReturnsFallback_WhenItemIsNull()
  {
    // Act
    var template = BzTemplateFactory.SelectBentoTemplate(null);

    // Assert
    var cut = Render(builder => template(new BzItem())(builder));
    cut.Find("div.bzc-fallback-item").Should().NotBeNull();
  }

  [Fact]
  public void SelectBentoTemplate_ReturnsBentoCard_WhenFeaturedWithImage()
  {
    // Arrange
    var item = new BzItem { ImageUrl = "test.jpg", ColSpan = 2, RowSpan = 2 };

    // Act
    var template = BzTemplateFactory.SelectBentoTemplate(item);

    // Assert
    var cut = Render(builder => template(item)(builder));
    cut.Find("div.bzb-card").Should().NotBeNull();
  }

  [Fact]
  public void SelectBentoTemplate_ReturnsBentoCard_WhenColSpan2()
  {
    // Arrange
    var item = new BzItem { ImageUrl = "test.jpg", ColSpan = 2, RowSpan = 1 };

    // Act
    var template = BzTemplateFactory.SelectBentoTemplate(item);

    // Assert
    var cut = Render(builder => template(item)(builder));
    cut.Find("div.bzb-card").Should().NotBeNull();
  }

  [Fact]
  public void SelectBentoTemplate_ReturnsBentoCard_WhenRowSpan2()
  {
    // Arrange
    var item = new BzItem { ImageUrl = "test.jpg", ColSpan = 1, RowSpan = 2 };

    // Act
    var template = BzTemplateFactory.SelectBentoTemplate(item);

    // Assert
    var cut = Render(builder => template(item)(builder));
    cut.Find("div.bzb-card").Should().NotBeNull();
  }

  [Fact]
  public void SelectBentoTemplate_ReturnsBentoImageRich_WhenRegularWithImage()
  {
    // Arrange
    var item = new BzItem { ImageUrl = "test.jpg", ColSpan = 1, RowSpan = 1 };

    // Act
    var template = BzTemplateFactory.SelectBentoTemplate(item);

    // Assert
    var cut = Render(builder => template(item)(builder));
    cut.Find("div.bzb-card-simple").Should().NotBeNull();
  }

  [Fact]
  public void SelectBentoTemplate_ReturnsBentoStat_WhenNoImageButHasTitle()
  {
    // Arrange
    var item = new BzItem { Title = "Users", Description = "100" };

    // Act
    var template = BzTemplateFactory.SelectBentoTemplate(item);

    // Assert
    var cut = Render(builder => template(item)(builder));
    cut.Find("div.bzb-stat").Should().NotBeNull();
  }

  [Fact]
  public void SelectBentoTemplate_ReturnsBentoStat_WhenNoImageButHasDescription()
  {
    // Arrange
    var item = new BzItem { Description = "Some value" };

    // Act
    var template = BzTemplateFactory.SelectBentoTemplate(item);

    // Assert
    var cut = Render(builder => template(item)(builder));
    cut.Find("div.bzb-stat").Should().NotBeNull();
  }

  [Fact]
  public void SelectBentoTemplate_ReturnsFallback_WhenNoData()
  {
    // Arrange
    var item = new BzItem();

    // Act
    var template = BzTemplateFactory.SelectBentoTemplate(item);

    // Assert
    var cut = Render(builder => template(item)(builder));
    cut.Find("div.bzc-fallback-item").Should().NotBeNull();
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
