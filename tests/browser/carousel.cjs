const { chromium, devices } = require('playwright');
const AxeBuilder = require('@axe-core/playwright').default;
const assert = require('node:assert/strict');
const fs = require('node:fs/promises');
const path = require('node:path');
const baseUrl = process.env.BASE_URL || 'http://127.0.0.1:5130';

/** Waits for the real development server rather than treating an open process as readiness. */
async function waitForServer() {
    for (let attempt = 0; attempt < 120; attempt++) {
        try { if ((await fetch(baseUrl)).ok) return; } catch { /* The server may still be starting. */ }
        await new Promise(resolve => setTimeout(resolve, 250));
    }
    throw new Error(`Demo did not become ready at ${baseUrl}`);
}

/** Exercises presentation changes, themes, keyboard isolation, accessible controls and geometry. */
async function checkPresentation(browser, mode) {
    const context = await browser.newContext(mode === 'mobile'
        ? { ...devices['iPhone 13'] }
        : { viewport: { width: 1200, height: 950 }, reducedMotion: mode === 'reduced' ? 'reduce' : 'no-preference' });
    const page = await context.newPage();
    const errors = [];
    page.on('pageerror', error => errors.push(error.message));
    try {
        await page.goto(baseUrl);
        await page.waitForSelector('.bzc-visible');
        const root = page.locator('.bzc-carousel-box');
        for (const variant of ['Cards', 'Auto']) {
            await page.getByRole('button', { name: variant === 'Cards' ? 'Cards' : 'Coverflow', exact: true }).click();
            assert.equal(await page.locator('.bz-carousel-variant button[aria-pressed="true"]').count(), 1);
            await page.waitForFunction(value => document.querySelector('.swiper-container').swiper?.params.effect ===
                (matchMedia('(prefers-reduced-motion: reduce)').matches ? 'slide' : value === 'Cards' ? 'cards' : 'coverflow'), variant);
            const before = await root.locator('.bzc-status').textContent();
            await root.focus();
            await page.keyboard.press('ArrowRight');
            await page.waitForFunction(text => document.querySelector('.bzc-status').textContent !== text, before);
            await page.waitForTimeout(350);
            await page.getByRole('button', { name: 'Go to slide 4', exact: true }).click();
            await page.waitForFunction(() => document.querySelector('[data-bzc-index="3"]').getAttribute('aria-current') === 'true');
            await page.waitForTimeout(350);
            if (mode === 'mobile') await checkMobileSwipe(page, root);
            const input = await root.evaluateHandle(element => {
                const input = document.createElement('input');
                input.setAttribute('aria-label', 'Consumer input');
                element.append(input);
                return input;
            });
            const current = await root.locator('.bzc-status').textContent();
            await input.asElement().focus();
            await page.keyboard.press('ArrowRight');
            assert.equal(await root.locator('.bzc-status').textContent(), current, 'Input arrow keys must remain local');
            await input.evaluate(element => element.remove());
            for (const theme of ['Glass', 'Dark', 'Light', 'Minimal']) {
                await page.getByRole('button', { name: theme, exact: true }).click();
                await page.waitForTimeout(150);
                const geometry = await root.evaluate(element => {
                    const image = element.querySelector('.swiper-slide-active img');
                    const bounds = element.getBoundingClientRect();
                    const artwork = image.getBoundingClientRect();
                    return { height: bounds.height, imageWidth: image.offsetWidth,
                        visibleArtwork: [...element.querySelectorAll('.swiper-slide img')].filter(item => {
                            const box = item.getBoundingClientRect();
                            return box.width > 5 && box.right > bounds.left + 5 && box.left < bounds.right - 5;
                        }).length,
                        centerOffset: Math.abs((artwork.left + artwork.width / 2) - (bounds.left + bounds.width / 2)),
                        background: getComputedStyle(element).backgroundColor, border: getComputedStyle(element).borderTopWidth };
                });
                assert.ok(geometry.height < (mode === 'mobile' ? 390 : variant === 'Auto' ? 530 : 470), 'Avoid oversized empty panels');
                if (mode === 'mobile' && variant === 'Cards') assert.ok(geometry.imageWidth >= 145, 'The deck must retain its own artwork size');
                if (mode === 'mobile' && variant === 'Auto') {
                    assert.equal(geometry.imageWidth, 90, 'Preserve main branch mobile Coverflow image sizing');
                    assert.ok(geometry.visibleArtwork >= 5, 'Keep at least five intersecting Coverflow artworks on mobile');
                }
                if (variant === 'Cards') {
                    assert.equal(geometry.background, 'rgba(0, 0, 0, 0)', 'Cards must not have an outer panel');
                    assert.equal(geometry.border, '0px');
                    assert.ok(geometry.centerOffset < 20, 'The deck must be centered over its navigation');
                }
                const dotBackgrounds = await root.locator('.bzc-dot').evaluateAll(dots => dots.map(dot => getComputedStyle(dot).backgroundColor));
                assert.ok(dotBackgrounds.every(color => color === 'rgba(0, 0, 0, 0)'), 'Dot hit areas must remain transparent in every theme');
                const result = await new AxeBuilder({ page }).include('.bzc-carousel-box')
                    .withTags(['wcag2a', 'wcag2aa', 'wcag21aa', 'wcag22aa']).analyze();
                assert.deepEqual(result.violations.map(v => ({ id: v.id, targets: v.nodes.map(n => n.target) })), []);
                await root.screenshot({ path: path.join(__dirname, 'artifacts', `${mode}-${variant}-${theme}.png`) });
            }
        }
        assert.deepEqual(errors, []);
        console.log(`PASS ${mode}: both presentations, four themes, local keyboard navigation, geometry and scoped axe`);
    } finally { await context.close(); }
}

/** Exercises real touch input while checking fixed documentation access and document stability. */
async function checkMobileSwipe(page, root) {
    await root.locator('.swiper-slide-active img').scrollIntoViewIfNeeded();
    const before = await root.locator('.bzc-status').textContent();
    const documentId = await page.evaluate(() => window.__carouselDocumentId ||= crypto.randomUUID());
    const bounds = await root.locator('.swiper-slide-active img').boundingBox();
    const session = await page.context().newCDPSession(page);
    const startX = bounds.x + bounds.width * .8;
    const endX = bounds.x + bounds.width * .1;
    const y = bounds.y + bounds.height / 2;
    try {
        await session.send('Input.dispatchTouchEvent', { type: 'touchStart', touchPoints: [{ x: startX, y }] });
        for (let step = 1; step <= 12; step++) {
            await session.send('Input.dispatchTouchEvent', { type: 'touchMove', touchPoints: [{ x: startX + (endX - startX) * step / 12, y }] });
            await page.waitForTimeout(35);
            assert.ok(await page.evaluate(() => {
                const button = document.querySelector('.bz-home-layout__docs-btn');
                const box = button.getBoundingClientRect();
                return document.elementFromPoint(box.x + box.width / 2, box.y + box.height / 2)?.closest('.bz-home-layout__docs-btn') === button;
            }), 'The sticky Docs link must stay above the deck throughout a swipe');
        }
        await session.send('Input.dispatchTouchEvent', { type: 'touchEnd', touchPoints: [] });
        await page.waitForFunction(text => document.querySelector('.bzc-status').textContent !== text, before);
        assert.equal(await page.evaluate(() => window.__carouselDocumentId), documentId, 'Swipe must not reload the document');
    } finally { await session.detach(); }
}

/** Verifies active loop copies remain accessible and activate original consumer handlers. */
async function checkLoopCopies(browser) {
    const page = await browser.newPage();
    try {
        await page.goto(baseUrl);
        await page.waitForSelector('.bzc-visible');
        const results = await page.evaluate(async () => {
            const module = await import('/_content/BlazzyMotion.Core/js/blazzy-core.js');
            const results = [];
            for (const count of [4, 5, 6]) {
                const root = document.createElement('section');
                root.style.cssText = 'width:800px;height:400px';
                root.className = 'bzc-carousel-box';
                const slides = Array.from({ length: count }, (_, index) =>
                    `<div class="swiper-slide" style="width:180px;height:240px"><button data-original="${index}">Select ${index}</button></div>`).join('');
                root.innerHTML = `<div class="swiper-container"><div class="swiper-wrapper">${slides}</div></div>
                    <button class="bzc-previous">Previous</button><button class="bzc-next">Next</button><span class="bzc-status"></span>`;
                document.body.append(root);
                let selected = -1;
                root.querySelectorAll('[data-original]').forEach(button => button.addEventListener('click', () => {
                    selected = Number(button.dataset.original);
                }));
                await module.initializeCarousel(root, JSON.stringify({ effect: 'coverflow', loop: true, slidesPerView: 'auto', speed: 0 }));
                const swiper = root.querySelector('.swiper-container').swiper;
                let clones = 0;
                for (let index = 0; index < 20; index++) {
                    swiper.slideNext(0);
                    await new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)));
                    const active = root.querySelector('.swiper-slide-active');
                    if (!active.hasAttribute('data-bz-clone')) continue;
                    if (active.inert || active.getAttribute('aria-hidden') === 'true') throw new Error('Active clone inaccessible');
                    const button = active.querySelector('button');
                    button.click();
                    if (selected !== Number(button.dataset.original)) throw new Error('Clone callback did not reach original');
                    clones++;
                }
                if (!clones) throw new Error('No active clone exercised');
                results.push({ count, activeCloneSelections: clones });
                module.destroyCarousel(root);
                root.remove();
            }
            return results;
        });
        console.log('PASS loop copies: ' + JSON.stringify(results));
    } finally { await page.close(); }
}

/** Verifies missing Swiper resources do not turn available content into an empty panel. */
async function checkInitializationFailure(browser) {
    const page = await browser.newPage();
    try {
        await page.route('**/swiper-bundle.min.js', route => route.abort());
        await page.goto(baseUrl);
        await page.waitForSelector('.bzc-pending .swiper-slide img', { state: 'visible' });
        assert.ok(await page.locator('.bzc-pending .swiper-slide img').first().isVisible());
        assert.ok(await page.getByRole('button', { name: 'Go to slide 2', exact: true }).isDisabled());
        console.log('PASS initialization failure: first artwork remains visible and unavailable controls stay disabled');
    } finally { await page.close(); }
}

/** Runs the checks and always closes Chromium so assertion failures cannot hang CI. */
async function main() {
    await waitForServer();
    await fs.mkdir(path.join(__dirname, 'artifacts'), { recursive: true });
    const browser = await chromium.launch({ executablePath: process.env.CHROMIUM_PATH || undefined,
        args: process.env.CHROMIUM_PATH ? ['--no-sandbox'] : [] });
    try {
        for (const mode of ['desktop', 'mobile', 'reduced']) await checkPresentation(browser, mode);
        await checkLoopCopies(browser);
        await checkInitializationFailure(browser);
    } finally { await browser.close(); }
}
main().catch(error => { console.error(error); process.exitCode = 1; });
