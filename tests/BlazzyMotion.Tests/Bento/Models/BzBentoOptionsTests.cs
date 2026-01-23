using BlazzyMotion.Bento.Models;

namespace BlazzyMotion.Tests.Bento.Models;

/// <summary>
/// Tests for BzBentoOptions configuration class.
/// </summary>
public class BzBentoOptionsTests
{
  #region Default Values Tests

  [Fact]
  public void Constructor_ShouldSetDefaultColumns()
  {
    // Arrange & Act
    var options = new BzBentoOptions();

    // Assert
    options.Columns.Should().Be(4);
  }

  [Fact]
  public void Constructor_ShouldSetDefaultGap()
  {
    // Arrange & Act
    var options = new BzBentoOptions();

    // Assert
    options.Gap.Should().Be(16);
  }

  [Fact]
  public void Constructor_ShouldSetDefaultAnimationEnabled()
  {
    // Arrange & Act
    var options = new BzBentoOptions();

    // Assert
    options.AnimationEnabled.Should().BeTrue();
  }

  [Fact]
  public void Constructor_ShouldSetDefaultStaggerDelay()
  {
    // Arrange & Act
    var options = new BzBentoOptions();

    // Assert
    options.StaggerDelay.Should().Be(50);
  }

  #endregion

  #region Columns Tests

  [Fact]
  public void Columns_CanBeSet()
  {
    // Arrange
    var options = new BzBentoOptions();

    // Act
    options.Columns = 3;

    // Assert
    options.Columns.Should().Be(3);
  }

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(4)]
  [InlineData(6)]
  [InlineData(12)]
  public void Columns_SupportsCommonGridValues(int columns)
  {
    // Arrange
    var options = new BzBentoOptions();

    // Act
    options.Columns = columns;

    // Assert
    options.Columns.Should().Be(columns);
  }

  #endregion

  #region Gap Tests

  [Fact]
  public void Gap_CanBeSet()
  {
    // Arrange
    var options = new BzBentoOptions();

    // Act
    options.Gap = 20;

    // Assert
    options.Gap.Should().Be(20);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(8)]
  [InlineData(12)]
  [InlineData(16)]
  [InlineData(24)]
  [InlineData(32)]
  public void Gap_SupportsCommonSpacingValues(int gap)
  {
    // Arrange
    var options = new BzBentoOptions();

    // Act
    options.Gap = gap;

    // Assert
    options.Gap.Should().Be(gap);
  }

  #endregion

  #region AnimationEnabled Tests

  [Fact]
  public void AnimationEnabled_CanBeDisabled()
  {
    // Arrange
    var options = new BzBentoOptions();

    // Act
    options.AnimationEnabled = false;

    // Assert
    options.AnimationEnabled.Should().BeFalse();
  }

  [Fact]
  public void AnimationEnabled_CanBeEnabled()
  {
    // Arrange
    var options = new BzBentoOptions { AnimationEnabled = false };

    // Act
    options.AnimationEnabled = true;

    // Assert
    options.AnimationEnabled.Should().BeTrue();
  }

  #endregion

  #region StaggerDelay Tests

  [Fact]
  public void StaggerDelay_CanBeSet()
  {
    // Arrange
    var options = new BzBentoOptions();

    // Act
    options.StaggerDelay = 100;

    // Assert
    options.StaggerDelay.Should().Be(100);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(30)]
  [InlineData(50)]
  [InlineData(75)]
  [InlineData(100)]
  public void StaggerDelay_SupportsRecommendedRange(int delay)
  {
    // Arrange
    var options = new BzBentoOptions();

    // Act
    options.StaggerDelay = delay;

    // Assert
    options.StaggerDelay.Should().Be(delay);
  }

  #endregion

  #region Object Initialization Tests

  [Fact]
  public void Options_CanBeInitializedWithObjectInitializer()
  {
    // Arrange & Act
    var options = new BzBentoOptions
    {
      Columns = 3,
      Gap = 20,
      AnimationEnabled = false,
      StaggerDelay = 75
    };

    // Assert
    options.Columns.Should().Be(3);
    options.Gap.Should().Be(20);
    options.AnimationEnabled.Should().BeFalse();
    options.StaggerDelay.Should().Be(75);
  }

  [Fact]
  public void Options_WithPartialConfiguration_ShouldKeepDefaults()
  {
    // Arrange & Act
    var options = new BzBentoOptions
    {
      Columns = 3
    };

    // Assert
    options.Columns.Should().Be(3);
    options.Gap.Should().Be(16); // Default
    options.AnimationEnabled.Should().BeTrue(); // Default
    options.StaggerDelay.Should().Be(50); // Default
  }

  #endregion

  #region Property Independence Tests

  [Fact]
  public void AnimationEnabled_DoesNotAffectStaggerDelay()
  {
    // Arrange
    var options = new BzBentoOptions
    {
      AnimationEnabled = false,
      StaggerDelay = 100
    };

    // Act & Assert
    options.AnimationEnabled.Should().BeFalse();
    options.StaggerDelay.Should().Be(100);
  }

  [Fact]
  public void Columns_DoesNotAffectGap()
  {
    // Arrange
    var options = new BzBentoOptions
    {
      Columns = 6,
      Gap = 24
    };

    // Act & Assert
    options.Columns.Should().Be(6);
    options.Gap.Should().Be(24);
  }

  #endregion
}
