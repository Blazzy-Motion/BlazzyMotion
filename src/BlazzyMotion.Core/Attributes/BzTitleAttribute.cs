namespace BlazzyMotion.Core.Attributes;

/// <summary>
/// Marks a property as the title/alt text for BlazzyMotion components.
/// </summary>
/// <remarks>
/// <para>
/// Used for accessibility - generates alt and title attributes on img elements.
/// This attribute is optional. If not provided, components will use generic alt text.
/// </para>
/// <para>
/// <strong>Generated HTML:</strong>
/// <code>
/// &lt;img src="..." alt="{TitleProperty}" title="{TitleProperty}" /&gt;
/// </code>
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class Movie
/// {
///     [BzImage]
///     public string PosterUrl { get; set; } = "";
///     
///     [BzTitle]  // ← This will be used for alt="..." and title="..."
///     public string Title { get; set; } = "";
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class BzTitleAttribute : Attribute
{
    // Marker attribute for alt/title text
}