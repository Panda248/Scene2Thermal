using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class HandThermObject : ThermObject
{
    public bool returnToBodyTemp;
    public bool enableAmbientHeating;
    public static float minDistance = 5f;

    public float CalulateWeightedTemperatureDelta()
    {
        
        ThermResolver resolver = ThermResolver.Instance();
        float weightedDelta = returnToBodyTemp ? 37f - temperature : resolver.ambientTemperature - temperature;

        if (resolver.graph.thermObjects.Count <= 1)
        {
            return weightedDelta;
        }
        int consideredObjects = 0;
        foreach (ThermObject thermObject in resolver.graph.thermObjects)
        {
            float distance = (transform.position - thermObject.transform.position).sqrMagnitude;
            distance *= distance;
            if (!(thermObject is HandThermObject) && distance < minDistance * minDistance)
            {
                weightedDelta += (thermObject.temperature - temperature) / (distance + 1.2f);
                consideredObjects++;
            }
        }
        //Debug.Log(weightedDelta);
        //Debug.Log(weightedDelta / (consideredObjects + 1));
        return weightedDelta / (consideredObjects + 1);
    }

    public override void UpdateTemperature()
    {
        if (!ThermResolver.Instance().graph.HasEdge(this) && enableAmbientHeating)
        {
            float delta = CalulateWeightedTemperatureDelta();
            ApplyHeatFlow(delta * conductivity * 0.01f);
        }
        base.UpdateTemperature();
    }

    public void FixedUpdate()
    {

    }
}

    //public float GetSensedTemperature()
    //{
    //    if(ThermResolver.Instance().graph.HasEdge(this))
    //    {
    //        return temperature;
    //    }
    //    else
    //    {
    //        return CalulateWeightedTemperatureAverage();
    //    }
    //}

