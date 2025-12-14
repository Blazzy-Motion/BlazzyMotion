using BlazzyMotion.Tests.Helpers;

namespace BlazzyMotion.Tests.Core.Models;

/// <summary>
/// Tests for BzItem universal data model.
/// </summary>
public class BzItemTests
{
  #region Default Values Tests

  [Fact]
  public void Constructor_ImageUrl_ShouldBeNullByDefault()
  {
    // Act
    var item = new BzItem();

    // Assert
    item.ImageUrl.Should().BeNull();
  }

  [Fact]
  public void Constructor_Title_ShouldBeNullByDefault()
  {
    // Act
    var item = new BzItem();

    // Assert
    item.Title.Should().BeNull();
  }

  [Fact]
  public void Constructor_Description_ShouldBeNullByDefault()
  {
    // Act
    var item = new BzItem();

    // Assert
    item.Description.Should().BeNull();
  }

  [Fact]
  public void Constructor_OriginalItem_ShouldBeNullByDefault()
  {
    // Act
    var item = new BzItem();

    // Assert
    item.OriginalItem.Should().BeNull();
  }

  #endregion

  #region Property Setters Tests

  [Fact]
  public void ImageUrl_CanBeSetAndRetrieved()
  {
    // Arrange
    var item = new BzItem();
    var expected = "https://example.com/image.jpg";

    // Act
    item.ImageUrl = expected;

    // Assert
    item.ImageUrl.Should().Be(expected);
  }

  [Fact]
  public void Title_CanBeSetAndRetrieved()
  {
    // Arrange
    var item = new BzItem();
    var expected = "Test Title";

    // Act
    item.Title = expected;

    // Assert
    item.Title.Should().Be(expected);
  }

  [Fact]
  public void Description_CanBeSetAndRetrieved()
  {
    // Arrange
    var item = new BzItem();
    var expected = "Test Description";

    // Act
    item.Description = expected;

    // Assert
    item.Description.Should().Be(expected);
  }

  [Fact]
  public void OriginalItem_CanBeSetToAnyObject()
  {
    // Arrange
    var item = new BzItem();
    var original = new TestMovie { Title = "Test" };

    // Act
    item.OriginalItem = original;

    // Assert
    item.OriginalItem.Should().BeSameAs(original);
  }

  #endregion

  #region HasImage Tests

  [Fact]
  public void HasImage_ReturnsFalse_WhenImageUrlIsNull()
  {
    // Arrange
    var item = new BzItem { ImageUrl = null };

    // Assert
    item.HasImage.Should().BeFalse();
  }

  [Fact]
  public void HasImage_ReturnsFalse_WhenImageUrlIsEmpty()
  {
    // Arrange
    var item = new BzItem { ImageUrl = "" };

    // Assert
    item.HasImage.Should().BeFalse();
  }

  [Fact]
  public void HasImage_ReturnsFalse_WhenImageUrlIsWhitespace()
  {
    // Arrange
    var item = new BzItem { ImageUrl = "   " };

    // Assert
    item.HasImage.Should().BeFalse();
  }

  [Fact]
  public void HasImage_ReturnsTrue_WhenImageUrlHasValue()
  {
    // Arrange
    var item = new BzItem { ImageUrl = "https://example.com/image.jpg" };

    // Assert
    item.HasImage.Should().BeTrue();
  }

  #endregion

  #region HasTitle Tests

  [Fact]
  public void HasTitle_ReturnsFalse_WhenTitleIsNull()
  {
    // Arrange
    var item = new BzItem { Title = null };

    // Assert
    item.HasTitle.Should().BeFalse();
  }

  [Fact]
  public void HasTitle_ReturnsFalse_WhenTitleIsEmpty()
  {
    // Arrange
    var item = new BzItem { Title = "" };

    // Assert
    item.HasTitle.Should().BeFalse();
  }

  [Fact]
  public void HasTitle_ReturnsFalse_WhenTitleIsWhitespace()
  {
    // Arrange
    var item = new BzItem { Title = "   " };

    // Assert
    item.HasTitle.Should().BeFalse();
  }

  [Fact]
  public void HasTitle_ReturnsTrue_WhenTitleHasValue()
  {
    // Arrange
    var item = new BzItem { Title = "Test Title" };

    // Assert
    item.HasTitle.Should().BeTrue();
  }

  #endregion

  #region DisplayTitle Tests

  [Fact]
  public void DisplayTitle_ReturnsTitle_WhenHasTitleIsTrue()
  {
    // Arrange
    var item = new BzItem { Title = "My Title" };

    // Assert
    item.DisplayTitle.Should().Be("My Title");
  }

  [Fact]
  public void DisplayTitle_ReturnsItem_WhenTitleIsNull()
  {
    // Arrange
    var item = new BzItem { Title = null };

    // Assert
    item.DisplayTitle.Should().Be("Item");
  }

  [Fact]
  public void DisplayTitle_ReturnsItem_WhenTitleIsEmpty()
  {
    // Arrange
    var item = new BzItem { Title = "" };

    // Assert
    item.DisplayTitle.Should().Be("Item");
  }

  [Fact]
  public void DisplayTitle_ReturnsItem_WhenTitleIsWhitespace()
  {
    // Arrange
    var item = new BzItem { Title = "   " };

    // Assert
    item.DisplayTitle.Should().Be("Item");
  }

  #endregion

  #region GetOriginal<T> Tests

  [Fact]
  public void GetOriginal_ReturnsNull_WhenOriginalItemIsNull()
  {
    // Arrange
    var item = new BzItem { OriginalItem = null };

    // Act
    var result = item.GetOriginal<TestMovie>();

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public void GetOriginal_ReturnsNull_WhenOriginalItemIsWrongType()
  {
    // Arrange
    var item = new BzItem { OriginalItem = "string value" };

    // Act
    var result = item.GetOriginal<TestMovie>();

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public void GetOriginal_ReturnsCastedObject_WhenTypeMatches()
  {
    // Arrange
    var original = new TestMovie { Title = "Test Movie", ImageUrl = "test.jpg" };
    var item = new BzItem { OriginalItem = original };

    // Act
    var result = item.GetOriginal<TestMovie>();

    // Assert
    result.Should().NotBeNull();
    result.Should().BeSameAs(original);
    result!.Title.Should().Be("Test Movie");
  }

  [Fact]
  public void GetOriginal_WorksWithCorrectType()
  {
    // Arrange
    var original = new TestMovie { Title = "Test", Year = 2024 };
    var item = new BzItem { OriginalItem = original };

    // Act
    var result = item.GetOriginal<TestMovie>();

    // Assert
    result.Should().NotBeNull();
    result!.Year.Should().Be(2024);
    result.Title.Should().Be("Test");
  }

  #endregion

  #region Edge Cases

  [Fact]
  public void ImageUrl_CanContainUnicodeCharacters()
  {
    // Arrange
    var item = new BzItem { ImageUrl = "https://example.com/画像.jpg" };

    // Assert
    item.ImageUrl.Should().Be("https://example.com/画像.jpg");
    item.HasImage.Should().BeTrue();
  }

  [Fact]
  public void Title_CanContainUnicodeCharacters()
  {
    // Arrange
    var item = new BzItem { Title = "日本語タイトル" };

    // Assert
    item.Title.Should().Be("日本語タイトル");
    item.HasTitle.Should().BeTrue();
  }

  [Fact]
  public void ImageUrl_CanContainSpecialCharacters()
  {
    // Arrange
    var url = "https://example.com/image?id=123&size=large#section";
    var item = new BzItem { ImageUrl = url };

    // Assert
    item.ImageUrl.Should().Be(url);
    item.HasImage.Should().BeTrue();
  }

  #endregion
}
