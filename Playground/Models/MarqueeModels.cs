using BlazzyMotion.Core.Attributes;

namespace Playground.Models;

public class MarqueeBrand
{
    [BzImage]
    public string ImageUrl { get; set; } = "";

    [BzTitle]
    public string Name { get; set; } = "";
}

public class MarqueeTestimonial
{
    [BzImage]
    public string Avatar { get; set; } = "";

    [BzTitle]
    public string Name { get; set; } = "";

    [BzDescription]
    public string Quote { get; set; } = "";
}
