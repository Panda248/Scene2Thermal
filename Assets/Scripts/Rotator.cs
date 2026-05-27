using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public List<Vector3> rotations;
    public int rotateIndex { get; private set; }

    private void Awake()
    {
        rotateIndex = 0;
        transform.localEulerAngles = rotations[rotateIndex];
    }
    public void Rotate()
    {
        transform.localEulerAngles = rotations[rotateIndex++];
        if(rotateIndex >= rotations.Count) rotateIndex = 0;
    }
}
