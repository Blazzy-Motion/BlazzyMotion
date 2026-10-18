using System.Globalization;
using BlazzyMotion.Demo.Services;

namespace BlazzyMotion.Demo.Pages;

/// <summary>Provides copyable consumer examples matching the live Marquee controls and data.</summary>
public partial class MarqueeDemo
{
    /// <summary>Gets the logo example matching the current Marquee controls.</summary>
    private string LogoCode => $$"""
        @using BlazzyMotion.Marquee.Components
        @using BlazzyMotion.Marquee.Models
        @using BlazzyMotion.Core.Models
        @using YourApp.Models

        <BzMarquee TItem="LogoItem"
                   Items="@_logos"
                   Theme="BzTheme.{{_currentTheme}}"
                   Direction="BzDirection.{{_direction}}"
                   Speed="{{_speed}}"
                   Gap="{{_gap}}"
                   Rows="{{_rows}}"
                   AlternateDirection="true"
                   SpeedVariation="{{_speedVariation.ToString(CultureInfo.InvariantCulture)}}"
                   PauseOnHover="{{_pauseOnHover.ToString().ToLowerInvariant()}}"
                   ShowGradientEdges="{{_showGradients.ToString().ToLowerInvariant()}}"
                   FullWidth="{{_fullWidth.ToString().ToLowerInvariant()}}"
                   StaggerEntrance="{{_staggerEntrance.ToString().ToLowerInvariant()}}" />

        @code {
            private readonly List<LogoItem> _logos = new()
            {
        {{ExampleSource.Items(_logos, item => $"Name = {ExampleSource.Literal(item.Name)}, ImageUrl = {ExampleSource.Literal(item.ImageUrl, true)}")}}
            };
        }
        """;
    /// <summary>Gets the supported attribute model for this example.</summary>
    private string LogoModelCode => $$"""
        using BlazzyMotion.Core.Attributes;

        namespace YourApp.Models;

        /// <summary>Data displayed by the live LogoItem example.</summary>
        public class LogoItem
        {
            /// <summary>Gets or sets the Name value.</summary>
            [BzTitle]
            public string Name { get; set; } = "";

            /// <summary>Gets or sets the ImageUrl value.</summary>
            [BzImage]
            public string ImageUrl { get; set; } = "";
        }
        """;
    /// <summary>Gets the testimonial example matching the current Marquee controls.</summary>
    private string TestimonialCode => $$"""
        @using BlazzyMotion.Marquee.Components
        @using BlazzyMotion.Marquee.Models
        @using BlazzyMotion.Core.Models
        @using YourApp.Models

        <BzMarquee TItem="TestimonialItem"
                   Items="@_testimonials"
                   Theme="BzTheme.{{_currentTheme}}"
                   Direction="BzDirection.{{_direction}}"
                   Speed="{{_speed}}"
                   Gap="{{_gap}}"
                   PauseOnHover="{{_pauseOnHover.ToString().ToLowerInvariant()}}"
                   ShowGradientEdges="{{_showGradients.ToString().ToLowerInvariant()}}"
                   FullWidth="{{_fullWidth.ToString().ToLowerInvariant()}}"
                   StaggerEntrance="{{_staggerEntrance.ToString().ToLowerInvariant()}}" />

        @code {
            private readonly List<TestimonialItem> _testimonials = new()
            {
        {{ExampleSource.Items(_testimonials, item => $"Name = {ExampleSource.Literal(item.Name)}, Avatar = {ExampleSource.Literal(item.Avatar, true)}, Quote = {ExampleSource.Literal(item.Quote)}")}}
            };
        }
        """;
    /// <summary>Gets the supported attribute model for this example.</summary>
    private string TestimonialModelCode => $$"""
        using BlazzyMotion.Core.Attributes;

        namespace YourApp.Models;

        /// <summary>Data displayed by the live TestimonialItem example.</summary>
        public class TestimonialItem
        {
            /// <summary>Gets or sets the Name value.</summary>
            [BzTitle]
            public string Name { get; set; } = "";

            /// <summary>Gets or sets the Avatar value.</summary>
            [BzImage]
            public string Avatar { get; set; } = "";

            /// <summary>Gets or sets the Quote value.</summary>
            [BzDescription]
            public string Quote { get; set; } = "";
        }
        """;
    /// <summary>Gets the ticker example matching the current Marquee controls.</summary>
    private string TickerCode => $$"""
        @using BlazzyMotion.Marquee.Components
        @using BlazzyMotion.Marquee.Models
        @using BlazzyMotion.Core.Models

        <BzMarquee TItem="object"
                   Text="@_tickerText"
                   Theme="BzTheme.{{_currentTheme}}"
                   Direction="BzDirection.{{_direction}}"
                   Speed="{{_speed}}"
                   Gap="{{_gap}}"
                   PauseOnHover="{{_pauseOnHover.ToString().ToLowerInvariant()}}"
                   ShowGradientEdges="{{_showGradients.ToString().ToLowerInvariant()}}"
                   FullWidth="{{_fullWidth.ToString().ToLowerInvariant()}}"
                   StaggerEntrance="{{_staggerEntrance.ToString().ToLowerInvariant()}}" />

        @code {
            private string _tickerText = {{ExampleSource.Literal(_tickerText)}};
        }
        """;
}
