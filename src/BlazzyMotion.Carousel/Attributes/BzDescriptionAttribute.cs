// ═══════════════════════════════════════════════════════════════════════════════
// BACKWARD COMPATIBILITY - LEGACY NAMESPACE
// ═══════════════════════════════════════════════════════════════════════════════
// This file provides backward compatibility for code that references
// BlazzyMotion.Carousel.Attributes.BzDescriptionAttribute
// 
// The recommended namespace is now BlazzyMotion.Core.Attributes
// Both namespaces work identically with the Source Generator.
// ═══════════════════════════════════════════════════════════════════════════════

namespace BlazzyMotion.Carousel.Attributes
{
    /// <summary>
    /// Marks a property as the description for carousel items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Migration Notice:</strong> This attribute has been moved to
    /// <c>BlazzyMotion.Core.Attributes.BzDescriptionAttribute</c>.
    /// This version is provided for backward compatibility and works identically.
    /// </para>
    /// <para>
    /// Currently reserved for future use (tooltips, captions, etc.).
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class BzDescriptionAttribute : Attribute
    {
        // Reserved for future use - backward compatible
        // Could be used for: tooltips, captions, aria-describedby, etc.
    }
}