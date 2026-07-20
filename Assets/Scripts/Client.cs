using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
/// <summary>
/// Handles HTTP requests to server for Semantic Inference
/// </summary>
public class Client : MonoBehaviour
{
    public GameObject targetObject;
    public String objectInferenceJson, sceneInferenceJson, SceneCategory;
    static HttpClient httpClient = new()
    {
        BaseAddress = new Uri("http://127.0.0.1:5000"), // replace w/ final url
        Timeout = TimeSpan.FromSeconds(180)
    }; 
    static Client instance;
    public static Client Instance()
    {
        if (instance == null)
        {
            instance = FindAnyObjectByType<Client>();
        }
        return instance;
    }

    //private MultipartFormDataContent ConstructByteArrayContents(List<byte[]> files, string prefix)
    //{
    //    List<ByteArrayContent> list = new();

    //    foreach (byte[] file in files)
    //    {

    //    }
    //}

    public async Task<String> RequestSceneInference() 
    {
        SceneScanner scanner = SceneScanner.Instance();

        if(scanner == null) Debug.LogError("Scene Scanner is Missing");

        // Stuff scene images into HttpContent objects
        using ByteArrayContent image1 = new(scanner.scans[0]);
        using ByteArrayContent image2 = new(scanner.scans[1]);
        using ByteArrayContent image3 = new(scanner.scans[2]);
        using ByteArrayContent image4 = new(scanner.scans[3]);


        // Store Scene data in json
        string nameJson = JsonConvert.SerializeObject(new
        {
            name = transform.root.name
        });
        using StringContent name = new(nameJson, System.Text.Encoding.UTF8, "application/json");
        //Debug.Log(nameJson);


        // Put everything together
        using MultipartFormDataContent content = new();
        
        content.Add(name, "jsonText");
        
        content.Add(image1, "scene1", "scene1.jpg");
        content.Add(image2, "scene2", "scene2.jpg");
        content.Add(image3, "scene3", "scene3.jpg");
        content.Add(image4, "scene4", "scene4.jpg");
        
        // Send and await response
        using HttpResponseMessage response = await httpClient.PostAsync("scene-inference", content);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        //Debug.Log($"{responseContent}");
        //sceneInferenceJson = responseContent.ToString(); // this should have scene category
        //SceneCategory = responseContent.ToString();

        return responseContent.ToString();
    }

    public async Task<String> RequestObjectInference(GameObject obj)
    {
        ObjectScanner scanner = ObjectScanner.Instance();
     
        if (scanner == null) Debug.LogError("Object Scanner is Missing");

        // Grab images associated with obj
        List < byte[]> isoScans = scanner.isoScans[obj.GetEntityId()];
        List < byte[]> contextScans = scanner.contextScans[obj.GetEntityId()];

        //Debug.Log($"iso has{isoScans.Count}");
        //Debug.Log($"context has{contextScans.Count}");

        // Store Object data into json
        string objContentJson = JsonConvert.SerializeObject(new
        {
            name = obj.name, 
            scale = obj.transform.localScale.ToString(),
            size = obj.GetComponent<MeshFilter>()?.mesh.bounds.size.ToString(),
            scene_category = SceneCategory
        });
        using StringContent objContent = new (objContentJson, System.Text.Encoding.UTF8, "application/json");

        //Debug.Log(objContentJson);
        
        // Build final packet
        using MultipartFormDataContent content = new();

        content.Add(objContent, "jsonText");

        // Add Images
        for (int i = 0; i < isoScans.Count; i++)
        {
            //Debug.Log($"iso bytes: {isoScans[i].Length}");
            //Debug.Log($"context bytes: {contextScans[i].Length}");
            content.Add(new ByteArrayContent(isoScans[i]), $"iso{i+1}", $"iso{i+1}.jpg");
            content.Add(new ByteArrayContent(contextScans[i]), $"context{i+1}", $"context{i+1}.jpg");
        }

        // Send and Await Response
        using HttpResponseMessage response = await httpClient.PostAsync(
            "object-material-inference",
            content
         );

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        //Debug.Log($"{responseContent}");
        objectInferenceJson = responseContent.ToString();

        return responseContent.ToString();
    }
}
