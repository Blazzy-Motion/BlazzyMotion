using System.Reflection;
using BlazzyMotion.Demo.Components;
using BlazzyMotion.Demo.Services;

namespace BlazzyMotion.Tests.Demo;

/// <summary>Protects version alignment between the running components and Demo installation examples.</summary>
public sealed class DemoVersionTests : TestContext
{
    /// <summary>Requires Demo metadata to describe the actual component assemblies in the build.</summary>
    [Fact]
    public void DisplayedVersionsMatchBuiltComponentAssemblies()
    {
        foreach (var component in new[] { "Core", "Carousel", "Bento", "Gallery", "Marquee" })
        {
            var packageName = $"BlazzyMotion.{component}";
            var actualVersion = Assembly.Load(packageName)
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion.Split('+')[0];
            AppVersions.GetPackageVersion(packageName).Should().Be(actualVersion);
        }
    }

    /// <summary>Distinguishes prerelease labels from hyphens appearing only in build metadata.</summary>
    /// <param name="version">The package version under test.</param>
    /// <param name="expected">Whether the package is a prerelease.</param>
    [Theory]
    [InlineData("1.2.3", false)]
    [InlineData("1.2.3-preview.2", true)]
    [InlineData("1.2.3-rc.1+source", true)]
    [InlineData("1.2.3+source-preview", false)]
    public void PrereleaseStatusUsesTheReleaseLabel(string version, bool expected)
    {
        AppVersions.IsPrerelease(version).Should().Be(expected);
    }

    /// <summary>Ensures users copy exactly the version-pinned command visible in the Demo.</summary>
    [Fact]
    public void InstallationCopiesTheDisplayedComponentVersion()
    {
        JSInterop.SetupVoid("navigator.clipboard.writeText", _ => true);
        var cut = RenderComponent<BzInstallBox>(parameters => parameters
            .Add(component => component.PackageName, "BlazzyMotion.Carousel"));
        var displayed = cut.Find("code").TextContent;
        displayed.Should().Contain($"--version {AppVersions.Carousel}");
        cut.Find("button").Click();
        JSInterop.VerifyInvoke("navigator.clipboard.writeText").Arguments[0].Should().Be(displayed);
    }

    /// <summary>Retains unversioned installation examples for packages outside the built-in collection.</summary>
    [Fact]
    public void UnknownPackageRetainsItsOriginalCommand()
    {
        var cut = RenderComponent<BzInstallBox>(parameters => parameters
            .Add(component => component.PackageName, "Other.Package"));
        cut.Find("code").TextContent.Should().Be("dotnet add package Other.Package");
        AppVersions.GetBadgeText("Unknown").Should().Be("v0.0.0");
    }
}
