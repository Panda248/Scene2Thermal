using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InferenceManager))]
public class InferenceEditor : Editor
{
    private void OnSceneGUI()
    {
        InferenceManager inferenceManager = (InferenceManager)target;
        if (GUILayout.Button("Run Startup"))
        {
            inferenceManager.RunStartup();
        }
    }
}

