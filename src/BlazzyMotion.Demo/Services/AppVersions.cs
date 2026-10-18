using System.Reflection;

namespace BlazzyMotion.Demo.Services;

/// <summary>Provides the component versions embedded in this Demo build by MSBuild.</summary>
public static class AppVersions
{
    public static readonly string Carousel = ReadVersion("Carousel");
    public static readonly string Core = ReadVersion("Core");
    public static readonly string Bento = ReadVersion("Bento");
    public static readonly string Gallery = ReadVersion("Gallery");
    public static readonly string Marquee = ReadVersion("Marquee");
    public const string DotNetVersion = ".NET 8";

    /// <summary>Reads required package metadata generated from the central build properties.</summary>
    /// <param name="component">The component name without the package prefix.</param>
    /// <returns>The package version used when compiling this Demo.</returns>
    /// <exception cref="InvalidOperationException">The build omitted required version metadata.</exception>
    private static string ReadVersion(string component) =>
        typeof(AppVersions).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .SingleOrDefault(attribute => attribute.Key == $"BlazzyMotion.{component}.Version")?.Value
        ?? throw new InvalidOperationException($"Missing Demo version metadata for {component}.");

    /// <summary>Identifies a prerelease suffix while ignoring build metadata.</summary>
    /// <param name="version">The package version to inspect.</param>
    /// <returns>Whether the version is a prerelease.</returns>
    public static bool IsPrerelease(string version) => version.Split('+')[0].Contains('-');

    /// <summary>Resolves a built-in package version for installation examples.</summary>
    /// <param name="packageName">The full NuGet package identifier.</param>
    /// <returns>The embedded version, or null for an unrelated package.</returns>
    public static string? GetPackageVersion(string packageName) => packageName switch
    {
        "BlazzyMotion.Carousel" => Carousel,
        "BlazzyMotion.Core" => Core,
        "BlazzyMotion.Bento" => Bento,
        "BlazzyMotion.Gallery" => Gallery,
        "BlazzyMotion.Marquee" => Marquee,
        _ => null
    };

    /// <summary>Formats a version badge while retaining the unknown-component fallback.</summary>
    /// <param name="componentName">The component name without the package prefix.</param>
    /// <returns>The prefixed version, or v0.0.0 for an unknown component.</returns>
    public static string GetBadgeText(string componentName) =>
        $"v{GetPackageVersion($"BlazzyMotion.{componentName}") ?? "0.0.0"}";
}
