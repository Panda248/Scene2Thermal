using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public List<Vector3> rotations;
    private int rotateIndex;

    private void Awake()
    {
        rotateIndex = 0;
    }
    public void Rotate()
    {
        transform.Rotate(rotations[rotateIndex++]);
        if(rotateIndex > rotations.Count) rotateIndex = 0;
    }
}
