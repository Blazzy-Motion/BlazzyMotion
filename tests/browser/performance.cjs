const { chromium, devices } = require('playwright');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const baseURL = process.env.BASE_URL || 'http://127.0.0.1:5130';
const artifacts = path.join(__dirname, 'artifacts');

/** Parse positive numeric settings without silently accepting invalid measurement profiles. */
function positiveNumbers(value) {
  const numbers = value.split(',').map(Number);
  assert(numbers.length && numbers.every(number => Number.isFinite(number) && number > 0));
  return numbers;
}

/** Record animation callback intervals and long tasks without reading DOM geometry per frame. */
async function startMeasurement(page) {
  await page.evaluate(() => {
    const sample = { frames: [], tasks: [], start: performance.now(), end: null, frame: null };
    const observer = new PerformanceObserver(list => {
      for (const entry of list.getEntries()) sample.tasks.push({ start: entry.startTime, duration: entry.duration });
    });
    observer.observe({ type: 'longtask' });
    let previous;
    const tick = time => {
      if (previous !== undefined) sample.frames.push(time - previous);
      previous = time;
      sample.frame = requestAnimationFrame(tick);
    };
    sample.frame = requestAnimationFrame(tick);
    window.bzcPerformanceSample = { sample, observer };
  });
}

/** Finish a sample before inspecting layout or taking screenshots, avoiding measurement overhead. */
async function finishMeasurement(page) {
  return page.evaluate(() => {
    const { sample, observer } = window.bzcPerformanceSample;
    sample.end = performance.now();
    cancelAnimationFrame(sample.frame);
    for (const entry of observer.takeRecords()) sample.tasks.push({ start: entry.startTime, duration: entry.duration });
    observer.disconnect();
    delete window.bzcPerformanceSample;
    return sample;
  });
}

/** Dispatch repeatable horizontal touch gestures through Chromium's input pipeline. */
async function swipeCarousel(cdp, page, bounds, count) {
  const x = bounds.x + bounds.width * 0.75;
  const y = bounds.y + bounds.height * 0.5;
  for (let swipe = 0; swipe < count; swipe++) {
    await cdp.send('Input.dispatchTouchEvent', { type: 'touchStart', touchPoints: [{ x, y }] });
    for (let step = 1; step <= 18; step++) {
      await cdp.send('Input.dispatchTouchEvent', { type: 'touchMove', touchPoints: [{ x: x - bounds.width * 0.55 * step / 18, y }] });
      await page.waitForTimeout(16);
    }
    await cdp.send('Input.dispatchTouchEvent', { type: 'touchEnd', touchPoints: [] });
    await page.waitForTimeout(220);
  }
  await page.waitForTimeout(300);
}

/** Measure all requested themes with real callbacks enabled; do not assert hardware frame rates. */
async function run() {
  const rates = positiveNumbers(process.env.PERF_CPU_RATES || '1,4');
  const repeats = positiveNumbers(process.env.PERF_REPEATS || '1')[0];
  const swipes = positiveNumbers(process.env.PERF_SWIPES || '8')[0];
  assert(Number.isInteger(repeats) && Number.isInteger(swipes));
  const themes = (process.env.PERF_THEMES || 'Glass,Dark,Light,Minimal').split(',');
  assert(themes.every(theme => ['Glass', 'Dark', 'Light', 'Minimal'].includes(theme)));
  fs.mkdirSync(artifacts, { recursive: true });
  const results = [];
  const browser = await chromium.launch({ executablePath: process.env.CHROMIUM_PATH || undefined, headless: true, args: ['--no-sandbox'] });
  try {
    for (const rate of rates) {
      const context = await browser.newContext({ ...devices['Galaxy S9+'], viewport: { width: 412, height: 915 } });
      const page = await context.newPage();
      const errors = [];
      page.on('pageerror', error => errors.push(error.message));
      const cdp = await context.newCDPSession(page);
      await page.goto(baseURL);
      const root = page.locator('.bz-demo .bzc-carousel-box');
      await root.locator('.swiper-slide-active[data-bzc-media=ready]').waitFor({ timeout: 60000 });
      await root.scrollIntoViewIfNeeded();
      await root.locator('img').evaluateAll(images => Promise.all(images.map(image => image.decode().catch(() => {}))));
      await page.waitForFunction(() => !document.querySelector('.bz-splash'));
      const documentID = await page.evaluate(() => { window.bzcPerformanceDocument = crypto.randomUUID(); return window.bzcPerformanceDocument; });
      await cdp.send('Emulation.setCPUThrottlingRate', { rate });
      for (const theme of themes) {
        await page.getByRole('button', { name: theme, exact: true }).click();
        await root.scrollIntoViewIfNeeded();
        for (let repeat = 1; repeat <= repeats; repeat++) {
          await root.evaluate(element => element.querySelector('.swiper-container').swiper.slideToLoop(0, 0));
          await page.waitForTimeout(1000);
          const bounds = await root.boundingBox();
          const state = await root.evaluate(element => {
            const swiper = element.querySelector('.swiper-container').swiper;
            window.bzcPerformanceChanges = [];
            const listener = () => window.bzcPerformanceChanges.push(swiper.realIndex);
            swiper.on('slideChange', listener);
            window.bzcPerformanceDetach = () => swiper.off('slideChange', listener);
            return { blur: getComputedStyle(element).backdropFilter, slideHeight: element.querySelector('.swiper-slide').offsetHeight, imageWidth: element.querySelector('img').offsetWidth };
          });
          await startMeasurement(page);
          await swipeCarousel(cdp, page, bounds, swipes);
          const sample = await finishMeasurement(page);
          const endState = await page.evaluate(() => {
            window.bzcPerformanceDetach();
            delete window.bzcPerformanceDetach;
            return { documentID: window.bzcPerformanceDocument, indices: window.bzcPerformanceChanges };
          });
          const frames = sample.frames.slice(1).sort((a, b) => a - b);
          assert(frames.length > 0, 'Animation sampling must run');
          assert.equal(endState.documentID, documentID, 'Swiping must not reload the page');
          assert.deepEqual(errors, [], 'Swiping must not cause page errors');
          assert.equal(state.slideHeight, 250, 'Keep original mobile slide height');
          assert.equal(state.imageWidth, 90, 'Keep original mobile artwork width');
          assert(new Set(endState.indices).size > 1, 'Touch input must actually navigate slides');
          const result = {
            theme, cpuRate: rate, repeat, state, durationMs: sample.end - sample.start,
            averageRafHz: 1000 * frames.length / frames.reduce((sum, value) => sum + value, 0),
            p95IntervalMs: frames[Math.floor(frames.length * 0.95)], maxIntervalMs: frames.at(-1),
            intervalsOver25ms: frames.filter(value => value > 25).length, longTasks: sample.tasks,
            slideChangeEvents: endState.indices.length,
            distinctIndexChanges: endState.indices.filter((index, position, indices) => position === 0 || index !== indices[position - 1]).length,
            pageReloaded: false, errors: [...errors]
          };
          results.push(result);
          console.log(JSON.stringify(result));
          await root.screenshot({ path: path.join(artifacts, `performance-${theme}-${rate}x-${repeat}.png`) });
        }
      }
      await context.close();
    }
  } finally {
    fs.writeFileSync(path.join(artifacts, 'performance.json'), JSON.stringify({ browser: browser.version(), note: 'rAF scheduling in emulated Chromium is not displayed FPS or a physical-device certification.', results }, null, 2));
    await browser.close();
  }
}

run().catch(error => { console.error(error); process.exit(1); });
