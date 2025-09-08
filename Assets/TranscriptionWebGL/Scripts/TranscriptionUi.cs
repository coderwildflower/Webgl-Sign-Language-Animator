using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devden.STT
{
    public class TranscriptionUi : MonoBehaviour
    {
        [SerializeField] Text resulttxt;
        [SerializeField] Toggle listenContinuously;
        [SerializeField] Button downloadbutton;
        [SerializeField] Text transcribeTxt;

        Coroutine loadingDotCR;

        private void Start()
        {
            TranscriptionHandler.SetGameObjectName(transform.gameObject.name);
        }
        public void StartListening()
        {
            TranscriptionHandler.RunSpeechRecognition(listenContinuously.isOn);

            if (loadingDotCR != null)
            {
                StopCoroutine(loadingDotCR);
                loadingDotCR = null;
            }

            loadingDotCR = StartCoroutine(LoadingDotAnimation());
        }

        public void StopListening()
        {
            TranscriptionHandler.StopRecognition();

            if (loadingDotCR != null)
            {
                StopCoroutine(loadingDotCR);
                loadingDotCR = null;
            }

            transcribeTxt.text = "Start Transcription";
        }

        public void ClearResult()
        {
            resulttxt.text = "";
            downloadbutton.interactable = false;
        }

        public void Result(string text)
        {
            resulttxt.text += "[" + DateTime.Now.ToString("H:m:ss") + "] : " + text + "\n";
            downloadbutton.interactable = !string.IsNullOrEmpty(resulttxt.text);
        }

        public void DownloadTranscript()
        {
            TranscriptionHandler.DownloadFile("Transcript", resulttxt.text);
        }

        IEnumerator LoadingDotAnimation()
        {
            while (true)
            {
                transcribeTxt.text = "Transcribing";
                yield return new WaitForSeconds(0.45f);
                transcribeTxt.text = "Transcribing.";
                yield return new WaitForSeconds(0.45f);
                transcribeTxt.text = "Transcribing..";
                yield return new WaitForSeconds(0.45f);
                transcribeTxt.text = "Transcribing...";
                yield return new WaitForSeconds(0.45f);
            }
        }

    }
}