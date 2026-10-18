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

/** Require the published Demo to expose central build versions and default to stable installation. */
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
      assert.equal((await command.textContent()).trim(), `dotnet add package BlazzyMotion.${name}`);
      await page.goto(baseURL + '/components');
      await page.locator('.component-entry').first().waitFor();
    }
    console.log('PASS Demo versions: central metadata, accurate preview badges and unversioned stable installation commands');
  } finally { await page.close(); }
}

/** Swipe the document using trusted browser touch input rather than programmatic scrolling. */
async function swipeDocument(page) {
  const input = await page.context().newCDPSession(page);
  try {
    await input.send('Input.dispatchTouchEvent', { type: 'touchStart', touchPoints: [{ x: 300, y: 550 }] });
    for (let y = 530; y >= 180; y -= 20) {
      await input.send('Input.dispatchTouchEvent', { type: 'touchMove', touchPoints: [{ x: 300, y }] });
      await page.waitForTimeout(16);
    }
    await input.send('Input.dispatchTouchEvent', { type: 'touchEnd', touchPoints: [] });
  } finally { await input.detach(); }
}

/** Require mobile document scrolling before and after using the documentation modal. */
async function inspectDocumentTouchScrolling(browser) {
  const context = await browser.newContext({ viewport: { width: 390, height: 650 }, isMobile: true, hasTouch: true });
  const page = await context.newPage();
  try {
    for (const route of ['/docs/quick-start', '/docs/api']) {
      await page.goto(baseURL + route);
      await page.waitForFunction(() => !document.querySelector('#splash') || document.querySelector('#splash').classList.contains('bz-splash--hidden'));
      await swipeDocument(page);
      await page.waitForFunction(() => window.scrollY > 50);
      await page.evaluate(() => window.scrollTo(0, 0));
      const trigger = page.getByRole('button', { name: 'On this site: open documentation navigation' });
      await trigger.click();
      await page.getByRole('dialog').waitFor();
      await page.keyboard.press('Escape');
      await page.getByRole('dialog').waitFor({ state: 'hidden' });
      await swipeDocument(page);
      await page.waitForFunction(() => window.scrollY > 50);
      await page.evaluate(() => window.scrollTo(0, 0));
      await trigger.click();
      await page.getByRole('dialog').getByRole('link', { name: 'Gallery Docs', exact: true }).click();
      await page.waitForURL('**/docs/gallery');
      await page.getByRole('dialog').waitFor({ state: 'hidden' });
      await page.evaluate(() => window.scrollTo(0, 0));
      await swipeDocument(page);
      await page.waitForFunction(() => window.scrollY > 50);
    }
    console.log('PASS document touch: Docs/API scroll on entry, after Escape and after modal navigation');
  } finally { await context.close(); }
}

/** Verify real touch scrolling reaches the end of documentation navigation without moving the page. */
async function inspectTouchNavigation(browser) {
  const context = await browser.newContext({ viewport: { width: 390, height: 650 }, isMobile: true, hasTouch: true });
  const page = await context.newPage();
  const input = await context.newCDPSession(page);
  try {
    for (const route of ['/docs/quick-start', '/docs/api']) {
      await page.goto(baseURL + route);
      const trigger = page.getByRole('button', { name: 'On this site: open documentation navigation' });
      await trigger.click();
      const drawer = page.locator('#mobile-docs-navigation');
      const backgroundScroll = await page.evaluate(() => window.scrollY);
      for (let swipe = 0; swipe < 7; swipe++) {
        await input.send('Input.dispatchTouchEvent', { type: 'touchStart', touchPoints: [{ x: 170, y: 500 }] });
        for (let y = 480; y >= 160; y -= 20) {
          await input.send('Input.dispatchTouchEvent', { type: 'touchMove', touchPoints: [{ x: 170, y }] });
          await page.waitForTimeout(16);
        }
        await input.send('Input.dispatchTouchEvent', { type: 'touchEnd', touchPoints: [] });
        await page.waitForTimeout(100);
      }
      await page.waitForFunction(() => {
        const drawer = document.querySelector('#mobile-docs-navigation');
        return drawer.scrollTop > 0 && drawer.scrollTop + drawer.clientHeight >= drawer.scrollHeight - 2;
      });
      const footer = await drawer.locator('.bz-sidebar__footer').boundingBox();
      assert(footer && footer.y >= 0 && footer.y + footer.height <= 651, `Footer reachable by touch on ${route}`);
      assert.equal(await page.evaluate(() => window.scrollY), backgroundScroll, 'Background remains stationary');
      await page.keyboard.press('Escape');
      await page.getByRole('dialog').waitFor({ state: 'hidden' });
    }
    console.log('PASS documentation touch: short mobile viewport, Docs/API end links and stationary background');
  } finally { await context.close(); }
}

/** Verify the isolated Carousel page keeps copied code, theme state and the real preview in sync. */
async function inspectCarouselCodeExample(browser) {
  for (const width of [320, 390, 1440]) {
    const context = await browser.newContext({ viewport: { width, height: 960 }, permissions: ['clipboard-read', 'clipboard-write'] });
    const page = await context.newPage();
    try {
      await page.goto(baseURL + '/carousel');
      await page.locator('#splash').waitFor({ state: 'hidden' });
      const themeBounds = await page.getByRole('group', { name: 'Component theme', exact: true }).boundingBox();
      assert(themeBounds && Math.abs(themeBounds.x + themeBounds.width / 2 - width / 2) <= 2, `Centered Carousel themes at ${width}px`);
      const preview = page.getByRole('region', { name: 'Live preview', exact: true });
      const swiper = preview.locator('.swiper-initialized');
      await swiper.waitFor();
      await swiper.evaluate(element => { window.codeExampleSwiper = element.swiper; });
      await preview.locator('.bzc-carousel-box').focus();
      await page.keyboard.press('ArrowRight');
      await page.waitForFunction(() => window.codeExampleSwiper.realIndex === 1 && document.querySelector('.carousel-example__selection').textContent.trim() === 'Joker');
      const code = page.locator('.carousel-example__component-code code');
      assert.equal((await code.textContent()).match(/new\(\) \{ Title =/g).length, Number(await preview.locator('.bzc-carousel-box').getAttribute('data-item-count')));
      for (const theme of ['Dark', 'Light', 'Minimal', 'Glass']) {
        await page.getByRole('group', { name: 'Component theme' }).getByRole('button', { name: theme, exact: true }).click();
        await page.waitForFunction(theme => document.querySelector('.carousel-example__component-code code').textContent.includes(`BzTheme.${theme}`), theme);
        assert(await swiper.evaluate(element => element.swiper === window.codeExampleSwiper && element.swiper.realIndex === 1), 'Theme changes retain the live component instance and selected movie');
      }
      assert(!await code.isVisible(), 'Code starts collapsed so the live preview remains the focus');
      const previewBefore = await preview.boundingBox();
      const disclosure = page.locator('.carousel-example__source > summary');
      await disclosure.focus();
      await page.keyboard.press('Enter');
      await code.waitFor();
      const displayed = await code.textContent();
      await page.locator('.carousel-example__component-code').getByRole('button', { name: 'Copy', exact: true }).click();
      await page.waitForFunction(expected => navigator.clipboard.readText().then(actual => actual === expected), displayed);
      await page.getByText('Movie.cs — model attributes', { exact: true }).click();
      const model = page.locator('.carousel-example__model code');
      await model.waitFor();
      assert((await model.textContent()).includes('[BzImage]'));
      assert((await model.textContent()).includes('namespace YourApp.Models;'));
      const before = await preview.boundingBox();
      const source = await page.getByRole('region', { name: 'Razor example', exact: true }).boundingBox();
      assert(before && source && source.y > before.y + before.height - 1, 'Optional code remains below the full-width preview at every viewport');
      assert(await page.evaluate(() => document.documentElement.scrollWidth <= innerWidth + 1), 'Long code stays inside its scroll region');
      const artifacts = path.join(__dirname, 'artifacts');
      fs.mkdirSync(artifacts, { recursive: true });
      fs.writeFileSync(path.join(artifacts, 'CarouselDemo.razor'), displayed);
      fs.writeFileSync(path.join(artifacts, 'Movie.cs'), await model.textContent());
      const audit = await new AxeBuilder({ page }).withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa', 'wcag22aa']).analyze();
      assert.deepEqual(audit.violations.map(violation => violation.id), [], `Expanded code accessibility at ${width}px`);
      await disclosure.focus();
      await page.keyboard.press('Enter');
      assert(!await code.isVisible(), 'Keyboard activation closes the code disclosure');
      assert(await disclosure.evaluate(element => document.activeElement === element), 'Focus remains on the disclosure');
      assert(await swiper.evaluate(element => element.swiper === window.codeExampleSwiper && element.swiper.realIndex === 1), 'Opening and closing code preserves the live preview');
      const previewAfter = await preview.boundingBox();
      assert.equal(previewAfter.width, previewBefore.width);
      assert.equal(previewAfter.height, previewBefore.height);
      console.log(`PASS Carousel example ${width}px: exact clipboard, four synchronized themes, model disclosure and responsive code layout`);
    } finally { await context.close(); }
  }
}

/** Verify every Demo example exposes usable code without replacing its live component. */
async function inspectAllComponentExamples(browser) {
  const examples = { bento: ['bento-items', 'bento-dashboard', 'bento-composition'], gallery: ['gallery-layout'], marquee: ['marquee-logos', 'marquee-testimonials', 'marquee-ticker'] };
  for (const width of [320, 390, 1440]) {
    const context = await browser.newContext({ viewport: { width, height: 960 }, permissions: ['clipboard-read', 'clipboard-write'] });
    const page = await context.newPage();
    try {
      for (const [route, ids] of Object.entries(examples)) {
        await page.goto(baseURL + '/' + route);
        await page.locator('#splash').waitFor({ state: 'hidden' });
        const themes = page.getByRole('group', { name: 'Component theme', exact: true });
        const bounds = await themes.boundingBox();
        assert(bounds && Math.abs(bounds.x + bounds.width / 2 - width / 2) <= 2, `Centered ${route} themes at ${width}px`);
        for (const theme of ['Dark', 'Light', 'Minimal', 'Glass']) {
          await themes.getByRole('button', { name: theme, exact: true }).click();
          await page.waitForFunction(({ ids, theme }) => ids.every(id => document.querySelector(`[data-example="${id}"] .example-source__component code`).textContent.includes(`BzTheme.${theme}`)), { ids, theme });
        }
        if (route === 'gallery') {
          const layout = width >= 768 ? 'Masonry' : 'List';
          await page.getByRole('button', { name: layout, exact: true }).click();
          await page.getByRole('slider', { name: /^Gap:/ }).focus();
          await page.keyboard.press('ArrowRight');
          await page.waitForFunction(() => document.querySelector('.example-source__component code').textContent.includes('Gap="20"'));
          assert((await page.locator('.example-source__component code').textContent()).includes(`BzGalleryLayout.${layout}`));
        }
        if (route === 'marquee') {
          await page.getByRole('group', { name: 'Direction', exact: true }).getByRole('button', { name: 'Right', exact: true }).click();
          await page.getByRole('slider', { name: /^Speed Var:/ }).focus();
          await page.keyboard.press('ArrowRight');
          await page.waitForFunction(() => document.querySelector('[data-example="marquee-logos"] code').textContent.includes('SpeedVariation="0.1"'));
          for (const id of ids) assert((await page.locator(`[data-example="${id}"] .example-source__component code`).textContent()).includes('BzDirection.Right'));
        }
        if (route === 'bento') {
          const swiper = page.locator('.swiper-initialized');
          await swiper.waitFor();
          await swiper.evaluate(element => { window.examplesSwiper = element.swiper; });
          await page.locator('.bzc-carousel-box').focus();
          await page.keyboard.press('ArrowRight');
          await page.waitForFunction(() => window.examplesSwiper.realIndex === 1);
        }
        for (const id of ids) {
          const source = page.locator(`[data-example="${id}"]`);
          const code = source.locator('.example-source__component code');
          assert(!await code.isVisible(), `${id} starts collapsed`);
          const summary = source.locator(':scope > summary');
          await summary.focus();
          await page.keyboard.press('Enter');
          await code.waitFor();
          const displayed = await code.textContent();
          await source.locator('.example-source__component').getByRole('button', { name: 'Copy', exact: true }).click();
          await page.waitForFunction(expected => navigator.clipboard.readText().then(actual => actual === expected), displayed);
          const modelDisclosure = source.locator('.example-source__model');
          let model = '';
          if (await modelDisclosure.count()) {
            await modelDisclosure.locator('summary').click();
            model = await modelDisclosure.locator('code').textContent();
            assert(model.includes('[BzTitle]') && model.includes('[BzImage]'));
          }
          const directory = path.join(__dirname, 'artifacts', 'examples', String(width), id);
          fs.mkdirSync(directory, { recursive: true });
          fs.writeFileSync(path.join(directory, 'Example.razor'), displayed);
          if (model) fs.writeFileSync(path.join(directory, 'Models.cs'), model);
          const audit = await new AxeBuilder({ page }).include(`[data-example="${id}"]`).withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa', 'wcag22aa']).analyze();
          assert.deepEqual(audit.violations.map(violation => violation.id), [], `Expanded ${id} source accessibility at ${width}px`);
          assert(await page.evaluate(() => document.documentElement.scrollWidth <= innerWidth + 1), `${id} fits the viewport`);
          await summary.focus();
          await page.keyboard.press('Enter');
          assert(!await code.isVisible());
          assert(await summary.evaluate(element => document.activeElement === element));
        }
        if (route === 'bento') assert(await page.locator('.swiper-initialized').evaluate(element => element.swiper === window.examplesSwiper && element.swiper.realIndex === 1), 'Bento disclosures preserve the nested Carousel instance and selection');
        console.log(`PASS ${route} examples ${width}px: centered themes, synchronized settings, exact clipboard, keyboard disclosure and WCAG checks`);
      }
    } finally { await context.close(); }
  }
}

(async () => {
  const browser = await chromium.launch({ executablePath: process.env.CHROMIUM_PATH || undefined, headless: true, args: ['--no-sandbox'] });
  try { await inspectSite(browser, 1440); await inspectSite(browser, 390); await inspectDocumentationNavigation(browser, 390); await inspectDocumentationNavigation(browser, 980); await inspectDocumentationNavigation(browser, 1440); await inspectDemoControls(browser); await inspectVersionMetadata(browser); await inspectTouchNavigation(browser); await inspectDocumentTouchScrolling(browser); await inspectCarouselCodeExample(browser); await inspectAllComponentExamples(browser); }
  finally { await browser.close(); }
})().catch(error => { console.error(error); process.exit(1); });
