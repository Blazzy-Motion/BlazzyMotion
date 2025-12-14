namespace BlazzyMotion.Tests.Helpers;

/// <summary>
/// Test model for book items with BzImage attribute for testing
/// </summary>
public class TestBook
{
  [BzImage]
  public string? CoverUrl { get; set; }

  [BzTitle]
  public string? Title { get; set; }

  [BzDescription]
  public string? Summary { get; set; }

  public string? Author { get; set; }

  public int Pages { get; set; }
}
