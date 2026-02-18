using BlazzyMotion.Core.Models;
using Playground.Models;

namespace Playground.Services;

/// <summary>
/// Central registry for all BlazzyMotion components available for testing
/// </summary>
public static class ComponentRegistry
{
    private static List<ComponentMetadata>? _components;

    public static List<ComponentMetadata> GetAllComponents()
    {
        if (_components != null)
            return _components;

        _components = new List<ComponentMetadata>
        {
            // BzCarousel Component
            new ComponentMetadata
            {
                Id = "carousel",
                Name = "BzCarousel",
                Description = "3D carousel component with smooth animations",
                Icon = Icons.Carousel,
                ComponentTypeName = "BzCarousel",
                Parameters = new List<ComponentParameter>
                {
                    new ComponentParameter
                    {
                        Name = "Theme",
                        DisplayName = "Theme",
                        Type = ParameterType.Select,
                        DefaultValue = BzTheme.Glass,
                        Description = "Visual theme of the carousel",
                        Options = new List<ParameterOption>
                        {
                            new() { Label = "Glass", Value = "Glass" },
                            new() { Label = "Dark", Value = "Dark" },
                            new() { Label = "Light", Value = "Light" },
                            new() { Label = "Minimal", Value = "Minimal" }
                        }
                    },
                    new ComponentParameter
                    {
                        Name = "RotateDegree",
                        DisplayName = "Rotate Degree",
                        Type = ParameterType.Range,
                        DefaultValue = 50,
                        MinValue = 0,
                        MaxValue = 180,
                        Description = "Rotation angle of side items",
                        Unit = "°"
                    },
                    new ComponentParameter
                    {
                        Name = "Depth",
                        DisplayName = "Depth",
                        Type = ParameterType.Range,
                        DefaultValue = 150,
                        MinValue = 0,
                        MaxValue = 500,
                        Description = "3D depth perspective",
                        Unit = "px"
                    },
                    new ComponentParameter
                    {
                        Name = "ShowOverlay",
                        DisplayName = "Show Overlay",
                        Type = ParameterType.Boolean,
                        DefaultValue = true,
                        Description = "Display overlay on items"
                    },
                    new ComponentParameter
                    {
                        Name = "Loop",
                        DisplayName = "Loop Carousel",
                        Type = ParameterType.Boolean,
                        DefaultValue = true,
                        Description = "Enable infinite looping"
                    },
                    new ComponentParameter
                    {
                        Name = "Width",
                        DisplayName = "Width",
                        Type = ParameterType.Text,
                        DefaultValue = "",
                        Description = "Maximum width (e.g., 800px, 80%, 50vw)"
                    },
                    new ComponentParameter
                    {
                        Name = "Height",
                        DisplayName = "Height",
                        Type = ParameterType.Text,
                        DefaultValue = "",
                        Description = "Container height (e.g., 400px, 50vh)"
                    }
                }
            },

            // BzBento Component
            new ComponentMetadata
            {
                Id = "bento",
                Name = "BzBento",
                Description = "Modern Bento Grid layout with glassmorphism design",
                Icon = Icons.BentoGrid,
                ComponentTypeName = "BzBento",
                Parameters = new List<ComponentParameter>
                {
                    new ComponentParameter
                    {
                        Name = "Theme",
                        DisplayName = "Theme",
                        Type = ParameterType.Select,
                        DefaultValue = BzTheme.Glass,
                        Description = "Visual theme of the grid",
                        Options = new List<ParameterOption>
                        {
                            new() { Label = "Glass", Value = "Glass" },
                            new() { Label = "Dark", Value = "Dark" },
                            new() { Label = "Light", Value = "Light" },
                            new() { Label = "Minimal", Value = "Minimal" }
                        }
                    },
                    new ComponentParameter
                    {
                        Name = "Columns",
                        DisplayName = "Columns",
                        Type = ParameterType.Range,
                        DefaultValue = 4,
                        MinValue = 1,
                        MaxValue = 12,
                        Description = "Number of columns in the grid"
                    },
                    new ComponentParameter
                    {
                        Name = "Gap",
                        DisplayName = "Gap",
                        Type = ParameterType.Range,
                        DefaultValue = 16,
                        MinValue = 0,
                        MaxValue = 48,
                        Description = "Space between grid items",
                        Unit = "px"
                    },
                    new ComponentParameter
                    {
                        Name = "AnimationEnabled",
                        DisplayName = "Enable Animations",
                        Type = ParameterType.Boolean,
                        DefaultValue = true,
                        Description = "Enable staggered entrance animations"
                    },
                    new ComponentParameter
                    {
                        Name = "StaggerDelay",
                        DisplayName = "Stagger Delay",
                        Type = ParameterType.Range,
                        DefaultValue = 50,
                        MinValue = 0,
                        MaxValue = 1000,
                        Description = "Delay between each item's animation",
                        Unit = "ms"
                    }
                }
            },

            // BzGallery Component
            new ComponentMetadata
            {
                Id = "gallery",
                Name = "BzGallery",
                Description = "Image gallery with Grid, Masonry, and List layouts plus lightbox and filtering",
                Icon = Icons.Image,
                ComponentTypeName = "BzGallery",
                Parameters = new List<ComponentParameter>
                {
                    new ComponentParameter
                    {
                        Name = "Theme",
                        DisplayName = "Theme",
                        Type = ParameterType.Select,
                        DefaultValue = BzTheme.Glass,
                        Description = "Visual theme of the gallery",
                        Options = new List<ParameterOption>
                        {
                            new() { Label = "Glass", Value = "Glass" },
                            new() { Label = "Dark", Value = "Dark" },
                            new() { Label = "Light", Value = "Light" },
                            new() { Label = "Minimal", Value = "Minimal" }
                        }
                    },
                    new ComponentParameter
                    {
                        Name = "Layout",
                        DisplayName = "Layout",
                        Type = ParameterType.Select,
                        DefaultValue = "Grid",
                        Description = "Gallery layout mode",
                        Options = new List<ParameterOption>
                        {
                            new() { Label = "Grid", Value = "Grid" },
                            new() { Label = "Masonry", Value = "Masonry" },
                            new() { Label = "List", Value = "List" }
                        }
                    },
                    new ComponentParameter
                    {
                        Name = "Columns",
                        DisplayName = "Columns",
                        Type = ParameterType.Range,
                        DefaultValue = 3,
                        MinValue = 1,
                        MaxValue = 6,
                        Description = "Number of columns in the grid"
                    },
                    new ComponentParameter
                    {
                        Name = "Gap",
                        DisplayName = "Gap",
                        Type = ParameterType.Range,
                        DefaultValue = 16,
                        MinValue = 0,
                        MaxValue = 48,
                        Description = "Space between gallery items",
                        Unit = "px"
                    },
                    new ComponentParameter
                    {
                        Name = "EnableLightbox",
                        DisplayName = "Enable Lightbox",
                        Type = ParameterType.Boolean,
                        DefaultValue = true,
                        Description = "Enable fullscreen lightbox on click"
                    },
                    new ComponentParameter
                    {
                        Name = "EnableFilter",
                        DisplayName = "Enable Filter",
                        Type = ParameterType.Boolean,
                        DefaultValue = true,
                        Description = "Show category filter bar"
                    },
                    new ComponentParameter
                    {
                        Name = "AnimationEnabled",
                        DisplayName = "Enable Animations",
                        Type = ParameterType.Boolean,
                        DefaultValue = true,
                        Description = "Enable staggered entry animations"
                    }
                }
            },

            // BzMarquee Component
            new ComponentMetadata
            {
                Id = "marquee",
                Name = "BzMarquee",
                Description = "Infinite scrolling marquee with logo bars, testimonials, and text tickers",
                Icon = Icons.Film,
                ComponentTypeName = "BzMarquee",
                Parameters = new List<ComponentParameter>
                {
                    new ComponentParameter
                    {
                        Name = "Mode",
                        DisplayName = "Mode",
                        Type = ParameterType.Select,
                        DefaultValue = "Logos",
                        Description = "Display mode",
                        Options = new List<ParameterOption>
                        {
                            new() { Label = "Logo Bar", Value = "Logos" },
                            new() { Label = "Testimonials", Value = "Testimonials" },
                            new() { Label = "Text Ticker", Value = "Ticker" }
                        }
                    },
                    new ComponentParameter
                    {
                        Name = "Theme",
                        DisplayName = "Theme",
                        Type = ParameterType.Select,
                        DefaultValue = BzTheme.Glass,
                        Description = "Visual theme of the marquee",
                        Options = new List<ParameterOption>
                        {
                            new() { Label = "Glass", Value = "Glass" },
                            new() { Label = "Dark", Value = "Dark" },
                            new() { Label = "Light", Value = "Light" },
                            new() { Label = "Minimal", Value = "Minimal" }
                        }
                    },
                    new ComponentParameter
                    {
                        Name = "Direction",
                        DisplayName = "Direction",
                        Type = ParameterType.Select,
                        DefaultValue = "Left",
                        Description = "Scroll direction",
                        Options = new List<ParameterOption>
                        {
                            new() { Label = "Left", Value = "Left" },
                            new() { Label = "Right", Value = "Right" }
                        }
                    },
                    new ComponentParameter
                    {
                        Name = "Speed",
                        DisplayName = "Speed",
                        Type = ParameterType.Range,
                        DefaultValue = 50,
                        MinValue = 10,
                        MaxValue = 200,
                        Description = "Animation speed in pixels per second",
                        Unit = "px/s"
                    },
                    new ComponentParameter
                    {
                        Name = "Gap",
                        DisplayName = "Gap",
                        Type = ParameterType.Range,
                        DefaultValue = 40,
                        MinValue = 0,
                        MaxValue = 120,
                        Description = "Gap between items",
                        Unit = "px"
                    },
                    new ComponentParameter
                    {
                        Name = "Rows",
                        DisplayName = "Rows",
                        Type = ParameterType.Range,
                        DefaultValue = 1,
                        MinValue = 1,
                        MaxValue = 5,
                        Description = "Number of rows"
                    },
                    new ComponentParameter
                    {
                        Name = "PauseOnHover",
                        DisplayName = "Pause on Hover",
                        Type = ParameterType.Boolean,
                        DefaultValue = true,
                        Description = "Pause animation on mouse hover"
                    },
                    new ComponentParameter
                    {
                        Name = "ShowGradientEdges",
                        DisplayName = "Gradient Edges",
                        Type = ParameterType.Boolean,
                        DefaultValue = true,
                        Description = "Show gradient fade on edges"
                    },
                    new ComponentParameter
                    {
                        Name = "AlternateDirection",
                        DisplayName = "Alternate Direction",
                        Type = ParameterType.Boolean,
                        DefaultValue = true,
                        Description = "Adjacent rows scroll in opposite directions"
                    },
                    new ComponentParameter
                    {
                        Name = "StaggerEntrance",
                        DisplayName = "Stagger Entrance",
                        Type = ParameterType.Boolean,
                        DefaultValue = true,
                        Description = "Enable staggered entrance animation"
                    }
                }
            }
        };

        return _components;
    }

    public static ComponentMetadata? GetComponentById(string id)
    {
        return GetAllComponents().FirstOrDefault(c => c.Id == id);
    }
}
