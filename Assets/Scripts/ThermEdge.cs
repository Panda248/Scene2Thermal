using UnityEngine;

public class ThermEdge
{
    public ThermObject from;
    public ThermObject to;
    public ThermEdge(ThermObject from, ThermObject to)
    {
        this.from = from;
        this.to = to;
    }
}
