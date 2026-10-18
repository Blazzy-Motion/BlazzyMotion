using BlazzyMotion.Demo.Services;

namespace BlazzyMotion.Demo.Pages;

/// <summary>Provides copyable consumer examples matching the live Gallery controls and data.</summary>
public partial class GalleryDemo
{
    /// <summary>Gets a complete example matching every Gallery control.</summary>
    private string GalleryCode => $$"""
        @using BlazzyMotion.Gallery.Components
        @using BlazzyMotion.Gallery.Models
        @using BlazzyMotion.Core.Models
        @using YourApp.Models

        <BzGallery TItem="GalleryPhoto" Items="@_photos"
                   Layout="BzGalleryLayout.{{_layout}}" Theme="BzTheme.{{_currentTheme}}"
                   Columns="{{_columns}}" Gap="{{_gap}}" AspectRatio="{{_aspectRatio}}"
                   EnableLightbox="{{_enableLightbox.ToString().ToLowerInvariant()}}"
                   EnableFilter="{{_enableFilter.ToString().ToLowerInvariant()}}"
                   CategorySelector="@(photo => photo.Category)"
                   OnItemSelected="@OnPhotoSelected" />
        @if (!_enableLightbox && _selectedPhoto != null)
        {
            <p>Selected: @_selectedPhoto.Title</p>
        }

        @code {
            private bool _enableLightbox = {{_enableLightbox.ToString().ToLowerInvariant()}};
            private GalleryPhoto? _selectedPhoto;
            private readonly List<GalleryPhoto> _photos = new()
            {
        {{ExampleSource.Items(_photos, item => $"Title = {ExampleSource.Literal(item.Title)}, Description = {ExampleSource.Literal(item.Description)}, Category = {ExampleSource.Literal(item.Category)}, ImageUrl = {ExampleSource.Literal(item.ImageUrl, true)}")}}
            };

            /// <summary>Tracks the photo selected by the gallery.</summary>
            /// <param name="photo">The selected photo.</param>
            private void OnPhotoSelected(GalleryPhoto photo) => _selectedPhoto = photo;
        }
        """;
    /// <summary>Gets the supported attribute model for this example.</summary>
    private string PhotoModelCode => $$"""
        using BlazzyMotion.Core.Attributes;

        namespace YourApp.Models;

        /// <summary>Data displayed by the live GalleryPhoto example.</summary>
        public class GalleryPhoto
        {
            /// <summary>Gets or sets the Title value.</summary>
            [BzTitle]
            public string Title { get; set; } = "";

            /// <summary>Gets or sets the Description value.</summary>
            [BzDescription]
            public string Description { get; set; } = "";

            /// <summary>Gets or sets the Category value.</summary>
            public string Category { get; set; } = "";

            /// <summary>Gets or sets the ImageUrl value.</summary>
            [BzImage]
            public string ImageUrl { get; set; } = "";
        }
        """;
}
