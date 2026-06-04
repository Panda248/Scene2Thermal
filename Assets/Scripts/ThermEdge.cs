using UnityEngine;

public class ThermEdge : MonoBehaviour
{
    ThermObject from;
    ThermObject to;
    public float weight;
    public ThermEdge(ThermObject from, ThermObject to, float weight)
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
    }

    public void ApplyWeight()
    {
        from.ApplyThermalDelta(-weight * Time.fixedDeltaTime);
        to.ApplyThermalDelta(weight * Time.fixedDeltaTime);
    }
}
