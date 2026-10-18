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
            PreviewImage = "images/previews/carousel.jpg",
            PreviewWidth = 800,
            PreviewHeight = 451,
            Name = "Carousel",
            Icon = Icons.Icons.Carousel,
            Version = AppVersions.Carousel,
            Status = AppVersions.IsPrerelease(AppVersions.Carousel) ? ComponentStatus.Preview : ComponentStatus.Stable,
            Description = "3D coverflow carousel with glassmorphism design",
            DocsUrl = "/docs/carousel",
            NuGetPackage = "BlazzyMotion.Carousel"
        },
        new()
        {
            Id = "bento",
            PreviewImage = "images/previews/bento.jpg",
            PreviewWidth = 960,
            PreviewHeight = 615,
            Name = "Bento Grid",
            Icon = Icons.Icons.BentoGrid,
            Version = AppVersions.Bento,
            Status = AppVersions.IsPrerelease(AppVersions.Bento) ? ComponentStatus.Preview : ComponentStatus.Stable,
            Description = "Responsive bento grid layout component",
            DocsUrl = "/docs/bento",
            NuGetPackage = "BlazzyMotion.Bento"
        },
        new()
        {
            Id = "gallery",
            PreviewImage = "images/previews/gallery.jpg",
            PreviewWidth = 850,
            PreviewHeight = 646,
            Name = "Gallery",
            Icon = Icons.Icons.Image,
            Version = AppVersions.Gallery,
            Status = AppVersions.IsPrerelease(AppVersions.Gallery) ? ComponentStatus.Preview : ComponentStatus.Stable,
            Description = "Image gallery with Grid, Masonry, and List layouts",
            DocsUrl = "/docs/gallery",
            NuGetPackage = "BlazzyMotion.Gallery"
        },
        new()
        {
            Id = "marquee",
            PreviewImage = "images/previews/marquee.jpg",
            PreviewWidth = 800,
            PreviewHeight = 202,
            Name = "Marquee",
            Icon = Icons.Icons.Marquee,
            Version = AppVersions.Marquee,
            Status = AppVersions.IsPrerelease(AppVersions.Marquee) ? ComponentStatus.Preview : ComponentStatus.Stable,
            Description = "Infinite scrolling content with CSS-driven animations",
            DocsUrl = "/docs/marquee",
            NuGetPackage = "BlazzyMotion.Marquee"
        }
    };
    
    public static ComponentInfo? GetComponent(string id) => 
        Components.FirstOrDefault(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    
    public static IEnumerable<ComponentInfo> GetAvailableComponents() => 
        Components.Where(c => c.IsAvailable);
    
    public static IEnumerable<ComponentInfo> GetComingSoonComponents() => 
        Components.Where(c => c.Status == ComponentStatus.ComingSoon);
}
