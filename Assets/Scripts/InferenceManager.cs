using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Client))]
[RequireComponent(typeof(BatchScan))]
[RequireComponent(typeof(PropertySetter))]
public class InferenceManager : MonoBehaviour
{
    public bool runStartup;
    public Transform environmentParent;
    public PropertySetter materialSetter;
    public BatchScan batchScan;
    public List<ThermObject> thermObjects;
    public List<ThermObject> thermHands;

    private void Awake()
    {
        materialSetter = GetComponent<PropertySetter>();
        batchScan = GetComponent<BatchScan>();
        thermObjects = FindObjectsByType<ThermObject>().ToList();

        foreach (ThermObject thermObject in thermObjects)
        {
            if (thermObject.isHand)
            {
                thermHands.Add(thermObject);
            }
        }
        foreach(ThermObject thermObject in thermHands)
        {
            thermObjects.Remove(thermObject);
        }

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
        batchScan.ScanAll(thermObjects);
        await materialSetter.BatchSet(thermObjects);
        //ThermResolver.Instance().ResetGraph();
        ThermResolver.Instance().freeze = false;
    }
}
