using UnityEngine;
using TMPro;
using Devden.STT;

public class WebGLMessageReceiver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private VoiceCommandHandlerDemo voiceCommandHandlerDemo;

    void Start()
    {
       
    }

    public void ReceiveMessage(string msg)
    {
        Debug.Log("Message from parent: " + msg);
        if (debugText != null)
            debugText.text = "Received: " + msg;
        //voiceCommandHandlerDemo.StartListening();
    }
}