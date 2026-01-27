using Microsoft.AspNetCore.Components;

namespace Playground.Models;

/// <summary>
/// Metadata about a BlazzyMotion component available for testing
/// </summary>
public class ComponentMetadata
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public MarkupString Icon { get; set; }
    public List<ComponentParameter> Parameters { get; set; } = new();
    public string ComponentTypeName { get; set; } = "";
}

/// <summary>
/// Metadata about a component parameter
/// </summary>
public class ComponentParameter
{
    public string Name { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public ParameterType Type { get; set; }
    public object? DefaultValue { get; set; }
    public object? MinValue { get; set; }
    public object? MaxValue { get; set; }
    public List<ParameterOption>? Options { get; set; }
    public string Description { get; set; } = "";
    public string? Unit { get; set; }
}

/// <summary>
/// Option for enum/select parameters
/// </summary>
public class ParameterOption
{
    public string Label { get; set; } = "";
    public string Value { get; set; } = "";
}

/// <summary>
/// Type of component parameter
/// </summary>
public enum ParameterType
{
    Range,      // int/double with min/max (e.g., RotateDegree, Depth)
    Boolean,    // checkbox (e.g., ShowOverlay, Loop)
    Select,     // dropdown (e.g., Theme)
    Text,       // text input (e.g., custom strings)
    Number      // numeric input without slider
}
