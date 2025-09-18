using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Devden.STT;


public class WebBridge : MonoBehaviour
{
    public TextMeshProUGUI debugText; // Assign a UI Text object in Inspector
    public VoiceCommandHandlerDemo voiceCommandHandlerDemo;
    // Called from JS
    public void ReceiveMessage(string msg)
    {
        Debug.Log("Message from parent: " + msg);
        if (debugText != null)
            debugText.text = "Received: " + msg;

        voiceCommandHandlerDemo.StartListening();
    }
}