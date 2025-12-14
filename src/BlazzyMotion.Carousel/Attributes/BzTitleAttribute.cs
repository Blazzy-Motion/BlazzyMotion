// ═══════════════════════════════════════════════════════════════════════════════
// BACKWARD COMPATIBILITY - LEGACY NAMESPACE
// ═══════════════════════════════════════════════════════════════════════════════
// This file provides backward compatibility for code that references
// BlazzyMotion.Carousel.Attributes.BzTitleAttribute
// 
// The recommended namespace is now BlazzyMotion.Core.Attributes
// Both namespaces work identically with the Source Generator.
// ═══════════════════════════════════════════════════════════════════════════════

namespace BlazzyMotion.Carousel.Attributes
{
    /// <summary>
    /// Marks a property as the title/alt text for carousel items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Migration Notice:</strong> This attribute has been moved to
    /// <c>BlazzyMotion.Core.Attributes.BzTitleAttribute</c>.
    /// This version is provided for backward compatibility and works identically.
    /// </para>
    /// <para>
    /// <strong>Recommended:</strong> Update your using statements to:
    /// <code>
    /// using BlazzyMotion.Core.Attributes;
    /// </code>
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class BzTitleAttribute : Attribute
    {
        // Marker attribute for alt/title text - backward compatible
    }
}