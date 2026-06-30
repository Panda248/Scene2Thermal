using UnityEngine;

public class HandThermObject : ThermObject
{

    void Cool()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        ThermObject thermObject = other.gameObject.GetComponentInParent<ThermObject>();
        if(thermObject != null)
        {
            ThermResolver.Instance().graph.AddEdge(this, thermObject);
            Debug.Log("Hand edge added");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        ThermObject thermObject = other.gameObject.GetComponentInParent<ThermObject>();
        if (thermObject != null)
        {
            ThermResolver.Instance().graph.RemoveEdge(this, thermObject);
            Debug.Log("hand edge removed");
        }
    }
}
