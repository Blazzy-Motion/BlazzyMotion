using BlazzyMotion.Core.Attributes;

namespace BlazzyMotion.Tests.Helpers;

public class TestMarqueeItem
{
    [BzImage]
    public string? ImageUrl { get; set; }

    [BzTitle]
    public string? Name { get; set; }

    [BzDescription]
    public string? Quote { get; set; }

    public int Id { get; set; }
}
