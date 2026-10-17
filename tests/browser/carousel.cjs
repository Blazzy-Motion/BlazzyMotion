const { chromium, devices } = require('playwright');
const AxeBuilder = require('@axe-core/playwright').default;
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const artifacts = path.join(__dirname, 'artifacts');
const baseURL = process.env.BASE_URL || 'http://127.0.0.1:5130';

/** Wait for the host before opening the Demo; CI starts it in the background. */
async function waitForHost() {
  for (let attempt = 0; attempt < 60; attempt++) {
    try { if ((await fetch(baseURL)).ok) return; } catch { /* Host is still starting. */ }
    await new Promise(resolve => setTimeout(resolve, 1000));
  }
  throw new Error('Demo host did not start');
}

/** Inspect the visible image geometry, keeping mobile main-branch dimensions. */
async function inspectGeometry(root, mobile) {
  const geometry = await root.evaluate(element => {
    const bounds = element.getBoundingClientRect();
    return [...element.querySelectorAll('.swiper-slide img')].map(img => {
      const rect = img.getBoundingClientRect();
      return { width: img.offsetWidth, visible: rect.right > bounds.left && rect.left < bounds.right };
    });
  });
  assert(geometry.filter(item => item.visible).length >= 5, 'Five artworks must intersect the stage');
  if (mobile) assert(geometry.some(item => item.width === 90), 'Preserve original mobile image sizing');
}

/** Exercise real Swiper input, accessible controls, all themes and reduced motion. */
async function inspectCarousel(browser, name, contextOptions) {
  const context = await browser.newContext(contextOptions);
  const page = await context.newPage();
  const errors = [];
  page.on('pageerror', error => errors.push(error.message));
  await page.goto(baseURL);
  const root = page.locator('.bz-demo .bzc-carousel-box');
  await root.locator('.bzc-visible').waitFor();
  await root.locator('.swiper-slide-active[data-bzc-media=ready]').waitFor();
  await root.scrollIntoViewIfNeeded();
  const before = await root.locator('.swiper-slide-active').getAttribute('data-swiper-slide-index');
  await root.focus();
  await page.keyboard.press('ArrowRight');
  await page.waitForTimeout(400);
  assert.notEqual(await root.locator('.swiper-slide-active').getAttribute('data-swiper-slide-index'), before);
  await root.locator('[data-bzc-direction=next]').focus();
  await page.keyboard.press('Enter');
  await page.waitForTimeout(400);
  assert.match(await root.locator('[role=status]').textContent(), /Slide \d+ of 8/);
  assert.equal(await root.locator('.swiper-slide-active[aria-hidden=false]').count(), 1);
  await root.locator('.swiper-slide-active').focus();
  await page.keyboard.press('ArrowRight');
  await page.waitForTimeout(400);
  assert(await root.locator('.swiper-slide-active').evaluate(slide => slide === document.activeElement), 'Keyboard focus must follow the active slide');
  assert.equal(await root.locator('.swiper-slide-active[aria-hidden=false]').count(), 1);
  await inspectGeometry(root, Boolean(contextOptions.isMobile));
  const input = root.locator('.swiper-slide-active input');
  await root.locator('.swiper-slide-active').evaluate(slide => {
    const field = document.createElement('input');
    field.setAttribute('aria-label', 'Nested input');
    slide.appendChild(field);
  });
  const inputIndex = await root.locator('.swiper-slide-active').getAttribute('data-swiper-slide-index');
  await input.focus();
  await page.keyboard.press('ArrowLeft');
  assert.equal(await root.locator('.swiper-slide-active').getAttribute('data-swiper-slide-index'), inputIndex);
  await input.evaluate(field => field.remove());
  const themeLighting = new Map();
  for (const theme of ['Glass', 'Dark', 'Light', 'Minimal']) {
    await page.getByRole('button', { name: theme, exact: true }).click();
    // Theme changes include a delayed artwork-shadow transition.
    await page.waitForTimeout(750);
    const depth = await root.locator('.swiper-slide-active').evaluate(slide => ({
      syntheticSurface: getComputedStyle(slide, '::before').content,
      shadow: getComputedStyle(slide).boxShadow,
      artworkShadow: getComputedStyle(slide.querySelector('img')).boxShadow,
      artworkFilter: getComputedStyle(slide.querySelector('img')).filter,
      image: slide.querySelector('img').offsetHeight,
      height: slide.offsetHeight,
      container: slide.closest('.swiper-container').offsetHeight,
      overlay: slide.querySelector('.bzc-overlay').offsetHeight
    }));
    themeLighting.set(theme, depth.artworkShadow);
    assert.equal(depth.artworkFilter, 'none', 'Moving artwork must not introduce blur or color filters');
    assert.equal(depth.syntheticSurface, 'none', 'Use the actual Swiper slide, not a shorter pseudo-element');
    assert.equal(depth.height, depth.container, 'Preserve main full-height Swiper slides');
    assert.equal(depth.overlay, depth.height, 'Preserve main full-height overlays');
    assert(depth.height > depth.image, 'Swiper slides must extend beyond the poster');
    if (theme === 'Minimal') assert.equal(depth.shadow, 'none');
    else assert.notEqual(depth.shadow, 'none');
    if (theme === 'Glass' || theme === 'Dark') {
      assert(depth.shadow.includes('15px 40px') && depth.shadow.includes('5px 15px'), 'Restore the main-branch shadow on the real slide');
    }
    if (contextOptions.isMobile) assert.equal(depth.height, 250, 'Preserve main mobile slide height');
    const result = await new AxeBuilder({ page }).include('.bz-demo .bzc-carousel-box').analyze();
    assert.deepEqual(result.violations.map(v => ({ id: v.id, impact: v.impact })), [], `${name}/${theme} accessibility`);
    await page.screenshot({ path: path.join(artifacts, `${name}-${theme}.png`) });
  }
  assert.equal(new Set(themeLighting.values()).size, 4, 'Each theme must retain its own active artwork lighting');
  if (contextOptions.reducedMotion === 'reduce') {
    assert.equal(await root.evaluate(el => el.querySelector('.swiper-container').swiper.params.effect), 'slide');
    assert.equal(await root.evaluate(el => el.querySelector('.swiper-container').swiper.params.speed), 0);
  }
  if (name === 'desktop') {
    const activeIndex = await root.locator('.swiper-slide-active').getAttribute('data-swiper-slide-index');
    await page.emulateMedia({ reducedMotion: 'reduce' });
    await page.waitForFunction(() => document.querySelector('.bz-demo .swiper-container').swiper.params.effect === 'slide');
    assert.equal(await root.locator('.swiper-slide-active').getAttribute('data-swiper-slide-index'), activeIndex);
    await page.emulateMedia({ reducedMotion: 'no-preference' });
    await page.waitForFunction(() => document.querySelector('.bz-demo .swiper-container').swiper.params.effect === 'coverflow');
  }
  assert.deepEqual(errors, []);
  console.log(`PASS ${name}: original geometry, local keys, assistive controls and four themes`);
  await context.close();
}

/** Verify manual loop copies forward selection once and are removed on teardown. */
async function inspectLoopCopies(browser) {
  const page = await browser.newPage();
  await page.goto(baseURL);
  await page.locator('.bz-demo .bzc-visible').waitFor();
  const counts = await page.evaluate(async () => {
    const core = await import('/_content/BlazzyMotion.Core/js/blazzy-core.js');
    const source = document.querySelector('.bz-demo .swiper-slide:not(.swiper-slide-duplicate)');
    const results = [];
    for (const count of [4, 5, 6]) {
      const root = document.createElement('div');
      root.className = 'bzc-carousel-box';
      root.innerHTML = '<div class="swiper-container"><div class="swiper-wrapper"></div></div>';
      document.body.appendChild(root);
      let selections = 0;
      for (let i = 0; i < count; i++) {
        const slide = source.cloneNode(true);
        slide.removeAttribute('data-bz-clone');
        slide.removeAttribute('data-swiper-slide-index');
        slide.className = 'swiper-slide';
        slide.addEventListener('click', () => { selections++; });
        root.querySelector('.swiper-wrapper').appendChild(slide);
      }
      await core.initializeCarousel(root, JSON.stringify({ loop: true, slidesPerView: 'auto' }));
      root.querySelector('[data-bz-clone="true"]').click();
      if (selections !== 1) throw new Error(`Expected one forwarded selection for ${count} items, got ${selections}`);
      core.destroyCarousel(root);
      if (root.querySelectorAll('.swiper-slide').length !== count) throw new Error('Loop copies survived teardown');
      root.remove();
      results.push(count);
    }
    return results;
  });
  assert.deepEqual(counts, [4, 5, 6]);
  await page.close();
  console.log('PASS loop copies: one selection and clean teardown for 4, 5 and 6 items');
}

/** Capture each theme on independent host backgrounds without changing shared theme tokens. */
async function inspectHostBackgrounds(browser) {
  const page = await browser.newPage({ viewport: { width: 1200, height: 1000 } });
  await page.goto(baseURL);
  const root = page.locator('.bz-demo .bzc-carousel-box');
  await root.locator('.swiper-slide-active[data-bzc-media=ready]').waitFor();
  const backgrounds = { light: '#f1f5f9', dark: '#111827', mixed: 'linear-gradient(135deg, #e2e8f0, #334155 50%, #22d3ee)' };
  for (const theme of ['Glass', 'Dark', 'Light', 'Minimal']) {
    await page.getByRole('button', { name: theme, exact: true }).click();
    for (const [name, background] of Object.entries(backgrounds)) {
      await root.evaluate((el, value) => { el.parentElement.style.background = value; }, background);
      await root.screenshot({ path: path.join(artifacts, `surface-${theme}-${name}.png`) });
      if (theme === 'Glass') {
        const filter = await root.evaluate(el => getComputedStyle(el).backdropFilter);
        assert.match(filter, /blur/);
        assert(!filter.includes('saturate'), 'Glass should not require extra saturation');
      }
    }
  }
  await page.close();
  console.log('PASS theme surfaces: four themes on light, dark and mixed host backgrounds');
}

/** Exercise slow, cached, failed and replaced mapped artwork independently of Swiper. */
async function inspectImageReadiness(browser) {
  const page = await browser.newPage();
  let release;
  const gate = new Promise(resolve => { release = resolve; });
  await page.route('**/images/*.webp', async route => { await gate; await route.continue(); });
  try {
    await page.goto(baseURL, { waitUntil: 'domcontentloaded' });
    const root = page.locator('.bz-demo .bzc-carousel-box');
    await root.locator('.bzc-visible').waitFor();
    const active = root.locator('.swiper-slide-active');
    await active.locator('.bzc-media-placeholder').waitFor({ state: 'visible' });
    const pending = await active.evaluate(slide => ({ state: slide.dataset.bzcMedia, shadow: getComputedStyle(slide).boxShadow, imageOpacity: getComputedStyle(slide.querySelector('img')).opacity, overlay: getComputedStyle(slide.querySelector('.bzc-overlay')).visibility }));
    assert.deepEqual(pending, { state: 'loading', shadow: 'none', imageOpacity: '0', overlay: 'hidden' });
    const contrast = await active.locator('.bzc-media-placeholder').evaluate(el => {
      const style = getComputedStyle(el);
      const parse = value => value.match(/[\d.]+/g).map(Number);
      const foreground = parse(style.color).slice(0, 3);
      const background = parse(style.backgroundColor);
      const alpha = background[3] ?? 1;
      // White behind this dark placeholder is the least favorable default host surface.
      const composite = background.slice(0, 3).map(value => value * alpha + 255 * (1 - alpha));
      const luminance = values => values.map(value => value / 255)
        .map(value => value <= 0.04045 ? value / 12.92 : ((value + 0.055) / 1.055) ** 2.4)
        .reduce((total, value, index) => total + value * [0.2126, 0.7152, 0.0722][index], 0);
      const values = [luminance(foreground), luminance(composite)].sort((a, b) => a - b);
      return (values[1] + 0.05) / (values[0] + 0.05);
    });
    assert(contrast >= 4.5, `Loading text contrast must reach 4.5:1, got ${contrast}`);
    assert.equal(await root.locator('.swiper-slide:not(.swiper-slide-active) .bzc-media-placeholder').first().evaluate(el => getComputedStyle(el).fontSize), '0px');
    await root.screenshot({ path: path.join(artifacts, 'loading-slow-posters.png') });
    assert.equal(await root.locator('img[loading=eager][fetchpriority=high]').count(), 1);
    release();
    await root.locator('.swiper-slide-active[data-bzc-media=ready]').waitFor();
    assert(await active.locator('.bzc-media-placeholder').isHidden());
    const dimensions = await active.evaluate(slide => ({ image: slide.querySelector('img').offsetHeight, overlay: slide.querySelector('.bzc-overlay').offsetHeight, total: slide.offsetHeight }));
    assert.equal(dimensions.total, dimensions.overlay, 'Decoded slides restore the original full-height overlay');
    assert(dimensions.total > dimensions.image);
    await page.route('**/missing-poster.webp', route => route.abort());
    await active.locator('img').evaluate(image => { image.src = '/missing-poster.webp'; });
    await root.locator('.swiper-slide-active[data-bzc-media=error]').waitFor();
    assert.equal(await active.locator('.bzc-media-placeholder').textContent(), 'Image unavailable');
    await active.locator('img').evaluate(image => { image.src = '/images/despicableme.webp'; });
    await root.locator('.swiper-slide-active[data-bzc-media=ready]').waitFor();
    console.log('PASS image lifecycle: slow placeholder, full-height decoded slide, failure and source replacement');
  } finally { release(); await page.close(); }
}

/** Verify that unavailable Swiper assets leave useful content rather than a blank stage. */
async function inspectFallback(browser) {
  const page = await browser.newPage();
  await page.route('**/swiper-bundle.min.js', route => route.abort());
  await page.goto(baseURL);
  const image = page.locator('.bz-demo .bzc-pending .swiper-slide').first().locator('img');
  await image.waitFor({ state: 'visible' });
  await page.locator('.bz-demo .bzc-pending [data-bzc-media=ready]').first().waitFor();
  assert(await image.evaluate(img => getComputedStyle(img.closest('.swiper-container')).visibility === 'visible' && getComputedStyle(img).opacity === '1'));
  assert(await page.locator('.bz-demo [data-bzc-direction=next]').isDisabled());
  console.log('PASS missing Swiper: first artwork remains visible');
  await page.close();
}

(async () => {
  fs.mkdirSync(artifacts, { recursive: true });
  await waitForHost();
  const browser = await chromium.launch({ executablePath: process.env.CHROMIUM_PATH || undefined, headless: true, args: ['--no-sandbox'] });
  try {
    await inspectCarousel(browser, 'desktop', { viewport: { width: 1440, height: 1000 } });
    await inspectCarousel(browser, 'mobile', { ...devices['iPhone 13'] });
    await inspectCarousel(browser, 'reduced', { viewport: { width: 1440, height: 1000 }, reducedMotion: 'reduce' });
    await inspectLoopCopies(browser);
    await inspectHostBackgrounds(browser);
    await inspectImageReadiness(browser);
    await inspectFallback(browser);
  } finally { await browser.close(); }
})().catch(error => { console.error(error); process.exit(1); });
