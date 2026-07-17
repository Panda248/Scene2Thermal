using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class PropertySetter : MonoBehaviour
{
    // For testing
    public string sampleJson = "    ";
    public ThermObject targetObject;
    public List<ThermObject> targetObjects;

    string sceneCategory;

    public async Task BatchSet(List<ThermObject> thermObjects)
    {
        sceneCategory = await Client.Instance().RequestSceneInference();
        Client.Instance().SceneCategory = sceneCategory;
        List<Tuple<Task<string>, ThermObject>> objectInferenceTasks = new();


        foreach (ThermObject thermObj in thermObjects)
        {

            objectInferenceTasks.Add(new(Client.Instance().RequestObjectInference(thermObj.gameObject),
                thermObj));
        }

        foreach (Tuple<Task<string>, ThermObject> task in objectInferenceTasks)
        {
            string objectInferenceJson = await task.Item1;
            SetMaterial(task.Item2, objectInferenceJson);
        }

        Debug.Log("BatchSet complete");
    }

    public async void SetMaterial(ThermObject thermObject)
    {
        string objectInferenceJson = await Client.Instance().RequestObjectInference(thermObject.gameObject);
        JsonClasses.ThermObjectProperties materialInference = JsonConvert.DeserializeObject<JsonClasses.ThermObjectProperties>(objectInferenceJson);
        thermObject.SetProperties(materialInference);
    }

    public void SetMaterial(ThermObject target,string thermJson)
    {
        Debug.Log($"{target.gameObject.name} received JSON: {thermJson}");
        JsonClasses.ThermObjectProperties materialInference = JsonConvert.DeserializeObject<JsonClasses.ThermObjectProperties>(thermJson);
        target.SetProperties(materialInference);
    }
}