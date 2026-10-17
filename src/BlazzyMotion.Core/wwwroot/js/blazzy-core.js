let swiperLoaded = false;
let coreStylesLoaded = false;
const swiperInstances = new Map();
const carouselBindings = new WeakMap();
const bentoInstances = new Map();

// script/stylesheet loaders


function loadScript(src) {
    return new Promise((resolve, reject) => {
        const existing = document.querySelector(`script[src="${src}"]`);

        if (existing) {
            if (typeof Swiper !== "undefined") {
                resolve();
                return;
            }

            const check = setInterval(() => {
                if (typeof Swiper !== "undefined") {
                    clearInterval(check);
                    resolve();
                }
            }, 100);
            return;
        }

        const script = document.createElement("script");
        script.src = src;
        script.async = false;

        script.onload = () => {
            let tries = 0;

            const check = setInterval(() => {
                tries++;

                if (typeof Swiper !== "undefined") {
                    clearInterval(check);
                    resolve();
                }
                else if (tries > 50) {
                    clearInterval(check);
                    reject(new Error("Swiper script loaded but not defined"));
                }
            }, 100);
        };

        script.onerror = () => reject(new Error(`Failed to load script: ${src}`));
        document.head.appendChild(script);
    });
}

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


// Core styles loading (shared by all components)

export async function ensureCoreStylesLoaded() {
    if (coreStylesLoaded) return;
    await loadStylesheet("_content/BlazzyMotion.Core/css/blazzy-core.css");
    coreStylesLoaded = true;
}

// Swiper loading

export async function ensureSwiperLoaded() {
    if (swiperLoaded) return;

    await Promise.all([
        loadStylesheet("_content/BlazzyMotion.Core/css/swiper-bundle.min.css"),
        ensureCoreStylesLoaded(),
    ]);

    await loadScript("_content/BlazzyMotion.Core/js/swiper-bundle.min.js");

    if (typeof Swiper === "undefined") {
        throw new Error("Swiper is not defined after load");
    }

    swiperLoaded = true;
}

const DEFAULT_TOUCH_SETTINGS = {
    touchRatio: 1.0,
    threshold: 10,
    shortSwipes: false,
    resistanceRatio: 0.85,
    longSwipesRatio: 0.3
};

// CAROUSEL INITIALIZATION

/**
 * Initialize BzCarousel without changing the consumer's Coverflow geometry.
 * @param {HTMLElement} element - Carousel root, including its accessible controls.
 * @param {string} optionsJson - Serialized component options.
 * @param {object|null} dotNetRef - Existing Blazor selection callback reference.
 * @returns {Promise<void>} Resolves after binding the Swiper instance.
 */
export async function initializeCarousel(element, optionsJson, dotNetRef = null) {
    try {
        const container = element.querySelector(".swiper-container");

        if (!container) {
            console.warn("[BlazzyMotion] No .swiper-container found in element");
            return;
        }

        destroyCarousel(element);
        const motion = window.matchMedia('(prefers-reduced-motion: reduce)');
        const bindings = new AbortController();
        carouselBindings.set(element, bindings);
        const options = optionsJson ? JSON.parse(optionsJson) : {};
        const wrapper = container.querySelector('.swiper-wrapper');
        const originalSlides = wrapper.querySelectorAll('.swiper-slide');
        const slideCount = originalSlides.length;
        if (!slideCount) return;
        const status = element.querySelector('.bzc-status');

        const minSlidesForLoop = 4;
        const minSlidesForAutoLoop = 7;
        const shouldLoop = options.loop === true && slideCount >= minSlidesForLoop;

        // Store original slide count for correct realIndex calculation with clones
        const originalSlideCount = slideCount;

        if (shouldLoop && slideCount < minSlidesForAutoLoop) {
            const slidesToAdd = minSlidesForAutoLoop - slideCount + 2;
            for (let i = 0; i < slidesToAdd; i++) {
                const clone = originalSlides[i % slideCount].cloneNode(true);
                clone.setAttribute('data-bz-clone', 'true');
                wrapper.appendChild(clone);
            }
        }

        const touchRatio = options.touchRatio ?? DEFAULT_TOUCH_SETTINGS.touchRatio;
        const threshold = options.threshold ?? DEFAULT_TOUCH_SETTINGS.threshold;
        const shortSwipes = options.shortSwipes ?? DEFAULT_TOUCH_SETTINGS.shortSwipes;
        const resistanceRatio = options.resistanceRatio ?? DEFAULT_TOUCH_SETTINGS.resistanceRatio;
        const longSwipesRatio = options.longSwipesRatio ?? DEFAULT_TOUCH_SETTINGS.longSwipesRatio;

        const swiperConfig = {
            // Effect settings
            effect: motion.matches ? "slide" : (options.effect || "coverflow"),
            grabCursor: options.grabCursor ?? true,
            centeredSlides: options.centeredSlides ?? true,
            spaceBetween: options.spaceBetween || 0,
            slidesPerView: options.slidesPerView || "auto",
            initialSlide: options.initialSlide || 0,
            loop: shouldLoop,
            speed: 0,
            runCallbacksOnInit: false,
            slideToClickedSlide: true,
            watchSlidesProgress: true,
            observer: true,
            observeParents: true,
            // Touch settings - mobile optimization
            touchRatio: touchRatio,
            threshold: threshold,
            shortSwipes: shortSwipes,
            resistanceRatio: resistanceRatio,
            longSwipesRatio: longSwipesRatio,

            coverflowEffect: {
                rotate: options.rotateDegree || 50,
                stretch: options.stretch || 0,
                depth: options.depth || 150,
                modifier: options.modifier || 1.5,
                slideShadows: options.slideShadows ?? false,
            },

            on: {
                afterInit: function () {
                    this.params.speed = motion.matches ? 0 : (options.speed || 300);
                    this.params.runCallbacksOnInit = true;

                    // Show carousel after initialization
                    requestAnimationFrame(() => {
                        requestAnimationFrame(() => {
                            if (this.el) {
                                this.el.classList.remove('bzc-hidden', 'bzc-pending');
                                this.el.classList.add('bzc-visible');
                            }
                        });
                    });
                },

                slideChange: function () {
                    updateCarouselAccessibility(this, originalSlideCount, status);
                    if (dotNetRef) {
                        // Use modulo to get correct index when clones are present
                        const realIndex = this.realIndex % originalSlideCount;
                        dotNetRef.invokeMethodAsync('OnSlideChangeFromJS', realIndex)
                            .catch(err => {
                                if (!err.message?.includes('disposed')) {
                                    console.warn('[BlazzyMotion] slideChange callback error:', err);
                                }
                            });
                    }
                }
            }
        };

        if (shouldLoop) {
            swiperConfig.loopedSlides = slideCount;
            swiperConfig.loopAdditionalSlides = 2;
        }

        // Remove fallback-only layout before Swiper measures the original slide geometry.
        container.classList.remove('bzc-pending', 'bzc-hidden');
        container.classList.add('bzc-visible');
        const swiperInstance = new Swiper(container, swiperConfig);

        // Store instance for later reference
        swiperInstances.set(element, swiperInstance);
        updateCarouselAccessibility(swiperInstance, originalSlideCount, null);
        element.querySelectorAll('[data-bzc-direction]').forEach(control => { control.disabled = false; });
        bindCarouselInput(element, swiperInstance, originalSlides, bindings.signal);
        motion.addEventListener('change', () => {
            if (swiperInstance.destroyed) return;
            initializeCarousel(element, optionsJson, dotNetRef);
        }, { signal: bindings.signal });

    } catch (err) {
        console.error("[BlazzyMotion] Initialization error:", err);
    }
}

/**
 * Update slide semantics, including the currently active loop copy.
 * @param {object} swiper - Current Swiper instance.
 * @param {number} count - Number of consumer items, excluding loop copies.
 * @param {HTMLElement|null} status - Optional polite announcement target.
 */
function updateCarouselAccessibility(swiper, count, status) {
    if (swiper.destroyed) return;
    const index = ((swiper.realIndex % count) + count) % count;
    for (const slide of swiper.slides) {
        const active = slide.classList.contains('swiper-slide-active');
        for (const control of slide.querySelectorAll('a[href],button,input,select,textarea,[tabindex]')) {
            if (!control.hasAttribute('data-bzc-tabindex')) {
                control.setAttribute('data-bzc-tabindex', control.getAttribute('tabindex') ?? '');
            }
            const original = control.getAttribute('data-bzc-tabindex');
            if (!active) control.tabIndex = -1;
            else if (original === '') control.removeAttribute('tabindex');
            else control.setAttribute('tabindex', original);
        }
        slide.setAttribute('aria-hidden', String(!active));
        slide.tabIndex = active && slide.dataset.bzcSelectable === 'true' ? 0 : -1;
    }
    if (status) status.textContent = `Slide ${index + 1} of ${count}`;
}

/**
 * Bind local keyboard and assistive controls without capturing nested widget keys.
 * Loop copies forward selection to their original Blazor event handler.
 * @param {HTMLElement} element - Carousel root.
 * @param {object} swiper - Initialized Swiper instance.
 * @param {NodeList} originals - Original slides retaining Blazor event handlers.
 * @param {AbortSignal} signal - Aborts all input handlers when the instance is destroyed.
 */
function bindCarouselInput(element, swiper, originals, signal) {
    element.addEventListener('keydown', event => {
        const target = event.target;
        const slide = target.closest('.swiper-slide');
        if (target !== element && target !== slide) return;
        if (event.altKey || event.ctrlKey || event.metaKey || event.shiftKey) return;
        const reverse = getComputedStyle(element).direction === 'rtl';
        if (event.key === 'ArrowRight' || event.key === 'ArrowLeft') {
            event.preventDefault();
            const next = (event.key === 'ArrowRight') !== reverse;
            if (next) swiper.slideNext();
            else swiper.slidePrev();
        } else if (slide?.dataset.bzcSelectable === 'true' &&
                   (event.key === 'Enter' || event.key === ' ')) {
            event.preventDefault();
            slide.click();
        }
    }, { signal });
    element.addEventListener('click', event => {
        const control = event.target.closest('[data-bzc-direction]');
        if (control && element.contains(control)) {
            if (control.dataset.bzcDirection === 'next') swiper.slideNext();
            else swiper.slidePrev();
            return;
        }
        const clone = event.target.closest('[data-bz-clone="true"]');
        if (!clone || !element.contains(clone)) return;
        // Interactive templates own their links and controls; do not synthesize a second action.
        if (event.target.closest('a,button,input,select,textarea,[contenteditable]')) return;
        event.stopPropagation();
        const itemIndex = Number(clone.getAttribute('data-swiper-slide-index')) % originals.length;
        originals[itemIndex]?.click();
    }, { signal });
}

/**
 * Destroy a Carousel and remove only copies and handlers created by this module.
 * @param {HTMLElement} element - Carousel root previously initialized by this module.
 */
export function destroyCarousel(element) {
    carouselBindings.get(element)?.abort();
    carouselBindings.delete(element);
    if (swiperInstances.has(element)) {
        const instance = swiperInstances.get(element);
        instance.destroy(true, true);
        swiperInstances.delete(element);
    }
    element.querySelectorAll('[data-bz-clone="true"]').forEach(slide => slide.remove());
}

export function getActiveIndex(element) {
    if (swiperInstances.has(element)) {
        return swiperInstances.get(element).activeIndex;
    }
    return 0;
}

export function slideTo(element, index, speed = 300) {
    if (swiperInstances.has(element)) {
        swiperInstances.get(element).slideTo(index, speed);
    }
}

export function slideNext(element, speed = 300) {
    if (swiperInstances.has(element)) {
        swiperInstances.get(element).slideNext(speed);
    }
}

export function slidePrev(element, speed = 300) {
    if (swiperInstances.has(element)) {
        swiperInstances.get(element).slidePrev(speed);
    }
}

// BENTO INITIALIZATION

/**
 * Initialize BzBento Grid with staggered animations
 * @param {HTMLElement} element - The grid container element
 * @param {string} optionsJson - JSON string with options
 * @param {object} dotNetRef - .NET object reference for callbacks
 */
export async function initializeBento(element, optionsJson, dotNetRef = null) {
    try {
        await ensureCoreStylesLoaded();

        const options = optionsJson ? JSON.parse(optionsJson) : {};

        // Destroy existing instance if present
        if (bentoInstances.has(element)) {
            destroyBento(element);
        }

        // Initialize static Bento Grid
        initializeBentoStatic(element, options, dotNetRef);

    } catch (err) {
        console.error("[BlazzyMotion] Bento initialization error:", err);
    }
}

/**
 * Initialize static Bento Grid with staggered animations
 */
function initializeBentoStatic(element, options, dotNetRef) {
    const items = element.querySelectorAll('.bzb-item');

    requestAnimationFrame(() => {
        requestAnimationFrame(() => {
            element.style.removeProperty('opacity');
            element.style.removeProperty('visibility');
            element.classList.remove('bzb-hidden');
            element.classList.add('bzb-visible');
        });
    });

    if (items.length === 0) {
        return;
    }

    const staggerDelay = options.staggerDelay || 50;
    const animationEnabled = options.animationEnabled !== false;

    if (animationEnabled) {
        // Setup Intersection Observer for staggered animations - per item
        const observer = new IntersectionObserver((entries) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    const item = entry.target;
                    const itemIndex = Array.from(items).indexOf(item);

                    // Apply staggered delay
                    item.style.animationDelay = `${itemIndex * staggerDelay}ms`;
                    item.classList.add('bzb-animate');

                    observer.unobserve(item);
                }
            });
        }, {
            threshold: 0.1,
            rootMargin: '50px'
        });

        // Observe all items individually
        items.forEach(item => observer.observe(item));

        bentoInstances.set(element, {
            type: 'bento-static',
            observer: observer,
            dotNetRef: dotNetRef
        });
    } else {
        // No animation - just make items visible
        element.classList.add('bzb-no-animation');
    }

    // Callback to Blazor when initialized
    if (dotNetRef) {
        dotNetRef.invokeMethodAsync('OnBentoInitializedFromJS', items.length)
            .catch(err => {
                if (!err.message?.includes('disposed')) {
                    console.warn('[BlazzyMotion] Bento init callback error:', err);
                }
            });
    }
}

/**
 * Destroy Bento Grid instance
 */
export function destroyBento(element) {
    if (bentoInstances.has(element)) {
        const instance = bentoInstances.get(element);

        if (instance.type === 'bento-static' && instance.observer) {
            instance.observer.disconnect();
        } else if (instance.destroy) {
            // Legacy: direct Swiper instance
            instance.destroy(true, true);
        }

        bentoInstances.delete(element);
    }
}

/**
 * Refresh Bento Grid (re-trigger animations)
 */
export function refreshBento(element) {
    if (bentoInstances.has(element)) {
        const instance = bentoInstances.get(element);

        if (instance.type === 'bento-static') {
            // Re-animate all items
            const items = element.querySelectorAll('.bzb-item');
            items.forEach((item, index) => {
                item.classList.remove('bzb-animate');
                void item.offsetWidth; // Trigger reflow
                item.style.animationDelay = `${index * 50}ms`;
                item.classList.add('bzb-animate');
            });
        }
    }
}
