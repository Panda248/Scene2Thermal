using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Client))]
public class ClientEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Client targ = (Client)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Send Scene Request"))
        {
            _ = targ.RequestSceneInference();
        }
        if(GUILayout.Button("Send Object Request"))
        {
            
            _ = targ.RequestObjectInference(targ.targetObject);
        }
    }
}
