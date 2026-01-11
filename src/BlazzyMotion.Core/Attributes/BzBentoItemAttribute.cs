namespace BlazzyMotion.Core.Attributes;

/// <summary>
/// Specifies layout properties for an item in a BzBento grid.
/// Used by Source Generator to automatically configure grid positioning.
/// </summary>
/// <remarks>
/// <para>
/// This attribute enables zero-configuration Bento Grid layouts.
/// Simply decorate a property in your model class, and the Source Generator
/// will automatically create the appropriate grid positioning.
/// </para>
/// <para>
/// <strong>Usage:</strong>
/// <code>
/// public class DashboardItem
/// {
///     [BzImage]
///     public string Icon { get; set; }
///
///     [BzTitle]
///     public string Label { get; set; }
///
///     [BzBentoItem(ColSpan = 2, RowSpan = 2)]
///     public bool IsFeatureCard { get; set; }
/// }
///
/// // Then in Razor - just one line!
/// &lt;BzBento Items="items" Theme="BzTheme.Glass" /&gt;
/// </code>
/// </para>
/// <para>
/// <strong>Note:</strong> This attribute only affects BzBento component.
/// Other components like BzCarousel will ignore these layout properties.
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class BzBentoItemAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the number of columns this item should span in the grid.
    /// </summary>
    /// <remarks>
    /// Valid values are 1-4. Values greater than 4 will be clamped to 4.
    /// On tablet devices, values 3-4 are automatically reduced to 2.
    /// On mobile devices, all items become single column.
    /// Default: 1
    /// </remarks>
    public int ColSpan { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of rows this item should span in the grid.
    /// </summary>
    /// <remarks>
    /// Valid values are 1-4. Values greater than 4 will be clamped to 4.
    /// On mobile devices, row spans are reset to 1 for better layout.
    /// Default: 1
    /// </remarks>
    public int RowSpan { get; set; } = 1;

    /// <summary>
    /// Gets or sets the display order of this item in the grid.
    /// </summary>
    /// <remarks>
    /// Lower values appear first. Items with the same order value
    /// maintain their original sequence from the data source.
    /// Default: 0
    /// </remarks>
    public int Order { get; set; } = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="BzBentoItemAttribute"/> class
    /// with default values (ColSpan=1, RowSpan=1, Order=0).
    /// </summary>
    public BzBentoItemAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BzBentoItemAttribute"/> class
    /// with specified column and row spans.
    /// </summary>
    /// <param name="colSpan">Number of columns to span (1-4)</param>
    /// <param name="rowSpan">Number of rows to span (1-4)</param>
    public BzBentoItemAttribute(int colSpan, int rowSpan)
    {
        ColSpan = colSpan;
        RowSpan = rowSpan;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BzBentoItemAttribute"/> class
    /// with specified column span, row span, and order.
    /// </summary>
    /// <param name="colSpan">Number of columns to span (1-4)</param>
    /// <param name="rowSpan">Number of rows to span (1-4)</param>
    /// <param name="order">Display order (lower values first)</param>
    public BzBentoItemAttribute(int colSpan, int rowSpan, int order)
    {
        ColSpan = colSpan;
        RowSpan = rowSpan;
        Order = order;
    }
}
