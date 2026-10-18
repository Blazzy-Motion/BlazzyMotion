using BlazzyMotion.Core.Models;
using BlazzyMotion.Demo.Model;

namespace BlazzyMotion.Demo.Pages;

/// <summary>Presents the live library showcase and copyable introductory example.</summary>
public partial class Home
{
    private BzTheme _currentTheme = BzTheme.Glass;
    private Movie? _selectedMovie;
    private readonly List<Movie> _movies = BlazzyMotion.Demo.Services.MovieExamples.CreateMovies();
    private const string QuickExample = """
        @using BlazzyMotion.Carousel.Components
        @using YourApp.Models

        <BzCarousel Items="@movies" />

        @code {
            private readonly Movie[] movies =
            [
                new()
                {
                    Title = "Survive",
                    ImageUrl = "https://blazzymotion.com/images/survive.webp"
                }
            ];
        }
        """;
    private const string ModelExample = """
        using BlazzyMotion.Core.Attributes;

        namespace YourApp.Models;

        public class Movie
        {
            [BzImage]
            public string ImageUrl { get; set; } = "";

            [BzTitle]
            public string Title { get; set; } = "";
        }
        """;

    /// <summary>Updates the showcase caption without replacing the live Carousel.</summary>
    /// <param name="selectedItem">The movie selected by the Carousel.</param>
    private void OnItemSelected(Movie selectedItem)
    {
        _selectedMovie = selectedItem;
    }
}
