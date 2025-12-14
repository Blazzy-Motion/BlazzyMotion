using BlazzyMotion.Core.Models;
using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Core.Templates;

/// <summary>
/// Factory for creating RenderFragments for BlazzyMotion components.
/// </summary>
/// <remarks>
/// <para>
/// Provides pre-built templates for common rendering scenarios.
/// All templates work with <see cref="BzItem"/> instances.
/// </para>
/// <para>
/// <strong>Available Templates:</strong>
/// <list type="bullet">
/// <item><see cref="CreateImage"/> - Simple img element</item>
/// <item><see cref="CreateFallback"/> - Fallback for unmapped items</item>
/// </list>
/// </para>
/// <para>
/// <strong>Future Templates (planned):</strong>
/// <list type="bullet">
/// <item>CreateCard - Image with title overlay</item>
/// <item>CreateThumbnail - Small preview image</item>
/// <item>CreateVideo - Video element with poster</item>
/// </list>
/// </para>
/// </remarks>
public static class BzTemplateFactory
{
    /// <summary>
    /// Creates a RenderFragment that renders a BzItem as an img element.
    /// </summary>
    /// <returns>RenderFragment for image rendering</returns>
    /// <remarks>
    /// <para>
    /// Generated HTML:
    /// <code>
    /// &lt;img src="{ImageUrl}" alt="{Title}" title="{Title}" loading="lazy" /&gt;
    /// </code>
    /// </para>
    /// <para>
    /// <strong>Features:</strong>
    /// <list type="bullet">
    /// <item>Null safety - skips null items or empty URLs</item>
    /// <item>Accessibility - uses Title for alt/title attributes</item>
    /// <item>Performance - lazy loading enabled</item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Usage in component
    /// @foreach (var item in MappedItems)
    /// {
    ///     @BzTemplateFactory.CreateImage()(item)
    /// }
    /// </code>
    /// </example>
    public static RenderFragment<BzItem> CreateImage()
    {
        return item => builder =>
        {
            if (item is null || !item.HasImage)
            {
                return;
            }

            builder.OpenElement(0, "img");
            builder.AddAttribute(1, "src", item.ImageUrl);

            if (item.HasTitle)
            {
                builder.AddAttribute(2, "alt", item.Title);
                builder.AddAttribute(3, "title", item.Title);
            }
            else
            {
                builder.AddAttribute(2, "alt", "Image");
            }

            builder.AddAttribute(4, "loading", "lazy");

            builder.CloseElement();
        };
    }

    /// <summary>
    /// Creates a fallback RenderFragment for items without valid data.
    /// </summary>
    /// <returns>RenderFragment for fallback rendering</returns>
    /// <remarks>
    /// <para>
    /// Used when:
    /// <list type="bullet">
    /// <item>No mapper is registered for the type</item>
    /// <item>Item has no valid ImageUrl</item>
    /// <item>Custom ItemTemplate is not provided</item>
    /// </list>
    /// </para>
    /// <para>
    /// Generated HTML:
    /// <code>
    /// &lt;div class="bzc-fallback-item"&gt;{Title or "Item"}&lt;/div&gt;
    /// </code>
    /// </para>
    /// </remarks>
    public static RenderFragment<BzItem> CreateFallback()
    {
        return item => builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "bzc-fallback-item");
            builder.AddContent(2, item?.DisplayTitle ?? "Item");
            builder.CloseElement();
        };
    }

    /// <summary>
    /// Creates a generic fallback for any object type.
    /// </summary>
    /// <typeparam name="T">The type of item to render</typeparam>
    /// <returns>RenderFragment that displays ToString() result</returns>
    /// <remarks>
    /// Used when ItemTemplate is not provided and no [BzImage] attribute exists.
    /// Displays the item's ToString() representation.
    /// </remarks>
    public static RenderFragment<T> CreateGenericFallback<T>()
    {
        return item => builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "bzc-fallback-item");
            builder.AddContent(2, item?.ToString() ?? "[null]");
            builder.CloseElement();
        };
    }

    /// <summary>
    /// Selects the appropriate template based on item state.
    /// </summary>
    /// <param name="item">The BzItem to render</param>
    /// <returns>Appropriate RenderFragment based on item data</returns>
    /// <remarks>
    /// <para>
    /// Selection logic:
    /// <list type="number">
    /// <item>If item has image → CreateImage()</item>
    /// <item>Otherwise → CreateFallback()</item>
    /// </list>
    /// </para>
    /// </remarks>
    public static RenderFragment<BzItem> SelectTemplate(BzItem? item)
    {
        if (item is null || !item.HasImage)
        {
            return CreateFallback();
        }

        return CreateImage();
    }
}