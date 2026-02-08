using BlazzyMotion.Core.Models;
using BlazzyMotion.Core.Services;
using BlazzyMotion.Tests.Helpers;
using Bunit.JSInterop;

namespace BlazzyMotion.Tests;

/// <summary>
/// Base class for all component tests that use bUnit TestContext.
/// Configures JSInterop to prevent JavaScript module loading timeouts.
/// </summary>
public abstract class TestBase : TestContext
{
  protected BunitJSModuleInterop CoreModule { get; }

  protected TestBase()
  {
    // Register mapper for TestGalleryPhoto (source generator doesn't run on test project)
    if (!BzRegistry.HasMapper<TestGalleryPhoto>())
    {
      BzRegistry.Register<TestGalleryPhoto>(photo => new BzItem
      {
        ImageUrl = photo.ImageUrl,
        Title = photo.Title,
        Description = photo.Description,
        OriginalItem = photo
      });
    }

    // Configure JSInterop to Loose mode - automatically handles unmocked calls
    JSInterop.Mode = JSRuntimeMode.Loose;

    // Setup the module that BzCarouselJsInterop imports
    // Now points to Core module (v1.1.0+)
    CoreModule = JSInterop.SetupModule(
        "./_content/BlazzyMotion.Core/js/blazzy-core.js");

    // Setup all methods the module exposes
    // Updated signature for v1.2.0 - initializeCarousel now accepts dotNetRef
    CoreModule.SetupVoid("initializeCarousel", _ => true);
    CoreModule.SetupVoid("destroyCarousel", _ => true);
    CoreModule.SetupVoid("ensureSwiperLoaded", _ => true);
    CoreModule.Setup<int>("getActiveIndex", _ => true).SetResult(0);
    CoreModule.Setup<int>("getRealIndex", _ => true).SetResult(0);

    // Setup Bento methods
    CoreModule.SetupVoid("initializeBento", _ => true);
    CoreModule.SetupVoid("refreshBento", _ => true);
    CoreModule.SetupVoid("destroyBento", _ => true);

    // Setup Gallery module
    GalleryModule = JSInterop.SetupModule(
        "./_content/BlazzyMotion.Gallery/js/blazzy-gallery.js");

    GalleryModule.SetupVoid("initializeGallery", _ => true);
    GalleryModule.SetupVoid("destroyGallery", _ => true);
    GalleryModule.SetupVoid("filterGallery", _ => true);
    GalleryModule.SetupVoid("recalculateMasonry", _ => true);
    GalleryModule.SetupVoid("focusLightbox", _ => true);
    GalleryModule.SetupVoid("lockBodyScroll", _ => true);
    GalleryModule.SetupVoid("unlockBodyScroll", _ => true);
    GalleryModule.SetupVoid("openLightbox", _ => true);
    GalleryModule.SetupVoid("closeLightbox", _ => true);
  }

  // Legacy property for backward compatibility with existing tests
  protected BunitJSModuleInterop CarouselModule => CoreModule;
  protected BunitJSModuleInterop GalleryModule { get; }
}
