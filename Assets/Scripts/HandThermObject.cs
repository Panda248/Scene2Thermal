using UnityEngine;
using System.Collections.Generic;
using UnityEngine;

public class HandThermObject : ThermObject
{

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

    public void ApplyRadiativeHeat()
    {
        // Apply bidirectional radiative heat transfer to ThermObjects based on distance
        // Optimization: Cache singleton and graph reference
        ThermResolver resolver = ThermResolver.Instance();
        List<ThermObject> thermObjects = resolver.graph.thermObjects;
        float timeStep = 0.001f; // Match ThermResolver's fixed timestep
        Vector3 handPosition = transform.position;

        foreach (ThermObject thermObject in thermObjects)
        {
            if (thermObject == this) continue;

            // Optimization: Skip objects already in contact (have edges)
            if (resolver.graph.GetEdge(this, thermObject) != null || resolver.graph.GetEdge(thermObject, this) != null)
            {
                continue;
            }

            // Calculate radiative heat transfer in both directions
            // Hand to object
            float radiationTempFromHand = RadiationTemperature(thermObject.transform.position);
            float heatFlowToObject = radiationTempFromHand - thermObject.temperature;
            thermObject.ApplyHeatFlow(heatFlowToObject * timeStep);

            // Object to hand (reciprocal)
            float radiationTempFromObject = thermObject.RadiationTemperature(handPosition);
            float heatFlowToHand = radiationTempFromObject - temperature;
            ApplyHeatFlow(heatFlowToHand * timeStep);
        }
    }
}

