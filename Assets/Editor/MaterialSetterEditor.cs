using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(MaterialSetter))]
public class MaterialSetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("BatchSet"))
        {
            ((MaterialSetter)target).BatchSet();
        }
        if (GUILayout.Button("Set"))
        {
            ((MaterialSetter)target).SetMaterial(((MaterialSetter)target).targetObject);
        }
        if (GUILayout.Button("Set No API"))
        {
            ((MaterialSetter)target).SetMaterial(((MaterialSetter)target).targetObject, ((MaterialSetter)target).sampleJson);
        }
        if (GUILayout.Button("Add ThermObjects"))
        {
            ((MaterialSetter)target).AddThermObjects();
        }
    }
}
