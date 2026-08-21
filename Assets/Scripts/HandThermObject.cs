using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class HandThermObject : ThermObject
{
    public bool returnToBodyTemp;
    public bool enableAmbientHeating;
    public float mappedTemperature { get; private set; }
    public static float minDistance = 5f;

    public void OnValidate()
    {
        conductivity = 0.3f;
        mass = 1f;
        specificHeat = 4.814f;
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
        base.UpdateTemperature();
        MapDelta();
    }

    public void MapDelta()
    {
        float x = Mathf.Abs(lastTemperatureDelta / Time.fixedDeltaTime);
        float yOffset = 12;
        float numerator = 46;
        float xOffset = 3.833f;

        float mappedDelta = Mathf.Sign(lastTemperatureDelta) * (-(numerator / (x - xOffset)) + yOffset);
        //float mappedDelta = Mathf.Clamp(lastTemperatureDelta / Time.fixedDeltaTime, -10f, 10f) * 1.2f;
        mappedTemperature = mappedDelta + 27f;
    }

    public float GetData()
    {
        return mappedTemperature;
        //return lastTemperatureDelta * 1000.0f;
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