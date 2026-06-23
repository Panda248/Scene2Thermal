using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ThermResolver))]
public class ThermalResolverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Update All"))
        {
            ((ThermResolver)target).ResolveEdges();
            ((ThermResolver)target).UpdateObjects();
        }
        if (GUILayout.Button("Resolve Edges"))
        {
            ((ThermResolver)target).ResolveEdges();
        }
        if (GUILayout.Button("Update Temps"))
        {
            ((ThermResolver)target).UpdateObjects();
        }
        if (GUILayout.Button("Reset Graph"))
        {
            ((ThermResolver)target).ResetGraph();
        }
    }
}
