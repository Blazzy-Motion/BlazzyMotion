using BlazzyMotion.Carousel.Attributes;

namespace BlazzyMotion.Demo.Model;

public class Movie
{
    [BzTitle]
    public string? Title { get; set; }

    [BzImage]
    public string? ImageUrl { get; set; }
}
