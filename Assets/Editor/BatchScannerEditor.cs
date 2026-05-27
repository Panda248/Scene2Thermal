using UnityEditor;
using UnityEngine.Audio;
using UnityEngine;

[CustomEditor(typeof(BatchScan))]
public class BatchScannerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Scan"))
        {
            ((BatchScan)target).Scan();
        }
    }
}
