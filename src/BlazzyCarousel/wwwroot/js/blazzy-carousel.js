let swiperLoaded = false;
let swiperInstance = null;

/* ═══════════════════════════════════════════════════════════
   Utility: Dynamic script loader
   ═══════════════════════════════════════════════════════════ */
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
                } else if (tries > 50) {
                    clearInterval(check);
                    reject(new Error("Swiper script loaded but not defined"));
                }
            }, 100);
        };
        script.onerror = () => reject(new Error(`Failed to load script: ${src}`));
        document.head.appendChild(script);
    });
}

/* ═══════════════════════════════════════════════════════════
   Utility: Dynamic stylesheet loader
   ═══════════════════════════════════════════════════════════ */
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

/* ═══════════════════════════════════════════════════════════
   Ensure Swiper is available
   ═══════════════════════════════════════════════════════════ */
export async function ensureSwiperLoaded() {
    if (swiperLoaded) return;

    await Promise.all([
        loadStylesheet("_content/BlazzyCarousel/css/swiper-bundle.min.css"),
        loadStylesheet("_content/BlazzyCarousel/css/blazzy-carousel.css"),
    ]);
    await loadScript("_content/BlazzyCarousel/js/swiper-bundle.min.js");

    if (typeof Swiper === "undefined") {
        throw new Error("Swiper is not defined after load");
    }

    swiperLoaded = true;
}

/* ═══════════════════════════════════════════════════════════
   Initialize carousel
   ═══════════════════════════════════════════════════════════ */
export async function initializeCarousel(element, optionsJson) {
    try {
        const container = element.querySelector(".swiper-container");
        if (!container) {
            return;
        }


        if (swiperInstance) {
            swiperInstance.destroy(true, true);
            swiperInstance = null;
        }



        const options = optionsJson ? JSON.parse(optionsJson) : {};


        swiperInstance = new Swiper(container, {
            effect: options.effect || "coverflow",
            grabCursor: options.grabCursor ?? true,
            centeredSlides: options.centeredSlides ?? true,
            slidesPerView: options.slidesPerView || "auto",
            initialSlide: options.initialSlide || 0,
            loop: options.loop ?? true,
            loopAdditionalSlides: 2,
            speed: 0,
            runCallbacksOnInit: false,
            slideToClickedSlide: true,
            watchSlidesProgress: true,
            watchSlidesVisibility: true,
            observer: true,
            observeParents: true,
            touchRatio: 1,
            touchEventsTarget: 'container',
            coverflowEffect: {
                rotate: options.rotateDegree || 50,
                stretch: options.stretch || 0,
                depth: options.depth || 150,
                modifier: options.modifier || 1.5,
                slideShadows: options.slideShadows ?? true,
            },
            on: {
                setTranslate: function () {
                    this.slides.forEach(slide => {
                        const currentZ = parseInt(slide.style.zIndex);
                        if (currentZ < 0 || isNaN(currentZ)) {
                            slide.style.zIndex = '1';
                        }
                        slide.style.pointerEvents = 'auto';
                    });
                    if (this.params.speed === 0) {
                        // Enable smooth animations for future interactions
                        this.params.speed = 300;
                        this.params.runCallbacksOnInit = true;

                        // Reveal carousel with smooth fade-in (slides already transformed!)
                        requestAnimationFrame(() => {
                            this.el.style.opacity = '1';
                            this.el.style.visibility = 'visible';
                            this.el.style.transition = 'opacity 0.4s ease-in';
                        });
                    }
                }

            }
        });
    } catch (err) {
        console.error("[BlazzyCarousel] Initialization error:", err);
    }
}
export function destroyCarousel() {
    if (swiperInstance) {
        swiperInstance.destroy(true, true);
        swiperInstance = null;
    }
}