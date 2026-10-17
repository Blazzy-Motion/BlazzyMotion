# Carousel browser regression checks

These checks exercise real Swiper behavior in the Demo, both presentations and
all four themes on desktop, mobile emulation and reduced-motion settings. They
assert keyboard isolation, artwork size, compact layout and a panel-free Cards
presentation, run scoped axe checks, and save screenshots for visual review.
Automated checks do not replace real-device or screen-reader testing.

Start the Demo with the repository's .NET 8 SDK:

```sh
dotnet run --project src/BlazzyMotion.Demo -c Release --no-launch-profile --urls http://127.0.0.1:5130
```

Then, in another terminal:

```sh
cd tests/browser
npm ci
npx playwright install chromium
npm test
```

`BASE_URL` overrides the server address. `CHROMIUM_PATH` can select an existing
Chromium executable. Dependencies and screenshots are ignored by Git. CI starts
the Demo, runs the checks and uploads screenshots alongside test/coverage results.
