/**
 * Applies one temporary diagnostic to the company dashboard without changing library defaults.
 * Open a fresh page for each mode; callback isolation deliberately freezes company metrics.
 * @param {HTMLElement} stage The interactive company dashboard.
 * @param {string} mode One of images, layers, or callbacks.
 * @returns {Promise<string>} A visible preparation result for the device tester.
 */
export async function apply(stage, mode) {
    if (!['images', 'layers', 'callbacks'].includes(mode)) return 'Unknown diagnostic.';
    const deadline = performance.now() + 15000;
    let swiper;
    while (stage.isConnected && performance.now() < deadline) {
        swiper = stage.querySelector('.swiper-container')?.swiper;
        if (swiper?.initialized && !swiper.destroyed) break;
        await new Promise(resolve => requestAnimationFrame(resolve));
    }
    if (!swiper?.initialized || swiper.destroyed) return 'Diagnostic unavailable: carousel not ready.';

    if (mode === 'images') {
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
