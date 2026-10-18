using BlazzyMotion.Demo.Model;

namespace BlazzyMotion.Demo.Services;

/// <summary>Supplies the shared sample artwork without sharing mutable model instances between pages.</summary>
internal static class MovieExamples
{
    /// <summary>Creates the movie collection used by the landing page and focused Carousel example.</summary>
    /// <returns>A new list containing new sample movie instances for the caller.</returns>
    internal static List<Movie> CreateMovies() => new()
    {
        new() { Title = "Despicable Me 4", ImageUrl = "images/despicableme.webp" },
        new() { Title = "Joker", ImageUrl = "images/joker.webp" },
        new() { Title = "The Platform 2", ImageUrl = "images/the-platform.webp" },
        new() { Title = "The Substance", ImageUrl = "images/thesubstance.webp" },
        new() { Title = "Venom", ImageUrl = "images/venom.webp" },
        new() { Title = "Survive", ImageUrl = "images/survive.webp" },
        new() { Title = "The Wild Robot", ImageUrl = "images/wild-robot.webp" },
        new() { Title = "Deadpool & Wolverine", ImageUrl = "images/wolwerine.webp" }
    };
}
