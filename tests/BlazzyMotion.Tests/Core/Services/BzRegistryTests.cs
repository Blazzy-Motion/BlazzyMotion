using BlazzyMotion.Tests.Helpers;

namespace BlazzyMotion.Tests.Core.Services;

/// <summary>
/// Tests for BzRegistry central mapper service.
/// </summary>
public class BzRegistryTests : IDisposable
{
  public BzRegistryTests()
  {
    // Clear registry before each test to ensure isolation
    BzRegistry.Clear();
  }

  public void Dispose()
  {
    // Clean up after each test
    BzRegistry.Clear();
  }

  #region Register<T> Tests

  [Fact]
  public void Register_ShouldRegisterMapperSuccessfully()
  {
    // Arrange
    Func<TestMovie, BzItem> mapper = m => new BzItem { ImageUrl = m.ImageUrl };

    // Act
    BzRegistry.Register(mapper);

    // Assert
    BzRegistry.HasMapper<TestMovie>().Should().BeTrue();
  }

  [Fact]
  public void Register_ShouldOverwriteExistingMapper()
  {
    // Arrange
    Func<TestMovie, BzItem> mapper1 = m => new BzItem { ImageUrl = "first" };
    Func<TestMovie, BzItem> mapper2 = m => new BzItem { ImageUrl = "second" };

    // Act
    BzRegistry.Register(mapper1);
    BzRegistry.Register(mapper2);

    var result = BzRegistry.ToBzItem(new TestMovie { ImageUrl = "test" });

    // Assert
    result.Should().NotBeNull();
    result!.ImageUrl.Should().Be("second");
  }

  [Fact]
  public void Register_ShouldWorkWithDifferentTypesIndependently()
  {
    // Arrange & Act
    BzRegistry.Register<TestMovie>(m => new BzItem { ImageUrl = m.ImageUrl });
    BzRegistry.Register<TestBook>(b => new BzItem { ImageUrl = b.CoverUrl });

    // Assert
    BzRegistry.HasMapper<TestMovie>().Should().BeTrue();
    BzRegistry.HasMapper<TestBook>().Should().BeTrue();
    BzRegistry.MapperCount.Should().BeGreaterThanOrEqualTo(2);
  }

  #endregion

  #region Register(Type, Func) Tests

  [Fact]
  public void RegisterWithType_ShouldRegisterMapper()
  {
    // Arrange
    Func<object, BzItem> mapper = obj => new BzItem { ImageUrl = ((TestMovie)obj).ImageUrl };

    // Act
    BzRegistry.Register(typeof(TestMovie), mapper);

    // Assert
    BzRegistry.HasMapper(typeof(TestMovie)).Should().BeTrue();
  }

  #endregion

  #region ToBzItems<T> Tests

  [Fact]
  public void ToBzItems_ReturnsEmptyList_WhenItemsIsNull()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem { ImageUrl = m.ImageUrl });

    // Act
    var result = BzRegistry.ToBzItems<TestMovie>(null);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeEmpty();
  }

  [Fact]
  public void ToBzItems_ReturnsEmptyList_WhenNoMapperRegistered()
  {
    // Arrange
    var items = new List<TestMovie> { new() { ImageUrl = "test.jpg" } };

    // Act
    var result = BzRegistry.ToBzItems(items);

    // Assert
    result.Should().BeEmpty();
  }

  [Fact]
  public void ToBzItems_ConvertsAllItemsSuccessfully()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem
    {
      ImageUrl = m.ImageUrl,
      Title = m.Title,
      OriginalItem = m
    });

    var items = new List<TestMovie>
        {
            new() { ImageUrl = "image1.jpg", Title = "Movie 1" },
            new() { ImageUrl = "image2.jpg", Title = "Movie 2" },
            new() { ImageUrl = "image3.jpg", Title = "Movie 3" }
        };

    // Act
    var result = BzRegistry.ToBzItems(items);

    // Assert
    result.Should().HaveCount(3);
    result[0].ImageUrl.Should().Be("image1.jpg");
    result[1].ImageUrl.Should().Be("image2.jpg");
    result[2].ImageUrl.Should().Be("image3.jpg");
  }

  [Fact]
  public void ToBzItems_SkipsNullItems()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem { ImageUrl = m.ImageUrl });

    var items = new List<TestMovie>
        {
            new() { ImageUrl = "image1.jpg" },
            null!,
            new() { ImageUrl = "image3.jpg" }
        };

    // Act
    var result = BzRegistry.ToBzItems(items);

    // Assert
    result.Should().HaveCount(2);
  }

  [Fact]
  public void ToBzItems_SkipsItemsThatThrowDuringMapping()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m =>
    {
      if (m.ImageUrl == "throw")
        throw new InvalidOperationException("Test exception");
      return new BzItem { ImageUrl = m.ImageUrl };
    });

    var items = new List<TestMovie>
        {
            new() { ImageUrl = "image1.jpg" },
            new() { ImageUrl = "throw" },
            new() { ImageUrl = "image3.jpg" }
        };

    // Act
    var result = BzRegistry.ToBzItems(items);

    // Assert
    result.Should().HaveCount(2);
    result[0].ImageUrl.Should().Be("image1.jpg");
    result[1].ImageUrl.Should().Be("image3.jpg");
  }

  [Fact]
  public void ToBzItems_PreservesItemOrder()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem { Title = m.Title });

    var items = new List<TestMovie>
        {
            new() { Title = "First" },
            new() { Title = "Second" },
            new() { Title = "Third" }
        };

    // Act
    var result = BzRegistry.ToBzItems(items);

    // Assert
    result[0].Title.Should().Be("First");
    result[1].Title.Should().Be("Second");
    result[2].Title.Should().Be("Third");
  }

  [Fact]
  public void ToBzItems_ReturnsIReadOnlyList()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem { ImageUrl = m.ImageUrl });
    var items = new List<TestMovie> { new() { ImageUrl = "test.jpg" } };

    // Act
    var result = BzRegistry.ToBzItems(items);

    // Assert
    result.Should().BeAssignableTo<IReadOnlyList<BzItem>>();
  }

  [Fact]
  public void ToBzItems_ReturnsEmptyList_WhenInputIsEmptyEnumerable()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem { ImageUrl = m.ImageUrl });
    var items = Enumerable.Empty<TestMovie>();

    // Act
    var result = BzRegistry.ToBzItems(items);

    // Assert
    result.Should().BeEmpty();
  }

  #endregion

  #region ToBzItem<T> Tests

  [Fact]
  public void ToBzItem_ReturnsNull_WhenItemIsNull()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem { ImageUrl = m.ImageUrl });

    // Act
    var result = BzRegistry.ToBzItem<TestMovie>(null);

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public void ToBzItem_ReturnsNull_WhenNoMapperRegistered()
  {
    // Arrange
    var item = new TestMovie { ImageUrl = "test.jpg" };

    // Act
    var result = BzRegistry.ToBzItem(item);

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public void ToBzItem_ReturnsNull_WhenMapperThrows()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => throw new InvalidOperationException("Test"));
    var item = new TestMovie { ImageUrl = "test.jpg" };

    // Act
    var result = BzRegistry.ToBzItem(item);

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public void ToBzItem_ReturnsBzItem_WhenSuccessful()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem
    {
      ImageUrl = m.ImageUrl,
      Title = m.Title
    });
    var item = new TestMovie { ImageUrl = "test.jpg", Title = "Test" };

    // Act
    var result = BzRegistry.ToBzItem(item);

    // Assert
    result.Should().NotBeNull();
    result!.ImageUrl.Should().Be("test.jpg");
    result.Title.Should().Be("Test");
  }

  #endregion

  #region HasMapper Tests

  [Fact]
  public void HasMapperGeneric_ReturnsFalse_WhenNoMapperRegistered()
  {
    // Assert
    BzRegistry.HasMapper<TestMovie>().Should().BeFalse();
  }

  [Fact]
  public void HasMapperGeneric_ReturnsTrue_WhenMapperRegistered()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem());

    // Assert
    BzRegistry.HasMapper<TestMovie>().Should().BeTrue();
  }

  [Fact]
  public void HasMapperType_ReturnsFalse_WhenNoMapperRegistered()
  {
    // Assert
    BzRegistry.HasMapper(typeof(TestMovie)).Should().BeFalse();
  }

  [Fact]
  public void HasMapperType_ReturnsTrue_WhenMapperRegistered()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem());

    // Assert
    BzRegistry.HasMapper(typeof(TestMovie)).Should().BeTrue();
  }

  #endregion

  #region MapperCount Tests

  [Fact]
  public void MapperCount_ReturnsZero_WhenEmpty()
  {
    // Assert
    BzRegistry.MapperCount.Should().Be(0);
  }

  [Fact]
  public void MapperCount_ReturnsCorrectCount_AfterRegistrations()
  {
    // Act
    BzRegistry.Register<TestMovie>(m => new BzItem());
    BzRegistry.Register<TestBook>(b => new BzItem());
    BzRegistry.Register<TestAlbum>(a => new BzItem());

    // Assert
    BzRegistry.MapperCount.Should().Be(3);
  }

  #endregion

  #region Unregister Tests

  [Fact]
  public void Unregister_ReturnsTrue_WhenMapperRemoved()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem());

    // Act
    var result = BzRegistry.Unregister<TestMovie>();

    // Assert
    result.Should().BeTrue();
    BzRegistry.HasMapper<TestMovie>().Should().BeFalse();
  }

  [Fact]
  public void Unregister_ReturnsFalse_WhenNoMapperExisted()
  {
    // Act
    var result = BzRegistry.Unregister<TestMovie>();

    // Assert
    result.Should().BeFalse();
  }

  [Fact]
  public void Unregister_OnlyRemovesSpecifiedType()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem());
    BzRegistry.Register<TestBook>(b => new BzItem());

    // Act
    BzRegistry.Unregister<TestMovie>();

    // Assert
    BzRegistry.HasMapper<TestMovie>().Should().BeFalse();
    BzRegistry.HasMapper<TestBook>().Should().BeTrue();
  }

  #endregion

  #region Clear Tests

  [Fact]
  public void Clear_RemovesAllMappers()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem());
    BzRegistry.Register<TestBook>(b => new BzItem());

    // Act
    BzRegistry.Clear();

    // Assert
    BzRegistry.HasMapper<TestMovie>().Should().BeFalse();
    BzRegistry.HasMapper<TestBook>().Should().BeFalse();
  }

  [Fact]
  public void Clear_SetsMapperCountToZero()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem());
    BzRegistry.Register<TestBook>(b => new BzItem());

    // Act
    BzRegistry.Clear();

    // Assert
    BzRegistry.MapperCount.Should().Be(0);
  }

  #endregion

  #region Thread Safety Tests

  [Fact]
  public void ConcurrentRegister_DoesNotCorruptState()
  {
    // Arrange & Act
    Parallel.For(0, 100, i =>
    {
      BzRegistry.Register<TestMovie>(m => new BzItem { Title = $"Movie {i}" });
    });

    // Assert - should have exactly one mapper (last one wins)
    BzRegistry.HasMapper<TestMovie>().Should().BeTrue();
    BzRegistry.MapperCount.Should().BeGreaterThanOrEqualTo(1);
  }

  [Fact]
  public void ConcurrentToBzItems_WorksCorrectly()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem { ImageUrl = m.ImageUrl });
    var items = Enumerable.Range(1, 100)
        .Select(i => new TestMovie { ImageUrl = $"image{i}.jpg" })
        .ToList();

    // Act
    var results = new List<IReadOnlyList<BzItem>>();
    Parallel.For(0, 10, _ =>
    {
      var result = BzRegistry.ToBzItems(items);
      lock (results)
      {
        results.Add(result);
      }
    });

    // Assert
    results.Should().HaveCount(10);
    results.All(r => r.Count == 100).Should().BeTrue();
  }

  #endregion

  #region Mapper Exception Handling Tests

  [Fact]
  public void ToBzItems_ContinuesProcessing_WhenMapperThrowsForSomeItems()
  {
    // Arrange
    int callCount = 0;
    BzRegistry.Register<TestMovie>(m =>
    {
      callCount++;
      if (callCount == 2)
        throw new InvalidOperationException("Test exception");
      return new BzItem { ImageUrl = m.ImageUrl };
    });

    var items = new List<TestMovie>
    {
      new() { ImageUrl = "image1.jpg" },
      new() { ImageUrl = "image2.jpg" }, // This will throw
      new() { ImageUrl = "image3.jpg" }
    };

    // Act
    var result = BzRegistry.ToBzItems(items);

    // Assert - should have 2 items (skipped the one that threw)
    result.Should().HaveCount(2);
    result[0].ImageUrl.Should().Be("image1.jpg");
    result[1].ImageUrl.Should().Be("image3.jpg");
  }

  [Fact]
  public void ToBzItem_ReturnsNull_WhenMapperThrowsException()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => throw new InvalidOperationException("Test"));
    var item = new TestMovie { ImageUrl = "test.jpg" };

    // Act
    var result = BzRegistry.ToBzItem(item);

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public void ToBzItems_HandlesAllItemsThrowingException()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => throw new InvalidOperationException("Always throws"));

    var items = new List<TestMovie>
    {
      new() { ImageUrl = "image1.jpg" },
      new() { ImageUrl = "image2.jpg" }
    };

    // Act
    var result = BzRegistry.ToBzItems(items);

    // Assert
    result.Should().BeEmpty();
  }

  #endregion

  #region HasMapper Type Parameter Tests

  [Fact]
  public void HasMapper_ReturnsFalse_ForUnregisteredType()
  {
    // Act & Assert
    BzRegistry.HasMapper(typeof(TestAlbum)).Should().BeFalse();
  }

  [Fact]
  public void HasMapper_ReturnsTrue_AfterRegisterWithType()
  {
    // Arrange
    BzRegistry.Register(typeof(TestAlbum), obj => new BzItem());

    // Act & Assert
    BzRegistry.HasMapper(typeof(TestAlbum)).Should().BeTrue();
  }

  #endregion

  #region Empty/Edge Case Tests

  [Fact]
  public void ToBzItems_ReturnsEmptyList_ForEmptyEnumerable()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem { ImageUrl = m.ImageUrl });
    var items = new List<TestMovie>();

    // Act
    var result = BzRegistry.ToBzItems(items);

    // Assert
    result.Should().BeEmpty();
  }

  [Fact]
  public void MapperCount_ReturnsZero_WhenNoMappersRegistered()
  {
    // Assert
    BzRegistry.MapperCount.Should().Be(0);
  }

  [Fact]
  public void Clear_CanBeCalledMultipleTimes()
  {
    // Arrange
    BzRegistry.Register<TestMovie>(m => new BzItem());

    // Act
    BzRegistry.Clear();
    BzRegistry.Clear();
    BzRegistry.Clear();

    // Assert - should not throw
    BzRegistry.MapperCount.Should().Be(0);
  }

  [Fact]
  public void Unregister_CanBeCalledForNonExistentType()
  {
    // Act
    var result = BzRegistry.Unregister<TestAlbum>();

    // Assert
    result.Should().BeFalse();
  }

  #endregion
}
