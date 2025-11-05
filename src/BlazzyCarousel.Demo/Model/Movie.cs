using BlazzyCarousel.Attributes;

namespace BlazzyCarousel.Model;

public class Movie
{
    [BzTitle]
    public string? Title { get; set; }

    [BzImage]
    public string? ImageUrl { get; set; }
}
