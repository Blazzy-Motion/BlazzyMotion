using BlazzyMotion.Core.Attributes;

namespace BlazzyMotion.Tests.Helpers;

/// <summary>
/// Test model for Gallery tests.
/// </summary>
public class TestGalleryPhoto
{
    [BzImage]
    public string? ImageUrl { get; set; }

    [BzTitle]
    public string? Title { get; set; }

    [BzDescription]
    public string? Description { get; set; }

    public int Id { get; set; }
    public string? Category { get; set; }
}
