using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TranscriptionSetup : MonoBehaviour
{
    [MenuItem("Tools/Transcription For WebGL/Configure project for Transcription")]
    public static void SetupTTSDemo()
    {
        string sourceTemplateFolder = "Assets/TranscriptionWebGL/WebGLTemplates/WebSpeech";
        string destinationTemplateRoot = "Assets/WebGLTemplates";
        string destinationTemplateFolder = "Assets/WebGLTemplates/WebSpeech";

        // Prompt user for confirmation
        bool proceed = EditorUtility.DisplayDialog(
            "Configure Transcription for WebGL",
            "This will move the 'WebSpeech' template from its current location to 'Assets/WebGLTemplates', " +
            "allowing Unity to recognize it as a valid WebGL template.\n\nDo you want to proceed?",
            "Yes, move it",
            "Cancel"
        );

        if (!proceed)
        {
            Debug.Log("Transcription configuration cancelled by user.");
            return;
        }

        // Ensure destination WebGLTemplates folder exists
        if (!AssetDatabase.IsValidFolder(destinationTemplateRoot))
        {
            AssetDatabase.CreateFolder("Assets", "WebGLTemplates");
            Debug.Log("Created folder: Assets/WebGLTemplates");
        }

        // Move the WebSpeech folder if it exists and isn't already moved
        if (AssetDatabase.IsValidFolder(sourceTemplateFolder))
        {
            if (!AssetDatabase.IsValidFolder(destinationTemplateFolder))
            {
                string moveError = AssetDatabase.MoveAsset(sourceTemplateFolder, destinationTemplateFolder);
                if (string.IsNullOrEmpty(moveError))
                {
                    Debug.Log("Moved WebSpeech folder to Assets/WebGLTemplates.");
                }
                else
                {
                    Debug.LogError("Error moving WebSpeech folder: " + moveError);
                    return;
                }
            }
            else
            {
                Debug.Log("WebSpeech folder already exists in Assets/WebGLTemplates.");
            }
        }
        else
        {
            Debug.LogWarning("WebSpeech folder not found at: " + sourceTemplateFolder);
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Transcription For WebGL/Set WebSpeech as WebGL Template")]
    public static void SetWebGLTemplate()
    {
        string destinationTemplateFolder = "Assets/WebGLTemplates/WebSpeech";

        if (AssetDatabase.IsValidFolder(destinationTemplateFolder))
        {
            PlayerSettings.WebGL.template = "WebSpeech";
            Debug.Log("WebGL template set to: WebSpeech");
        }
        else
        {
            Debug.LogWarning("WebSpeech folder not found at Assets/WebGLTemplates. Use the top menu: 'Tools > Transcription For WebGL > Configure project for Transcription'");
        }
    }

    [MenuItem("Tools/Transcription For WebGL/Open Transcription Demo Scene")]
    public static void OpenTTSDemoScene()
    {
        string scenePath = "Assets/TranscriptionWebGL/Scenes/TranscriptionDemo.unity";

        if (!File.Exists(scenePath))
        {
            Debug.LogError("TranscriptionDemo scene not found at path: " + scenePath);
            return;
        }

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        Debug.Log("Opened scene: " + scenePath);
    }
}
