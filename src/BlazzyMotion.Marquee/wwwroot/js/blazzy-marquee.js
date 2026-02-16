let marqueeLoaded = false;
const marqueeInstances = new Map();

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

async function ensureMarqueeLoaded() {
    if (marqueeLoaded) return;

    await Promise.all([
        loadStylesheet("_content/BlazzyMotion.Core/css/blazzy-core.css"),
        loadStylesheet("_content/BlazzyMotion.Marquee/css/blazzy-marquee.css"),
    ]);

    marqueeLoaded = true;
}

/* Marquee Initialization */

/**
 * @param {HTMLElement} element - The marquee container element
 * @param {string} optionsJson - JSON string with options
 * @param {object} dotNetRef - .NET object reference for callbacks
 */
export async function initializeMarquee(element, optionsJson, dotNetRef = null) {
    try {
        await ensureMarqueeLoaded();

        if (marqueeInstances.has(element)) {
            destroyMarquee(element);
        }

        const options = optionsJson ? JSON.parse(optionsJson) : {};
        const track = element.querySelector('.bzm-track');
        const content = element.querySelector('.bzm-content');

        if (!track || !content) return;

        // Clone content for seamless loop
        const clone = content.cloneNode(true);
        clone.setAttribute('aria-hidden', 'true');
        clone.classList.add('bzm-clone');
        track.appendChild(clone);

        // Calculate animation duration based on content width and speed
        const speed = options.speed || 50;

        const calculateDuration = () => {
            const contentSize = content.scrollWidth;
            const duration = contentSize / speed;
            track.style.setProperty('--bzm-duration', `${duration}s`);
        };

        calculateDuration();

        const resizeObserver = new ResizeObserver(() => {
            calculateDuration();
        });
        resizeObserver.observe(content);

        marqueeInstances.set(element, {
            track,
            content,
            clone,
            resizeObserver,
            options,
            dotNetRef
        });

        if (dotNetRef) {
            dotNetRef.invokeMethodAsync('OnMarqueeInitializedFromJS')
                .catch(err => {
                    if (!err.message?.includes('disposed')) {
                        console.warn('[BlazzyMotion] Marquee init callback error:', err);
                    }
                });
        }

    } catch (err) {
        console.error("[BlazzyMotion] Marquee initialization error:", err);
    }
}

/* Marquee Control */

/** @param {HTMLElement} element */
export function destroyMarquee(element) {
    if (!marqueeInstances.has(element)) return;

    const instance = marqueeInstances.get(element);

    if (instance.resizeObserver) {
        instance.resizeObserver.disconnect();
    }

    if (instance.clone && instance.clone.parentNode) {
        instance.clone.parentNode.removeChild(instance.clone);
    }

    marqueeInstances.delete(element);
}

/** @param {HTMLElement} element */
export function pauseMarquee(element) {
    if (!marqueeInstances.has(element)) return;
    const instance = marqueeInstances.get(element);
    instance.track.style.animationPlayState = 'paused';
}

/** @param {HTMLElement} element */
export function resumeMarquee(element) {
    if (!marqueeInstances.has(element)) return;
    const instance = marqueeInstances.get(element);
    instance.track.style.animationPlayState = 'running';
}
