using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VisemeData
{
    public int visemeId;
    public int time; // in ms
}

[System.Serializable]
public class VisemeDataWrapper
{
    public List<VisemeData> data;
}

public class VisemeReceiver : MonoBehaviour
{
    // Mapping visemeId -> blendshape index
    public SkinnedMeshRenderer faceMesh;
    public int[] visemeToBlendshape; // fill in inspector

    private void OnEnable()
    {
        Application.logMessageReceived += OnLogMessage; // optional for debug
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= OnLogMessage;
    }

    void OnLogMessage(string condition, string stackTrace, LogType type)
    {
        // just for debug
    }

    // This method will be called by JS via SendMessage
    public void ReceiveViseme(string json)
    {
        Debug.Log("Received viseme data: " + json);

        VisemeData[] visemes = JsonHelper.FromJson<VisemeData>(json);
        //StartCoroutine(PlayVisemes(visemes));
    }

    private System.Collections.IEnumerator PlayVisemes(VisemeData[] visemes)
    {
        float startTime = Time.time;

        foreach (var v in visemes)
        {
            float waitTime = v.time / 1000f - (Time.time - startTime);
            if (waitTime > 0) yield return new WaitForSeconds(waitTime);

            // Reset all blendshapes
            for (int i = 0; i < visemeToBlendshape.Length; i++)
                faceMesh.SetBlendShapeWeight(i, 0);

            // Set current viseme
            int index = visemeToBlendshape[v.visemeId];
            faceMesh.SetBlendShapeWeight(index, 100);
        }

        // Reset all at end
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < visemeToBlendshape.Length; i++)
            faceMesh.SetBlendShapeWeight(i, 0);
    }
}

// Helper to parse JSON array from JS
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        json = "{\"data\":" + json + "}";
        return JsonUtility.FromJson<Wrapper<T>>(json).data;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] data;
    }
}
