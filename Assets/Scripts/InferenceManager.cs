using UnityEngine;

[RequireComponent(typeof(Client))]
[RequireComponent(typeof(BatchScan))]
[RequireComponent(typeof(MaterialSetter))]
public class InferenceManager : MonoBehaviour
{
    public bool runStartup;
    public Transform environmentParent;
    public MaterialSetter materialSetter;
    public BatchScan batchScan;

    private void Awake()
    {
        materialSetter = GetComponent<MaterialSetter>();
        batchScan = GetComponent<BatchScan>();
        materialSetter.environmentParent = environmentParent;
        batchScan.environmentParent = environmentParent;
    }
    void Start()
    {
        if(runStartup)
        {
            RunStartup();
        }
    }

    public async void RunStartup()
    {
        ThermResolver.Instance().freeze = true;
        batchScan.ScanAll();
        await materialSetter.BatchSet();
        //ThermResolver.Instance().ResetGraph();
        ThermResolver.Instance().freeze = false;
    }
}
