using BlazzyMotion.Bento.Abstractions;
using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Bento.Components;

/// <summary>
/// Displays a feature with icon and label in a Bento Grid.
/// </summary>
/// <remarks>
/// <para>
/// Perfect for showcasing product features, benefits, or capabilities.
/// Supports any icon library through RenderFragment.
/// </para>
/// <para>
/// <strong>Usage:</strong>
/// <code>
/// // With emoji
/// &lt;BzBentoFeature IconText="🚀" Label="Lightning Fast" /&gt;
/// 
/// // With Bootstrap Icons
/// &lt;BzBentoFeature Label="Secure"&gt;
///     &lt;Icon&gt;&lt;i class="bi bi-shield-check"&gt;&lt;/i&gt;&lt;/Icon&gt;
/// &lt;/BzBentoFeature&gt;
/// 
/// // With Font Awesome
/// &lt;BzBentoFeature Label="Cloud Sync"&gt;
///     &lt;Icon&gt;&lt;i class="fa-solid fa-cloud"&gt;&lt;/i&gt;&lt;/Icon&gt;
/// &lt;/BzBentoFeature&gt;
/// 
/// // With MudBlazor
/// &lt;BzBentoFeature Label="Settings"&gt;
///     &lt;Icon&gt;&lt;MudIcon Icon="@Icons.Material.Filled.Settings" /&gt;&lt;/Icon&gt;
/// &lt;/BzBentoFeature&gt;
/// 
/// // With description
/// &lt;BzBentoFeature IconText="⚡" Label="Real-time" Description="Updates in milliseconds" /&gt;
/// </code>
/// </para>
/// </remarks>
public partial class BzBentoFeature : BzBentoItemBase
{
  /// <summary>
  /// Icon content using RenderFragment. Supports any content:
  /// HTML elements, Blazor components (MudIcon, RadzenIcon), SVG, images.
  /// </summary>
  [Parameter]
  public RenderFragment? Icon { get; set; }

  /// <summary>
  /// Simple string icon (emoji or text).
  /// Use Icon RenderFragment for complex icons.
  /// </summary>
  [Parameter]
  public string? IconText { get; set; }

  /// <summary>
  /// The feature label/name.
  /// </summary>
  [Parameter, EditorRequired]
  public string Label { get; set; } = string.Empty;

  /// <summary>
  /// Optional description text.
  /// </summary>
  [Parameter]
  public string? Description { get; set; }

  private bool HasIcon => Icon != null || !string.IsNullOrWhiteSpace(IconText);
  private bool HasDescription => !string.IsNullOrWhiteSpace(Description);
}
