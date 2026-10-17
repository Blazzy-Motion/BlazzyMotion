const { chromium, devices } = require('playwright');
const assert = require('node:assert/strict');
/** Verify isolated Demo diagnostics preserve navigation and selectively pause callbacks. */
(async()=>{
const browser=await chromium.launch({executablePath:'/usr/bin/chromium',args:['--no-sandbox']});
for(const mode of ['images','layers','callbacks','main-css','default']){
 const context=await browser.newContext({...devices['Galaxy S9+']});
 const page=await context.newPage(); const errors=[]; page.on('pageerror',e=>errors.push(e.message));
 await page.goto((process.env.BASE_URL || 'http://127.0.0.1:5135') + '/bento'+(mode==='default'?'':'?swipeTest='+mode));
 const stage=page.locator('.bzb-presentation__stage').filter({has:page.locator('.bzc-carousel-box')}).first();
 await stage.waitFor();
 if(mode!=='default') await page.waitForFunction(m=>document.querySelector(`[data-swipe-test="${m}"]`),mode);
 else await page.waitForFunction(()=>document.querySelector('.bzb-presentation__stage .swiper-container')?.swiper?.initialized);
 if(mode==='main-css') {
  await page.waitForTimeout(800);
  const baseline=await stage.evaluate(el=>{const root=el.querySelector('.bzc-carousel-box'); const slide=root.querySelector('.swiper-slide-active'); return {blur:getComputedStyle(root).backdropFilter,zIndex:getComputedStyle(slide).zIndex,shadow:getComputedStyle(slide.querySelector('img')).boxShadow,helper:getComputedStyle(root.querySelector('.bzc-sr-only')).width};});
  assert.equal(baseline.blur,'blur(12px)'); assert.equal(baseline.zIndex,'100'); assert(baseline.shadow.includes('0.95')); assert.equal(baseline.helper,'1px');
 }
 const before=await stage.locator('.bzb-metric').allTextContents();
 const result=await stage.evaluate((el,mode)=>{const s=el.querySelector('.swiper-container').swiper; const images=[...s.el.querySelectorAll('img')]; const before=s.realIndex; s.slideNext(0); return {before,after:s.realIndex,ready:images.every(i=>i.complete&&i.naturalWidth>0),layers:images.every(i=>i.style.willChange==='transform')};},mode);
 assert.notEqual(result.before,result.after); if(mode==='images')assert(result.ready); if(mode==='layers')assert(result.layers);
 await page.waitForTimeout(500);
 const after=await stage.locator('.bzb-metric').allTextContents();
 if(mode==='callbacks')assert.deepEqual(before,after);else assert.notDeepEqual(before,after);
 assert.deepEqual(errors,[]); console.log(mode,'passed'); await context.close();
} await browser.close();
})().catch(e=>{console.error(e);process.exit(1)});
