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
  assert.equal(await root.locator('.swiper-slide[aria-hidden=false]').count(), 1);
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
  for (const theme of ['Glass', 'Dark', 'Light', 'Minimal']) {
    await page.getByRole('button', { name: theme, exact: true }).click();
    await page.waitForTimeout(150);
    const result = await new AxeBuilder({ page }).include('.bz-demo .bzc-carousel-box').analyze();
    assert.deepEqual(result.violations.map(v => ({ id: v.id, impact: v.impact })), [], `${name}/${theme} accessibility`);
    await page.screenshot({ path: path.join(artifacts, `${name}-${theme}.png`) });
  }
  if (contextOptions.reducedMotion === 'reduce') {
    assert.equal(await root.evaluate(el => el.querySelector('.swiper-container').swiper.params.effect), 'slide');
    assert.equal(await root.evaluate(el => el.querySelector('.swiper-container').swiper.params.speed), 0);
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

/** Verify that unavailable Swiper assets leave useful content rather than a blank stage. */
async function inspectFallback(browser) {
  const page = await browser.newPage();
  await page.route('**/swiper-bundle.min.js', route => route.abort());
  await page.goto(baseURL);
  const image = page.locator('.bz-demo .bzc-pending .swiper-slide').first().locator('img');
  await image.waitFor({ state: 'visible' });
  assert(await image.evaluate(img => getComputedStyle(img.closest('.swiper-container')).visibility === 'visible'));
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
    await inspectFallback(browser);
  } finally { await browser.close(); }
})().catch(error => { console.error(error); process.exit(1); });
