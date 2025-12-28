namespace BlazzyMotion.Demo.Models;

/// <summary>
/// Represents a BlazzyMotion component in the documentation portal.
/// </summary>
public class ComponentInfo
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Icon { get; init; }
    public required string Version { get; init; }
    public required ComponentStatus Status { get; init; }
    public required string Description { get; init; }
    public string? DocsUrl { get; init; }
    public string? NuGetPackage { get; init; }
    
    public string StatusBadge => Status switch
    {
        ComponentStatus.Stable => "stable",
        ComponentStatus.Beta => "beta",
        ComponentStatus.Alpha => "alpha",
        ComponentStatus.ComingSoon => "soon",
        _ => ""
    };
    
    public bool IsAvailable => Status != ComponentStatus.ComingSoon;
}

public enum ComponentStatus
{
    Stable,
    Beta,
    Alpha,
    ComingSoon
}
