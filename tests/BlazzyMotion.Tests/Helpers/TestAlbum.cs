namespace BlazzyMotion.Tests.Helpers;

/// <summary>
/// Test model for album items with BzImage attribute for testing
/// </summary>
public class TestAlbum
{
  [BzImage]
  public string? ArtworkUrl { get; set; }

  [BzTitle]
  public string? Name { get; set; }

  [BzDescription]
  public string? Artist { get; set; }

  public int Year { get; set; }

  public int TrackCount { get; set; }
}
