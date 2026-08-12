using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Script for caputring isolated and context images for a set 
/// </summary>
public class ObjectScanner : MonoBehaviour
{
    private Camera isoCamera, contextCamera;
    private Light isoLight, contextLight;
    private GameObject scanObject;
    public RenderTexture isoOutput,contextOutput;
    public Texture2D isoTexture, contextTexture;
    public Dictionary<EntityId, List<byte[]>> contextScans;
    public Dictionary<EntityId, List<byte[]>> isoScans;
    List<GameObject> culled;
    public GameObject ScanObject {  get { return scanObject; }
        set
        {
            scanObject = value;

            if(value != null)
            {
                transform.position = scanObject.transform.position + scanObject.GetComponent<Rigidbody>().centerOfMass;
            }
        }
    }
    private Rotator hRotator, vRotator;

    static ObjectScanner instance;
    public static ObjectScanner Instance()
    {
        if (instance == null)
        {
            instance = FindAnyObjectByType<ObjectScanner>();
        }
        return instance;
    }

    public void Awake()
    {
        hRotator = transform.GetChild(0).GetComponent<Rotator>();
        vRotator = hRotator.transform.GetChild(0).GetComponent<Rotator>();
        isoCamera = vRotator.transform.GetChild(0).GetComponent<Camera>();
        contextCamera = vRotator.transform.GetChild(1).GetComponent<Camera>();
        isoLight = isoCamera.GetComponentInChildren<Light>();
        contextLight = contextCamera.GetComponentInChildren<Light>();
        culled = new List<GameObject>();

        isoTexture = new Texture2D(isoOutput.width, isoOutput.height);
        contextTexture = new Texture2D(contextOutput.width, contextOutput.height);

        isoScans = new();
        contextScans = new();
    }

    /// <summary>
    /// Performs a scan on the scanObject, taking both isolated and context pictures of the <br></br>
    /// object from 8 angles
    /// </summary>
    public void Scan(bool saveToDisk, GameObject target)
    {
        Debug.Log($"Scanning {target.name}");
        
        int prevLayer = target.layer;
        target.layer = 6; // Set to scan layer

        EntityId scanObjectId = target.GetEntityId();
        
        if(contextScans.ContainsKey(scanObjectId))
        {
            contextScans[scanObjectId].Clear();
        }
        else
        {
            contextScans.Add(scanObjectId, new List<byte[]>());
        }

        if(isoScans.ContainsKey(scanObjectId))
        {
            isoScans[scanObjectId].Clear();
        }
        else
        {
            isoScans.Add(scanObjectId, new List<byte[]>());
        }
        Vector3 contextOffset = CullUtility.GetTargetDistance(contextCamera.fieldOfView, target.GetComponentInChildren<Renderer>().bounds, 0.3f) * Vector3.back;
        contextCamera.transform.localPosition = contextOffset;
        Vector3 isoOffset = CullUtility.GetTargetDistance(contextCamera.fieldOfView, target.GetComponentInChildren<Renderer>().bounds, 0.6f) * Vector3.back;
        isoCamera.transform.localPosition = isoOffset;

        transform.position = target.transform.position + target.GetComponent<Rigidbody>().centerOfMass;

        for (int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                // Capture Iso Image
                isoLight.enabled = true;
                isoCamera.Render();
                isoLight.enabled = false;

                RenderTexture.active = isoOutput;
                isoTexture.ReadPixels(new Rect(0, 0, isoOutput.width, isoOutput.height), 0, 0);
                isoTexture.Apply();
                
                isoScans[scanObjectId].Add(isoTexture.EncodeToJPG());
                
                if (saveToDisk) File.WriteAllBytes($"Assets/Scans/{target.name}_iso_scan{hRotator.rotateIndex}{vRotator.rotateIndex}.png", isoTexture.EncodeToPNG());

                // Capture Context Image
                List<GameObject> culled = CullUtility.Cull(contextCamera.transform.position, target);
                //Cull(target);
                //culled = CullUtility.Cull(contextCamera.transform.position, scanObject.transform.position);
                contextLight.enabled = true;
                Debug.Log($"Culled {culled.Count} objects");
                contextCamera.Render();
                contextLight.enabled = false;

                RenderTexture.active = contextOutput;
                contextTexture.ReadPixels(new Rect(0, 0, contextOutput.width, contextOutput.height), 0, 0);
                contextTexture.Apply();
                
                contextScans[scanObjectId].Add(contextTexture.EncodeToJPG());
                
                if (saveToDisk) File.WriteAllBytes($"Assets/Scans/{target.name}_context_scan{hRotator.rotateIndex}{vRotator.rotateIndex}.png", contextTexture.EncodeToPNG());
                CullUtility.UnCull(culled);

                // Rotation about X Axis (Pitch)
                vRotator.Rotate();
            }
            // Rotation about Y Axis (Yaw)
            hRotator.Rotate();
        }

        target.layer = prevLayer;
    }

    void Cull(GameObject target)
    {
        List<RaycastHit> hits;
        Vector3 center = target.transform.position + target.GetComponent<Rigidbody>().centerOfMass;
        Vector3 origin = contextCamera.transform.position;
        Ray ray = new Ray(origin, center - origin);
        hits = new List<RaycastHit>(Physics.RaycastAll(ray, (center - origin).magnitude));

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject == scanObject) continue;

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
