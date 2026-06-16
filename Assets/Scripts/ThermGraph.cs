using System.Collections.Generic;

public class ThermGraph
{
    public List<ThermObject> thermObjects;
    public List<ThermEdge> edges;

    public ThermGraph()
    {
        thermObjects = new List<ThermObject>();
        edges = new List<ThermEdge>();
    }

    public ThermEdge GetEdge(ThermObject from, ThermObject to)
    {
        foreach (ThermEdge edge in edges)
        {
            if (edge.from == from && edge.to == to)
            {
                return edge;
            }
        }
        return null;
    }

    public bool AddEdge(ThermObject from, ThermObject to)
    {
        if (GetEdge(from, to) == null && GetEdge(to, from) == null)
        {
            edges.Add(new ThermEdge(from, to));
            return true;
        }
        return false;
    }
     public bool RemoveEdge(ThermObject from, ThermObject to)
    {
        ThermEdge edge = GetEdge(from, to);
        if (edge == null)
        {
            edge = GetEdge(to, from);
        }
        if(edge != null)
        {
            edges.Remove(edge);
            return true;
        }
        return false;
    }

    public void Clear()
    {
        thermObjects.Clear();
        edges.Clear();
    }

}
