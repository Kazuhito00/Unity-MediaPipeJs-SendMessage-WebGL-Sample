// Unity StartUp Process Start///////////////////////////////////////////////// 
let _unityInstance = null;

var container = document.querySelector("#unity-container");
var canvas = document.querySelector("#unity-canvas");
var loadingBar = document.querySelector("#unity-loading-bar");
var progressBarFull = document.querySelector("#unity-progress-bar-full");
var fullscreenButton = document.querySelector("#unity-fullscreen-button");
var warningBanner = document.querySelector("#unity-warning");

// Shows a temporary message banner/ribbon for a few seconds, or
// a permanent error message on top of the canvas if type=='error'.
// If type=='warning', a yellow highlight color is used.
// Modify or remove this function to customize the visually presented
// way that non-critical warnings and error messages are presented to the
// user.
function unityShowBanner(msg, type) {
    function updateBannerVisibility() {
        warningBanner.style.display = warningBanner.children.length ? 'block' : 'none';
    }
    var div = document.createElement('div');
    div.innerHTML = msg;
    warningBanner.appendChild(div);
    if (type == 'error') div.style = 'background: red; padding: 10px;';
    else {
        if (type == 'warning') div.style = 'background: yellow; padding: 10px;';
        setTimeout(function() {
            warningBanner.removeChild(div);
            updateBannerVisibility();
        }, 5000);
    }
    updateBannerVisibility();
}

var buildUrl = "Build";
var loaderUrl = buildUrl + "/unity-temp.loader.js";
var config = {
    dataUrl: buildUrl + "/unity-temp.data",
    frameworkUrl: buildUrl + "/unity-temp.framework.js",
    codeUrl: buildUrl + "/unity-temp.wasm",
    streamingAssetsUrl: "StreamingAssets",
    companyName: "DefaultCompany",
    productName: "Unity-MediaPipeJs-SendMessage-WebGL-Sample",
    productVersion: "0.1",
    showBanner: unityShowBanner,
};

// By default Unity keeps WebGL canvas render target size matched with
// the DOM size of the canvas element (scaled by window.devicePixelRatio)
// Set this to false if you want to decouple this synchronization from
// happening inside the engine, and you would instead like to size up
// the canvas DOM size and WebGL render target sizes yourself.
// config.matchWebGLToCanvasSize = false;

if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
    // Mobile device style: fill the whole browser client area with the game canvas:

    var meta = document.createElement('meta');
    meta.name = 'viewport';
    meta.content = 'width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes';
    document.getElementsByTagName('head')[0].appendChild(meta);
    container.className = "unity-mobile";

    // To lower canvas resolution on mobile devices to gain some
    // performance, uncomment the following line:
    // config.devicePixelRatio = 1;

    canvas.style.width = window.innerWidth + 'px';
    canvas.style.height = window.innerHeight + 'px';

    unityShowBanner('WebGL builds are not supported on mobile devices.');
} else {
    // Desktop style: Render the game canvas in a window that can be maximized to fullscreen:

    canvas.style.width = "800px";
    canvas.style.height = "450px";
}

loadingBar.style.display = "block";

var script = document.createElement("script");
script.src = loaderUrl;
script.onload = () => {
    createUnityInstance(canvas, config, (progress) => {
        progressBarFull.style.width = 100 * progress + "%";
    }).then((unityInstance) => {
        loadingBar.style.display = "none";
        fullscreenButton.onclick = () => {
            unityInstance.SetFullscreen(1);
        };

        // MediaPipe Process Start
        _unityInstance = unityInstance;
        setTimeout(
            function () {
                mediapipeHandsStars();
            }, 
            "3000"
        );
    }).catch((message) => {
        alert(message);
    });
};
document.body.appendChild(script);
// Unity StartUp Process End///////////////////////////////////////////////////

// MediaPipe Hands Process Start///////////////////////////////////////////////
// Capture Size
let capWidth = 320;
let capHeight = 180;

// Display Loading Background 
const videoElement = document.getElementsByClassName('input_video')[0];
const canvasElement = document.getElementsByClassName('output_canvas')[0];
const canvasCtx = canvasElement.getContext('2d');
canvasCtx.fillStyle = "rgba(" + [0, 0, 0, 1.0] + ")";
canvasCtx.fillRect(1, 1, capWidth, capHeight); 

// MediaPipe Hands Instance
let hands = null;
hands = new Hands({locateFile: (file) => {
    return `https://cdn.jsdelivr.net/npm/@mediapipe/hands/${file}`;
}});

function mediapipeHandsStars() {
    const videoElement = document.getElementsByClassName('input_video')[0];
    const canvasElement = document.getElementsByClassName('output_canvas')[0];
    const canvasCtx = canvasElement.getContext('2d');
    
    // MediaPipe Callback
    function onResults(results) {
        canvasCtx.save();
    
        // JS->Unity:ClearCanvas
        if (_unityInstance != null) {
            _unityInstance.SendMessage('Plane', 'ClearCanvas');
        }
    
        canvasCtx.clearRect(0, 0, canvasElement.width, canvasElement.height);
        canvasCtx.drawImage(results.image, 0, 0, canvasElement.width, canvasElement.height);

        if (results.multiHandLandmarks) {
            for (const landmarks of results.multiHandLandmarks) {
                // JS->Unity:DrawHandsLandmark
                if (_unityInstance != null) {
                    _unityInstance.SendMessage('Plane', 'DrawHandsLandmark', String(JSON.stringify(landmarks)));
                }
                drawConnectors(canvasCtx, landmarks, HAND_CONNECTIONS, {color: '#FFFFFF', lineWidth: 5});
                drawConnectors(canvasCtx, landmarks, HAND_CONNECTIONS, {color: '#000000', lineWidth: 2});
            }
        }
    
        canvasCtx.restore();
    }
    
    // Start-up MediaPipe Hands 
    hands.setOptions({
        maxNumHands: 1,
        minDetectionConfidence: 0.5,
        minTrackingConfidence: 0.5
    });
    hands.onResults(onResults);
    
    const camera = new Camera(videoElement, {
        onFrame: async () => {
            // Remove Loading Icon
            var loadingElement = document.getElementById("loading");
            if (loadingElement != null) {
                loadingElement.remove();
            }

            // JS->Unity:ClearCanvas
            if (_unityInstance != null) {
                _unityInstance.SendMessage('Plane', 'ClearCanvas');
            }
    
            // Video->Canvas
            const canvasElement = document.getElementsByClassName('output_canvas')[0];
            const canvasCtx = canvasElement.getContext('2d');
            canvasCtx.drawImage(videoElement, 0, 0, capWidth, capHeight);
    
            await hands.send({image: videoElement});
        },
        width: capWidth*2,
        height: capHeight*2
    });
    camera.start();
}
// MediaPipe Hands Process End/////////////////////////////////////////////////

// MediaPipe Setting Callback Start////////////////////////////////////////////
const maxNumHandsOptionElement = document.getElementById('maxNumHandsOption');
const maxNumHandsValueElement = document.getElementById('maxNumHandsValue');
const setMaxNumHandsValue = (val) => {
    maxNumHandsValueElement.innerText = val;

    if (hands != null) {
        hands.setOptions({
            maxNumHands: parseFloat(val)
        });
    }
}
const maxNumHandsValueChange = (e) =>{
    setMaxNumHandsValue(e.target.value);
}

const minDetectionConfidenceOptionElement = document.getElementById('minDetectionConfidenceOption');
const minDetectionConfidenceValueElement = document.getElementById('minDetectionConfidenceValue');
const setMinDetectionConfidenceValue = (val) => {
    minDetectionConfidenceValueElement.innerText = val;
    
    if (hands != null) {
        hands.setOptions({
            minDetectionConfidence: parseFloat(val)
        });
    }
}
const maxMinDetectionConfidenceChange = (e) =>{
    setMinDetectionConfidenceValue(e.target.value);
}

const minTrackingConfidenceOptionElement = document.getElementById('minTrackingConfidenceOption');
const minTrackingConfidenceValueElement = document.getElementById('minTrackingConfidenceValue');
const setMinTrackingConfidenceValue = (val) => {
    minTrackingConfidenceValueElement.innerText = val;
    
    if (hands != null) {
        hands.setOptions({
            minTrackingConfidence: parseFloat(val)
        });
    }
}
const maxMinTrackingConfidenceChange = (e) =>{
    setMinTrackingConfidenceValue(e.target.value);
}

window.onload = () => {
    maxNumHandsOptionElement.addEventListener('input', maxNumHandsValueChange);
    setMaxNumHandsValue(maxNumHandsOptionElement.value);
    
    minDetectionConfidenceOptionElement.addEventListener('input', maxMinDetectionConfidenceChange);
    setMinDetectionConfidenceValue(minDetectionConfidenceOptionElement.value);
    
    minTrackingConfidenceOptionElement.addEventListener('input', maxMinTrackingConfidenceChange);
    setMinTrackingConfidenceValue(minTrackingConfidenceOptionElement.value);
}
// MediaPipe Setting Callback End//////////////////////////////////////////////
