using UnityEngine;

public class ThermResolver : MonoBehaviour
{
    public ThermGraph graph;
    public bool freeze;
    public float smallestTempDelta = 0f;
    public float timeStep = 0.01f;

    static ThermResolver instance;
    public static ThermResolver Instance()
    {
        if (instance == null) instance = FindAnyObjectByType<ThermResolver>();
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (freeze) return;

        ResolveEdges();
        UpdateThermObjects();
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
    * For now, I'm going to have k be the "total resistance" of
    * a circuit in series.
    *
    * R = R1 + R2 + R3 + ...
    *
    *For now I wont use cross sectional area or length.TODO: implement
    *
    * 1 / k = (1 / k1) + (1 / k2) + (1 / k3) + ...
    *
    */

    void ResolveEdgeFourier(ThermEdge edge)
    {
        //Debug.Log($"Resolving edge from {edge.from.name} to {edge.to.name}");
        //Debug.Log($"{edge.from.name} temp: {edge.from.temperature}, conductivity: {edge.from.conductivity}");
        //Debug.Log($"{edge.to.name} temp: {edge.to.temperature}, conductivity: {edge.to.conductivity}");

        float tempDelta = edge.from.temperature - edge.to.temperature;
        if(Mathf.Abs(tempDelta) < smallestTempDelta)
        {
            return;
        }
        float k = 1f / (1f / edge.from.conductivity + 1f / edge.to.conductivity);
        float flux = k * tempDelta;
        float qt = flux * timeStep;

        //Debug.Log($"tempDelta: {tempDelta}, k: {k}, flux: {flux}, qt: {qt}");

        edge.to.ApplyHeatFlow(qt);
        edge.from.ApplyHeatFlow(-qt);
    }

    //// Not good for scale. kd tree?
    //public ThermObject ClosestThermObject(Vector3 position)
    //{
    //    ThermObject result = null;
    //    float distance = Mathf.Infinity;
    //    foreach (ThermObject obj in graph.thermObjects)
    //    {
    //        float dist = (obj.transform.position - position).sqrMagnitude;
    //        if (dist < distance)
    //        {
    //            distance = dist;
    //            result = obj;
    //        }
    //    }

    //    return result;
    //}

    public void ResolveEdges()
    {
        ThermEdge[] edges = graph.edges.ToArray();
        foreach (ThermEdge edge in edges)
        {
            ResolveEdgeFourier(edge);
        }
    }
    public void UpdateThermObjects()
    {
        foreach(ThermObject obj in graph.thermObjects)
        {
            obj.UpdateTemperature();
        }
    }
    public void ResetGraph()
    {
        graph.Clear();
        graph.thermObjects.AddRange(FindObjectsByType<ThermObject>());
        Debug.Log(graph.thermObjects.Count + " ThermObjects found.");
    }
}