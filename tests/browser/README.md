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
reduced-motion rendering and an unavailable Swiper script. Screenshots are saved
under `artifacts/`. These checks do not establish real-device performance, full
application accessibility or screen-reader interoperability.
