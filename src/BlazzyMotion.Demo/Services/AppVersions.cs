namespace BlazzyMotion.Demo.Services;

/// <summary>
/// Centralized version management for all BlazzyMotion packages.
/// Update versions here when releasing new NuGet packages.
/// </summary>
public static class AppVersions
{
    public const string Carousel = "1.4.0";
    public const string Core = "1.3.0";
    public const string Bento = "1.0.1";
    public const string Gallery = "1.0.0";
    public const string DotNetVersion = ".NET 8";

    public static string GetBadgeText(string componentName) => componentName switch
    {
        "Carousel" => $"v{Carousel}",
        "Core" => $"v{Core}",
        "Bento" => $"v{Bento}",
        "Gallery" => $"v{Gallery}",
        _ => "v0.0.0"
    };
}
