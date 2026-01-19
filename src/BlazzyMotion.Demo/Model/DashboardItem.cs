using BlazzyMotion.Core.Attributes;

namespace BlazzyMotion.Demo.Model;

/// <summary>
/// Demo model for BzBento paginated mode - dashboard metrics.
/// </summary>
public class DashboardItem
{
    [BzImage]
    public string ImageUrl { get; set; } = "";

    [BzTitle]
    public string Title { get; set; } = "";

    public string Value { get; set; } = "";

    public string Trend { get; set; } = "";
}
