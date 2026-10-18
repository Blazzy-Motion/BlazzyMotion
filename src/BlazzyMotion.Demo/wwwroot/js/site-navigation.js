/**
 * Open native navigation with browser focus containment and background inertness.
 * @param {HTMLDialogElement} dialog The documentation navigation dialog.
 */
export function openNavigation(dialog) {
    if (!dialog.open) dialog.showModal();
}

/**
 * Close navigation and restore focus to the invoking control through the browser.
 * @param {HTMLDialogElement} dialog The documentation navigation dialog.
 */
export function closeNavigation(dialog) {
    if (dialog.open) dialog.close();
}
