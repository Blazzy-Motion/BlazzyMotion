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
/// <item><see cref="CreateImage"/> - Simple img element (for Carousel)</item>
/// <item><see cref="CreateBentoItem"/> - Bento grid item with image, title, description</item>
/// <item><see cref="CreateBentoCard"/> - Bento card with image and overlay text</item>
/// <item><see cref="CreateBentoStat"/> - Bento stat/metric display</item>
/// <item><see cref="CreateFallback"/> - Fallback for unmapped items</item>
/// </list>
/// </para>
/// </remarks>
public static class BzTemplateFactory
{
    #region Carousel Templates

    /// <summary>
    /// Creates a RenderFragment that renders a BzItem as an img element.
    /// </summary>
    public static RenderFragment<BzItem> CreateImage()
    {
        return item => builder =>
        {
            if (item is null || !item.HasImage) return;

            builder.OpenElement(0, "img");
            builder.AddAttribute(1, "src", item.ImageUrl);
            builder.AddAttribute(2, "alt", item.HasTitle ? item.Title : "Image");
            if (item.HasTitle) builder.AddAttribute(3, "title", item.Title);
            builder.AddAttribute(4, "loading", "lazy");
            builder.CloseElement();
        };
    }

    #endregion

    #region Bento Templates

    /// <summary>
    /// Creates a RenderFragment that renders a BzItem for Bento Grid.
    /// </summary>
    public static RenderFragment<BzItem> CreateBentoItem()
    {
        return item => builder =>
        {
            if (item is null) return;
            var seq = 0;

            if (item.HasImage)
            {
                builder.OpenElement(seq++, "img");
                builder.AddAttribute(seq++, "class", "bzb-item-image");
                builder.AddAttribute(seq++, "src", item.ImageUrl);
                builder.AddAttribute(seq++, "alt", item.HasTitle ? item.Title : "Image");
                builder.AddAttribute(seq++, "loading", "lazy");
                builder.CloseElement();
            }

            if (item.HasTitle)
            {
                builder.OpenElement(seq++, "h4");
                builder.AddAttribute(seq++, "class", "bzb-item-title");
                builder.AddContent(seq++, item.Title);
                builder.CloseElement();
            }

            if (!string.IsNullOrWhiteSpace(item.Description))
            {
                builder.OpenElement(seq++, "p");
                builder.AddAttribute(seq++, "class", "bzb-item-description");
                builder.AddContent(seq++, item.Description);
                builder.CloseElement();
            }
        };
    }

    /// <summary>
    /// Creates a RenderFragment that renders a BzItem as a card with overlay.
    /// </summary>
    public static RenderFragment<BzItem> CreateBentoCard()
    {
        return item => builder =>
        {
            if (item is null) return;
            var seq = 0;

            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "bzb-card");

            if (item.HasImage)
            {
                builder.OpenElement(seq++, "img");
                builder.AddAttribute(seq++, "class", "bzb-card-image");
                builder.AddAttribute(seq++, "src", item.ImageUrl);
                builder.AddAttribute(seq++, "alt", item.HasTitle ? item.Title : "Image");
                builder.AddAttribute(seq++, "loading", "lazy");
                builder.CloseElement();
            }

            if (item.HasTitle || !string.IsNullOrWhiteSpace(item.Description))
            {
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", "bzb-card-overlay");

                if (item.HasTitle)
                {
                    builder.OpenElement(seq++, "h4");
                    builder.AddAttribute(seq++, "class", "bzb-card-title");
                    builder.AddContent(seq++, item.Title);
                    builder.CloseElement();
                }

                if (!string.IsNullOrWhiteSpace(item.Description))
                {
                    builder.OpenElement(seq++, "p");
                    builder.AddAttribute(seq++, "class", "bzb-card-description");
                    builder.AddContent(seq++, item.Description);
                    builder.CloseElement();
                }

                builder.CloseElement();
            }

            builder.CloseElement();
        };
    }

    /// <summary>
    /// Creates a RenderFragment that renders a BzItem as a stat/metric display.
    /// </summary>
    public static RenderFragment<BzItem> CreateBentoStat()
    {
        return item => builder =>
        {
            if (item is null) return;
            var seq = 0;

            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "bzb-stat");

            builder.OpenElement(seq++, "span");
            builder.AddAttribute(seq++, "class", "bzb-stat-value");
            builder.AddContent(seq++, item.Description ?? "-");
            builder.CloseElement();

            builder.OpenElement(seq++, "span");
            builder.AddAttribute(seq++, "class", "bzb-stat-label");
            builder.AddContent(seq++, item.Title ?? "Metric");
            builder.CloseElement();

            builder.CloseElement();
        };
    }

    #endregion

    #region Fallback Templates

    /// <summary>
    /// Creates a fallback RenderFragment for items without valid data.
    /// </summary>
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
    public static RenderFragment<T> CreateGenericFallback<T>()
    {
        return item => builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "bz-fallback-item");
            builder.AddContent(2, item?.ToString() ?? "[null]");
            builder.CloseElement();
        };
    }

    #endregion

    #region Template Selection

    /// <summary>
    /// Selects the appropriate template based on item state (for Carousel).
    /// </summary>
    public static RenderFragment<BzItem> SelectTemplate(BzItem? item)
    {
        if (item is null || !item.HasImage) return CreateFallback();
        return CreateImage();
    }

    /// <summary>
    /// Selects the appropriate Bento template based on item state.
    /// </summary>
    public static RenderFragment<BzItem> SelectBentoTemplate(BzItem? item)
    {
        if (item is null) return CreateFallback();
        if (item.HasImage && item.ColSpan > 1) return CreateBentoCard();
        if (item.HasImage) return CreateBentoItem();
        if (item.HasTitle || !string.IsNullOrWhiteSpace(item.Description)) return CreateBentoStat();
        return CreateFallback();
    }

    #endregion
}
