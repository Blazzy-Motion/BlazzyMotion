using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Bento.Abstractions;

/// <summary>
/// Base class for Bento Grid helper components (Card, Metric, Feature, Quote).
/// </summary>
/// <remarks>
/// <para>
/// Provides common layout and styling parameters shared by all Bento item components.
/// Unlike <see cref="BlazzyMotion.Core.Abstractions.BzComponentBase"/>, this class
/// does not include Theme (inherited from parent BzBento) or IAsyncDisposable
/// (helper components don't need JS interop cleanup).
/// </para>
/// <para>
/// <strong>Inheritance:</strong>
/// <code>
/// ComponentBase
///     └── BzBentoItemBase
///             ├── BzBentoItem (generic wrapper)
///             ├── BzBentoCard&lt;TItem&gt;
///             ├── BzBentoMetric
///             ├── BzBentoFeature
///             └── BzBentoQuote&lt;TItem&gt;
/// </code>
/// </para>
/// </remarks>
public abstract class BzBentoItemBase : ComponentBase
{
  /// <summary>
  /// Number of columns this item spans in the grid (1-4).
  /// </summary>
  /// <remarks>
  /// On tablet (≤991px), values 3-4 are reduced to 2.
  /// On mobile (≤600px), all items become single column.
  /// </remarks>
  [Parameter]
  public int ColSpan { get; set; } = 1;

  /// <summary>
  /// Number of rows this item spans in the grid (1-4).
  /// </summary>
  /// <remarks>
  /// On mobile (≤600px), row spans are reset to 1.
  /// </remarks>
  [Parameter]
  public int RowSpan { get; set; } = 1;

  /// <summary>
  /// CSS order property for custom item ordering.
  /// </summary>
  /// <remarks>
  /// Lower values appear first. Default is 0.
  /// Use with grid-auto-flow: dense for optimal gap-free layouts.
  /// </remarks>
  [Parameter]
  public int Order { get; set; } = 0;

  /// <summary>
  /// Additional CSS classes to apply to the item.
  /// </summary>
  [Parameter]
  public string? CssClass { get; set; }

  /// <summary>
  /// Inline styles to apply to the item.
  /// </summary>
  [Parameter]
  public string? Style { get; set; }

  /// <summary>
  /// Additional HTML attributes (id, data-*, aria-*, etc.).
  /// </summary>
  [Parameter(CaptureUnmatchedValues = true)]
  public Dictionary<string, object>? AdditionalAttributes { get; set; }

  /// <summary>
  /// Callback invoked when the item is clicked.
  /// </summary>
  [Parameter]
  public EventCallback OnClick { get; set; }

  /// <summary>
  /// Gets the CSS classes for column and row spans.
  /// </summary>
  protected string SpanClasses
  {
    get
    {
      var col = Math.Clamp(ColSpan, 1, 4);
      var row = Math.Clamp(RowSpan, 1, 4);
      return $"bzb-col-{col} bzb-row-{row}";
    }
  }

  /// <summary>
  /// Generates the inline style string including order and custom styles.
  /// </summary>
  protected string GetItemStyle()
  {
    var styles = new List<string>();

    if (Order != 0)
      styles.Add($"order: {Order}");

    if (!string.IsNullOrWhiteSpace(Style))
      styles.Add(Style);

    return string.Join("; ", styles);
  }

  /// <summary>
  /// Combines base class, span classes, and custom CSS class.
  /// </summary>
  /// <param name="componentClass">The component-specific CSS class (e.g., "bzb-card")</param>
  protected string GetCombinedClass(string componentClass)
  {
    var classes = new List<string> { "bzb-item", componentClass, SpanClasses };

    if (!string.IsNullOrWhiteSpace(CssClass))
      classes.Add(CssClass);

    return string.Join(" ", classes.Where(c => !string.IsNullOrEmpty(c)));
  }

  /// <summary>
  /// Handles click events on the item.
  /// </summary>
  protected async Task HandleClick()
  {
    if (OnClick.HasDelegate)
    {
      await OnClick.InvokeAsync();
    }
  }
}
