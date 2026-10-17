namespace BlazzyMotion.Carousel.Models;

/// <summary>Controls the carousel presentation without changing item mapping.</summary>
public enum BzCarouselVariant
{
    /// <summary>Preserves automatic simple/coverflow selection and AutoDetectMode.</summary>
    Auto = 0,
    /// <summary>Displays a deck of cards, including for small collections.</summary>
    Cards = 1
}
