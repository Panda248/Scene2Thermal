using UnityEngine;

public class ThermResolver : MonoBehaviour
{
    public ThermGraph graph;
    public bool freeze;
    public float smallestTempDelta = 0f;

    static ThermResolver instance;
    public static ThermResolver Instance()
    {
        instance = instance == null ? FindAnyObjectByType<ThermResolver>() : instance;
        return instance;
    }

    private void Awake()
    {
        graph = new ThermGraph();
    }

    private void Start()
    {
        ResetGraph();
    }

    public void ResetGraph()
    {
        graph.Clear();
        graph.thermObjects.AddRange(FindObjectsByType<ThermObject>());
        Debug.Log(graph.thermObjects.Count + " ThermObjects found.");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (freeze) return;
        /***
         * Get all collisions.
         * Decide which are Thermobjects.
         * Get their temperatures and thermal conductivities.
         */
        ResolveEdges();
        UpdateObjects();
    }

    /* 
    * use the following equation
    * T(t) = T0 + (T1 - T0) * (e^(-k* t))
    * 
    * maybe derive?
    * T'(t) = -k * (T1 - T0) * e^(-k*t)\
    * 
    * t = time since contact
    * T1 = temperature of other object
    * T0 = temperature of this object at time of contact
    * k = thermal conductivity of other object
    *
    * Fouriers Law:
    * q = -k * (T1 - T0)
    * dT = q * t
    * T(t) = T0 + (t * -k * (T1 - T0)) ^ 2 / 2
    * For now, I'm going to have k be the "total resistance" of
    * a circuit in series.
    *
    * R = R1 + R2 + R3 + ...
    *
    *For now I wont use cross sectional area or length.TODO: implement
    *
    * k = k1 + k2 + k3 + ...
    *

    *sum each component for each contact.
    * 
    */
    void ResolveEdgeFourier(ThermEdge edge)
    {
        // From = T0, To = T1
        //Debug.Log($"Resolving edge from {edge.from.name} to {edge.to.name}");
        //Debug.Log($"{edge.from.name} temp: {edge.from.temperature}, conductivity: {edge.from.conductivity}");
        //Debug.Log($"{edge.to.name} temp: {edge.to.temperature}, conductivity: {edge.to.conductivity}");

        float tempDelta = edge.to.temperature - edge.from.temperature;
        if(Mathf.Abs(tempDelta) < smallestTempDelta)
        {
            return;
        }
        float k = edge.from.conductivity + edge.to.conductivity;
        float t = 0.001f;
        float flux = -k * tempDelta;
        float qt = flux * t;

        //Debug.Log($"tempDelta: {tempDelta}, k: {k}, flux: {flux}, qt: {qt}");

        edge.from.ApplyHeatFlow(-qt);
        edge.to.ApplyHeatFlow(qt);
    }

    // Not good for scale. kd tree?
    public ThermObject ClosestThermObject(Vector3 position)
    {
        ThermObject result = null;
        float distance = Mathf.Infinity;
        foreach (ThermObject obj in graph.thermObjects)
        {
            float dist = (obj.transform.position - position).sqrMagnitude;
            if (dist < distance)
            {
                distance = dist;
                result = obj;
            }
        }

        return result;
    }

    public void ResolveEdges()
    {
        foreach(ThermEdge edge in graph.edges)
            {
                ResolveEdgeFourier(edge);
        }
    }
    public void UpdateObjects()
    {
        foreach(ThermObject obj in graph.thermObjects)
        {
            obj.UpdateTemperature();
        }
    }
}