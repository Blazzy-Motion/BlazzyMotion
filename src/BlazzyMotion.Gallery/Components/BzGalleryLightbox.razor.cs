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

    /// <summary>
    /// Do NOT unconditionally prevent default — Tab key must be allowed through
    /// so the browser and JS focus trap can manage focus cycling.
    /// Body scroll is already locked via LockBodyScrollAsync when lightbox opens.
    /// </summary>
    private bool ShouldPreventDefault => false;

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

    private async Task GoFirst()
    {
        if (Items == null || Items.Count <= 1 || CurrentIndex == 0) return;

        await OnIndexChanged.InvokeAsync(0);
    }

    private async Task GoLast()
    {
        if (Items == null || Items.Count <= 1 || CurrentIndex == Items.Count - 1) return;

        await OnIndexChanged.InvokeAsync(Items.Count - 1);
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
            case "Home":
                await GoFirst();
                break;
            case "End":
                await GoLast();
                break;
        }
    }

    private static string GetThumbAriaLabel(int index, BzItem item)
    {
        var label = $"Go to image {index + 1}";
        if (item.HasTitle)
        {
            label += $", {item.Title}";
        }

        return label;
    }

    private string GetLightboxAriaLabel()
    {
        if (CurrentItem?.HasTitle == true && Items != null)
        {
            return $"{CurrentItem.Title}, image {CurrentIndex + 1} of {Items.Count}";
        }

        if (Items != null)
        {
            return $"Image lightbox, image {CurrentIndex + 1} of {Items.Count}";
        }

        return "Image lightbox";
    }
}
