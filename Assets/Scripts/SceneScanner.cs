using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    public void Scan()
    {
        transform.position = GetSceneCenter();
        for (int i = 0; i < 4; i++)
        {
            Cull();
            Debug.Log($"Center is {GetSceneCenter()}");
            Debug.Log($"{culled.Count} objects culled");
            RenderTexture.active = scanOutput;
            scan.Render();
            outputTexture.ReadPixels(new Rect(0, 0, scanOutput.width, scanOutput.height), 0, 0);
            outputTexture.Apply();
            scans.Add(outputTexture.EncodeToJPG());
            File.WriteAllBytes($"Assets/Scans/scene_scan{rotator.rotateIndex}.png",outputTexture.EncodeToPNG());
            rotator.Rotate();
            UnCull();
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
        Ray ray = new Ray(origin, center - origin);
        hits = new List<RaycastHit>(Physics.RaycastAll(ray, (center - origin).magnitude));

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
