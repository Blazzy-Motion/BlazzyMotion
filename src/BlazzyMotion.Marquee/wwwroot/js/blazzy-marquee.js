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

/* Stagger Entrance Animation */

/**
 * Sets up IntersectionObserver for stagger entrance.
 * Waits for bzm-ready before playing to avoid animating invisible containers.
 * @param {HTMLElement} container
 * @param {Array} rowInstances
 * @param {number} staggerDelay
 * @returns {IntersectionObserver}
 */
function setupEntranceAnimation(container, rowInstances, staggerDelay) {
    const startEntrance = () => {
        playEntranceAnimation(rowInstances, staggerDelay);
    };

    const waitForReady = () => {
        if (container.classList.contains('bzm-ready')) {
            startEntrance();
            return;
        }

        const checkReady = setInterval(() => {
            if (container.classList.contains('bzm-ready')) {
                clearInterval(checkReady);
                startEntrance();
            }
        }, 16);

        setTimeout(() => clearInterval(checkReady), 2000);
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
            if (!entry.isIntersecting) return;

            observer.unobserve(container);
            waitForReady();
        });
    }, {
        threshold: 0.1,
        rootMargin: '50px'
    });

    observer.observe(container);
    return observer;
}

/**
 * Plays staggered entrance animation across all rows.
 * Each row starts after a row-level delay, items animate with per-item stagger.
 * @param {Array} rowInstances
 * @param {number} staggerDelay - Delay between items in ms
 */
function playEntranceAnimation(rowInstances, staggerDelay) {
    const animationDuration = 600;
    const rowDelay = 150;
    let maxCompletionTime = 0;

    rowInstances.forEach((rowData, rowIndex) => {
        const { track, content } = rowData;
        const items = content.querySelectorAll('.bzm-item, .bzm-text-item');

        if (items.length === 0) {
            track.classList.remove('bzm-entrance-pending');
            return;
        }

        const rowOffset = rowIndex * rowDelay;

        items.forEach((item, itemIndex) => {
            const delay = rowOffset + (itemIndex * staggerDelay);
            item.style.animationDelay = `${delay}ms`;
            item.classList.add('bzm-animate');
        });

        const lastItemDelay = rowOffset + ((items.length - 1) * staggerDelay);
        const rowCompletionTime = lastItemDelay + animationDuration;
        maxCompletionTime = Math.max(maxCompletionTime, rowCompletionTime);
    });

    setTimeout(() => {
        rowInstances.forEach(({ track, content }) => {
            track.classList.remove('bzm-entrance-pending');

            const items = content.querySelectorAll('.bzm-item, .bzm-text-item');
            items.forEach((item) => {
                item.style.animationDelay = '';
            });
        });
    }, maxCompletionTime + 50);
}

/* Marquee Initialization */

/**
 * Initialize BzMarquee with infinite scroll, content cloning and stagger entrance.
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
        const rows = element.querySelectorAll('.bzm-row');
        const rowInstances = [];
        const staggerEntrance = options.staggerEntrance !== false;
        const staggerDelay = options.staggerDelay || 60;
        const reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

        for (const row of rows) {
            const track = row.querySelector('.bzm-track');
            const content = row.querySelector('.bzm-content');
            if (!track || !content) continue;

            if (staggerEntrance && !reducedMotion) {
                track.classList.add('bzm-entrance-pending');
            }

            // Clone content for seamless loop
            const clone = content.cloneNode(true);
            clone.setAttribute('aria-hidden', 'true');
            clone.classList.add('bzm-clone');
            track.appendChild(clone);

            const speed = parseInt(row.dataset.speed) || options.speed || 50;

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

            rowInstances.push({ track, content, clone, resizeObserver, speed });
        }

        let entranceObserver = null;

        if (staggerEntrance && !reducedMotion && rowInstances.length > 0) {
            entranceObserver = setupEntranceAnimation(element, rowInstances, staggerDelay);
        }

        marqueeInstances.set(element, {
            rows: rowInstances,
            options,
            dotNetRef,
            entranceObserver
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

/**
 * Destroy marquee instance and clean up
 */
export function destroyMarquee(element) {
    if (!marqueeInstances.has(element)) return;

    const instance = marqueeInstances.get(element);

    if (instance.entranceObserver) {
        instance.entranceObserver.disconnect();
    }

    for (const row of instance.rows) {
        if (row.resizeObserver) {
            row.resizeObserver.disconnect();
        }
        if (row.clone && row.clone.parentNode) {
            row.clone.parentNode.removeChild(row.clone);
        }

        if (row.track) {
            row.track.classList.remove('bzm-entrance-pending');
        }
        if (row.content) {
            const items = row.content.querySelectorAll('.bzm-item, .bzm-text-item');
            items.forEach(item => {
                item.classList.remove('bzm-animate');
                item.style.animationDelay = '';
            });
        }
    }

    marqueeInstances.delete(element);
}

/**
 * Pause marquee animation
 */
export function pauseMarquee(element) {
    if (!marqueeInstances.has(element)) return;
    element.classList.add('bzm-paused');
}

/**
 * Resume marquee animation
 */
export function resumeMarquee(element) {
    if (!marqueeInstances.has(element)) return;
    element.classList.remove('bzm-paused');
}
