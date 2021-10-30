// Register service worker
if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('/sw.js')
        .then((reg) => {
            console.log('service worker registered', reg);
        })
        .catch((err) => {
            console.log('service worker not registered', err);
        })
}

// Is the app already installed
let isInStandaloneMode = window.matchMedia('(display-mode: standalone)').matches;

// Is the browser SAFARI
const isSafari = /^((?!chrome|android).)*safari/i.test(navigator.userAgent);

// Is the operating IOS 
const isIos = /iphone|ipad|ipod/.test(window.navigator.userAgent.toLowerCase());

// Install notification
let deferredPrompt;

// Initialize the Prompt and save it in a variable
window.addEventListener('beforeinstallprompt', (e) => {
    try {
        // Prevent Chrome 67 and earlier from automatically showing the prompt
        e.preventDefault();
        // Stash the event so it can be triggered later.
        deferredPrompt = e;

    } catch { }
});

// Install Prompt
function promptInstall() {

    // !!!! This does not work on IOS, You can choose to do something else if the user 
    // Operating system is IOS, like showing a modal
    if (isIos) {
        // Do something else;
        IosSupport();
        return;
    }


    // Attempt to show the prompt
    try {
        // Success
        deferredPrompt.prompt();
    }
    catch (e) {
        // Error
        console.log(e);
        return;
    }

    // Wait for the user to respond to the prompt
    deferredPrompt.userChoice.then((choiceResult) => {
        if (choiceResult.outcome === 'accepted') {
            // The user has accepted the install Prompt
            console.log('User accepted the A2HS prompt');
            hidePwaButtons();
            window.location.reload();
        } else {
            // The user rejected the install Prompt
            console.log('User dismissed the A2HS prompt');
        }
        deferredPrompt = null;
    });
}

// Ios Specific behaviour
function IosSupport() {
    // 
}