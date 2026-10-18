using System.Text.Json;

namespace BlazzyMotion.Demo.Services;

/// <summary>Formats consumer examples from the data used by the live Demo.</summary>
internal static class ExampleSource
{
    /// <summary>Formats a string as a C# literal, optionally resolving public sample artwork.</summary>
    /// <param name="value">The model value.</param>
    /// <param name="image">Whether to resolve the value against the public Demo URL.</param>
    /// <returns>A quoted and escaped C# string literal.</returns>
    internal static string Literal(string value, bool image = false) =>
        JsonSerializer.Serialize(image ? new Uri(new Uri("https://blazzymotion.com/"), value).AbsoluteUri : value);

    /// <summary>Formats declarations from the same sequence rendered by a live example.</summary>
    /// <typeparam name="T">The Demo item type.</typeparam>
    /// <param name="items">The live items.</param>
    /// <param name="properties">Formats the item properties in an object initializer.</param>
    /// <returns>One target-typed object initializer per item.</returns>
    internal static string Items<T>(IEnumerable<T> items, Func<T, string> properties) =>
        string.Join("\n", items.Select(item => "        new() { " + properties(item) + " },"));
}
