using BlazzyMotion.Bento.Abstractions;
using BlazzyMotion.Core.Models;
using BlazzyMotion.Core.Services;
using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Bento.Components;

/// <summary>
/// Displays a testimonial or quote in a Bento Grid.
/// </summary>
/// <typeparam name="TItem">The type of item containing quote data</typeparam>
/// <remarks>
/// <para>
/// BzBentoQuote can be used in two ways:
/// <list type="number">
/// <item>With an Item that has [BzDescription] (quote text), [BzTitle] (author), [BzImage] (avatar)</item>
/// <item>With direct parameter values (Text, Author, Role, Avatar)</item>
/// </list>
/// </para>
/// <para>
/// <strong>Usage:</strong>
/// <code>
/// // With automatic mapping
/// &lt;BzBentoQuote Item="testimonial" ColSpan="2" /&gt;
/// 
/// // With direct values
/// &lt;BzBentoQuote TItem="object"
///               Text="Amazing product!"
///               Author="John Doe"
///               Role="CEO, TechCorp"
///               ColSpan="2" /&gt;
/// 
/// // Mixed (Item + override)
/// &lt;BzBentoQuote Item="testimonial" Role="Updated Role" /&gt;
/// </code>
/// </para>
/// </remarks>
public partial class BzBentoQuote<TItem> : BzBentoItemBase where TItem : class
{
  /// <summary>
  /// The item containing quote data. Uses BzRegistry for mapping.
  /// </summary>
  [Parameter]
  public TItem? Item { get; set; }

  /// <summary>
  /// Override quote text. If not set, uses [BzDescription] from Item.
  /// </summary>
  [Parameter]
  public string? Text { get; set; }

  /// <summary>
  /// Override author name. If not set, uses [BzTitle] from Item.
  /// </summary>
  [Parameter]
  public string? Author { get; set; }

  /// <summary>
  /// Author's role or title (e.g., "CEO, TechCorp").
  /// </summary>
  [Parameter]
  public string? Role { get; set; }

  /// <summary>
  /// Override avatar URL. If not set, uses [BzImage] from Item.
  /// </summary>
  [Parameter]
  public string? Avatar { get; set; }

  private BzItem? _mapped;

  /// <inheritdoc />
  protected override void OnParametersSet()
  {
    if (Item != null)
    {
      _mapped = BzRegistry.ToBzItem(Item);
    }
  }

  private string GetText() => Text ?? _mapped?.Description ?? string.Empty;
  private string GetAuthor() => Author ?? _mapped?.Title ?? string.Empty;
  private string GetAvatar() => Avatar ?? _mapped?.ImageUrl ?? string.Empty;

  private bool HasText => !string.IsNullOrWhiteSpace(GetText());
  private bool HasAuthor => !string.IsNullOrWhiteSpace(GetAuthor());
  private bool HasAvatar => !string.IsNullOrWhiteSpace(GetAvatar());
  private bool HasRole => !string.IsNullOrWhiteSpace(Role);
  private bool HasAuthorInfo => HasAuthor || HasRole || HasAvatar;
}
