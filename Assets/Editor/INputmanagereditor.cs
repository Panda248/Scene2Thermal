using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor
{
    private void OnSceneGUI()
    {
        InputManager inputManager = (InputManager)target;
        if (GUILayout.Button("Run Startup"))
        {
            inputManager.OnToggleThermals();
        }
    }
}

