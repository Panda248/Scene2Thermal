using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Batch scanning Script that runs Scene Scanning script
/// and Object scanning on each environment object
/// Environment objects are determined as objects directly childed to the environmentParent field
/// </summary>
public class BatchScan : MonoBehaviour
{
    public List<ThermObject> thermObjects;
    public Transform environmentRoot;
    public bool saveToDisk;

    public void ScanAll(List<ThermObject> thermObjects, bool saveToDisk=false)
    {
        Vector3 center = GetCenter(thermObjects);
        ScanScene(center, saveToDisk);
        ScanObjects(thermObjects, saveToDisk);
    }

    public void ScanRoot(bool saveToDisk=false)
    {
        thermObjects = new List<ThermObject>();
        foreach (Transform child in environmentRoot)
        {
            if (child.TryGetComponent<ThermObject>(out var thermObj))
            {
                thermObjects.Add(thermObj);
            }
            else
            {
                thermObjects.Add(child.AddComponent<ThermObject>());
            }
        }
        ScanAll(thermObjects, saveToDisk);
    }

    public void ScanScene(Vector3 center, bool saveToDisk=false)
    {
        SceneScanner.Instance().Scan(center, saveToDisk);
    }

    public void ScanObjects(List<ThermObject> thermObjects, bool saveToDisk=false)
    {
        foreach (ThermObject thermObj in thermObjects)
        {
            ObjectScanner.Instance().Scan(saveToDisk, thermObj.gameObject);
        }
        ObjectScanner.Instance().ScanObject = null;
    }

    public Vector3 GetCenter(List<ThermObject> thermObjects)
    {
        if (thermObjects == null || thermObjects.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 center = Vector3.zero;
        foreach (ThermObject thermObj in thermObjects)
        {
            center += thermObj.transform.position;
        }
        center /= thermObjects.Count;
        return center;
    }
}
