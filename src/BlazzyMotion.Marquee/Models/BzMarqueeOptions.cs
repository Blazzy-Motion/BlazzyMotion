namespace BlazzyMotion.Marquee.Models;

/// <summary>
/// Options passed to JavaScript for marquee initialization.
/// </summary>
public sealed class BzMarqueeOptions
{
    /// <summary>
    /// Scroll direction. Values: "left", "right".
    /// </summary>
    public string Direction { get; set; } = "left";

    /// <summary>
    /// Animation speed in pixels per second.
    /// </summary>
    public int Speed { get; set; } = 50;

    /// <summary>
    /// Gap between items in pixels.
    /// </summary>
    public int Gap { get; set; } = 40;

    /// <summary>
    /// Whether to pause animation on mouse hover.
    /// </summary>
    public bool PauseOnHover { get; set; } = true;

    /// <summary>
    /// Whether to show gradient fade on edges.
    /// </summary>
    public bool ShowGradientEdges { get; set; } = true;

    /// <summary>
    /// Whether the marquee spans full viewport width.
    /// </summary>
    public bool FullWidth { get; set; }

    /// <summary>
    /// Whether animation direction is reversed (right).
    /// </summary>
    public bool Reverse { get; set; }
}
