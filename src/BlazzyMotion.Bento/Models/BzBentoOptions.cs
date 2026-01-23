namespace BlazzyMotion.Bento.Models;

/// <summary>
/// Configuration options for the BzBento component.
/// </summary>
/// <remarks>
/// <para>
/// Provides advanced configuration for the Bento Grid component.
/// For basic usage, individual component parameters are sufficient.
/// Use this class for complex configurations or programmatic setup.
/// </para>
/// <para>
/// <strong>Usage:</strong>
/// <code>
/// var options = new BzBentoOptions
/// {
///     Columns = 3,
///     Gap = 20,
///     AnimationEnabled = true,
///     StaggerDelay = 75
/// };
///
/// &lt;BzBento Items="items" Options="options" /&gt;
/// </code>
/// </para>
/// </remarks>
public class BzBentoOptions
{
    #region Layout Options

    /// <summary>
    /// Number of columns in the grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Responsive behavior:
    /// <list type="bullet">
    /// <item>Desktop: Uses this value</item>
    /// <item>Tablet (≤991px): Reduced to 2 columns</item>
    /// <item>Mobile (≤600px): Forced to 1 column</item>
    /// </list>
    /// </para>
    /// Default: 4
    /// </remarks>
    public int Columns { get; set; } = 4;

    /// <summary>
    /// Gap between grid items in pixels.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Responsive behavior:
    /// <list type="bullet">
    /// <item>Desktop: Uses this value</item>
    /// <item>Tablet (≤991px): 12px</item>
    /// <item>Mobile (≤600px): 8px</item>
    /// </list>
    /// </para>
    /// Default: 16
    /// </remarks>
    public int Gap { get; set; } = 16;

    #endregion

    #region Animation Options

    /// <summary>
    /// Enable staggered entrance animations.
    /// </summary>
    /// <remarks>
    /// When enabled, items fade in one by one with a delay.
    /// Uses Intersection Observer for performance.
    /// Default: true
    /// </remarks>
    public bool AnimationEnabled { get; set; } = true;

    /// <summary>
    /// Delay between each item's animation in milliseconds.
    /// </summary>
    /// <remarks>
    /// Only applies when <see cref="AnimationEnabled"/> is true.
    /// Recommended range: 30-100ms.
    /// Default: 50
    /// </remarks>
    public int StaggerDelay { get; set; } = 50;

    #endregion
}
