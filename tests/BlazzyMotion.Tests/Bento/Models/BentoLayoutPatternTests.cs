using BlazzyMotion.Bento.Models;

namespace BlazzyMotion.Tests.Bento.Models;

/// <summary>
/// Tests for BentoLayoutPattern enum.
/// </summary>
public class BentoLayoutPatternTests
{
  #region Enum Values Tests

  [Fact]
  public void Dynamic_ShouldHaveValue0()
  {
    // Arrange & Act
    var value = (int)BentoLayoutPattern.Dynamic;

    // Assert
    value.Should().Be(0);
  }

  [Fact]
  public void Featured_ShouldHaveValue1()
  {
    // Arrange & Act
    var value = (int)BentoLayoutPattern.Featured;

    // Assert
    value.Should().Be(1);
  }

  [Fact]
  public void Balanced_ShouldHaveValue2()
  {
    // Arrange & Act
    var value = (int)BentoLayoutPattern.Balanced;

    // Assert
    value.Should().Be(2);
  }

  #endregion

  #region Enum Definition Tests

  [Fact]
  public void BentoLayoutPattern_ShouldHaveThreeValues()
  {
    // Arrange & Act
    var values = Enum.GetValues<BentoLayoutPattern>();

    // Assert
    values.Should().HaveCount(3);
  }

  [Fact]
  public void BentoLayoutPattern_ShouldContainAllExpectedValues()
  {
    // Arrange & Act
    var values = Enum.GetValues<BentoLayoutPattern>();

    // Assert
    values.Should().Contain(BentoLayoutPattern.Dynamic);
    values.Should().Contain(BentoLayoutPattern.Featured);
    values.Should().Contain(BentoLayoutPattern.Balanced);
  }

  #endregion

  #region Default Value Tests

  [Fact]
  public void Default_ShouldBeDynamic()
  {
    // Arrange & Act
    BentoLayoutPattern defaultPattern = default;

    // Assert
    defaultPattern.Should().Be(BentoLayoutPattern.Dynamic);
  }

  #endregion

  #region Name Tests

  [Fact]
  public void Dynamic_ShouldHaveCorrectName()
  {
    // Arrange & Act
    var name = BentoLayoutPattern.Dynamic.ToString();

    // Assert
    name.Should().Be("Dynamic");
  }

  [Fact]
  public void Featured_ShouldHaveCorrectName()
  {
    // Arrange & Act
    var name = BentoLayoutPattern.Featured.ToString();

    // Assert
    name.Should().Be("Featured");
  }

  [Fact]
  public void Balanced_ShouldHaveCorrectName()
  {
    // Arrange & Act
    var name = BentoLayoutPattern.Balanced.ToString();

    // Assert
    name.Should().Be("Balanced");
  }

  #endregion

  #region Parsing Tests

  [Theory]
  [InlineData("Dynamic", BentoLayoutPattern.Dynamic)]
  [InlineData("Featured", BentoLayoutPattern.Featured)]
  [InlineData("Balanced", BentoLayoutPattern.Balanced)]
  public void Parse_WithValidName_ShouldReturnCorrectValue(string name, BentoLayoutPattern expected)
  {
    // Arrange & Act
    var result = Enum.Parse<BentoLayoutPattern>(name);

    // Assert
    result.Should().Be(expected);
  }

  [Theory]
  [InlineData(0, BentoLayoutPattern.Dynamic)]
  [InlineData(1, BentoLayoutPattern.Featured)]
  [InlineData(2, BentoLayoutPattern.Balanced)]
  public void Cast_FromInt_ShouldReturnCorrectValue(int value, BentoLayoutPattern expected)
  {
    // Arrange & Act
    var result = (BentoLayoutPattern)value;

    // Assert
    result.Should().Be(expected);
  }

  #endregion

  #region IsDefined Tests

  [Theory]
  [InlineData(BentoLayoutPattern.Dynamic)]
  [InlineData(BentoLayoutPattern.Featured)]
  [InlineData(BentoLayoutPattern.Balanced)]
  public void IsDefined_WithValidValue_ShouldReturnTrue(BentoLayoutPattern pattern)
  {
    // Arrange & Act
    var isDefined = Enum.IsDefined(typeof(BentoLayoutPattern), pattern);

    // Assert
    isDefined.Should().BeTrue();
  }

  [Theory]
  [InlineData(99)]
  [InlineData(-1)]
  [InlineData(3)]
  public void IsDefined_WithInvalidValue_ShouldReturnFalse(int value)
  {
    // Arrange & Act
    var isDefined = Enum.IsDefined(typeof(BentoLayoutPattern), value);

    // Assert
    isDefined.Should().BeFalse();
  }

  #endregion
}
