using BlazzyMotion.Bento.Abstractions;
using BlazzyMotion.Bento.Components;
using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Tests.Bento.Abstractions;

/// <summary>
/// Tests for BzBentoItemBase abstract class.
/// Uses BzBentoFeature as concrete implementation for testing.
/// </summary>
public class BzBentoItemBaseTests : TestBase
{
  #region Default Values Tests

  [Fact]
  public void DefaultColSpan_ShouldBe1()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test"));

    // Assert
    cut.Instance.ColSpan.Should().Be(1);
  }

  [Fact]
  public void DefaultRowSpan_ShouldBe1()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test"));

    // Assert
    cut.Instance.RowSpan.Should().Be(1);
  }

  [Fact]
  public void DefaultOrder_ShouldBe0()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test"));

    // Assert
    cut.Instance.Order.Should().Be(0);
  }

  [Fact]
  public void DefaultCssClass_ShouldBeNull()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test"));

    // Assert
    cut.Instance.CssClass.Should().BeNull();
  }

  [Fact]
  public void DefaultStyle_ShouldBeNull()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test"));

    // Assert
    cut.Instance.Style.Should().BeNull();
  }

  #endregion

  #region ColSpan Tests

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(4)]
  public void ColSpan_CanBeSetToValidValues(int colSpan)
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.ColSpan, colSpan));

    // Assert
    cut.Instance.ColSpan.Should().Be(colSpan);
  }

  [Fact]
  public void ColSpan_GeneratesCorrectCssClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.ColSpan, 2));

    // Assert
    cut.Markup.Should().Contain("bzb-col-2");
  }

  #endregion

  #region RowSpan Tests

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(4)]
  public void RowSpan_CanBeSetToValidValues(int rowSpan)
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.RowSpan, rowSpan));

    // Assert
    cut.Instance.RowSpan.Should().Be(rowSpan);
  }

  [Fact]
  public void RowSpan_GeneratesCorrectCssClass()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.RowSpan, 2));

    // Assert
    cut.Markup.Should().Contain("bzb-row-2");
  }

  #endregion

  #region SpanClasses Tests

  [Fact]
  public void SpanClasses_WithDefaultValues_ShouldGenerateCol1Row1()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test"));

    // Assert
    cut.Markup.Should().Contain("bzb-col-1");
    cut.Markup.Should().Contain("bzb-row-1");
  }

  [Theory]
  [InlineData(2, 2, "bzb-col-2", "bzb-row-2")]
  [InlineData(1, 3, "bzb-col-1", "bzb-row-3")]
  [InlineData(4, 1, "bzb-col-4", "bzb-row-1")]
  [InlineData(3, 4, "bzb-col-3", "bzb-row-4")]
  public void SpanClasses_GeneratesCorrectCombination(int colSpan, int rowSpan, string expectedCol, string expectedRow)
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.ColSpan, colSpan)
        .Add(p => p.RowSpan, rowSpan));

    // Assert
    cut.Markup.Should().Contain(expectedCol);
    cut.Markup.Should().Contain(expectedRow);
  }

  [Theory]
  [InlineData(0, 1)] // Below min
  [InlineData(5, 4)] // Above max, clamped to 4
  [InlineData(-1, 1)] // Negative, clamped to 1
  public void SpanClasses_WithInvalidColSpan_ShouldClampValue(int input, int expectedClamped)
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.ColSpan, input));

    // Assert
    cut.Markup.Should().Contain($"bzb-col-{expectedClamped}");
  }

  [Theory]
  [InlineData(0, 1)] // Below min
  [InlineData(5, 4)] // Above max, clamped to 4
  [InlineData(-1, 1)] // Negative, clamped to 1
  public void SpanClasses_WithInvalidRowSpan_ShouldClampValue(int input, int expectedClamped)
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.RowSpan, input));

    // Assert
    cut.Markup.Should().Contain($"bzb-row-{expectedClamped}");
  }

  #endregion

  #region Order Tests

  [Theory]
  [InlineData(0)]
  [InlineData(1)]
  [InlineData(5)]
  [InlineData(-1)]
  [InlineData(10)]
  public void Order_CanBeSetToAnyIntValue(int order)
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.Order, order));

    // Assert
    cut.Instance.Order.Should().Be(order);
  }

  [Fact]
  public void Order_IsIncludedInStyle()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.Order, 5));

    // Assert
    cut.Markup.Should().Contain("order: 5");
  }

  [Fact]
  public void Order_WithNegativeValue_IsIncludedInStyle()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.Order, -1));

    // Assert
    cut.Markup.Should().Contain("order: -1");
  }

  #endregion

  #region CssClass Tests

  [Fact]
  public void CssClass_CanBeSet()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.CssClass, "custom-class"));

    // Assert
    cut.Markup.Should().Contain("custom-class");
  }

  [Fact]
  public void CssClass_WithMultipleClasses_AreAllApplied()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.CssClass, "class-1 class-2 class-3"));

    // Assert
    cut.Markup.Should().Contain("class-1");
    cut.Markup.Should().Contain("class-2");
    cut.Markup.Should().Contain("class-3");
  }

  #endregion

  #region Style Tests

  [Fact]
  public void Style_CanBeSet()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.Style, "background-color: red"));

    // Assert
    cut.Markup.Should().Contain("background-color: red");
  }

  [Fact]
  public void Style_CombinesWithOrderStyle()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.Order, 3)
        .Add(p => p.Style, "color: blue"));

    // Assert
    cut.Markup.Should().Contain("order: 3");
    cut.Markup.Should().Contain("color: blue");
  }

  #endregion

  #region AdditionalAttributes Tests

  [Fact]
  public void AdditionalAttributes_DataAttribute_IsApplied()
  {
    // Arrange
    var attributes = new Dictionary<string, object> { { "data-test", "value" } };

    // Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.AdditionalAttributes, attributes));

    // Assert
    cut.Markup.Should().Contain("data-test=\"value\"");
  }

  [Fact]
  public void AdditionalAttributes_Id_IsApplied()
  {
    // Arrange
    var attributes = new Dictionary<string, object> { { "id", "my-id" } };

    // Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.AdditionalAttributes, attributes));

    // Assert
    cut.Markup.Should().Contain("id=\"my-id\"");
  }

  [Fact]
  public void AdditionalAttributes_AriaAttributes_AreApplied()
  {
    // Arrange
    var attributes = new Dictionary<string, object> { { "aria-label", "Test Label" } };

    // Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.AdditionalAttributes, attributes));
  }

  #endregion

  #region OnClick Tests

  [Fact]
  public void OnClick_WhenProvided_CanBeInvoked()
  {
    // Arrange
    var clicked = false;
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.OnClick, EventCallback.Factory.Create(this, () => clicked = true)));

    // Act
    cut.Find(".bzb-feature").Click();

    // Assert
    clicked.Should().BeTrue();
  }

  #endregion

  #region GetItemStyle Tests

  [Fact]
  public void GetItemStyle_WithOrderOnly_GeneratesCorrectStyle()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.Order, 2));

    // Assert
    cut.Markup.Should().Contain("order: 2");
  }

  [Fact]
  public void GetItemStyle_WithStyleOnly_GeneratesCorrectStyle()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.Style, "padding: 10px"));

    // Assert
    cut.Markup.Should().Contain("padding: 10px");
  }

  [Fact]
  public void GetItemStyle_WithBothOrderAndStyle_CombinesBoth()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.Order, 3)
        .Add(p => p.Style, "margin: 5px"));

    // Assert
    cut.Markup.Should().Contain("order: 3");
    cut.Markup.Should().Contain("margin: 5px");
  }

  [Fact]
  public void GetItemStyle_WithOrderZero_IncludesOrderInStyle()
  {
    // Arrange & Act
    var cut = RenderComponent<BzBentoFeature>(parameters => parameters
        .Add(p => p.Label, "Test")
        .Add(p => p.Order, 0));

    // Assert
    // Order 0 is default but should still be in style
    cut.Markup.Should().Contain("order: 0");
  }

  #endregion
}
