using System.Reflection;

namespace BlazzyMotion.Tests.Core.Attributes;

/// <summary>
/// Tests for BzTitleAttribute marker attribute.
/// </summary>
public class BzTitleAttributeTests
{
  #region Constructor Tests

  [Fact]
  public void Constructor_ShouldCreateInstance()
  {
    // Act
    var attribute = new BzTitleAttribute();

    // Assert
    attribute.Should().NotBeNull();
  }

  [Fact]
  public void Attribute_ShouldBeAssignableToAttribute()
  {
    // Act
    var attribute = new BzTitleAttribute();

    // Assert
    attribute.Should().BeAssignableTo<Attribute>();
  }

  #endregion

  #region AttributeUsage Tests

  [Fact]
  public void Attribute_ShouldOnlyBeValidOnProperties()
  {
    // Arrange
    var attributeUsage = typeof(BzTitleAttribute)
        .GetCustomAttribute<AttributeUsageAttribute>();

    // Assert
    attributeUsage.Should().NotBeNull();
    attributeUsage!.ValidOn.Should().Be(AttributeTargets.Property);
  }

  [Fact]
  public void Attribute_ShouldNotAllowMultiple()
  {
    // Arrange
    var attributeUsage = typeof(BzTitleAttribute)
        .GetCustomAttribute<AttributeUsageAttribute>();

    // Assert
    attributeUsage.Should().NotBeNull();
    attributeUsage!.AllowMultiple.Should().BeFalse();
  }

  [Fact]
  public void Attribute_ShouldBeInherited()
  {
    // Arrange
    var attributeUsage = typeof(BzTitleAttribute)
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
    var propertyInfo = typeof(TestModelWithTitle).GetProperty(nameof(TestModelWithTitle.Title));

    // Act
    var attribute = propertyInfo?.GetCustomAttribute<BzTitleAttribute>();

    // Assert
    attribute.Should().NotBeNull();
  }

  [Fact]
  public void Attribute_IsInheritedFromBaseClass()
  {
    // Arrange
    var propertyInfo = typeof(DerivedModelWithTitle).GetProperty(nameof(DerivedModelWithTitle.Title));

    // Act
    var attribute = propertyInfo?.GetCustomAttribute<BzTitleAttribute>(inherit: true);

    // Assert
    attribute.Should().NotBeNull();
  }

  #endregion

  #region Test Models

  private class TestModelWithTitle
  {
    [BzTitle]
    public string? Title { get; set; }
  }

  private class BaseModelWithTitle
  {
    [BzTitle]
    public virtual string? Title { get; set; }
  }

  private class DerivedModelWithTitle : BaseModelWithTitle
  {
    public override string? Title { get; set; }
  }

  #endregion
}
