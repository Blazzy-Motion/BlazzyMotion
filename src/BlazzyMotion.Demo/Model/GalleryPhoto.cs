using BlazzyMotion.Core.Attributes;

namespace BlazzyMotion.Demo.Model;

/// <summary>
/// Demo model for BzGallery component.
/// </summary>
public class GalleryPhoto
{
    [BzImage]
    public string ImageUrl { get; set; } = "";

    [BzTitle]
    public string Title { get; set; } = "";

    [BzDescription]
    public string Description { get; set; } = "";

    public string Category { get; set; } = "";
}
