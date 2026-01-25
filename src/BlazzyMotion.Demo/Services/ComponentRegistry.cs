using BlazzyMotion.Demo.Icons;
using BlazzyMotion.Demo.Models;

namespace BlazzyMotion.Demo.Services;

/// <summary>
/// Registry of all BlazzyMotion components.
/// Add new components here when they become available.
/// </summary>
public static class ComponentRegistry
{
    public static IReadOnlyList<ComponentInfo> Components { get; } = new List<ComponentInfo>
    {
        new()
        {
            Id = "carousel",
            Name = "Carousel",
            Icon = Icons.Icons.Carousel,
            Version = AppVersions.Carousel,
            Status = ComponentStatus.Stable,
            Description = "3D coverflow carousel with glassmorphism design",
            DocsUrl = "/docs/carousel",
            NuGetPackage = "BlazzyMotion.Carousel"
        },
        new()
        {
            Id = "bento",
            Name = "Bento Grid",
            Icon = Icons.Icons.BentoGrid,
            Version = AppVersions.Bento,
            Status = ComponentStatus.Stable,
            Description = "Responsive bento grid layout component",
            DocsUrl = "/docs/bento",
            NuGetPackage = "BlazzyMotion.Bento"
        }
    };
    
    public static ComponentInfo? GetComponent(string id) => 
        Components.FirstOrDefault(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    
    public static IEnumerable<ComponentInfo> GetAvailableComponents() => 
        Components.Where(c => c.IsAvailable);
    
    public static IEnumerable<ComponentInfo> GetComingSoonComponents() => 
        Components.Where(c => c.Status == ComponentStatus.ComingSoon);
}
