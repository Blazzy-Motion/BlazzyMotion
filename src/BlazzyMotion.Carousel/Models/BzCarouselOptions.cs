namespace BlazzyMotion.Carousel.Models;

/// <summary>
/// Configuration options for the BlazzyCarousel component.
/// </summary>
/// <remarks>
/// <para>
/// <strong>Usage:</strong>
/// <code>
/// var options = new BzCarouselOptions
/// {
///     TouchRatio = 0.8,
///     Threshold = 10,
///     ShortSwipes = false
/// };
/// 
/// &lt;BzCarousel Items="movies" Options="options" /&gt;
/// </code>
/// </para>
/// </remarks>
public class BzCarouselOptions
{
    #region Effect Options

    /// <summary>
    /// Swiper effect type: "slide", "coverflow", "fade", etc.
    /// </summary>
    public string Effect { get; set; } = "coverflow";

    /// <summary>
    /// Number of slides per view. Can be "auto" or a number.
    /// </summary>
    public string SlidesPerView { get; set; } = "auto";

    /// <summary>
    /// Index of the initially active slide.
    /// </summary>
    public int InitialSlide { get; set; } = 0;

    /// <summary>
    /// Whether slides should be centered.
    /// </summary>
    public bool CenteredSlides { get; set; } = true;

    /// <summary>
    /// Enable continuous loop mode.
    /// </summary>
    public bool Loop { get; set; } = true;

    /// <summary>
    /// Space between slides in pixels.
    /// </summary>
    public int SpaceBetween { get; set; } = 0;

    /// <summary>
    /// Transition speed in milliseconds.
    /// </summary>
    public int Speed { get; set; } = 300;

    /// <summary>
    /// Enable grab cursor.
    /// </summary>
    public bool GrabCursor { get; set; } = true;

    #endregion

    #region Coverflow Effect Options

    /// <summary>
    /// Rotation angle for coverflow effect (degrees).
    /// </summary>
    public int RotateDegree { get; set; } = 50;

    /// <summary>
    /// Depth of coverflow effect.
    /// </summary>
    public int Depth { get; set; } = 150;

    /// <summary>
    /// Stretch space between slides in coverflow (pixels).
    /// </summary>
    public int Stretch { get; set; } = 0;

    /// <summary>
    /// Coverflow modifier value.
    /// </summary>
    public double Modifier { get; set; } = 1.5;

    /// <summary>
    /// Enable slide shadows in coverflow.
    /// </summary>
    public bool SlideShadows { get; set; } = true;

    #endregion

    #region Touch/Swipe Sensitivity Options

    /// <summary>
    /// Touch sensitivity ratio. Lower values mean less sensitive.
    /// </summary>
    public double TouchRatio { get; set; } = 0.8;

    /// <summary>
    /// Minimum distance (in pixels) required to trigger a swipe.
    /// </summary>
    public int Threshold { get; set; } = 10;

    /// <summary>
    /// Enable short swipes. Set to false to reduce glitchiness on mobile.
    /// </summary>
    public bool ShortSwipes { get; set; } = false;

    /// <summary>
    /// Resistance ratio for edge bouncing (0-1).
    /// </summary>
    public double ResistanceRatio { get; set; } = 0.85;

    /// <summary>
    /// Ratio to trigger swipe to next/previous slide during long swipes.
    /// </summary>
    public double LongSwipesRatio { get; set; } = 0.5;

    /// <summary>
    /// Allow touch move (swiping). Set to false to disable touch/swipe.
    /// </summary>
    public bool AllowTouchMove { get; set; } = true;

    /// <summary>
    /// Follow finger movement precisely during touch.
    /// </summary>
    public bool FollowFinger { get; set; } = true;

    #endregion
}
