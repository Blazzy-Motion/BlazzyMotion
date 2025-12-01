using BlazzyMotion.Carousel.Attributes;

namespace Playground.Models;

public class Photo
{
    [BzImage]
    public string Url { get; set; } = "";

    [BzTitle]
    public string Title { get; set; } = "";
}

public class Product
{
    [BzImage]
    public string Image { get; set; } = "";

    [BzTitle]
    public string Name { get; set; } = "";

    public decimal Price { get; set; }
    public string Description { get; set; } = "";
}

public class TeamMember
{
    [BzImage]
    public string Avatar { get; set; } = "";

    [BzTitle]
    public string Name { get; set; } = "";

    public string Role { get; set; } = "";
}
