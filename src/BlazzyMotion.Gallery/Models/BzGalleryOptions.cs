using System.Text.Json.Serialization;

namespace BlazzyMotion.Gallery.Models;

/// <summary>
/// Options passed to JavaScript for gallery initialization.
/// </summary>
public sealed class BzGalleryOptions
{
    [JsonPropertyName("layout")]
    public string Layout { get; set; } = "grid";

    [JsonPropertyName("columns")]
    public int Columns { get; set; } = 3;

    [JsonPropertyName("gap")]
    public int Gap { get; set; } = 16;

    [JsonPropertyName("enableLightbox")]
    public bool EnableLightbox { get; set; } = true;

    [JsonPropertyName("animationEnabled")]
    public bool AnimationEnabled { get; set; } = true;

    [JsonPropertyName("staggerDelay")]
    public int StaggerDelay { get; set; } = 50;

    [JsonPropertyName("aspectRatio")]
    public string? AspectRatio { get; set; }
}
