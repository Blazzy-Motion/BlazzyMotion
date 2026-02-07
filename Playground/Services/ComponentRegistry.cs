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
            }
        };

        return _components;
    }

    public static ComponentMetadata? GetComponentById(string id)
    {
        return GetAllComponents().FirstOrDefault(c => c.Id == id);
    }
}
