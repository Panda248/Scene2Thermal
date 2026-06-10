using UnityEngine;

public class ThermSensor : MonoBehaviour
{
    public ThermObject target;

    private void FixedUpdate()
    {
        if (target != null)
        {
            Debug.Log($"target temp is {target.temperature}");
        }
    }

}
