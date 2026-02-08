using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace BlazzyMotion.Tests.Gallery.Models;

/// <summary>
/// Tests for BzGalleryOptions configuration class.
/// </summary>
public class BzGalleryOptionsTests
{
    #region Default Values Tests

    [Fact]
    public void Constructor_ShouldSetDefaultLayout()
    {
        var options = new BzGalleryOptions();
        options.Layout.Should().Be("grid");
    }

    [Fact]
    public void Constructor_ShouldSetDefaultColumns()
    {
        var options = new BzGalleryOptions();
        options.Columns.Should().Be(3);
    }

    [Fact]
    public void Constructor_ShouldSetDefaultGap()
    {
        var options = new BzGalleryOptions();
        options.Gap.Should().Be(16);
    }

    [Fact]
    public void Constructor_ShouldSetDefaultEnableLightbox()
    {
        var options = new BzGalleryOptions();
        options.EnableLightbox.Should().BeTrue();
    }

    [Fact]
    public void Constructor_ShouldSetDefaultAnimationEnabled()
    {
        var options = new BzGalleryOptions();
        options.AnimationEnabled.Should().BeTrue();
    }

    [Fact]
    public void Constructor_ShouldSetDefaultStaggerDelay()
    {
        var options = new BzGalleryOptions();
        options.StaggerDelay.Should().Be(50);
    }

    [Fact]
    public void Constructor_ShouldSetDefaultAspectRatioToNull()
    {
        var options = new BzGalleryOptions();
        options.AspectRatio.Should().BeNull();
    }

    #endregion

    #region Layout Tests

    [Theory]
    [InlineData("grid")]
    [InlineData("masonry")]
    [InlineData("list")]
    public void Layout_CanBeSetToValidValues(string layout)
    {
        var options = new BzGalleryOptions { Layout = layout };
        options.Layout.Should().Be(layout);
    }

    #endregion

    #region Columns Tests

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void Columns_SupportsValidRange(int columns)
    {
        var options = new BzGalleryOptions { Columns = columns };
        options.Columns.Should().Be(columns);
    }

    #endregion

    #region Gap Tests

    [Theory]
    [InlineData(0)]
    [InlineData(8)]
    [InlineData(12)]
    [InlineData(16)]
    [InlineData(24)]
    [InlineData(32)]
    public void Gap_SupportsCommonSpacingValues(int gap)
    {
        var options = new BzGalleryOptions { Gap = gap };
        options.Gap.Should().Be(gap);
    }

    #endregion

    #region EnableLightbox Tests

    [Fact]
    public void EnableLightbox_CanBeDisabled()
    {
        var options = new BzGalleryOptions { EnableLightbox = false };
        options.EnableLightbox.Should().BeFalse();
    }

    [Fact]
    public void EnableLightbox_CanBeEnabled()
    {
        var options = new BzGalleryOptions { EnableLightbox = true };
        options.EnableLightbox.Should().BeTrue();
    }

    #endregion

    #region AnimationEnabled Tests

    [Fact]
    public void AnimationEnabled_CanBeDisabled()
    {
        var options = new BzGalleryOptions { AnimationEnabled = false };
        options.AnimationEnabled.Should().BeFalse();
    }

    [Fact]
    public void AnimationEnabled_CanBeEnabled()
    {
        var options = new BzGalleryOptions { AnimationEnabled = true };
        options.AnimationEnabled.Should().BeTrue();
    }

    #endregion

    #region StaggerDelay Tests

    [Theory]
    [InlineData(0)]
    [InlineData(30)]
    [InlineData(50)]
    [InlineData(75)]
    [InlineData(100)]
    public void StaggerDelay_SupportsRecommendedRange(int delay)
    {
        var options = new BzGalleryOptions { StaggerDelay = delay };
        options.StaggerDelay.Should().Be(delay);
    }

    #endregion

    #region AspectRatio Tests

    [Fact]
    public void AspectRatio_CanBeSetToNull()
    {
        var options = new BzGalleryOptions { AspectRatio = null };
        options.AspectRatio.Should().BeNull();
    }

    [Theory]
    [InlineData("1/1")]
    [InlineData("4/3")]
    [InlineData("16/9")]
    public void AspectRatio_CanBeSetToValidValues(string ratio)
    {
        var options = new BzGalleryOptions { AspectRatio = ratio };
        options.AspectRatio.Should().Be(ratio);
    }

    #endregion

    #region Object Initialization Tests

    [Fact]
    public void Options_CanBeInitializedWithObjectInitializer()
    {
        var options = new BzGalleryOptions
        {
            Layout = "masonry",
            Columns = 4,
            Gap = 24,
            EnableLightbox = false,
            AnimationEnabled = false,
            StaggerDelay = 75,
            AspectRatio = "16/9"
        };

        options.Layout.Should().Be("masonry");
        options.Columns.Should().Be(4);
        options.Gap.Should().Be(24);
        options.EnableLightbox.Should().BeFalse();
        options.AnimationEnabled.Should().BeFalse();
        options.StaggerDelay.Should().Be(75);
        options.AspectRatio.Should().Be("16/9");
    }

    [Fact]
    public void Options_WithPartialConfiguration_ShouldKeepDefaults()
    {
        var options = new BzGalleryOptions { Columns = 2 };

        options.Layout.Should().Be("grid");
        options.Gap.Should().Be(16);
        options.EnableLightbox.Should().BeTrue();
        options.AnimationEnabled.Should().BeTrue();
        options.StaggerDelay.Should().Be(50);
        options.AspectRatio.Should().BeNull();
    }

    #endregion

    #region Property Independence Tests

    [Fact]
    public void AnimationEnabled_DoesNotAffectStaggerDelay()
    {
        var options = new BzGalleryOptions { AnimationEnabled = false };
        options.StaggerDelay.Should().Be(50);
    }

    [Fact]
    public void Columns_DoesNotAffectGap()
    {
        var options = new BzGalleryOptions { Columns = 5 };
        options.Gap.Should().Be(16);
    }

    [Fact]
    public void Layout_DoesNotAffectColumns()
    {
        var options = new BzGalleryOptions { Layout = "list" };
        options.Columns.Should().Be(3);
    }

    #endregion

    #region JSON Serialization Tests

    [Fact]
    public void Options_ShouldHaveJsonPropertyNameAttributes()
    {
        var properties = typeof(BzGalleryOptions).GetProperties();

        foreach (var property in properties)
        {
            var attr = property.GetCustomAttribute<JsonPropertyNameAttribute>();
            attr.Should().NotBeNull($"Property '{property.Name}' should have [JsonPropertyName]");
        }
    }

    [Fact]
    public void Options_ShouldSerializeToCamelCase()
    {
        var options = new BzGalleryOptions { Layout = "masonry", Columns = 2 };
        var json = JsonSerializer.Serialize(options);

        json.Should().Contain("\"layout\"");
        json.Should().Contain("\"columns\"");
        json.Should().Contain("\"gap\"");
        json.Should().Contain("\"enableLightbox\"");
        json.Should().Contain("\"animationEnabled\"");
        json.Should().Contain("\"staggerDelay\"");
        json.Should().Contain("\"aspectRatio\"");
    }

    #endregion
}
