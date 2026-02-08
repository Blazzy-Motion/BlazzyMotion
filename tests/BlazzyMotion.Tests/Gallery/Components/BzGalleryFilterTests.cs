namespace BlazzyMotion.Tests.Gallery.Components;

/// <summary>
/// Tests for BzGallery filtering behavior.
/// </summary>
public class BzGalleryFilterTests : TestBase
{
    private static List<TestGalleryPhoto> CreatePhotosWithCategories() => new()
    {
        new TestGalleryPhoto { Id = 1, ImageUrl = "https://example.com/1.jpg", Title = "Nature 1", Description = "Landscape", Category = "Nature" },
        new TestGalleryPhoto { Id = 2, ImageUrl = "https://example.com/2.jpg", Title = "City 1", Description = "Urban", Category = "City" },
        new TestGalleryPhoto { Id = 3, ImageUrl = "https://example.com/3.jpg", Title = "Nature 2", Description = "Landscape", Category = "Nature" },
        new TestGalleryPhoto { Id = 4, ImageUrl = "https://example.com/4.jpg", Title = "City 2", Description = "Urban", Category = "City" },
        new TestGalleryPhoto { Id = 5, ImageUrl = "https://example.com/5.jpg", Title = "Abstract 1", Description = "Art", Category = "Abstract" },
    };

    private static List<TestGalleryPhoto> CreatePhotosWithSingleCategory() => new()
    {
        new TestGalleryPhoto { Id = 1, ImageUrl = "https://example.com/1.jpg", Title = "Photo 1", Description = "Nature", Category = "Nature" },
        new TestGalleryPhoto { Id = 2, ImageUrl = "https://example.com/2.jpg", Title = "Photo 2", Description = "Nature", Category = "Nature" },
    };

    #region Filter Bar Visibility Tests

    [Fact]
    public void BzGallery_EnableFilterWithMultipleCategories_ShouldShowFilterBar()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Assert
        cut.Markup.Should().Contain("bzg-filter-bar");
    }

    [Fact]
    public void BzGallery_EnableFilterFalse_ShouldNotShowFilterBar()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, false)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Assert
        cut.Markup.Should().NotContain("bzg-filter-bar");
    }

    [Fact]
    public void BzGallery_EnableFilterWithSingleCategory_ShouldNotShowFilterBar()
    {
        // Arrange
        var items = CreatePhotosWithSingleCategory();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Assert
        cut.Markup.Should().NotContain("bzg-filter-bar");
    }

    [Fact]
    public void BzGallery_DefaultEnableFilter_ShouldNotShowFilterBar()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        cut.Markup.Should().NotContain("bzg-filter-bar");
    }

    #endregion

    #region Filter Bar Content Tests

    [Fact]
    public void BzGallery_FilterBar_ShouldShowAllButton()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Assert
        var filterButtons = cut.FindAll(".bzg-filter-btn");
        filterButtons.Should().Contain(b => b.TextContent.Trim() == "All");
    }

    [Fact]
    public void BzGallery_FilterBar_ShouldShowCategoryButtons()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Assert
        var filterButtons = cut.FindAll(".bzg-filter-btn");
        // "All" + 3 categories (Abstract, City, Nature)
        filterButtons.Should().HaveCount(4);
    }

    [Fact]
    public void BzGallery_FilterBar_CategoriesShouldBeSortedAlphabetically()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Assert
        var filterButtons = cut.FindAll(".bzg-filter-btn");
        // Skip the first ("All") button, remaining should be alphabetically sorted
        var categoryTexts = filterButtons.Skip(1).Select(b => b.TextContent.Trim()).ToList();
        categoryTexts.Should().BeInAscendingOrder();
        categoryTexts.Should().ContainInOrder("Abstract", "City", "Nature");
    }

    [Fact]
    public void BzGallery_FilterBar_AllButtonShouldBeActiveByDefault()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Assert
        var allButton = cut.FindAll(".bzg-filter-btn").First(b => b.TextContent.Trim() == "All");
        allButton.ClassList.Should().Contain("bzg-filter-active");
    }

    #endregion

    #region Filter Click Behavior Tests

    [Fact]
    public void BzGallery_ClickCategoryButton_ShouldFilterItems()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Act - click the "Nature" category button
        var natureButton = cut.FindAll(".bzg-filter-btn").First(b => b.TextContent.Trim() == "Nature");
        natureButton.Click();

        // Assert - all items in DOM, but only Nature items visible (2 out of 5)
        var allItems = cut.FindAll(".bzg-item");
        allItems.Should().HaveCount(5);
        var hiddenItems = cut.FindAll(".bzg-item-hidden");
        hiddenItems.Should().HaveCount(3);
        var visibleItems = allItems.Where(i => !i.ClassList.Contains("bzg-item-hidden")).ToList();
        visibleItems.Should().HaveCount(2);
    }

    [Fact]
    public void BzGallery_ClickCategoryButton_ShouldSetActiveClass()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Act
        var cityButton = cut.FindAll(".bzg-filter-btn").First(b => b.TextContent.Trim() == "City");
        cityButton.Click();

        // Assert
        var activeCityButton = cut.FindAll(".bzg-filter-btn").First(b => b.TextContent.Trim() == "City");
        activeCityButton.ClassList.Should().Contain("bzg-filter-active");

        var allButton = cut.FindAll(".bzg-filter-btn").First(b => b.TextContent.Trim() == "All");
        allButton.ClassList.Should().NotContain("bzg-filter-active");
    }

    [Fact]
    public void BzGallery_ClickAllButton_ShouldShowAllItems()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // First filter by city
        var cityButton = cut.FindAll(".bzg-filter-btn").First(b => b.TextContent.Trim() == "City");
        cityButton.Click();

        // Act - click All to reset
        var allButton = cut.FindAll(".bzg-filter-btn").First(b => b.TextContent.Trim() == "All");
        allButton.Click();

        // Assert
        var visibleItems = cut.FindAll(".bzg-item");
        visibleItems.Should().HaveCount(5);
    }

    #endregion

    #region CategorySelector Fallback Tests

    [Fact]
    public void BzGallery_WithoutCategorySelector_ShouldFallbackToDescription()
    {
        // Arrange - items with different descriptions act as categories
        var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "https://example.com/1.jpg", Title = "P1", Description = "Landscape" },
            new() { Id = 2, ImageUrl = "https://example.com/2.jpg", Title = "P2", Description = "Urban" },
            new() { Id = 3, ImageUrl = "https://example.com/3.jpg", Title = "P3", Description = "Landscape" },
        };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true));

        // Assert - filter bar should show because there are 2 distinct descriptions
        cut.Markup.Should().Contain("bzg-filter-bar");
        var buttons = cut.FindAll(".bzg-filter-btn");
        buttons.Should().HaveCount(3); // All + Landscape + Urban
    }

    [Fact]
    public void BzGallery_WithCategorySelector_ShouldUseCustomExtraction()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Assert - should show Category property values, not Description
        var buttonTexts = cut.FindAll(".bzg-filter-btn").Select(b => b.TextContent.Trim()).ToList();
        buttonTexts.Should().Contain("Nature");
        buttonTexts.Should().Contain("City");
        buttonTexts.Should().Contain("Abstract");
    }

    #endregion

    #region Null/Empty Category Tests

    [Fact]
    public void BzGallery_ItemsWithNullCategories_ShouldSkipNullCategories()
    {
        // Arrange
        var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "https://example.com/1.jpg", Title = "P1", Description = "Nature", Category = "Nature" },
            new() { Id = 2, ImageUrl = "https://example.com/2.jpg", Title = "P2", Description = null, Category = null },
            new() { Id = 3, ImageUrl = "https://example.com/3.jpg", Title = "P3", Description = "Urban", Category = "Urban" },
        };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category ?? string.Empty));

        // Assert - null/empty categories are skipped, only Nature and Urban remain
        var buttons = cut.FindAll(".bzg-filter-btn");
        buttons.Should().HaveCount(3); // All + Nature + Urban
    }

    [Fact]
    public void BzGallery_ItemsWithEmptyStringCategories_ShouldSkipEmptyCategories()
    {
        // Arrange
        var items = new List<TestGalleryPhoto>
        {
            new() { Id = 1, ImageUrl = "https://example.com/1.jpg", Title = "P1", Description = "Nature", Category = "Nature" },
            new() { Id = 2, ImageUrl = "https://example.com/2.jpg", Title = "P2", Description = "  ", Category = "  " },
            new() { Id = 3, ImageUrl = "https://example.com/3.jpg", Title = "P3", Description = "Urban", Category = "Urban" },
        };

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Assert - whitespace-only categories are skipped
        var buttons = cut.FindAll(".bzg-filter-btn");
        buttons.Should().HaveCount(3); // All + Nature + Urban
    }

    #endregion

    #region Disable Filter Tests

    [Fact]
    public void BzGallery_DisableFilter_ShouldClearActiveCategory()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Filter by Nature first
        var natureButton = cut.FindAll(".bzg-filter-btn").First(b => b.TextContent.Trim() == "Nature");
        natureButton.Click();

        // Act - disable the filter
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, false)
            .Add(p => p.CategorySelector, (Func<TestGalleryPhoto, string>)(p => p.Category!)));

        // Assert - all items should be visible, no filter bar
        cut.Markup.Should().NotContain("bzg-filter-bar");
        var visibleItems = cut.FindAll(".bzg-item");
        visibleItems.Should().HaveCount(5);
    }

    [Fact]
    public void BzGallery_FilterBarWithDataCategory_ShouldSetDataAttribute()
    {
        // Arrange
        var items = CreatePhotosWithCategories();

        // Act
        var cut = RenderComponent<BzGallery<TestGalleryPhoto>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.EnableFilter, true)
            .Add(p => p.CategorySelector, p => p.Category!));

        // Assert - items should have data-category attribute
        cut.Markup.Should().Contain("data-category");
    }

    #endregion
}
