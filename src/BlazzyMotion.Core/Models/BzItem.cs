namespace BlazzyMotion.Core.Models;

/// <summary>
/// Universal data model for BlazzyMotion components.
/// </summary>
/// <remarks>
/// <para>
/// This class serves as the normalized data structure that all BlazzyMotion
/// components work with internally. User models decorated with [BzImage] and
/// [BzTitle] attributes are automatically mapped to this type at runtime.
/// </para>
/// <para>
/// <strong>Mapping Flow:</strong>
/// <code>
/// User Model (Movie)     →    BzItem
/// ─────────────────────────────────────
/// [BzImage] PosterUrl    →    ImageUrl
/// [BzTitle] Title        →    Title
/// [BzDescription] Plot   →    Description
/// (original object)      →    OriginalItem
/// </code>
/// </para>
/// <para>
/// <strong>Performance:</strong>
/// Mapping functions are generated at compile-time by the Source Generator
/// and cached in BzRegistry. Runtime overhead is minimal (single dictionary lookup).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // User's model
/// public class Movie
/// {
///     [BzImage] public string PosterUrl { get; set; }
///     [BzTitle] public string Title { get; set; }
/// }
/// 
/// // Automatic mapping (done by BzRegistry)
/// var bzItems = BzRegistry.ToBzItems(movies);
/// // Each BzItem now has ImageUrl, Title, and OriginalItem set
/// </code>
/// </example>
public sealed class BzItem
{
    /// <summary>
    /// The image URL/path from the [BzImage] property.
    /// </summary>
    /// <remarks>
    /// This is the primary visual content for carousel slides, gallery items, etc.
    /// Should be a valid URL or relative path to an image resource.
    /// </remarks>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// The title/alt text from the [BzTitle] property.
    /// </summary>
    /// <remarks>
    /// Used for accessibility (alt attribute) and optional display.
    /// If null, components will use a generic fallback.
    /// </remarks>
    public string? Title { get; set; }

    /// <summary>
    /// The description from the [BzDescription] property.
    /// </summary>
    /// <remarks>
    /// Reserved for future use. Will be used for tooltips, captions,
    /// and aria-describedby attributes.
    /// </remarks>
    public string? Description { get; set; }

    /// <summary>
    /// Reference to the original user object.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Preserves access to the complete original object for:
    /// <list type="bullet">
    /// <item>Custom templates that need additional properties</item>
    /// <item>Event handlers (OnItemSelected returns original type)</item>
    /// <item>Data binding scenarios</item>
    /// </list>
    /// </para>
    /// <para>
    /// Use <see cref="GetOriginal{T}"/> for type-safe access.
    /// </para>
    /// </remarks>
    public object? OriginalItem { get; set; }

    /// <summary>
    /// Gets the original item cast to the specified type.
    /// </summary>
    /// <typeparam name="T">The expected type of the original item</typeparam>
    /// <returns>The original item cast to type T, or default if null/invalid</returns>
    /// <example>
    /// <code>
    /// var movie = bzItem.GetOriginal&lt;Movie&gt;();
    /// if (movie != null)
    /// {
    ///     Console.WriteLine(movie.Director);
    /// }
    /// </code>
    /// </example>
    public T? GetOriginal<T>() where T : class
    {
        return OriginalItem as T;
    }

    /// <summary>
    /// Checks if the item has valid image data.
    /// </summary>
    /// <returns>True if ImageUrl is not null or whitespace</returns>
    public bool HasImage => !string.IsNullOrWhiteSpace(ImageUrl);

    /// <summary>
    /// Checks if the item has a title.
    /// </summary>
    /// <returns>True if Title is not null or whitespace</returns>
    public bool HasTitle => !string.IsNullOrWhiteSpace(Title);

    /// <summary>
    /// Gets the display title, with fallback to "Item" if not set.
    /// </summary>
    public string DisplayTitle => HasTitle ? Title! : "Item";
}