namespace BlazzyMotion.Core.Attributes;

/// <summary>
/// Marks a property as the description for BlazzyMotion components.
/// </summary>
/// <remarks>
/// <para>
/// Currently reserved for future use. Planned features include:
/// <list type="bullet">
/// <item>Tooltips on hover</item>
/// <item>Caption overlays</item>
/// <item>aria-describedby for accessibility</item>
/// <item>Detail panels in gallery views</item>
/// </list>
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class Movie
/// {
///     [BzImage]
///     public string PosterUrl { get; set; } = "";
///     
///     [BzTitle]
///     public string Title { get; set; } = "";
///     
///     [BzDescription]  // ← Reserved for future use
///     public string Synopsis { get; set; } = "";
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class BzDescriptionAttribute : Attribute
{
    // Reserved for future use
    // Could be used for: tooltips, captions, aria-describedby, etc.
}