using System.Text.Json;
using BlazzyMotion.Core.Models;
using BlazzyMotion.Demo.Model;

namespace BlazzyMotion.Demo.Pages;

/// <summary>Pairs a real Carousel demonstration with a complete theme-aware consumer example.</summary>
public partial class CarouselPlayground
{
    private BzTheme _currentTheme = BzTheme.Glass;
    private Movie? _selectedMovie;
    private readonly List<Movie> _movies = BlazzyMotion.Demo.Services.MovieExamples.CreateMovies();

    /// <summary>Gets a standalone Razor example generated from the same items and theme as the preview.</summary>
    private string ComponentCode
    {
        get
        {
            var items = string.Join("\n", _movies.Select(movie =>
                $"        new() {{ Title = {JsonSerializer.Serialize(movie.Title)}, ImageUrl = {JsonSerializer.Serialize(new Uri(new Uri("https://blazzymotion.com/"), movie.ImageUrl!).AbsoluteUri)} }},"));
            return $$"""
                @using BlazzyMotion.Carousel.Components
                @using BlazzyMotion.Core.Models
                @using YourApp.Models

                <BzCarousel TItem="Movie" Items="@_movies"
                            Theme="BzTheme.{{_currentTheme}}"
                            OnItemSelected="@OnItemSelected" />
                <p>@(_selectedMovie?.Title ?? _movies[0].Title)</p>

                @code {
                    private Movie? _selectedMovie;
                    private readonly List<Movie> _movies = new()
                    {
                {{items}}
                    };

                    /// <summary>Updates the title after a movie is selected.</summary>
                    /// <param name="movie">The selected movie.</param>
                    private void OnItemSelected(Movie movie) => _selectedMovie = movie;
                }
                """;
        }
    }

    /// <summary>Gets the existing supported attribute mapping used by the demonstration model.</summary>
    private const string ModelCode = """
        using BlazzyMotion.Carousel.Attributes;

        namespace YourApp.Models;

        /// <summary>A movie displayed by the carousel.</summary>
        public class Movie
        {
            /// <summary>Gets or sets the movie title.</summary>
            [BzTitle]
            public string? Title { get; set; }

            /// <summary>Gets or sets the artwork URL.</summary>
            [BzImage]
            public string? ImageUrl { get; set; }
        }
        """;

    /// <summary>Updates the visible movie title without replacing the preview component.</summary>
    /// <param name="movie">The item selected by the live Carousel.</param>
    private void OnItemSelected(Movie movie) => _selectedMovie = movie;
}
