using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PropertySetter))]
public class PropertySetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("BatchSet"))
        {
            _ = ((PropertySetter)target).BatchSet(((PropertySetter)target).targetObjects);
        }
        if (GUILayout.Button("Set"))
        {
            ((PropertySetter)target).SetMaterial(((PropertySetter)target).targetObject);
        }
        if (GUILayout.Button("Set No API"))
        {
            ((PropertySetter)target).SetMaterial(((PropertySetter)target).targetObject, ((PropertySetter)target).sampleJson);
        }
    }
}
