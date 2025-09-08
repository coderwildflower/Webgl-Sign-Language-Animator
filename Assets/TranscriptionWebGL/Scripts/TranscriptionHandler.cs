using System.Runtime.InteropServices;

namespace Devden.STT
{
    public static class TranscriptionHandler
    {
        /// <summary>
        /// Sets the name of the gameobject attached to script, call this function on start or awake
        /// </summary>
        /// <param name="name"> name of the gameobject</param>
        [DllImport("__Internal")]
        public static extern void SetGameObjectName(string name);

        [DllImport("__Internal")]
        public static extern void RunSpeechRecognition(bool listenContinuously);

        [DllImport("__Internal")]
        public static extern void StopRecognition();

        [DllImport("__Internal")]
        public static extern void DownloadFile(string filename, string content);
    }

}