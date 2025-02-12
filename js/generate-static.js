const puppeteer = require('puppeteer');
const fs = require('fs');
const pages = [
    { url: 'https://moto-club-cote-emeraude.pages.dev/', file: 'index.html' },
    { url: 'https://moto-club-cote-emeraude.pages.dev/about', file: 'about.html' },
    { url: 'https://moto-club-cote-emeraude.pages.dev/contact', file: 'contact.html' },
    { url: 'https://moto-club-cote-emeraude.pages.dev/events', file: 'events.html' },
    { url: 'https://moto-club-cote-emeraude.pages.dev/history', file: 'history.html' },
    { url: 'https://moto-club-cote-emeraude.pages.dev/legal', file: 'legal.html' },
    { url: 'https://moto-club-cote-emeraude.pages.dev/news', file: 'news.html' },
    { url: 'https://moto-club-cote-emeraude.pages.dev/presentation', file: 'presentation.html' },
    { url: 'https://moto-club-cote-emeraude.pages.dev/siteplan', file: 'siteplan.html' },
];

(async () => {
    const browser = await puppeteer.launch();
    for (const pageInfo of pages) {
        const page = await browser.newPage();
        await page.goto(pageInfo.url, { waitUntil: 'networkidle2' });
        const content = await page.content();
        fs.writeFileSync(pageInfo.file, content);
        console.log(`Page saved: ${pageInfo.file}`);
    }
    await browser.close();
})();
