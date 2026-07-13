using UnityEngine;
using System.Collections.Generic;

public class HandThermObject : ThermObject
{
    public float maxRadDistance = 5f; // Maximum distance for radiative heat transfer
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

    // Definitely inaccurate but it might pass as realistic
    public float RadiationTemperature(ThermObject target)
    {
        Vector3 position = transform.position;
        float distanceSquared = (target.rb.ClosestPointOnBounds(position) - position).sqrMagnitude;
        float result = temperature / ((4 * Mathf.PI * distanceSquared) + 1);
        //Debug.Log($"{name}: RadiationTemperature at {position} distanceSquared={distanceSquared}, result={result}");
        return result;
    }
    public void ApplyRadiativeHeat()
    {
        ThermResolver resolver = ThermResolver.Instance();
        List<ThermObject> thermObjects = resolver.graph.thermObjects;
        float timeStep = 0.001f; // Match ThermResolver's fixed timestep
        ThermObject closestObject = null;
        float distSq = maxRadDistance;
        foreach (ThermObject thermObject in thermObjects)
        {
            if (thermObject is HandThermObject) continue;

            if (!thermObject.actAsHeatSource) continue;

            if (resolver.graph.GetEdge(this, thermObject) != null || resolver.graph.GetEdge(thermObject, this) != null)
            {
                continue;
            }

            if((thermObject.transform.position - transform.position).sqrMagnitude < distSq)
            {
                closestObject = thermObject;
                distSq = (thermObject.transform.position - transform.position).sqrMagnitude;
            }
        }

        float radiationTempFromObject = RadiationTemperature(closestObject);
        float heatFlowToHand = radiationTempFromObject - temperature;
        if(heatFlowToHand > 0) ApplyHeatFlow(heatFlowToHand * timeStep);
    }

    //public override void UpdateTemperature()
    //{
    //    //ApplyRadiativeHeat();
    //    //ApplyCooling();
    //    temperature += temperatureDelta;
    //    temperatureDelta = 0;
    //}

    public override void ApplyCooling()
    {
        float environmentTemperature = 30;
        float temperatureDifference = temperature - environmentTemperature;
        float coolingRate = -conductivity * temperatureDifference;
        float deltaTemp = coolingRate / (mass * specificHeat);
        temperatureDelta += deltaTemp;
    }
}

