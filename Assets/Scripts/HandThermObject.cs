using UnityEngine;

public class HandThermObject : ThermObject
{
    public bool returnToBodyTemp;
    public static float minDistance = 5f;
    public float CalulateWeightedTemperatureDelta()
    {

        ThermResolver resolver = ThermResolver.Instance();
        float weightedDelta = returnToBodyTemp ? 37f - temperature : 0f;

        if (resolver.graph.thermObjects.Count <= 1)
        {
            return weightedDelta;
        }
        int consideredObjects = 1;
        foreach (ThermObject thermObject in resolver.graph.thermObjects)
        {
            float distance = (transform.position - thermObject.transform.position).sqrMagnitude;
            if (!(thermObject is HandThermObject) && distance < minDistance * minDistance)
            {
                weightedDelta += (thermObject.temperature - temperature) / (distance + 1f);
                consideredObjects++;
            }
        }
        Debug.Log(weightedDelta);
        Debug.Log(weightedDelta / (consideredObjects));
        return weightedDelta / (consideredObjects);
    }

    public override void UpdateTemperature()
    {
        if (!ThermResolver.Instance().graph.HasEdge(this))
        {
            float delta = CalulateWeightedTemperatureDelta();
            ApplyHeatFlow(delta * conductivity * 0.01f);
        }
        base.UpdateTemperature();
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
