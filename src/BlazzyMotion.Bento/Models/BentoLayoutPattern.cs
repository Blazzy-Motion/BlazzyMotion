namespace BlazzyMotion.Bento.Models;

/// <summary>
/// Predefined layout patterns for BzBento auto-layout mode.
/// </summary>
/// <remarks>
/// <para>
/// Auto-layout patterns automatically assign ColSpan and RowSpan values to items
/// based on their position in the collection, creating visually dynamic Bento grids
/// without manual configuration.
/// </para>
/// <para>
/// <strong>Usage:</strong>
/// <code>
/// &lt;BzBento Items="@items" 
///          AutoLayout="true" 
///          Pattern="BentoLayoutPattern.Dynamic" /&gt;
/// </code>
/// </para>
/// </remarks>
public enum BentoLayoutPattern
{
    /// <summary>
    /// Dynamic pattern with varied spans creating visual interest.
    /// </summary>
    /// <remarks>
    /// Pattern sequence (repeating):
    /// <list type="bullet">
    /// <item>Item 0: 2x2 (Hero card)</item>
    /// <item>Item 1: 1x2 (Tall card)</item>
    /// <item>Item 2: 1x1 (Normal)</item>
    /// <item>Item 3: 1x1 (Normal)</item>
    /// <item>Item 4: 2x1 (Wide card)</item>
    /// <item>Item 5: 1x1 (Normal)</item>
    /// <item>Item 6: 1x2 (Tall card)</item>
    /// <item>Item 7: 1x1 (Normal)</item>
    /// </list>
    /// Best for: Galleries, portfolios, dashboards
    /// </remarks>
    Dynamic = 0,

    /// <summary>
    /// Featured pattern with one large hero item at start, rest smaller.
    /// </summary>
    /// <remarks>
    /// Pattern sequence:
    /// <list type="bullet">
    /// <item>Item 0: 2x2 (Hero - featured content)</item>
    /// <item>Other items: 1x1 or 1x2 (every 3rd item is tall)</item>
    /// </list>
    /// Best for: Blog posts, product showcases, content feeds
    /// </remarks>
    Featured = 1,

    /// <summary>
    /// Balanced pattern with mostly uniform items, no large 2x2 cards.
    /// </summary>
    /// <remarks>
    /// Pattern sequence (repeating):
    /// <list type="bullet">
    /// <item>Items 0, 4: 2x1 (Wide cards)</item>
    /// <item>Item 3: 1x2 (Tall card)</item>
    /// <item>Items 1, 2, 5: 1x1 (Normal)</item>
    /// </list>
    /// Best for: Metric dashboards, stat displays, uniform content
    /// </remarks>
    Balanced = 2,
}
