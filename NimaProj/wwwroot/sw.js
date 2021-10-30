const staticCacheName = 'site-static-v0.0.6';
const isOnline = navigator?.onLine;

// install the service worker
self.addEventListener('install', (evt) => {
    evt.waitUntil(
        caches.open(staticCacheName).then(cache => {

        })
    )
});

// activate service worker
self.addEventListener('activate', (evt) => {

    // Clear all caches
    evt.waitUntil(
        caches.keys().then(keys => {
            return Promise.all(keys
                .map(key => { caches.delete(key); }))
        })
    )
});

// intercept caches
self.addEventListener('fetch', (evt) => {

    if (!(evt.request.url.indexOf('http') === 0)) return;

    if (evt.request.url.includes('a.tile.openstreetmap.org')) return;
})