using System.Text.Json;

namespace BlazzyMotion.Tests.Marquee.Models;

public class BzMarqueeOptionsTests
{
    #region Default Values Tests

    [Fact]
    public void Constructor_ShouldSetDefaultDirection()
    {
        var options = new BzMarqueeOptions();
        options.Direction.Should().Be("left");
    }

    [Fact]
    public void Constructor_ShouldSetDefaultSpeed()
    {
        var options = new BzMarqueeOptions();
        options.Speed.Should().Be(50);
    }

    [Fact]
    public void Constructor_ShouldSetDefaultGap()
    {
        var options = new BzMarqueeOptions();
        options.Gap.Should().Be(40);
    }

    [Fact]
    public void Constructor_ShouldSetDefaultPauseOnHover()
    {
        var options = new BzMarqueeOptions();
        options.PauseOnHover.Should().BeTrue();
    }

    [Fact]
    public void Constructor_ShouldSetDefaultShowGradientEdges()
    {
        var options = new BzMarqueeOptions();
        options.ShowGradientEdges.Should().BeTrue();
    }

    [Fact]
    public void Constructor_ShouldSetDefaultFullWidth()
    {
        var options = new BzMarqueeOptions();
        options.FullWidth.Should().BeFalse();
    }

    [Fact]
    public void Constructor_ShouldSetDefaultReverse()
    {
        var options = new BzMarqueeOptions();
        options.Reverse.Should().BeFalse();
    }

    [Fact]
    public void Constructor_ShouldSetDefaultStaggerEntrance()
    {
        var options = new BzMarqueeOptions();
        options.StaggerEntrance.Should().BeTrue();
    }

    [Fact]
    public void Constructor_ShouldSetDefaultStaggerDelay()
    {
        var options = new BzMarqueeOptions();
        options.StaggerDelay.Should().Be(60);
    }

    #endregion

    #region Direction Tests

    [Theory]
    [InlineData("left")]
    [InlineData("right")]
    public void Direction_CanBeSetToValidValues(string direction)
    {
        var options = new BzMarqueeOptions { Direction = direction };
        options.Direction.Should().Be(direction);
    }

    #endregion

    #region Speed Tests

    [Theory]
    [InlineData(10)]
    [InlineData(30)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(200)]
    public void Speed_SupportsValidRange(int speed)
    {
        var options = new BzMarqueeOptions { Speed = speed };
        options.Speed.Should().Be(speed);
    }

    #endregion

    #region Gap Tests

    [Theory]
    [InlineData(0)]
    [InlineData(16)]
    [InlineData(24)]
    [InlineData(40)]
    [InlineData(60)]
    public void Gap_SupportsCommonSpacingValues(int gap)
    {
        var options = new BzMarqueeOptions { Gap = gap };
        options.Gap.Should().Be(gap);
    }

    #endregion

    #region Boolean Property Tests

    [Fact]
    public void PauseOnHover_CanBeDisabled()
    {
        var options = new BzMarqueeOptions { PauseOnHover = false };
        options.PauseOnHover.Should().BeFalse();
    }

    [Fact]
    public void ShowGradientEdges_CanBeDisabled()
    {
        var options = new BzMarqueeOptions { ShowGradientEdges = false };
        options.ShowGradientEdges.Should().BeFalse();
    }

    [Fact]
    public void FullWidth_CanBeEnabled()
    {
        var options = new BzMarqueeOptions { FullWidth = true };
        options.FullWidth.Should().BeTrue();
    }

    [Fact]
    public void Reverse_CanBeEnabled()
    {
        var options = new BzMarqueeOptions { Reverse = true };
        options.Reverse.Should().BeTrue();
    }

    [Fact]
    public void StaggerEntrance_CanBeDisabled()
    {
        var options = new BzMarqueeOptions { StaggerEntrance = false };
        options.StaggerEntrance.Should().BeFalse();
    }

    #endregion

    #region StaggerDelay Tests

    [Theory]
    [InlineData(0)]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(100)]
    [InlineData(150)]
    public void StaggerDelay_SupportsRecommendedRange(int delay)
    {
        var options = new BzMarqueeOptions { StaggerDelay = delay };
        options.StaggerDelay.Should().Be(delay);
    }

    #endregion

    #region Object Initialization Tests

    [Fact]
    public void Options_CanBeInitializedWithObjectInitializer()
    {
        var options = new BzMarqueeOptions
        {
            Direction = "right",
            Speed = 80,
            Gap = 24,
            PauseOnHover = false,
            ShowGradientEdges = false,
            FullWidth = true,
            Reverse = true,
            StaggerEntrance = false,
            StaggerDelay = 100
        };

        options.Direction.Should().Be("right");
        options.Speed.Should().Be(80);
        options.Gap.Should().Be(24);
        options.PauseOnHover.Should().BeFalse();
        options.ShowGradientEdges.Should().BeFalse();
        options.FullWidth.Should().BeTrue();
        options.Reverse.Should().BeTrue();
        options.StaggerEntrance.Should().BeFalse();
        options.StaggerDelay.Should().Be(100);
    }

    [Fact]
    public void Options_WithPartialConfiguration_ShouldKeepDefaults()
    {
        var options = new BzMarqueeOptions { Speed = 80 };

        options.Direction.Should().Be("left");
        options.Gap.Should().Be(40);
        options.PauseOnHover.Should().BeTrue();
        options.ShowGradientEdges.Should().BeTrue();
        options.FullWidth.Should().BeFalse();
        options.Reverse.Should().BeFalse();
        options.StaggerEntrance.Should().BeTrue();
        options.StaggerDelay.Should().Be(60);
    }

    #endregion

    #region Property Independence Tests

    [Fact]
    public void Speed_DoesNotAffectGap()
    {
        var options = new BzMarqueeOptions { Speed = 100 };
        options.Gap.Should().Be(40);
    }

    [Fact]
    public void Direction_DoesNotAffectReverse()
    {
        var options = new BzMarqueeOptions { Direction = "right" };
        options.Reverse.Should().BeFalse();
    }

    [Fact]
    public void StaggerEntrance_DoesNotAffectStaggerDelay()
    {
        var options = new BzMarqueeOptions { StaggerEntrance = false };
        options.StaggerDelay.Should().Be(60);
    }

    [Fact]
    public void FullWidth_DoesNotAffectShowGradientEdges()
    {
        var options = new BzMarqueeOptions { FullWidth = true };
        options.ShowGradientEdges.Should().BeTrue();
    }

    #endregion

    #region JSON Serialization Tests

    [Fact]
    public void Options_ShouldSerializeToCamelCase()
    {
        var options = new BzMarqueeOptions { Direction = "right", Speed = 80 };
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var json = JsonSerializer.Serialize(options, jsonOptions);

        json.Should().Contain("\"direction\"");
        json.Should().Contain("\"speed\"");
        json.Should().Contain("\"gap\"");
        json.Should().Contain("\"pauseOnHover\"");
        json.Should().Contain("\"showGradientEdges\"");
        json.Should().Contain("\"fullWidth\"");
        json.Should().Contain("\"reverse\"");
        json.Should().Contain("\"staggerEntrance\"");
        json.Should().Contain("\"staggerDelay\"");
    }

    [Fact]
    public void Options_ShouldRoundTripThroughJson()
    {
        var original = new BzMarqueeOptions
        {
            Direction = "right",
            Speed = 80,
            Gap = 24,
            PauseOnHover = false,
            FullWidth = true,
            StaggerDelay = 100
        };

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(original, jsonOptions);
        var deserialized = JsonSerializer.Deserialize<BzMarqueeOptions>(json, jsonOptions);

        deserialized.Should().NotBeNull();
        deserialized!.Direction.Should().Be("right");
        deserialized.Speed.Should().Be(80);
        deserialized.Gap.Should().Be(24);
        deserialized.PauseOnHover.Should().BeFalse();
        deserialized.FullWidth.Should().BeTrue();
        deserialized.StaggerDelay.Should().Be(100);
    }

    [Fact]
    public void Options_DefaultsShouldSerializeCorrectly()
    {
        var options = new BzMarqueeOptions();
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var json = JsonSerializer.Serialize(options, jsonOptions);

        json.Should().Contain("\"direction\":\"left\"");
        json.Should().Contain("\"speed\":50");
        json.Should().Contain("\"gap\":40");
        json.Should().Contain("\"pauseOnHover\":true");
        json.Should().Contain("\"showGradientEdges\":true");
        json.Should().Contain("\"fullWidth\":false");
        json.Should().Contain("\"reverse\":false");
        json.Should().Contain("\"staggerEntrance\":true");
        json.Should().Contain("\"staggerDelay\":60");
    }

    #endregion
}
