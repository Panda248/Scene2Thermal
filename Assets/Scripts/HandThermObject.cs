using UnityEngine;

public class HandThermObject : ThermObject
{
    public float CalulateWeightedTemperatureDelta()
    {

        ThermResolver resolver = ThermResolver.Instance();
        float weightedDelta = 0f;

        if (resolver.graph.thermObjects.Count <= 1)
        {
            return weightedDelta;
        }

        foreach (ThermObject thermObject in resolver.graph.thermObjects)
        {
            if (thermObject != this)
            {
                float distance = Vector3.Distance(transform.position, thermObject.transform.position);

                weightedDelta += (thermObject.temperature - temperature) / (distance + 1f);
            }
        }
        Debug.Log(weightedDelta);
        Debug.Log(weightedDelta / (resolver.graph.thermObjects.Count - 1));
        return weightedDelta / (resolver.graph.thermObjects.Count - 1);
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
