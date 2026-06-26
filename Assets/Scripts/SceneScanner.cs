using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Transformation;

/// <summary>
/// Script for capturing Scene images
/// </summary>
public class SceneScanner : MonoBehaviour
{
    public RenderTexture scanOutput;
    public Texture2D outputTexture;
    public Transform environmentParent;
    public List<byte[]> scans;
    Rotator rotator;
    Camera scan;
    List<GameObject> culled;
    static SceneScanner instance;
    public static SceneScanner Instance()
    {
        if (instance == null)
        {
            instance = FindAnyObjectByType<SceneScanner>();
        }
        return instance;
    }
    
    private void Awake()
    {
        rotator = GetComponent<Rotator>();
        culled = new List<GameObject>();
        scan = GetComponentInChildren<Camera>();
        outputTexture = new Texture2D(scanOutput.width, scanOutput.height);
        scans = new List<byte[]>();
    }

    public void Scan(bool saveToDisk)
    {
        transform.position = GetSceneCenter();
        for (int i = 0; i < 4; i++)
        {
            Debug.Log($"scene scan {i} with rotation {transform.rotation.eulerAngles.y} and index {rotator.rotateIndex}");
            Cull();
            //culled = CullUtility.Cull(scan.transform.position, GetSceneCenter());
            //Debug.Log($"Center is {GetSceneCenter()}");
            //Debug.Log($"{culled.Count} objects culled");

            RenderTexture.active = scanOutput;
            scan.Render();
            outputTexture.ReadPixels(new Rect(0, 0, scanOutput.width, scanOutput.height), 0, 0);
            outputTexture.Apply();
            
            scans.Add(outputTexture.EncodeToJPG());
            
            if(saveToDisk) File.WriteAllBytes($"Assets/Scans/scene_scan{rotator.rotateIndex}.png",outputTexture.EncodeToPNG());
            
            UnCull();
            
            rotator.Rotate();
        }
        transform.position = Vector3.zero;
    }

    Vector3 GetSceneCenter()
    {
        int childCount = environmentParent.childCount;
        Vector3 center = Vector3.zero;
        for (int i = 0; i < childCount; i++)
        {
            center += environmentParent.GetChild(i).position;
        }
        return center / childCount;
    }

    void Cull()
    {
        List<RaycastHit> hits;
        Vector3 center = GetSceneCenter();
        Vector3 origin = scan.transform.position;
        Vector3 centerAdjusted = Vector3.Lerp(center, origin, 0.1f);
        Ray ray = new Ray(origin, centerAdjusted - origin);
        hits = new List<RaycastHit>(Physics.RaycastAll(ray, (centerAdjusted - origin).magnitude));

        foreach(RaycastHit hit in hits)
        {
            // Possible issue, adding children of already added gameobjects (shouldnt be an issue just redundant)
            hit.transform.gameObject.SetActive(false);
            culled.Add(hit.transform.gameObject);
        }
    }

    void UnCull()
    {
        foreach (GameObject child in culled)
        {
            child.SetActive(true);
        }
        culled.Clear();
    }
}
