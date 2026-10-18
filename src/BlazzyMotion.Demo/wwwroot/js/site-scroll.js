const controls = new WeakMap();

/**
 * Show a local return control after scrolling without triggering Blazor renders.
 * @param {HTMLButtonElement} button The control owned by the current layout.
 */
export function attach(button) {
    detach(button);
    const update = () => { button.hidden = window.scrollY < 600; };
    const activate = () => {
        document.getElementById('main-content')?.focus({ preventScroll: true });
        window.scrollTo({ top: 0, behavior: matchMedia('(prefers-reduced-motion: reduce)').matches ? 'instant' : 'smooth' });
    };
    controls.set(button, { update, activate });
    window.addEventListener('scroll', update, { passive: true });
    button.addEventListener('click', activate);
    update();
}

/**
 * Release listeners when a footer is removed, including layout navigation.
 * @param {HTMLButtonElement} button The previously attached control.
 */
export function detach(button) {
    const handlers = controls.get(button);
    if (!handlers) return;
    window.removeEventListener('scroll', handlers.update);
    button.removeEventListener('click', handlers.activate);
    controls.delete(button);
}
