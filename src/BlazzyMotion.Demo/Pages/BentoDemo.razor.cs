using BlazzyMotion.Demo.Services;

namespace BlazzyMotion.Demo.Pages;

/// <summary>Provides copyable consumer examples matching the live Bento controls and data.</summary>
public partial class BentoDemo
{
    /// <summary>Gets the item-based grid and its live data.</summary>
    private string ItemsCode => $$"""
        @using BlazzyMotion.Bento.Components
        @using BlazzyMotion.Core.Models
        @using YourApp.Models

        <BzBento TItem="BentoItem"
                 Items="@_bentoItems"
                 Theme="BzTheme.{{_currentTheme}}"
                 OnItemSelected="@OnItemSelected" />
        @if (_selectedItem != null)
        {
            <p>Selected: @_selectedItem.Title</p>
        }

        @code {
            private BentoItem? _selectedItem;
            private readonly List<BentoItem> _bentoItems = new()
            {
        {{ExampleSource.Items(_bentoItems, item => $"Title = {ExampleSource.Literal(item.Title)}, ImageUrl = {ExampleSource.Literal(item.ImageUrl, true)}")}}
            };

            /// <summary>Tracks the selected grid item.</summary>
            /// <param name="item">The selected item.</param>
            private void OnItemSelected(BentoItem item) => _selectedItem = item;
        }
        """;
    /// <summary>Gets the supported attribute model for this example.</summary>
    private string ItemModelCode => $$"""
        using BlazzyMotion.Core.Attributes;

        namespace YourApp.Models;

        /// <summary>Data displayed by the live BentoItem example.</summary>
        public class BentoItem
        {
            /// <summary>Gets or sets the Title value.</summary>
            [BzTitle]
            public string Title { get; set; } = "";

            /// <summary>Gets or sets the ImageUrl value.</summary>
            [BzImage]
            public string ImageUrl { get; set; } = "";
        }
        """;
    /// <summary>Gets the composed dashboard with its selection callback and company data.</summary>
    private string DashboardCode => $$"""
        @using BlazzyMotion.Bento.Components
        @using BlazzyMotion.Core.Models
        @using YourApp.Models
        @using BlazzyMotion.Carousel.Components

        <BzBento TItem="object" Theme="BzTheme.{{_currentTheme}}">

            <BzBentoItem ColSpan="2" RowSpan="2">
                <BzCarousel TItem="CompanyData"
                            Items="@_companies"
                            Theme="BzTheme.{{_currentTheme}}"
                            OnItemSelected="@OnCompanyChanged" />
            </BzBentoItem>

            <BzBentoMetric Value="@(_activeCompany?.Name ?? "Swipe")"
                           Label="Company">
            </BzBentoMetric>

            <BzBentoMetric Value="@(_activeCompany?.Revenue ?? "$0")"
                           Label="Revenue"
                           Trend="@(_activeCompany?.RevenueTrend ?? "")">
            </BzBentoMetric>

            <BzBentoMetric Value="@(_activeCompany?.Users ?? "0")"
                           Label="Users"
                           Trend="@(_activeCompany?.UsersTrend ?? "")">
            </BzBentoMetric>

            <BzBentoMetric Value="@(_activeCompany?.Growth ?? "0%")"
                           Label="Growth"
                           Trend="@(_activeCompany?.GrowthTrend ?? "")">
            </BzBentoMetric>

            <BzBentoFeature ColSpan="2"
                            Label="@(_activeCompany?.Name ?? "Select Company")"
                            Description="@(_activeCompany?.Description ?? "Swipe the carousel to see company details")">
            </BzBentoFeature>

            <BzBentoMetric Value="@(_activeCompany?.Rating ?? "0")"
                           Label="Rating">
            </BzBentoMetric>

            <BzBentoMetric Value="@(_activeCompany?.MarketCap ?? "$0")"
                           Label="Market Cap">
            </BzBentoMetric>

        </BzBento>

        @code {
            private CompanyData? _activeCompany;
            private readonly List<CompanyData> _companies = new()
            {
        {{ExampleSource.Items(_companies, item => $"Name = {ExampleSource.Literal(item.Name)}, ImageUrl = {ExampleSource.Literal(item.ImageUrl, true)}, Revenue = {ExampleSource.Literal(item.Revenue)}, RevenueTrend = {ExampleSource.Literal(item.RevenueTrend)}, Users = {ExampleSource.Literal(item.Users)}, UsersTrend = {ExampleSource.Literal(item.UsersTrend)}, Growth = {ExampleSource.Literal(item.Growth)}, GrowthTrend = {ExampleSource.Literal(item.GrowthTrend)}, Rating = {ExampleSource.Literal(item.Rating)}, MarketCap = {ExampleSource.Literal(item.MarketCap)}, Description = {ExampleSource.Literal(item.Description)}, Icon = {ExampleSource.Literal(item.Icon)}")}}
            };

            /// <summary>Initializes the dashboard with the first company.</summary>
            protected override void OnInitialized() => _activeCompany = _companies.FirstOrDefault();

            /// <summary>Updates the dashboard after a company is selected.</summary>
            /// <param name="company">The selected company.</param>
            private void OnCompanyChanged(CompanyData company) => _activeCompany = company;
        }
        """;
    /// <summary>Gets the supported attribute model for this example.</summary>
    private string CompanyModelCode => $$"""
        using BlazzyMotion.Core.Attributes;

        namespace YourApp.Models;

        /// <summary>Data displayed by the live CompanyData example.</summary>
        public class CompanyData
        {
            /// <summary>Gets or sets the Name value.</summary>
            [BzTitle]
            public string Name { get; set; } = "";

            /// <summary>Gets or sets the ImageUrl value.</summary>
            [BzImage]
            public string ImageUrl { get; set; } = "";

            /// <summary>Gets or sets the Revenue value.</summary>
            public string Revenue { get; set; } = "";

            /// <summary>Gets or sets the RevenueTrend value.</summary>
            public string RevenueTrend { get; set; } = "";

            /// <summary>Gets or sets the Users value.</summary>
            public string Users { get; set; } = "";

            /// <summary>Gets or sets the UsersTrend value.</summary>
            public string UsersTrend { get; set; } = "";

            /// <summary>Gets or sets the Growth value.</summary>
            public string Growth { get; set; } = "";

            /// <summary>Gets or sets the GrowthTrend value.</summary>
            public string GrowthTrend { get; set; } = "";

            /// <summary>Gets or sets the Rating value.</summary>
            public string Rating { get; set; } = "";

            /// <summary>Gets or sets the MarketCap value.</summary>
            public string MarketCap { get; set; } = "";

            /// <summary>Gets or sets the Description value.</summary>
            public string Description { get; set; } = "";

            /// <summary>Gets or sets the Icon value.</summary>
            public string Icon { get; set; } = "";
        }
        """;
    /// <summary>Gets the manually composed cards and metrics with the selected theme.</summary>
    private string CompositionCode => $$"""
        @using BlazzyMotion.Bento.Components
        @using BlazzyMotion.Core.Models

        <BzBento TItem="object" Theme="BzTheme.{{_currentTheme}}">
            <BzBentoCard TItem="object"
                         ColSpan="2" RowSpan="2"
                         Image="https://blazzymotion.com/images/joker.webp"
                         Title="Featured"
                         Description="Hero card with 2x2 span" />

            <BzBentoMetric Value="8" Label="Movies">
            </BzBentoMetric>

            <BzBentoMetric Value="4" Label="Themes">
            </BzBentoMetric>

            <BzBentoCard TItem="object"
                         Image="https://blazzymotion.com/images/venom.webp"
                         Title="Venom" />

            <BzBentoCard TItem="object"
                         Image="https://blazzymotion.com/images/wild-robot.webp"
                         Title="Wild Robot" />

            <BzBentoQuote TItem="object"
                          Text="Amazing UI components!"
                          Author="Dev Community"
                          Role="GitHub Stars"
                          Avatar="https://blazzymotion.com/images/avatar.webp" />

            <BzBentoMetric Value="60" Label="FPS">
            </BzBentoMetric>

            <BzBentoFeature ColSpan="2"
                            Label="Wide Card (2x1)"
                            Description="Perfect for CTAs or descriptions">
            </BzBentoFeature>

            <BzBentoMetric Value="99.9" Label="Uptime" Trend="+2%">
            </BzBentoMetric>

            <BzBentoMetric Value="24/7" Label="Support">
            </BzBentoMetric>

            <BzBentoMetric Value="50+" Label="Components">
            </BzBentoMetric>

            <BzBentoMetric Value="0" Label="Dependencies">
            </BzBentoMetric>
        </BzBento>
        """;
}
