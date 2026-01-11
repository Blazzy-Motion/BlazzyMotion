using BlazzyMotion.Core.Attributes;

namespace BlazzyMotion.Demo.Model;

/// <summary>
/// Demo model for BzBento component.
/// </summary>
public class BentoItem
{
    [BzImage]
    public string ImageUrl { get; set; } = "";

    [BzTitle]
    public string Title { get; set; } = "";
}
