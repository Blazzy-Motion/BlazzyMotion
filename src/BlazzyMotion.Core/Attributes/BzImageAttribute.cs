namespace BlazzyMotion.Core.Attributes;

/// <summary>
/// Marks a property as the image source for BlazzyMotion components.
/// </summary>
/// <remarks>
/// <para>
/// The Source Generator will automatically create mapping code using this property.
/// Apply this attribute to a string property that contains the image URL/path.
/// </para>
/// <para>
/// <strong>Requirements:</strong>
/// <list type="bullet">
/// <item>Property must be <c>public</c></item>
/// <item>Property must be of type <c>string</c></item>
/// <item>Only one property per class should have this attribute</item>
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
/// }
/// 
/// // Usage in component - no ItemTemplate needed!
/// &lt;BzCarousel Items="movies" /&gt;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class BzImageAttribute : Attribute
{
    // Empty - marker attribute only
    // Source Generator will scan for this at compile-time
}