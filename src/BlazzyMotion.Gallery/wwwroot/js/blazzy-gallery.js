/**
 * BlazzyMotion Gallery - JavaScript Module
 * Grid/Masonry layout, staggered animations, lightbox, filtering
 */

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

        // Touch swipe for lightbox (delegated)
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

            // Only handle horizontal swipes (not vertical scroll)
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

        // Show gallery with smooth fade-in
        requestAnimationFrame(() => {
            requestAnimationFrame(() => {
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
 * Filter gallery items by category with animation
 */
export function filterGallery(element, category) {
    if (!galleryInstances.has(element)) return;

    const instance = galleryInstances.get(element);
    const items = instance.grid.querySelectorAll('.bzg-item');

    // Items are shown/hidden via Blazor CSS class toggle
    // JS just handles re-animation of visible items
    requestAnimationFrame(() => {
        let visibleIndex = 0;
        items.forEach(item => {
            if (!item.classList.contains('bzg-item-hidden')) {
                item.style.animationDelay = `${visibleIndex * 30}ms`;
                item.classList.remove('bzg-animate');
                void item.offsetWidth; // Trigger reflow
                item.classList.add('bzg-animate');
                visibleIndex++;
            }
        });
    });
}

/**
 * Recalculate masonry layout after filter/resize
 */
export function recalculateMasonry(element) {
    // CSS columns handle masonry automatically
    // This function exists for future JS-based masonry if needed
    if (!galleryInstances.has(element)) return;

    const instance = galleryInstances.get(element);
    // Force reflow for masonry recalculation
    instance.grid.style.display = 'none';
    void instance.grid.offsetHeight;
    instance.grid.style.display = '';
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
