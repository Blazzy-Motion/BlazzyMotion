# Carousel browser checks

Start the Release Demo on port 5130, then run:

```sh
npm ci --ignore-scripts --prefix tests/browser
node tests/browser/node_modules/playwright/cli.js install chromium
npm test --prefix tests/browser
```

`BASE_URL` overrides the host URL; `CHROMIUM_PATH` selects a locally installed
Chromium executable. The checks cover desktop/mobile original image geometry,
local keyboard navigation, assistive controls, four themes, scoped axe checks,
reduced-motion rendering, an unavailable Swiper script, and the first Carousel
gesture inside a Bento composition (including dashboard updates and preserved
animations on ordinary Bento cards). Screenshots are saved
under `artifacts/`. These checks do not establish real-device performance, full
application accessibility or screen-reader interoperability.

For an opt-in touch performance diagnostic, run it alone after the regression suite:

```sh
BASE_URL=http://127.0.0.1:5130 npm run performance --prefix tests/browser
```

The diagnostic exercises all four themes at normal speed and 4x CPU throttling,
with real Blazor callbacks enabled. `PERF_CPU_RATES=1,4`, `PERF_THEMES=Glass,Dark,Light,Minimal`,
`PERF_REPEATS=1` and `PERF_SWIPES=8` select profiles. JSON measurements and screenshots
are written under `artifacts/`. Initialization and image decoding finish before
measurement; each animation callback records time only, without polling layout.
Do not run builds, other browser suites or tracing concurrently with this diagnostic.

The report contains animation callback intervals, long tasks and Swiper event counts.
Repeated Swiper events may exceed distinct consumer-index changes during loop repairs;
regression checks separately ensure only actual index changes notify Blazor. Rates
measure rAF scheduling, not displayed frames. CPU throttling and software rendering do
not reproduce a specific Android GPU, thermal state or browser. No fixed FPS threshold
is used in CI. Validate physical devices separately with Chrome remote debugging and
DevTools Performance recordings, including first load, fast swipes and theme changes.

Temporary first-swipe diagnostics are available on `/bento?swipeTest=images`,
`/bento?swipeTest=layers`, and `/bento?swipeTest=callbacks`. Open each in a fresh
page and wait for the Ready message before the first gesture. These isolate image
decoding, an image compositing hint, and the configured slide-change callback.
The callback variant intentionally pauses company metrics and accessibility
updates; it is a diagnostic, not a release configuration. The normal `/bento`
page is unchanged. Remove these temporary diagnostics before stable release.
Run `BASE_URL=http://127.0.0.1:5135 node tests/browser/bento-swipe-diagnostics.cjs`
to verify the isolation controls against a running Demo.

`/bento?swipeTest=main-css` replaces the Carousel scoped stylesheet with the
compiled original from main commit `447945580482bc437937fefa2cfaf012155063c8`.
It retains current JavaScript, image markup and the stationary Bento host fix,
so it isolates Carousel CSS rather than reproducing the entire old application.
Screen-reader/control helpers remain hidden; new loading placeholders are hidden
because the original stylesheet did not include them. This page-only stylesheet
replacement is temporary and must be removed before release.
