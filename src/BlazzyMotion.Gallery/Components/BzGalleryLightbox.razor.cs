using BlazzyMotion.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazzyMotion.Gallery.Components;

/// <summary>
/// Fullscreen lightbox sub-component for BzGallery.
/// </summary>
public partial class BzGalleryLightbox : ComponentBase
{
    [Parameter]
    public IReadOnlyList<BzItem>? Items { get; set; }

    [Parameter]
    public int CurrentIndex { get; set; }

    [Parameter]
    public bool IsOpen { get; set; }

    [Parameter]
    public EventCallback OnClose { get; set; }

    [Parameter]
    public EventCallback<int> OnIndexChanged { get; set; }

    private BzItem? CurrentItem => Items != null && CurrentIndex >= 0 && CurrentIndex < Items.Count
        ? Items[CurrentIndex]
        : null;

    private async Task GoPrev()
    {
        if (Items == null || Items.Count <= 1) return;

        var newIndex = CurrentIndex <= 0 ? Items.Count - 1 : CurrentIndex - 1;
        await OnIndexChanged.InvokeAsync(newIndex);
    }

    private async Task GoNext()
    {
        if (Items == null || Items.Count <= 1) return;

        var newIndex = CurrentIndex >= Items.Count - 1 ? 0 : CurrentIndex + 1;
        await OnIndexChanged.InvokeAsync(newIndex);
    }

    private async Task RequestClose()
    {
        await OnClose.InvokeAsync();
    }

    private async Task OnBackdropClick()
    {
        await RequestClose();
    }

    private async Task GoToIndex(int index)
    {
        if (Items == null || index < 0 || index >= Items.Count || index == CurrentIndex) return;

        await OnIndexChanged.InvokeAsync(index);
    }

    private async Task OnKeyDown(KeyboardEventArgs e)
    {
        switch (e.Key)
        {
            case "Escape":
                await RequestClose();
                break;
            case "ArrowLeft":
                await GoPrev();
                break;
            case "ArrowRight":
                await GoNext();
                break;
        }
    }
}
