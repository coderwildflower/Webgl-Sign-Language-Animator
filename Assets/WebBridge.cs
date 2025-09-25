using UnityEngine;
using TMPro;
using Devden.STT;
using System;

public class WebBridge : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private VoiceCommandHandlerDemo voiceCommandHandlerDemo;
    public static event Action<string> OnMessageReceived;

    void Start()
    {
       
    }

    public void ReceiveMessage(string msg)
    {
        Debug.Log("Message from parent: " + msg);
        if (debugText != null)
            debugText.text = "Received: " + msg;

        OnMessageReceived?.Invoke(msg);
    }
}