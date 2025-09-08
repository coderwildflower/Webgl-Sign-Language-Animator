mergeInto(LibraryManager.library, {

RunSpeechRecognition:function(listenContinuous) {
    runspeechrecognition(listenContinuous);
},
StopRecognition:function() {
    stoprecognition();
},
DownloadFile:function(fileName, content) {
    downloadfile(UTF8ToString(fileName),UTF8ToString(content));
},
SetGameObjectName:function(name) {
    SetGameObjectName(UTF8ToString(name));
}
});