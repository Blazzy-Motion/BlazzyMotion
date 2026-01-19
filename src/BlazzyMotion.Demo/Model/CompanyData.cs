using BlazzyMotion.Core.Attributes;

namespace BlazzyMotion.Demo.Model;

/// <summary>
/// Demo model for company data in interactive dashboard.
/// </summary>
public class CompanyData
{
    [BzImage]
    public string ImageUrl { get; set; } = "";

    [BzTitle]
    public string Name { get; set; } = "";

    public string Revenue { get; set; } = "";
    public string RevenueTrend { get; set; } = "";
    public string Users { get; set; } = "";
    public string UsersTrend { get; set; } = "";
    public string Growth { get; set; } = "";
    public string GrowthTrend { get; set; } = "";
    public string Rating { get; set; } = "";
    public string MarketCap { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";
}
