using BlazzyMotion.Core.Attributes;

namespace BlazzyMotion.Demo.Model;

public class LogoItem
{
    [BzImage]
    public string ImageUrl { get; set; } = "";

    [BzTitle]
    public string Name { get; set; } = "";
}
