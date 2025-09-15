using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devden.STT
{
    public class VoiceCommandHandlerDemo : MonoBehaviour
    {
        [System.Serializable]
        public class PhraseAnimationPair
        {
            public string phrase;
            public string animationName;
            public float matchThreshold = 0.8f;
        }
        [SerializeField] private PhraseAnimationPair[] phraseAnimationPairs;

        [SerializeField] private TextMeshProUGUI outputMessageText;
        [SerializeField] private Animator characterAnimator;

        [SerializeField] private Toggle listenContinuously;

        private Dictionary<string, string> wordAnimations = new Dictionary<string, string>();
        private string DefinedWords = "Hello and welcometo AbuDhabi Customs We are here to assist and support our community Our authority is to ban and block bad business Our job is to assess all goods and ban anything illegal We support a secure future for this area";
        private Dictionary<string, float> AnimationClipLength = new Dictionary<string, float>();
        private Coroutine animationCoroutine;
        private void Start()
        {
            TranscriptionHandler.SetGameObjectName(transform.gameObject.name);

            InitializeWordAnimations();

            foreach (var clip in characterAnimator.runtimeAnimatorController.animationClips)
            {
                if (!AnimationClipLength.ContainsKey(clip.name))
                {
                    AnimationClipLength.Add(clip.name.ToLower(), clip.length);
                }
            }
        }
        public void StartListening()
        {
            TranscriptionHandler.RunSpeechRecognition(listenContinuously.isOn);
        }

        public void StopListening()
        {
            TranscriptionHandler.StopRecognition();
          
        }

        //Receives result from speech recognition, use the same function name in case it needs to be used elsewhere.
        public void Result(string text)
        {
            outputMessageText.text = text;
            CheckPhraseMatch(text);
        }
        private void InitializeWordAnimations()
        {

            string cleanedDefinedWords = CleanText(DefinedWords);
            string[] definedWordsArray = cleanedDefinedWords.Split(' ');

            foreach (string word in definedWordsArray)
            {
                if (!string.IsNullOrEmpty(word) && !wordAnimations.ContainsKey(word))
                {
                    wordAnimations.Add(word, $"{word}");
                }
            }
        }

        private string CleanText(string textinput)
        {
            if (string.IsNullOrEmpty(textinput)) return string.Empty;

            return System.Text.RegularExpressions.Regex.Replace(textinput.ToLower().Trim(), @"[^\w\s]", "")
                   .Trim();
        }
        private void CheckPhraseMatch(string recognizedText)
        {
            if (string.IsNullOrEmpty(recognizedText)) return;

            string cleanedRecognizedText = CleanText(recognizedText);

            foreach (var pair in phraseAnimationPairs)
            {
                string cleanedTargetPhrase = CleanText(pair.phrase);

                // Try exact match first
                if (string.Equals(cleanedRecognizedText, cleanedTargetPhrase, StringComparison.OrdinalIgnoreCase))
                {
                    TriggerAnimation(pair.animationName, pair.phrase);
                    return;
                }

                // Try fuzzy matching using Levenshtein distance
                float similarity = CalculateSimilarity(cleanedRecognizedText, cleanedTargetPhrase);
                if (similarity >= pair.matchThreshold)
                {
                    TriggerAnimation(pair.animationName, pair.phrase);
                    return;
                }
            }

            Debug.Log($"No matching phrase found for: {recognizedText}");
            ProcessWordsIndividually(cleanedRecognizedText);

        }

        private void ProcessWordsIndividually(string text)
        {
            string[] words = text.Split(' ');

            animationCoroutine = StartCoroutine(ProcessWordsCoroutine(words));
        }


        private IEnumerator ProcessWordsCoroutine(string[] words)
        {
            foreach (string word in words)
            {
                if (string.IsNullOrEmpty(word)) continue;
              
                if (wordAnimations.ContainsKey(word))
                {
                    string animationName = wordAnimations[word];
                    characterAnimator.CrossFade(animationName,( 0.1f));
                    Debug.Log($"Playing word animation '{animationName}' for: {word}");
                    //yield return new WaitForSeconds(AnimationClipLength[word]);
                    yield return new WaitForSeconds(3f);
                }
                else
                {
                    Debug.Log($"No word animation found for: {word}. Breaking into letters.");
                    yield return StartCoroutine(AnimateWordLettersCoroutine(word));
                }
            }

            characterAnimator.CrossFade("idle", 0.2f);
        }

        private IEnumerator AnimateWordLettersCoroutine(string word)
        {
            foreach (char c in word)
            {
                char upperChar = char.ToUpper(c);
                string animationName = upperChar.ToString();
                Debug.Log(upperChar + " :Capital Letter Anim Word");
                characterAnimator.CrossFade(animationName,0.1f);
                //yield return new WaitForSeconds(AnimationClipLength[animationName]);
                yield return new WaitForSeconds(3f);
            }

            characterAnimator.CrossFade("idle", 0.2f);
        }

        private float CalculateSimilarity(string source, string target)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target))
                return 0f;

            if (source == target) return 1f;

            int distance = LevenshteinDistance(source, target);
            int maxLength = Mathf.Max(source.Length, target.Length);

            return 1f - ((float)distance / maxLength);
        }

        private int LevenshteinDistance(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
                return string.IsNullOrEmpty(target) ? 0 : target.Length;

            if (string.IsNullOrEmpty(target))
                return source.Length;

            int[,] distance = new int[source.Length + 1, target.Length + 1];

            for (int i = 0; i <= source.Length; i++)
                distance[i, 0] = i;

            for (int j = 0; j <= target.Length; j++)
                distance[0, j] = j;

            for (int i = 1; i <= source.Length; i++)
            {
                for (int j = 1; j <= target.Length; j++)
                {
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    distance[i, j] = Mathf.Min(
                        Mathf.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost);
                }
            }

            return distance[source.Length, target.Length];
        }
        private void TriggerAnimation(string animationName, string matchedPhrase)
        {
            if (characterAnimator != null && !string.IsNullOrEmpty(animationName))
            {
                characterAnimator.SetTrigger(animationName);
                Debug.Log($"Playing animation '{animationName}' for phrase: {matchedPhrase}");

            }
        }
    }
}