using BlazzyMotion.Core.Attributes;

namespace Playground.Models;

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
