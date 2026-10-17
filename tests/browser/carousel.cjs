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
    await inspectFallback(browser);
  } finally { await browser.close(); }
})().catch(error => { console.error(error); process.exit(1); });
