namespace BlazzyMotion.Tests.Core.Models;

/// <summary>
/// Tests for BzTheme enum.
/// </summary>
public class BzThemeTests
{
  #region Enum Values Tests

  [Fact]
  public void BzTheme_ShouldHaveGlassValue()
  {
    // Assert
    BzTheme.Glass.Should().BeDefined();
    ((int)BzTheme.Glass).Should().Be(0);
  }

  [Fact]
  public void BzTheme_ShouldHaveDarkValue()
  {
    // Assert
    BzTheme.Dark.Should().BeDefined();
    ((int)BzTheme.Dark).Should().Be(1);
  }

  [Fact]
  public void BzTheme_ShouldHaveLightValue()
  {
    // Assert
    BzTheme.Light.Should().BeDefined();
    ((int)BzTheme.Light).Should().Be(2);
  }

  [Fact]
  public void BzTheme_ShouldHaveMinimalValue()
  {
    // Assert
    BzTheme.Minimal.Should().BeDefined();
    ((int)BzTheme.Minimal).Should().Be(3);
  }

  [Fact]
  public void BzTheme_ShouldHaveExactlyFourValues()
  {
    // Act
    var values = Enum.GetValues<BzTheme>();

    // Assert
    values.Should().HaveCount(4);
  }

  #endregion

  #region ToString Tests

  [Theory]
  [InlineData(BzTheme.Glass, "Glass")]
  [InlineData(BzTheme.Dark, "Dark")]
  [InlineData(BzTheme.Light, "Light")]
  [InlineData(BzTheme.Minimal, "Minimal")]
  public void BzTheme_ShouldConvertToString(BzTheme theme, string expected)
  {
    // Assert
    theme.ToString().Should().Be(expected);
  }

  #endregion

  #region Parse Tests

  [Theory]
  [InlineData("Glass", BzTheme.Glass)]
  [InlineData("Dark", BzTheme.Dark)]
  [InlineData("Light", BzTheme.Light)]
  [InlineData("Minimal", BzTheme.Minimal)]
  public void BzTheme_ShouldParseFromString(string value, BzTheme expected)
  {
    // Act
    var result = Enum.Parse<BzTheme>(value);

    // Assert
    result.Should().Be(expected);
  }

  [Theory]
  [InlineData("glass")]
  [InlineData("GLASS")]
  [InlineData("GlAsS")]
  public void BzTheme_ShouldParseIgnoringCase(string value)
  {
    // Act
    var result = Enum.Parse<BzTheme>(value, ignoreCase: true);

    // Assert
    result.Should().Be(BzTheme.Glass);
  }

  [Fact]
  public void BzTheme_ShouldThrowForInvalidString()
  {
    // Act
    var act = () => Enum.Parse<BzTheme>("InvalidTheme");

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  #endregion

  #region TryParse Tests

  [Fact]
  public void BzTheme_TryParse_ReturnsTrue_ForValidValue()
  {
    // Act
    var success = Enum.TryParse<BzTheme>("Glass", out var result);

    // Assert
    success.Should().BeTrue();
    result.Should().Be(BzTheme.Glass);
  }

  [Fact]
  public void BzTheme_TryParse_ReturnsFalse_ForInvalidValue()
  {
    // Act
    var success = Enum.TryParse<BzTheme>("InvalidTheme", out var result);

    // Assert
    success.Should().BeFalse();
    result.Should().Be(default(BzTheme));
  }

  #endregion

  #region Comparison Tests

  [Fact]
  public void BzTheme_ShouldSupportEquality()
  {
    // Arrange
    var theme1 = BzTheme.Glass;
    var theme2 = BzTheme.Glass;

    // Assert
    (theme1 == theme2).Should().BeTrue();
    theme1.Equals(theme2).Should().BeTrue();
  }

  [Fact]
  public void BzTheme_ShouldSupportInequality()
  {
    // Arrange
    var theme1 = BzTheme.Glass;
    var theme2 = BzTheme.Dark;

    // Assert
    (theme1 != theme2).Should().BeTrue();
    theme1.Equals(theme2).Should().BeFalse();
  }

  [Fact]
  public void BzTheme_DefaultValue_ShouldBeGlass()
  {
    // Arrange
    var defaultTheme = default(BzTheme);

    // Assert
    defaultTheme.Should().Be(BzTheme.Glass);
  }

  #endregion

  #region GetValues Tests

  [Fact]
  public void BzTheme_GetValues_ShouldContainAllThemes()
  {
    // Act
    var values = Enum.GetValues<BzTheme>();

    // Assert
    values.Should().Contain(BzTheme.Glass);
    values.Should().Contain(BzTheme.Dark);
    values.Should().Contain(BzTheme.Light);
    values.Should().Contain(BzTheme.Minimal);
  }

  [Fact]
  public void BzTheme_GetNames_ShouldReturnCorrectNames()
  {
    // Act
    var names = Enum.GetNames<BzTheme>();

    // Assert
    names.Should().BeEquivalentTo(new[] { "Glass", "Dark", "Light", "Minimal" });
  }

  #endregion
}
