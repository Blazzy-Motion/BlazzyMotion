using System.Reflection;

namespace BlazzyMotion.Tests.Core.Attributes;

/// <summary>
/// Tests for BzDescriptionAttribute marker attribute.
/// </summary>
public class BzDescriptionAttributeTests
{
  #region Constructor Tests

  [Fact]
  public void Constructor_ShouldCreateInstance()
  {
    // Act
    var attribute = new BzDescriptionAttribute();

    // Assert
    attribute.Should().NotBeNull();
  }

  [Fact]
  public void Attribute_ShouldBeAssignableToAttribute()
  {
    // Act
    var attribute = new BzDescriptionAttribute();

    // Assert
    attribute.Should().BeAssignableTo<Attribute>();
  }

  #endregion

  #region AttributeUsage Tests

  [Fact]
  public void Attribute_ShouldOnlyBeValidOnProperties()
  {
    // Arrange
    var attributeUsage = typeof(BzDescriptionAttribute)
        .GetCustomAttribute<AttributeUsageAttribute>();

    // Assert
    attributeUsage.Should().NotBeNull();
    attributeUsage!.ValidOn.Should().Be(AttributeTargets.Property);
  }

  [Fact]
  public void Attribute_ShouldNotAllowMultiple()
  {
    // Arrange
    var attributeUsage = typeof(BzDescriptionAttribute)
        .GetCustomAttribute<AttributeUsageAttribute>();

    // Assert
    attributeUsage.Should().NotBeNull();
    attributeUsage!.AllowMultiple.Should().BeFalse();
  }

  [Fact]
  public void Attribute_ShouldBeInherited()
  {
    // Arrange
    var attributeUsage = typeof(BzDescriptionAttribute)
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
    var propertyInfo = typeof(TestModelWithDescription).GetProperty(nameof(TestModelWithDescription.Description));

    // Act
    var attribute = propertyInfo?.GetCustomAttribute<BzDescriptionAttribute>();

    // Assert
    attribute.Should().NotBeNull();
  }

  [Fact]
  public void Attribute_IsInheritedFromBaseClass()
  {
    // Arrange
    var propertyInfo = typeof(DerivedModelWithDescription).GetProperty(nameof(DerivedModelWithDescription.Description));

    // Act
    var attribute = propertyInfo?.GetCustomAttribute<BzDescriptionAttribute>(inherit: true);

    // Assert
    attribute.Should().NotBeNull();
  }

  #endregion

  #region Test Models

  private class TestModelWithDescription
  {
    [BzDescription]
    public string? Description { get; set; }
  }

  private class BaseModelWithDescription
  {
    [BzDescription]
    public virtual string? Description { get; set; }
  }

  private class DerivedModelWithDescription : BaseModelWithDescription
  {
    public override string? Description { get; set; }
  }

  #endregion
}
