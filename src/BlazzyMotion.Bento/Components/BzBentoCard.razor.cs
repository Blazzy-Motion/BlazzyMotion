using BlazzyMotion.Bento.Abstractions;
using BlazzyMotion.Core.Models;
using BlazzyMotion.Core.Services;
using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Bento.Components;

/// <summary>
/// Displays an image card with overlay text in a Bento Grid.
/// </summary>
/// <typeparam name="TItem">The type of item to display</typeparam>
/// <remarks>
/// <para>
/// BzBentoCard automatically maps your model using Source Generator attributes:
/// <list type="bullet">
/// <item>[BzImage] → Image source</item>
/// <item>[BzTitle] → Card title</item>
/// <item>[BzDescription] → Card description</item>
/// </list>
/// </para>
/// <para>
/// <strong>Usage:</strong>
/// <code>
/// // With automatic mapping
/// &lt;BzBentoCard Item="product" ColSpan="2" RowSpan="2" /&gt;
/// 
/// // With override values
/// &lt;BzBentoCard Item="product" Title="Custom Title" /&gt;
/// </code>
/// </para>
/// </remarks>
public partial class BzBentoCard<TItem> : BzBentoItemBase where TItem : class
{
  /// <summary>
  /// The item to display. Properties are mapped via BzRegistry.
  /// Optional in Composition mode - use Image, Title, Description parameters instead.
  /// </summary>
  [Parameter]
  public TItem? Item { get; set; }

  /// <summary>
  /// Override image URL. If not set, uses [BzImage] from Item.
  /// </summary>
  [Parameter]
  public string? Image { get; set; }

  /// <summary>
  /// Override title. If not set, uses [BzTitle] from Item.
  /// </summary>
  [Parameter]
  public string? Title { get; set; }

  /// <summary>
  /// Override description. If not set, uses [BzDescription] from Item.
  /// </summary>
  [Parameter]
  public string? Description { get; set; }

  private BzItem? _mapped;

  /// <inheritdoc />
  protected override void OnParametersSet()
  {
    if (Item != null)
    {
      _mapped = BzRegistry.ToBzItem(Item);
    }
  }

  private string GetImage() => Image ?? _mapped?.ImageUrl ?? string.Empty;
  private string GetTitle() => Title ?? _mapped?.Title ?? string.Empty;
  private string GetDescription() => Description ?? _mapped?.Description ?? string.Empty;
  private bool HasImage => !string.IsNullOrWhiteSpace(GetImage());
  private bool HasTitle => !string.IsNullOrWhiteSpace(GetTitle());
  private bool HasDescription => !string.IsNullOrWhiteSpace(GetDescription());
  private bool HasOverlay => HasTitle || HasDescription;
}
