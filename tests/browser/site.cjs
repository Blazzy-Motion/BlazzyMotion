const { chromium } = require('playwright');
const assert = require('node:assert/strict');
const AxeBuilder = require('@axe-core/playwright').default;
const fs = require('node:fs');
const path = require('node:path');
const baseURL = process.env.BASE_URL || 'http://127.0.0.1:5130';

/** Verify shared navigation, preserved routes, WCAG checks and mobile component discovery. */
async function inspectSite(browser, width) {
  const context = await browser.newContext({ viewport: { width, height: 960 } });
  const page = await context.newPage();
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
      const audit = await new AxeBuilder({ page }).withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa', 'wcag22aa']).analyze();
      assert.deepEqual(audit.violations.map(violation => ({ id: violation.id, targets: violation.nodes.map(node => node.target) })), [], `Accessibility ${width}px ${route}`);
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
  } finally { await context.close(); }
}

/** Exercise the responsive documentation rail, modal keyboard focus and independent preview surface. */
async function inspectDocumentationNavigation(browser, width) {
  const page = await browser.newPage({ viewport: { width, height: 900 } });
  try {
    await page.goto(baseURL + '/docs/bento');
    await page.locator('.site-header').waitFor();
    await page.waitForFunction(() => !document.querySelector('#splash') || document.querySelector('#splash').classList.contains('bz-splash--hidden'));
    const trigger = page.getByRole('button', { name: 'On this site: open documentation navigation' });
    if (width <= 1024) {
      await trigger.click();
      const dialog = page.getByRole('dialog', { name: 'Documentation navigation' });
      await dialog.waitFor();
      assert.equal(await trigger.getAttribute('aria-expanded'), 'true');
      for (let index = 0; index < 24; index++) {
        await page.keyboard.press('Tab');
        // Native dialogs may move focus into browser chrome; background page controls must remain inert.
        assert(await dialog.evaluate(element => document.activeElement === document.body || element.contains(document.activeElement)), 'Modal excludes background controls');
      }
      await page.locator('.site-brand').evaluate(element => element.focus());
      assert.equal(await page.locator('.site-brand').evaluate(element => document.activeElement === element), false, 'Background links cannot receive focus while modal is open');
      await page.keyboard.press('Escape');
      await dialog.waitFor({ state: 'hidden' });
      await page.waitForFunction(() => document.activeElement?.getAttribute('aria-controls') === 'docs-dialog');
      assert.equal(await trigger.getAttribute('aria-expanded'), 'false');
      await trigger.click();
      await dialog.getByRole('link', { name: 'Gallery Docs', exact: true }).click();
      await page.waitForURL('**/docs/gallery');
      await dialog.waitFor({ state: 'hidden' });
    } else {
      assert.equal(await trigger.isVisible(), false);
      const rail = await page.locator('#docs-navigation').boundingBox();
      const header = await page.locator('.site-header').boundingBox();
      assert(rail && header && rail.y >= header.y + header.height - 1);
      assert.equal(await page.locator('#docs-navigation').evaluate(element => getComputedStyle(element).backgroundColor), 'rgb(22, 23, 26)');
    }
    await page.goto(baseURL + '/carousel');
    const surface = page.locator('.demo-surface');
    await surface.locator('.bzc-visible').waitFor();
    await surface.locator('.swiper-container').evaluate(element => { window.previewSwiper = element.swiper; });
    await page.getByLabel('Preview background').selectOption('tinted');
    assert.equal(await surface.getAttribute('data-preview-background'), 'tinted');
    assert(await surface.locator('.swiper-container').evaluate(element => element.swiper === window.previewSwiper), 'Changing host background preserves the component instance');
    assert.equal(await page.getByRole('navigation', { name: 'Component resources' }).getByRole('link', { name: 'API reference', exact: true }).getAttribute('href'), '/docs/carousel#component-api');
    console.log(`PASS documentation ${width}px: responsive navigation, keyboard focus and independent host background`);
  } finally { await page.close(); }
}

/** Verify keyboard-operable playground controls and focusable documentation code blocks. */
async function inspectDemoControls(browser) {
  const page = await browser.newPage({ viewport: { width: 390, height: 900 } });
  try {
    await page.goto(baseURL + '/gallery');
    const columns = page.getByRole('slider', { name: /^Columns:/ });
    await columns.waitFor();
    await page.waitForFunction(() => !document.querySelector('#splash') || document.querySelector('#splash').classList.contains('bz-splash--hidden'));
    const previous = Number(await columns.inputValue());
    await columns.focus();
    await page.keyboard.press(previous < 5 ? 'ArrowRight' : 'ArrowLeft');
    assert.notEqual(Number(await columns.inputValue()), previous);
    assert.equal(await page.getByRole('slider', { name: /^Gap:/ }).count(), 1);
    await page.goto(baseURL + '/marquee');
    const direction = page.getByRole('group', { name: 'Direction', exact: true });
    await direction.waitFor();
    await page.waitForFunction(() => !document.querySelector('#splash') || document.querySelector('#splash').classList.contains('bz-splash--hidden'));
    const right = direction.getByRole('button', { name: 'Right', exact: true });
    await right.focus();
    await page.keyboard.press('Space');
    await page.waitForFunction(() => document.querySelector('[aria-labelledby="direction-label"] button:last-child').getAttribute('aria-pressed') === 'true');
    assert.equal(await direction.getByRole('button', { name: 'Left', exact: true }).getAttribute('aria-pressed'), 'false');
    await page.goto(baseURL + '/docs/examples');
    await page.locator('.bz-code-block pre').first().waitFor();
    assert(await page.locator('.bz-code-block pre').evaluateAll(elements => elements.every(element => element.tabIndex === 0)));
    const code = page.locator('.bz-code-block pre').first();
    await code.focus();
    assert(await code.evaluate(element => element === document.activeElement));
    console.log('PASS Demo controls: labeled Gallery sliders, keyboard Marquee state and code-block focus');
  } finally { await page.close(); }
}

/** Require the published Demo to expose central build versions and install matching packages. */
async function inspectVersionMetadata(browser) {
  const props = fs.readFileSync(path.join(__dirname, '../../Directory.Build.props'), 'utf8');
  const versions = Object.fromEntries(['Carousel', 'Bento', 'Gallery', 'Marquee'].map(name => {
    const match = props.match(new RegExp(`<Blazzy${name}Version[^>]*>([^<]+)</`));
    assert(match, `Central version exists for ${name}`);
    return [name, match[1]];
  }));
  const page = await browser.newPage({ viewport: { width: 390, height: 960 } });
  try {
    await page.goto(baseURL + '/components');
    await page.locator('.component-entry').first().waitFor();
    for (const [name, version] of Object.entries(versions)) {
      const entry = page.locator('.component-entry').filter({ has: page.locator(`h3 a[href="/${name.toLowerCase()}"]`) });
      const status = version.split('+')[0].includes('-') ? 'preview' : 'stable';
      assert.equal((await entry.locator('.component-entry__version').textContent()).trim(), `${status} ${version}`);
      await page.goto(baseURL + '/' + name.toLowerCase());
      const command = page.locator('.bz-install-box__command').first();
      await command.waitFor();
      assert.equal((await command.textContent()).trim(), `dotnet add package BlazzyMotion.${name} --version ${version}`);
      await page.goto(baseURL + '/components');
      await page.locator('.component-entry').first().waitFor();
    }
    console.log('PASS Demo versions: central metadata, accurate preview badges and pinned installation commands');
  } finally { await page.close(); }
}

(async () => {
  const browser = await chromium.launch({ executablePath: process.env.CHROMIUM_PATH || undefined, headless: true, args: ['--no-sandbox'] });
  try { await inspectSite(browser, 1440); await inspectSite(browser, 390); await inspectDocumentationNavigation(browser, 390); await inspectDocumentationNavigation(browser, 980); await inspectDocumentationNavigation(browser, 1440); await inspectDemoControls(browser); await inspectVersionMetadata(browser); }
  finally { await browser.close(); }
})().catch(error => { console.error(error); process.exit(1); });
