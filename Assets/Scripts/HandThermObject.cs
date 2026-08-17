using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class HandThermObject : ThermObject
{
    public bool returnToBodyTemp;
    public bool enableAmbientHeating;
    private float mappedTemperature;
    public static float minDistance = 5f;

    public void OnValidate()
    {
        conductivity = 0.3f;
        mass = 1f;
        specificHeat = 4.814f;
        temperature = 32f;
    }
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
        MapDelta();
        base.UpdateTemperature();
    }

    public void MapDelta()
    {
        float mappedDelta = Mathf.Clamp(lastTemperatureDelta / Time.fixedDeltaTime, -10f, 10f) * 1.2f;
        mappedTemperature = mappedDelta + 27f;
    }

    public float GetData()
    {
        return mappedTemperature;
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

}