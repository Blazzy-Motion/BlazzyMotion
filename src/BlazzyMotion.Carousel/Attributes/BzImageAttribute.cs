// ═══════════════════════════════════════════════════════════════════════════════
// BACKWARD COMPATIBILITY - LEGACY NAMESPACE
// ═══════════════════════════════════════════════════════════════════════════════
// This file provides backward compatibility for code that references
// BlazzyMotion.Carousel.Attributes.BzImageAttribute
// 
// The recommended namespace is now BlazzyMotion.Core.Attributes
// Both namespaces work identically with the Source Generator.
// ═══════════════════════════════════════════════════════════════════════════════

namespace BlazzyMotion.Carousel.Attributes
{
    /// <summary>
    /// Marks a property as the image source for carousel items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Migration Notice:</strong> This attribute has been moved to
    /// <c>BlazzyMotion.Core.Attributes.BzImageAttribute</c>.
    /// This version is provided for backward compatibility and works identically.
    /// </para>
    /// <para>
    /// <strong>Recommended:</strong> Update your using statements to:
    /// <code>
    /// using BlazzyMotion.Core.Attributes;
    /// </code>
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Legacy usage (still works)
    /// using BlazzyMotion.Carousel.Attributes;
    /// 
    /// public class Movie
    /// {
    ///     [BzImage]
    ///     public string PosterUrl { get; set; } = "";
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class BzImageAttribute : Attribute
    {
        // MARKER ATTRIBUTE - BACKWARD COMPATIBLE
        // The Source Generator recognizes both this attribute and the Core version.
        // No functional difference between the two.
    }
}