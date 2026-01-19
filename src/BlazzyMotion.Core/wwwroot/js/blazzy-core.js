let swiperLoaded = false;
const swiperInstances = new Map();

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


// swper loading

export async function ensureSwiperLoaded() {
    if (swiperLoaded) return;

    await Promise.all([
        loadStylesheet("_content/BlazzyMotion.Core/css/swiper-bundle.min.css"),
        loadStylesheet("_content/BlazzyMotion.Core/css/blazzy-core.css"),
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


export async function initializeCarousel(element, optionsJson, dotNetRef = null) {
    try {
        const container = element.querySelector(".swiper-container");

        if (!container) {
            console.warn("[BlazzyMotion] No .swiper-container found in element");
            return;
        }

        // Destroy existing instance if present
        if (swiperInstances.has(element)) {
            const instance = swiperInstances.get(element);
            instance.destroy(true, true);
            swiperInstances.delete(element);
        }

        const options = optionsJson ? JSON.parse(optionsJson) : {};
        const wrapper = container.querySelector('.swiper-wrapper');
        const originalSlides = wrapper.querySelectorAll('.swiper-slide');
        const slideCount = originalSlides.length;


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
            effect: options.effect || "coverflow",
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
                    this.params.speed = options.speed || 300;
                    this.params.runCallbacksOnInit = true;

                    // Show carousel after initialization
                    requestAnimationFrame(() => {
                        requestAnimationFrame(() => {
                            if (this.el) {
                                this.el.classList.remove('bzc-hidden');
                                this.el.classList.add('bzc-visible');
                            }
                        });
                    });
                },

                slideChange: function () {
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

        const swiperInstance = new Swiper(container, swiperConfig);

        // Store instance for later reference
        swiperInstances.set(element, swiperInstance);

    } catch (err) {
        console.error("[BlazzyMotion] Initialization error:", err);
    }
}


export function destroyCarousel(element) {
    if (swiperInstances.has(element)) {
        const instance = swiperInstances.get(element);
        instance.destroy(true, true);
        swiperInstances.delete(element);
    }
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

/* BENTO GRID INITIALIZATION */

/**
 * Initialize Bento Grid
 * @param {HTMLElement} element - The grid container element
 * @param {string} optionsJson - JSON string with options
 * @param {object} dotNetRef - .NET object reference for callbacks
 */
export async function initializeBento(element, optionsJson, dotNetRef = null) {
    try {
        const options = optionsJson ? JSON.parse(optionsJson) : {};
        const isPaginated = options.paginated === true;

        // Destroy existing instance if present
        if (swiperInstances.has(element)) {
            destroyBento(element);
        }

        if (isPaginated) {
            await initializeBentoPaginated(element, options, dotNetRef);
        } else {
            initializeBentoStatic(element, options, dotNetRef);
        }

    } catch (err) {
        console.error("[BlazzyMotion] Bento initialization error:", err);
    }
}

/**
 * Initialize static (non-paginated) Bento Grid with staggered animations
 */
function initializeBentoStatic(element, options, dotNetRef) {
    const items = element.querySelectorAll('.bzb-item');

    if (items.length === 0) {
        console.warn('[BlazzyMotion] No .bzb-item found in Bento grid');
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

        swiperInstances.set(element, {
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
 * Initialize paginated Bento Grid using Swiper
 */
async function initializeBentoPaginated(element, options, dotNetRef) {
    const container = element.querySelector(".swiper-container");

    if (!container) {
        console.warn("[BlazzyMotion] No .swiper-container found in paginated Bento");
        return;
    }

    // Ensure Swiper is loaded
    await ensureSwiperLoaded();

    const swiperConfig = {
        effect: 'slide',
        slidesPerView: 1,
        spaceBetween: 20,
        speed: options.speed || 300,
        grabCursor: true,

        // Touch settings
        touchRatio: options.touchRatio ?? DEFAULT_TOUCH_SETTINGS.touchRatio,
        threshold: options.threshold ?? DEFAULT_TOUCH_SETTINGS.threshold,
        shortSwipes: options.shortSwipes ?? DEFAULT_TOUCH_SETTINGS.shortSwipes,
        resistanceRatio: options.resistanceRatio ?? DEFAULT_TOUCH_SETTINGS.resistanceRatio,
        longSwipesRatio: options.longSwipesRatio ?? DEFAULT_TOUCH_SETTINGS.longSwipesRatio,

        // Pagination
        pagination: {
            el: element.querySelector('.bzb-pagination'),
            clickable: true,
            dynamicBullets: options.dynamicBullets ?? false,
        },

        on: {
            init: function () {
                // Animate items on first page
                const firstSlide = this.slides[0];
                if (firstSlide) {
                    animateBentoPage(firstSlide, options.staggerDelay || 50);
                }
            },

            slideChange: function () {
                // Animate items on new page
                const activeSlide = this.slides[this.activeIndex];
                if (activeSlide) {
                    animateBentoPage(activeSlide, options.staggerDelay || 50);
                }

                // Callback to Blazor
                if (dotNetRef) {
                    dotNetRef.invokeMethodAsync('OnBentoPageChangeFromJS', this.activeIndex)
                        .catch(err => {
                            if (!err.message?.includes('disposed')) {
                                console.warn('[BlazzyMotion] Bento page change callback error:', err);
                            }
                        });
                }
            }
        }
    };

    const swiperInstance = new Swiper(container, swiperConfig);

    // Store with type marker
    swiperInstances.set(element, {
        type: 'bento-paginated',
        swiper: swiperInstance,
        dotNetRef: dotNetRef
    });

    // Callback to Blazor when initialized
    if (dotNetRef) {
        const totalPages = swiperInstance.slides.length;
        dotNetRef.invokeMethodAsync('OnBentoInitializedFromJS', totalPages)
            .catch(err => {
                if (!err.message?.includes('disposed')) {
                    console.warn('[BlazzyMotion] Bento init callback error:', err);
                }
            });
    }
}

/**
 * Animate items within a Bento page (for paginated mode)
 */
function animateBentoPage(slideElement, staggerDelay) {
    const items = slideElement.querySelectorAll('.bzb-item');
    items.forEach((item, index) => {
        // Reset animation
        item.classList.remove('bzb-animate');
        void item.offsetWidth; // Trigger reflow

        // Apply staggered animation
        item.style.animationDelay = `${index * staggerDelay}ms`;
        item.classList.add('bzb-animate');
    });
}

/**
 * Destroy Bento Grid instance
 */
export function destroyBento(element) {
    if (swiperInstances.has(element)) {
        const instance = swiperInstances.get(element);

        if (instance.type === 'bento-static' && instance.observer) {
            instance.observer.disconnect();
        } else if (instance.type === 'bento-paginated' && instance.swiper) {
            instance.swiper.destroy(true, true);
        } else if (instance.destroy) {
            // Legacy: direct Swiper instance
            instance.destroy(true, true);
        }

        swiperInstances.delete(element);
    }
}

/**
 * Refresh Bento Grid (re-trigger animations)
 */
export function refreshBento(element) {
    if (swiperInstances.has(element)) {
        const instance = swiperInstances.get(element);

        if (instance.type === 'bento-static') {
            // Re-animate all items
            const items = element.querySelectorAll('.bzb-item');
            items.forEach((item, index) => {
                item.classList.remove('bzb-animate');
                void item.offsetWidth;
                item.style.animationDelay = `${index * 50}ms`;
                item.classList.add('bzb-animate');
            });
        } else if (instance.type === 'bento-paginated' && instance.swiper) {
            // Re-animate current page
            const activeSlide = instance.swiper.slides[instance.swiper.activeIndex];
            if (activeSlide) {
                animateBentoPage(activeSlide, 50);
            }
        }
    }
}

/**
 * Navigate to specific Bento page (paginated mode only)
 */
export function bentoSlideTo(element, pageIndex, speed = 300) {
    if (swiperInstances.has(element)) {
        const instance = swiperInstances.get(element);
        if (instance.type === 'bento-paginated' && instance.swiper) {
            instance.swiper.slideTo(pageIndex, speed);
        }
    }
}

/**
 * Get current Bento page index (paginated mode only)
 */
export function getBentoActiveIndex(element) {
    if (swiperInstances.has(element)) {
        const instance = swiperInstances.get(element);
        if (instance.type === 'bento-paginated' && instance.swiper) {
            return instance.swiper.activeIndex;
        }
    }
    return 0;
}
