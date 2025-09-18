let recognization = new webkitSpeechRecognition();

var result = false;
var stop = true; // Initially stopped
var started = false; // Recognition has not started yet
var GameObjName = null;

function SetGameObjectName(name) {
    GameObjName = name;
    console.log(GameObjName);
}

function runspeechrecognition(listenContinuous) {
    recognization.onstart = () => {
        console.log("Speech recognition started");
        started = true;
        result = false;
        stop = false;
    };

    recognization.onresult = (e) => {
        var transcript = e.results[0][0].transcript;
        console.log("Speech recognition result:", transcript);

        window.Gameinstance.SendMessage(GameObjName, "Result", transcript);
        result = true;

        if (listenContinuous && !stop) {
            setTimeout(() => {
                if (!started) {
                    recognization.start(); // Restart recognition
                }
            }, 100);
        }
    };

    recognization.onend = () => {
        console.log("Speech recognition ended");
        started = false;

        if (!result && listenContinuous && !stop) {
            setTimeout(() => {
                if (!started) {
                    recognization.start(); // Restart recognition
                }
            }, 100);
        }
    };

    // Start recognition if it is not running and has not been stopped
    if (!started && stop) {
        stop = false;
        recognization.start();
    }
}

function stoprecognition() {
    console.log("Stopping speech recognition");
    recognization.stop();
    stop = true;
    started = false;
}

function downloadfile(fileName, content) {
    const link = document.createElement("a");
    const file = new Blob([content], { type: 'text/plain' });
    link.href = URL.createObjectURL(file);
    link.download = fileName + ".txt";
    link.click();
}
