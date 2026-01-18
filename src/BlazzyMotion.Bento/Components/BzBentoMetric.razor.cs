using BlazzyMotion.Bento.Abstractions;
using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Bento.Components;

/// <summary>
/// Displays a metric/statistic value with label in a Bento Grid.
/// </summary>
/// <remarks>
/// <para>
/// Perfect for dashboards showing KPIs, statistics, and numbers.
/// </para>
/// <para>
/// <strong>Usage:</strong>
/// <code>
/// // Simple metric
/// &lt;BzBentoMetric Value="1,234" Label="Active Users" /&gt;
/// 
/// // With trend indicator
/// &lt;BzBentoMetric Value="$48.5K" Label="Revenue" Trend="+12.5%" /&gt;
/// 
/// // With icon (emoji)
/// &lt;BzBentoMetric Value="99.9%" Label="Uptime" IconText="⚡" /&gt;
/// 
/// // With icon (any component)
/// &lt;BzBentoMetric Value="4.9" Label="Rating"&gt;
///     &lt;Icon&gt;&lt;i class="bi bi-star-fill"&gt;&lt;/i&gt;&lt;/Icon&gt;
/// &lt;/BzBentoMetric&gt;
/// </code>
/// </para>
/// </remarks>
public partial class BzBentoMetric : BzBentoItemBase
{
  /// <summary>
  /// The metric value to display (e.g., "1,234", "$48.5K", "99.9%").
  /// </summary>
  [Parameter, EditorRequired]
  public string Value { get; set; } = string.Empty;

  /// <summary>
  /// The label describing the metric.
  /// </summary>
  [Parameter, EditorRequired]
  public string Label { get; set; } = string.Empty;

  /// <summary>
  /// Optional trend indicator (e.g., "+12%", "-5%").
  /// Positive values show green, negative show red.
  /// </summary>
  [Parameter]
  public string? Trend { get; set; }

  /// <summary>
  /// Icon content using RenderFragment. Supports any content:
  /// HTML elements, Blazor components, SVG, etc.
  /// </summary>
  /// <remarks>
  /// Use this for complex icons like Bootstrap Icons, Font Awesome,
  /// MudBlazor icons, or custom SVG.
  /// </remarks>
  [Parameter]
  public RenderFragment? Icon { get; set; }

  /// <summary>
  /// Simple string icon (emoji or text).
  /// Use Icon RenderFragment for complex icons.
  /// </summary>
  [Parameter]
  public string? IconText { get; set; }

  private bool HasIcon => Icon != null || !string.IsNullOrWhiteSpace(IconText);
  private bool HasTrend => !string.IsNullOrWhiteSpace(Trend);

  private string TrendClass
  {
    get
    {
      if (string.IsNullOrWhiteSpace(Trend))
        return string.Empty;

      if (Trend.StartsWith("+"))
        return "bzb-metric-trend-up";

      if (Trend.StartsWith("-"))
        return "bzb-metric-trend-down";

      return string.Empty;
    }
  }
}
