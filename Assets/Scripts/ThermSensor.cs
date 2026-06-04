using UnityEngine;

public class ThermSensor : MonoBehaviour
{
    public ThermObject target;

    private void FixedUpdate()
    {
        Debug.Log($"target temp is {target.temperature}");
    }

}
