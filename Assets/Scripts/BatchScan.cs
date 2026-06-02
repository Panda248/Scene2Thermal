using UnityEngine;

/// <summary>
/// Batch scanning Script that runs Scene Scanning script
/// and Object scanning on each environment object
/// Environment objects are determined as objects directly childed to the environmentParent field
/// </summary>
public class BatchScan : MonoBehaviour
{
    public Transform environmentParent;
    public bool saveToDisk;
    bool started;
    public void Scan(bool saveToDisk)
    {
        Debug.Log($"Scanning {environmentParent.childCount} objects");
        SceneScanner.Instance().environmentParent = environmentParent;
        SceneScanner.Instance().Scan(saveToDisk);
        for (int i = 0; i < environmentParent.childCount; i++)
        {
            ObjectScanner.Instance().ScanObject = environmentParent.GetChild(i).gameObject;
            ObjectScanner.Instance().Scan(saveToDisk);
        }
        ObjectScanner.Instance().ScanObject = null;
    }

    private void Awake()
    {
        started = true;
    }

    private void Update()
    {
        if(started) 
        {
            Scan(saveToDisk);
            started = false;
        }
    }
}
