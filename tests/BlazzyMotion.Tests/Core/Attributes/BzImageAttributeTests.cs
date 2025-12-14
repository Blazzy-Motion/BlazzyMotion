using System.Reflection;

namespace BlazzyMotion.Tests.Core.Attributes;

/// <summary>
/// Tests for BzImageAttribute marker attribute.
/// </summary>
public class BzImageAttributeTests
{
  #region Constructor Tests

  [Fact]
  public void Constructor_ShouldCreateInstance()
  {
    // Act
    var attribute = new BzImageAttribute();

    // Assert
    attribute.Should().NotBeNull();
  }

  [Fact]
  public void Attribute_ShouldBeAssignableToAttribute()
  {
    // Act
    var attribute = new BzImageAttribute();

    // Assert
    attribute.Should().BeAssignableTo<Attribute>();
  }

  #endregion

  #region AttributeUsage Tests

  [Fact]
  public void Attribute_ShouldOnlyBeValidOnProperties()
  {
    // Arrange
    var attributeUsage = typeof(BzImageAttribute)
        .GetCustomAttribute<AttributeUsageAttribute>();

    // Assert
    attributeUsage.Should().NotBeNull();
    attributeUsage!.ValidOn.Should().Be(AttributeTargets.Property);
  }

  [Fact]
  public void Attribute_ShouldNotAllowMultiple()
  {
    // Arrange
    var attributeUsage = typeof(BzImageAttribute)
        .GetCustomAttribute<AttributeUsageAttribute>();

    // Assert
    attributeUsage.Should().NotBeNull();
    attributeUsage!.AllowMultiple.Should().BeFalse();
  }

  [Fact]
  public void Attribute_ShouldBeInherited()
  {
    // Arrange
    var attributeUsage = typeof(BzImageAttribute)
        .GetCustomAttribute<AttributeUsageAttribute>();

    // Assert
    attributeUsage.Should().NotBeNull();
    attributeUsage!.Inherited.Should().BeTrue();
  }

  #endregion

  #region Application Tests

  [Fact]
  public void Attribute_CanBeAppliedToProperty()
  {
    // Arrange
    var propertyInfo = typeof(TestModelWithImage).GetProperty(nameof(TestModelWithImage.ImageUrl));

    // Act
    var attribute = propertyInfo?.GetCustomAttribute<BzImageAttribute>();

    // Assert
    attribute.Should().NotBeNull();
  }

  [Fact]
  public void Attribute_IsInheritedFromBaseClass()
  {
    // Arrange
    var propertyInfo = typeof(DerivedModelWithImage).GetProperty(nameof(DerivedModelWithImage.ImageUrl));

    // Act
    var attribute = propertyInfo?.GetCustomAttribute<BzImageAttribute>(inherit: true);

    // Assert
    attribute.Should().NotBeNull();
  }

  #endregion

  #region Test Models

  private class TestModelWithImage
  {
    [BzImage]
    public string? ImageUrl { get; set; }
  }

  private class BaseModelWithImage
  {
    [BzImage]
    public virtual string? ImageUrl { get; set; }
  }

  private class DerivedModelWithImage : BaseModelWithImage
  {
    public override string? ImageUrl { get; set; }
  }

  #endregion
}
