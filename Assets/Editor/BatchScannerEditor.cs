using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BatchScan))]
public class BatchScannerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Scan"))
        {
            ((BatchScan)target).ScanAll(((BatchScan)target).thermObjects, ((BatchScan)target).saveToDisk);
        }
        if(GUILayout.Button("Scan Root"))
        {
            ((BatchScan)target).ScanRoot(((BatchScan)target).saveToDisk);
        }
    }
}
