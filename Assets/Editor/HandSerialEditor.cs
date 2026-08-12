using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HandSerial))]
public class HandSerialEditor : Editor
{
    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();
        HandSerial handSerial = (HandSerial)target;
        if (GUILayout.Button("Send Data"))
        {
            handSerial.SendDataBytes();
        }
    }
}
