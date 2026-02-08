let galleryLoaded = false;
const galleryInstances = new Map();

/* CSS/Resource Loading */

function loadStylesheet(href) {
    return new Promise((resolve, reject) => {
        if (document.querySelector(`link[href="${href}"]`)) {
            resolve();
            return;
        }

        const link = document.createElement("link");
        link.rel = "stylesheet";
        link.href = href;
        link.onload = () => resolve();
        link.onerror = () => reject(new Error(`Failed to load stylesheet: ${href}`));
        document.head.appendChild(link);
    });
}

async function ensureGalleryLoaded() {
    if (galleryLoaded) return;

    await Promise.all([
        loadStylesheet("_content/BlazzyMotion.Core/css/blazzy-core.css"),
        loadStylesheet("_content/BlazzyMotion.Gallery/css/blazzy-gallery.css"),
    ]);

    galleryLoaded = true;
}

/* Gallery Initialization */

/**
 * Initialize BzGallery with staggered animations and layout
 * @param {HTMLElement} element - The gallery container element
 * @param {string} optionsJson - JSON string with options
 * @param {object} dotNetRef - .NET object reference for callbacks
 */
export async function initializeGallery(element, optionsJson, dotNetRef = null) {
    try {
        await ensureGalleryLoaded();

        // Destroy existing instance
        if (galleryInstances.has(element)) {
            destroyGallery(element);
        }

        const options = optionsJson ? JSON.parse(optionsJson) : {};
        const grid = element.querySelector('.bzg-grid');

        if (!grid) {
            console.warn('[BlazzyMotion] No .bzg-grid found in gallery');
            return;
        }

        const items = grid.querySelectorAll('.bzg-item');
        const staggerDelay = options.staggerDelay || 50;
        const animationEnabled = options.animationEnabled !== false;

        let observer = null;

        if (animationEnabled && items.length > 0) {
            // Intersection Observer for staggered entry animations
            observer = new IntersectionObserver((entries) => {
                entries.forEach((entry) => {
                    if (entry.isIntersecting) {
                        const item = entry.target;
                        const itemIndex = Array.from(items).indexOf(item);
                        item.style.animationDelay = `${itemIndex * staggerDelay}ms`;
                        item.classList.add('bzg-animate');
                        observer.unobserve(item);
                    }
                });
            }, {
                threshold: 0.1,
                rootMargin: '50px'
            });

            items.forEach(item => observer.observe(item));
        } else {
            // No animation — show immediately
            grid.classList.add('bzg-no-animation');
            items.forEach(item => {
                item.style.opacity = '1';
            });
        }

        // Touch swipe for lightbox
        let touchStartX = 0;
        let touchStartY = 0;

        const handleTouchStart = (e) => {
            touchStartX = e.touches[0].clientX;
            touchStartY = e.touches[0].clientY;
        };

        const handleTouchEnd = (e) => {
            const lightbox = document.querySelector('.bzg-lightbox-open');
            if (!lightbox) return;

            const deltaX = e.changedTouches[0].clientX - touchStartX;
            const deltaY = e.changedTouches[0].clientY - touchStartY;

            if (Math.abs(deltaX) > Math.abs(deltaY) && Math.abs(deltaX) > 50) {
                if (deltaX > 0) {
                    // Swipe right → previous
                    const prevBtn = lightbox.querySelector('.bzg-lightbox-prev');
                    if (prevBtn) prevBtn.click();
                } else {
                    // Swipe left → next
                    const nextBtn = lightbox.querySelector('.bzg-lightbox-next');
                    if (nextBtn) nextBtn.click();
                }
            }
        };

        document.addEventListener('touchstart', handleTouchStart, { passive: true });
        document.addEventListener('touchend', handleTouchEnd, { passive: true });

        // Reveal gallery
        requestAnimationFrame(() => {
            requestAnimationFrame(() => {
                element.style.removeProperty('opacity');
                element.style.removeProperty('visibility');
                element.classList.add('bzg-ready');
            });
        });

        // Store instance
        galleryInstances.set(element, {
            grid: grid,
            observer: observer,
            options: options,
            dotNetRef: dotNetRef,
            touchHandlers: { handleTouchStart, handleTouchEnd }
        });

        // Callback to Blazor
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync('OnGalleryInitializedFromJS', items.length)
                .catch(err => {
                    if (!err.message?.includes('disposed')) {
                        console.warn('[BlazzyMotion] Gallery init callback error:', err);
                    }
                });
        }

    } catch (err) {
        console.error("[BlazzyMotion] Gallery initialization error:", err);
    }
}

/* Lightbox Control */

/**
 * Open lightbox at specified index
 */
export function openLightbox(element, index) {
    if (!galleryInstances.has(element)) return;

    // Focus lightbox for keyboard events
    requestAnimationFrame(() => {
        const lightbox = element.querySelector('.bzg-lightbox');
        if (lightbox) lightbox.focus();
    });
}

/**
 * Close lightbox
 */
export function closeLightbox(element) {
    // Handled by Blazor component
}

/**
 * Focus the lightbox element for keyboard navigation
 */
export function focusLightbox() {
    requestAnimationFrame(() => {
        const lightbox = document.querySelector('.bzg-lightbox');
        if (lightbox) lightbox.focus();
    });
}

/* Filtering */

/**
 * Lock grid height before Blazor re-render to prevent layout jump
 */
export function prepareFilter(element) {
    if (!galleryInstances.has(element)) return;

    const instance = galleryInstances.get(element);
    const grid = instance.grid;
    if (!grid) return;

    // Cancel any pending filter cleanup
    if (instance.filterCleanupTimer) {
        clearTimeout(instance.filterCleanupTimer);
        instance.filterCleanupTimer = null;
    }

    // Lock current height to prevent jump during Blazor re-render
    grid.style.height = grid.offsetHeight + 'px';
    grid.style.overflow = 'hidden';
}

/**
 * Filter gallery items by category with smooth animation
 */
export function filterGallery(element, category) {
    if (!galleryInstances.has(element)) return;

    const instance = galleryInstances.get(element);
    const grid = instance.grid;
    const items = grid.querySelectorAll('.bzg-item');

    // Cancel any pending filter cleanup
    if (instance.filterCleanupTimer) {
        clearTimeout(instance.filterCleanupTimer);
        instance.filterCleanupTimer = null;
    }

    requestAnimationFrame(() => {
        // Phase 1: Set all visible items to invisible
        // Must disable CSS animation (bz-fade-in forwards keeps opacity:1) so inline styles take effect
        let visibleIndex = 0;
        items.forEach(item => {
            if (!item.classList.contains('bzg-item-hidden')) {
                item.style.animation = 'none';
                item.style.transition = 'none';
                item.style.opacity = '0';
                item.style.transform = 'scale(0.95)';
                visibleIndex++;
            }
        });

        // Force reflow to commit the opacity:0 state
        void grid.offsetWidth;

        // Phase 2: Fade in visible items with staggered delays
        let staggerIndex = 0;
        items.forEach(item => {
            if (!item.classList.contains('bzg-item-hidden')) {
                item.style.transition = 'opacity 0.35s ease, transform 0.35s ease';
                item.style.transitionDelay = `${staggerIndex * 30}ms`;
                item.style.opacity = '1';
                item.style.transform = 'scale(1)';
                staggerIndex++;
            }
        });

        // Phase 3: Smooth height transition
        requestAnimationFrame(() => {
            const lockedHeight = parseInt(grid.style.height);
            grid.style.height = 'auto';
            const newHeight = grid.offsetHeight;

            if (lockedHeight && lockedHeight !== newHeight) {
                grid.style.height = lockedHeight + 'px';
                grid.style.transition = 'height 0.35s cubic-bezier(0.4, 0, 0.2, 1)';
                void grid.offsetHeight;
                grid.style.height = newHeight + 'px';
            }

            // Clean up transition styles after completion
            // Keep animation:none + opacity:1 inline to prevent flash
            const maxDelay = staggerIndex * 30;
            instance.filterCleanupTimer = setTimeout(() => {
                grid.style.height = '';
                grid.style.transition = '';
                grid.style.overflow = '';
                items.forEach(item => {
                    item.style.transition = '';
                    item.style.transitionDelay = '';
                });
                instance.filterCleanupTimer = null;
            }, maxDelay + 400);
        });
    });
}

/**
 * Recalculate masonry layout after filter/resize
 */
export function recalculateMasonry(element) {
    // CSS columns handle masonry automatically
    // Force a gentle reflow without display:none to avoid breaking height transitions
    if (!galleryInstances.has(element)) return;

    const instance = galleryInstances.get(element);
    void instance.grid.offsetHeight;
}

/* Body Scroll Lock */

export function lockBodyScroll() {
    document.body.style.overflow = 'hidden';
}

export function unlockBodyScroll() {
    document.body.style.overflow = '';
}

/* Cleanup */

/**
 * Destroy gallery instance and clean up
 */
export function destroyGallery(element) {
    if (galleryInstances.has(element)) {
        const instance = galleryInstances.get(element);

        // Disconnect observer
        if (instance.observer) {
            instance.observer.disconnect();
        }

        // Remove touch handlers
        if (instance.touchHandlers) {
            document.removeEventListener('touchstart', instance.touchHandlers.handleTouchStart);
            document.removeEventListener('touchend', instance.touchHandlers.handleTouchEnd);
        }

        // Remove ready class
        element.classList.remove('bzg-ready');

        galleryInstances.delete(element);
    }
}
