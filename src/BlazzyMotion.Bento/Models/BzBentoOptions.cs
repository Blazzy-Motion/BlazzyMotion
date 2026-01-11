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
///     Paginated = true,
///     ItemsPerPage = 6
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

    #region Pagination Options

    /// <summary>
    /// Enable paginated mode with swipeable pages.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When enabled:
    /// <list type="bullet">
    /// <item>Items are grouped into pages</item>
    /// <item>Swiper.js handles page transitions</item>
    /// <item>Pagination dots are shown</item>
    /// <item>Touch/swipe gestures supported on mobile</item>
    /// </list>
    /// </para>
    /// Default: false
    /// </remarks>
    public bool Paginated { get; set; } = false;

    /// <summary>
    /// Number of items per page when paginated.
    /// </summary>
    /// <remarks>
    /// Only applies when <see cref="Paginated"/> is true.
    /// Items are automatically distributed across pages.
    /// Default: 6
    /// </remarks>
    public int ItemsPerPage { get; set; } = 6;

    /// <summary>
    /// Use dynamic bullets for pagination (shrink inactive bullets).
    /// </summary>
    /// <remarks>
    /// Useful when there are many pages. Shows only a few
    /// bullets at a time with smooth transitions.
    /// Default: false
    /// </remarks>
    public bool DynamicBullets { get; set; } = false;

    /// <summary>
    /// Transition speed between pages in milliseconds.
    /// </summary>
    /// <remarks>
    /// Only applies when <see cref="Paginated"/> is true.
    /// Default: 300
    /// </remarks>
    public int Speed { get; set; } = 300;

    #endregion

    #region Touch Options (for paginated mode)

    /// <summary>
    /// Touch sensitivity multiplier (0.1 - 2.0).
    /// </summary>
    /// <remarks>
    /// Only applies when <see cref="Paginated"/> is true.
    /// Higher values = more sensitive to touch.
    /// Default: 1.0
    /// </remarks>
    public double TouchRatio { get; set; } = 1.0;

    /// <summary>
    /// Minimum pixels to trigger swipe.
    /// </summary>
    /// <remarks>
    /// Only applies when <see cref="Paginated"/> is true.
    /// Higher values require more deliberate swipe gesture.
    /// Default: 10
    /// </remarks>
    public int Threshold { get; set; } = 10;

    /// <summary>
    /// Allow quick flick gestures.
    /// </summary>
    /// <remarks>
    /// Only applies when <see cref="Paginated"/> is true.
    /// When false, prevents glitchy behavior from rapid swipes.
    /// Default: false
    /// </remarks>
    public bool ShortSwipes { get; set; } = false;

    /// <summary>
    /// Edge resistance ratio (0 - 1).
    /// </summary>
    /// <remarks>
    /// Only applies when <see cref="Paginated"/> is true.
    /// Controls bounce resistance at grid edges.
    /// Default: 0.85
    /// </remarks>
    public double ResistanceRatio { get; set; } = 0.85;

    /// <summary>
    /// Percentage of slide width to trigger page advance (0.1 - 0.9).
    /// </summary>
    /// <remarks>
    /// Only applies when <see cref="Paginated"/> is true.
    /// How far you must swipe to move to next page.
    /// Default: 0.3
    /// </remarks>
    public double LongSwipesRatio { get; set; } = 0.3;

    #endregion
}
