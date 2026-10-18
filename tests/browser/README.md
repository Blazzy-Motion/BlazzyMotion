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
gesture and complete eight-company loop in both directions inside a Bento
composition (including stable dimensions, dashboard updates and preserved
animations on ordinary Bento cards). A separate route-lifecycle regression covers
Home/Bento reentry, ancestor updates while a touch is held, container resizing
without a window resize, metric updates and instance disposal. Screenshots are saved
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

The Carousel example checks exercise `/carousel` at 320, 390 and 1440 px, including real clipboard permissions in an isolated browser context, four theme changes after keyboard navigation, selection/instance and preview-size preservation, keyboard opening/closing of initially hidden source through the example toolbar, model disclosure and expanded-content WCAG scans. The copied `CarouselDemo.razor` and `Movie.cs` are saved in `artifacts/` for optional standalone compilation.

Shared example checks cover Bento, Gallery and Marquee at 320, 390 and 1440 px: centered theme controls, four synchronized themes, changed Gallery/Marquee settings, toolbar source interaction, exact clipboard contents, expanded source-content WCAG scans, horizontal overflow and retention of the nested Bento Carousel instance and index. The seven copied snippets and their models are saved under `artifacts/examples/<width>/<example>/` for standalone consumer compilation.

Compact playground checks also verify local navigation beneath the component introduction, active Playground links, the first example toolbar position, copying while source is hidden, ARIA expanded/control relationships, persistent preview dimensions, clipboard denial/recovery and focus restoration through Close/Escape.

Density checks verify Carousel toolbar/source alignment with the unchanged desktop 800 × 450 px frame, a compact preview/source gap, keyboard access to Advanced options, retained secondary settings after closing/reopening, and expanded-controls accessibility at 320, 390 and 1440 px.
