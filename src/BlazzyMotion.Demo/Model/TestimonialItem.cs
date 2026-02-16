using BlazzyMotion.Core.Attributes;

namespace BlazzyMotion.Demo.Model;

public class TestimonialItem
{
    [BzImage]
    public string Avatar { get; set; } = "";

    [BzTitle]
    public string Name { get; set; } = "";

    [BzDescription]
    public string Quote { get; set; } = "";
}
