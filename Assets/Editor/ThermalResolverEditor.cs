using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ThermResolver))]
public class ThermalResolverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Update"))
        {
            ((ThermResolver)target).ResolveEdges();
            ((ThermResolver)target).UpdateObjects();
        }
        if(GUILayout.Button("Reset Graph"))
        {
            ((ThermResolver)target).ResetGraph();
        }
    }
}
