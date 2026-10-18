/** Capture lightweight catalog artwork from the actual locally published component demos. */
const { chromium } = require('playwright');
const fs = require('node:fs');
const path = require('node:path');
const baseURL = process.env.BASE_URL || 'http://127.0.0.1:5130';
const output = path.resolve(__dirname, '../../src/BlazzyMotion.Demo/wwwroot/images/previews');

/** Wait for a real demo, its entrance state and decoded images, then capture its component bounds. */
async function capturePreview(browser, id, selector, theme) {
  const page = await browser.newPage({ viewport: { width: 1008, height: 900 }, reducedMotion: 'no-preference' });
  try {
    await page.goto(baseURL + '/' + id);
    await page.locator('.site-header').waitFor();
    await page.locator('#splash').waitFor({ state: 'hidden' });
    if (theme) {
      await page.getByRole('group', { name: 'Component theme' }).first().getByRole('button', { name: theme, exact: true }).click();
      await page.waitForFunction(theme => document.querySelector('.bz-theme-pills button[aria-pressed="true"]')?.textContent.includes(theme), theme);
    }
    if (id === 'marquee') {
      await page.locator('#rows-range').focus();
      await page.keyboard.press('End');
      await page.waitForFunction(() => document.querySelector('#rows-range').getAttribute('aria-valuenow') === '4');
    }
    const component = page.locator(selector).first();
    await component.scrollIntoViewIfNeeded();
    if (id === 'carousel') await component.locator('.swiper-initialized').waitFor();
    await component.evaluate(async element => {
      await document.fonts.ready;
      await Promise.all([...element.querySelectorAll('img')].map(async image => {
        image.loading = 'eager';
        await image.decode();
      }));
    });
    await page.waitForFunction(selector => {
      const element = document.querySelector(selector);
      return element && getComputedStyle(element).opacity === '1' &&
        [...element.querySelectorAll('img')].every(image => getComputedStyle(image).opacity === '1');
    }, selector);
    const screenshot = await component.screenshot({ path: path.join(output, `${id}.jpg`), type: 'jpeg', quality: 78, animations: 'disabled' });
    const size = await page.evaluate(async data => {
      const image = new Image();
      image.src = 'data:image/jpeg;base64,' + data;
      await image.decode();
      return { width: image.naturalWidth, height: image.naturalHeight };
    }, screenshot.toString('base64'));
    console.log(`${id}: ${size.width} × ${size.height}, ${screenshot.length} bytes`);
  } finally { await page.close(); }
}

(async () => {
  fs.mkdirSync(output, { recursive: true });
  const browser = await chromium.launch({ executablePath: process.env.CHROMIUM_PATH || undefined, args: ['--no-sandbox'] });
  try {
    await capturePreview(browser, 'carousel', '.bzc-carousel-box');
    await capturePreview(browser, 'bento', '.bzb-presentation:nth-of-type(3) .bzb-grid');
    await capturePreview(browser, 'gallery', '.bzg-grid');
    await capturePreview(browser, 'marquee', '.bzm-container', 'Light');
  } finally { await browser.close(); }
})().catch(error => { console.error(error); process.exitCode = 1; });
