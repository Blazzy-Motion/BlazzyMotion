namespace BlazzyMotion.Core.Models;

/// <summary>
/// Available visual themes for BlazzyMotion components.
/// </summary>
/// <remarks>
/// <para>
/// All BlazzyMotion components share the same theming system for consistency.
/// Each theme applies a distinct visual style through CSS custom properties.
/// </para>
/// <para>
/// <strong>CSS Classes Generated:</strong>
/// <list type="bullet">
/// <item><c>Glass</c> → <c>bzc-theme-glass</c></item>
/// <item><c>Dark</c> → <c>bzc-theme-dark</c></item>
/// <item><c>Light</c> → <c>bzc-theme-light</c></item>
/// <item><c>Minimal</c> → <c>bzc-theme-minimal</c></item>
/// </list>
/// </para>
/// </remarks>
/// <example>
/// <code>
/// &lt;BzCarousel Items="movies" Theme="BzTheme.Glass" /&gt;
/// &lt;BzGallery Items="photos" Theme="BzTheme.Dark" /&gt;
/// </code>
/// </example>
public enum BzTheme
{
    /// <summary>
    /// Glassmorphism effect with blur and transparency.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Default theme. Features:
    /// <list type="bullet">
    /// <item>Semi-transparent background</item>
    /// <item>Backdrop blur effect</item>
    /// <item>Subtle border glow</item>
    /// <item>Modern frosted glass appearance</item>
    /// </list>
    /// </para>
    /// </remarks>
    Glass = 0,

    /// <summary>
    /// Dark theme with solid background and subtle glow effects.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Ideal for dark mode applications. Features:
    /// <list type="bullet">
    /// <item>Gradient dark background</item>
    /// <item>Subtle blue glow accent</item>
    /// <item>High contrast for readability</item>
    /// </list>
    /// </para>
    /// </remarks>
    Dark = 1,

    /// <summary>
    /// Light theme with bright colors and soft shadows.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Ideal for light mode applications. Features:
    /// <list type="bullet">
    /// <item>White/light gray background</item>
    /// <item>Soft drop shadows</item>
    /// <item>Clean, professional appearance</item>
    /// </list>
    /// </para>
    /// </remarks>
    Light = 2,

    /// <summary>
    /// Minimal theme without background container.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For seamless integration. Features:
    /// <list type="bullet">
    /// <item>Transparent background</item>
    /// <item>No borders or shadows</item>
    /// <item>Blends with parent container</item>
    /// </list>
    /// </para>
    /// </remarks>
    Minimal = 3
}