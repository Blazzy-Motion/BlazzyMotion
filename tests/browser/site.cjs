const { chromium } = require('playwright');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const baseURL = process.env.BASE_URL || 'http://127.0.0.1:5130';

/** Verify shared navigation, preserved documentation routes and mobile component discovery. */
async function inspectSite(browser, width) {
  const page = await browser.newPage({ viewport: { width, height: 960 } });
  const errors = [];
  page.on('pageerror', error => errors.push(error.message));
  try {
    const routes = ['/', '/components', '/carousel', '/bento', '/gallery', '/marquee',
      '/docs/installation', '/docs/quick-start', '/docs/carousel', '/docs/bento',
      '/docs/gallery', '/docs/marquee', '/docs/theming', '/docs/css-variables', '/docs/api', '/docs/examples'];
    for (const route of routes) {
      await page.goto(baseURL + route);
      await page.getByRole('navigation', { name: 'Primary navigation', exact: true }).waitFor();
      await page.locator('#main-content h1, #main-content h2').first().waitFor();
      await page.waitForFunction(() => !document.querySelector('#splash') || document.querySelector('#splash').classList.contains('bz-splash--hidden'));
      assert.equal(await page.getByRole('alert').filter({ hasText: "Sorry, there's nothing" }).count(), 0, route);
      assert.equal(await page.locator('#main-content').count(), 1);
      const navigation = page.getByRole('navigation', { name: 'Primary navigation', exact: true });
      const box = await navigation.boundingBox();
      assert(box && box.x >= 0 && box.x + box.width <= width + 1, `Navigation fits ${route}`);
    }
    await page.getByRole('navigation', { name: 'Primary navigation', exact: true }).getByRole('link', { name: 'Components', exact: true }).click();
    await page.waitForURL('**/components');
    assert.equal(await page.locator('.component-entry').count(), 4);
    await page.locator('.component-entry').getByRole('link', { name: 'Bento Grid' }).click();
    await page.waitForURL('**/bento');
    await page.getByRole('group', { name: 'Component theme' }).first().getByRole('button', { name: 'Light', exact: true }).click();
    await page.waitForFunction(() => document.querySelector('.bz-theme-pills button[aria-pressed="true"]').textContent.includes('Light'));
    assert.deepEqual(errors, []);
    const artifacts = path.join(__dirname, 'artifacts');
    fs.mkdirSync(artifacts, { recursive: true });
    await page.screenshot({ path: path.join(artifacts, `site-bento-${width}.png`), fullPage: true });
    console.log(`PASS site ${width}px: all 16 routes, shared navigation, component discovery and theme state`);
  } finally { await page.close(); }
}

(async () => {
  const browser = await chromium.launch({ executablePath: process.env.CHROMIUM_PATH || undefined, headless: true, args: ['--no-sandbox'] });
  try { await inspectSite(browser, 1440); await inspectSite(browser, 390); }
  finally { await browser.close(); }
})().catch(error => { console.error(error); process.exit(1); });
