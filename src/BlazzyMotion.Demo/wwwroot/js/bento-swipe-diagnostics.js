/**
 * Applies one temporary diagnostic to the company dashboard without changing library defaults.
 * Open a fresh page for each mode; callback isolation deliberately freezes company metrics.
 * @param {HTMLElement} stage The interactive company dashboard.
 * @param {string} mode One of images, layers, callbacks, or main-css.
 * @returns {Promise<string>} A visible preparation result for the device tester.
 */
export async function apply(stage, mode) {
    if (!['images', 'layers', 'callbacks', 'main-css'].includes(mode)) return 'Unknown diagnostic.';
    const deadline = performance.now() + 15000;
    let swiper;
    while (stage.isConnected && performance.now() < deadline) {
        swiper = stage.querySelector('.swiper-container')?.swiper;
        if (swiper?.initialized && !swiper.destroyed) break;
        await new Promise(resolve => requestAnimationFrame(resolve));
    }
    if (!swiper?.initialized || swiper.destroyed) return 'Diagnostic unavailable: carousel not ready.';

    if (mode === 'main-css') {
        try { await applyMainCss(); } catch { return 'Main CSS comparison unavailable. Reload to retry.'; }
    } else if (mode === 'images') {
        const images = [...swiper.el.querySelectorAll('img')];
        for (const image of images) image.loading = 'eager';
        const results = await Promise.allSettled(images.map(image => image.decode()));
        if (results.some(result => result.status === 'rejected')) return 'Image preparation failed. Reload to retry.';
    } else if (mode === 'layers') {
        for (const image of swiper.el.querySelectorAll('img')) image.style.willChange = 'transform';
    } else {
        const handler = swiper.params.on?.slideChange;
        if (typeof handler !== 'function') return 'Callback isolation unavailable.';
        swiper.off('slideChange', handler);
    }
    stage.dataset.swipeTest = mode;
    return `Ready: ${mode}. Try the first swipe. ${mode === 'callbacks' ? 'Company metrics are intentionally paused.' : 'Library defaults remain unchanged outside this test.'}`;
}

/**
 * Replaces only the Carousel scoped stylesheet with the main-branch comparison fixture.
 * Keeps new screen-reader/control helpers hidden and leaves shared Core CSS and JS intact.
 * @returns {Promise<void>} Resolves once the baseline stylesheet is installed.
 */
async function applyMainCss() {
    const response = await fetch(new URL('../diagnostics/carousel-main.css', import.meta.url));
    if (!response.ok) throw new Error('Main CSS fixture unavailable.');
    const baseline = new CSSStyleSheet();
    baseline.replaceSync(await response.text());
    const sheets = [...document.styleSheets];
    let replaced = false;
    while (sheets.length) {
        const sheet = sheets.pop();
        // Font providers and other third-party stylesheets are outside this comparison.
        if (sheet.href && new URL(sheet.href).origin !== location.origin) continue;
        if (sheet.href?.includes('/BlazzyMotion.Carousel.bundle.scp.css')) {
            const helpers = [...sheet.cssRules].filter(rule =>
                rule.selectorText?.includes('.bzc-sr-only') ||
                rule.selectorText?.includes('.bzc-keyboard-control') ||
                rule.selectorText?.includes('.bzc-previous') ||
                rule.selectorText?.includes('.bzc-next')).map(rule => rule.cssText);
            while (sheet.cssRules.length) sheet.deleteRule(0);
            for (const rule of [...baseline.cssRules].map(rule => rule.cssText).concat(helpers)) {
                sheet.insertRule(rule, sheet.cssRules.length);
            }
            sheet.insertRule('.bzc-media-placeholder { display: none; }', sheet.cssRules.length);
            replaced = true;
        } else {
            for (const rule of sheet.cssRules) {
                if (rule.styleSheet) sheets.push(rule.styleSheet);
            }
        }
    }
    if (!replaced) throw new Error('Carousel stylesheet not found.');
}
