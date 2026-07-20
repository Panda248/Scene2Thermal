using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VisualizerToggle))]
public class VisualizerToggleEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Toggle"))
        {
            ((VisualizerToggle)target).OnToggleThermals();
        }
    }
}

