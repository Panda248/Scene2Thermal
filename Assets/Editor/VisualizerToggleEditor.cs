using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VisualizerToggle))]
public class VisualizerToggleEditor: Editor
{
    private void OnSceneGUI()
    {
        VisualizerToggle inputManager = (VisualizerToggle)target;
        if (GUILayout.Button("Run Startup"))
        {
            inputManager.OnToggleThermals();
        }
    }
}

