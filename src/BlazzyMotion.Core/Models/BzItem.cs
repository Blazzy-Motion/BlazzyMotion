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
/// [BzBentoItem] Layout   →    ColSpan, RowSpan, Order
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
    #region Content Properties

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

    #endregion

    #region Layout Properties (Bento Grid)

    /// <summary>
    /// Number of columns this item spans in a Bento Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is populated from the [BzBentoItem] attribute by the Source Generator.
    /// </para>
    /// <para>
    /// <strong>Component Support:</strong>
    /// <list type="bullet">
    /// <item><strong>BzBento:</strong> Uses this value for CSS Grid column span</item>
    /// <item><strong>BzCarousel:</strong> Ignores this property</item>
    /// <item><strong>BzGallery:</strong> Ignores this property</item>
    /// </list>
    /// </para>
    /// <para>
    /// Valid values are 1-4. Values greater than 4 will be clamped.
    /// On tablet devices (≤991px), values 3-4 are reduced to 2.
    /// On mobile devices (≤600px), all items become single column.
    /// </para>
    /// Default: 1
    /// </remarks>
    public int ColSpan { get; set; } = 1;

    /// <summary>
    /// Number of rows this item spans in a Bento Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is populated from the [BzBentoItem] attribute by the Source Generator.
    /// </para>
    /// <para>
    /// <strong>Component Support:</strong>
    /// <list type="bullet">
    /// <item><strong>BzBento:</strong> Uses this value for CSS Grid row span</item>
    /// <item><strong>BzCarousel:</strong> Ignores this property</item>
    /// <item><strong>BzGallery:</strong> Ignores this property</item>
    /// </list>
    /// </para>
    /// <para>
    /// Valid values are 1-4. Values greater than 4 will be clamped.
    /// On mobile devices (≤600px), row spans are reset to 1.
    /// </para>
    /// Default: 1
    /// </remarks>
    public int RowSpan { get; set; } = 1;

    /// <summary>
    /// Display order of this item in a Bento Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is populated from the [BzBentoItem] attribute by the Source Generator.
    /// </para>
    /// <para>
    /// <strong>Component Support:</strong>
    /// <list type="bullet">
    /// <item><strong>BzBento:</strong> Uses this value for CSS order property</item>
    /// <item><strong>BzCarousel:</strong> Ignores this property</item>
    /// <item><strong>BzGallery:</strong> Ignores this property</item>
    /// </list>
    /// </para>
    /// <para>
    /// Lower values appear first. Items with the same order value
    /// maintain their original sequence from the data source.
    /// </para>
    /// Default: 0
    /// </remarks>
    public int Order { get; set; } = 0;

    #endregion

    #region Helper Methods

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

    #endregion

    #region Computed Properties

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

    /// <summary>
    /// Checks if this item has custom layout (non-default ColSpan or RowSpan).
    /// </summary>
    /// <remarks>
    /// Useful for components to detect if special grid layout handling is needed.
    /// </remarks>
    public bool HasCustomLayout => ColSpan > 1 || RowSpan > 1 || Order != 0;

    /// <summary>
    /// Gets the clamped column span value (1-4).
    /// </summary>
    public int ClampedColSpan => Math.Clamp(ColSpan, 1, 4);

    /// <summary>
    /// Gets the clamped row span value (1-4).
    /// </summary>
    public int ClampedRowSpan => Math.Clamp(RowSpan, 1, 4);

    #endregion
}
