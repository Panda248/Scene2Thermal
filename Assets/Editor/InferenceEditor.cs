using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InferenceManager))]
public class InferenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InferenceManager inferenceManager = (InferenceManager)target;
        if (GUILayout.Button("Run Startup"))
        {
            inferenceManager.RunStartup();
        }
    }
}

