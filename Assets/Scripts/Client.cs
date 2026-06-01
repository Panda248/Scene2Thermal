using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
/// <summary>
/// Handles HTTP requests to server for Semantic Inference
/// </summary>
public class Client : MonoBehaviour
{
    static HttpClient httpClient = new()
    {
        BaseAddress = new Uri("https://jsonplaceholder.typicode.com")// replace w/ final url
    }; 

    private void Awake()
    {

    }

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
        using StringContent name = new(JsonUtility.ToJson(new
        {
            name = scanner.environmentParent.name,
        }),
        System.Text.Encoding.UTF8,
        "application/json");

        // Put everything together
        using MultipartFormDataContent content = new();
        content.Add(name);
        content.Add(image1, "image1");
        content.Add(image2, "image2");
        content.Add(image3, "image3");
        content.Add(image4, "image4");
        using HttpResponseMessage response = await httpClient.PostAsync(
            "object-inference",
            content
         );

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        return responseContent;
    }

    public async Task<String> RequestObjectInference(GameObject obj)
    {
        ObjectScanner scanner = ObjectScanner.Instance();
     
        if (scanner == null) Debug.LogError("Object Scanner is Missing");

        // Grab images associated with obj
        List < byte[]> isoScans = scanner.isoScans[obj.name];
        List < byte[]> contextScans = scanner.contextScans[obj.name];
        
        // Stuff images into HttpContent objects
        List<ByteArrayContent> isoImages = new();
        List<ByteArrayContent> contextImages = new();
        for (int i = 0; i < isoScans.Count; i++)
        {
            isoImages.Add(new(isoScans[i]));
            contextImages.Add(new(contextScans[i]));
        }

        // Store Object data into json
        using StringContent objContent = new (JsonUtility.ToJson(new
        {
            name = obj.name,
            scale = obj.transform.localScale.ToString(),
            size = obj.GetComponent<MeshFilter>()?.mesh.bounds.size.ToString()
        }),
        System.Text.Encoding.UTF8,
        "application/json");
        
        // Put everything together into content
        using MultipartFormDataContent content = new();
        content.Add(objContent);
        for (int i = 0; i < contextImages.Count; i++)
        {
            content.Add(contextImages[i], $"context{i}");
            content.Add(isoImages[i], $"iso{i}");
        }

        using HttpResponseMessage response = await httpClient.PostAsync(
            "object-inference",
            content
         );

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        return responseContent;

    }


}
