using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    public string sampleJson = "    ";
    public Transform environmentParent;
    string sceneCategory;
    public ThermObject targetObject;

    void Start()
    {
        //BatchSet();
        AddThermObjects();
    }

    public async void BatchSet()
    {
        sceneCategory = await Client.Instance().RequestSceneInference();
        Client.Instance().SceneCategory = sceneCategory;
        List<Tuple<Task<string>, ThermObject>> objectInferenceTasks = new();


        foreach (Transform child in environmentParent)
        {
            // find better way
            objectInferenceTasks.Add(new(Client.Instance().RequestObjectInference(child.gameObject),
                child.gameObject.GetComponent<ThermObject>()));
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
        JsonClasses.ThermObjectProperties materialInference = JsonConvert.DeserializeObject<JsonClasses.ThermObjectProperties>(thermJson);
        target.SetProperties(materialInference);
    }


    public void AddThermObjects()
    {
        foreach(Transform child in environmentParent)
        {
            if (child.gameObject.GetComponent<ThermObject>() == null)
            {
                child.gameObject.AddComponent<ThermObject>();
            }
        }
    }
}