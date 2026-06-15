using UnityEngine;

public class ThermSensor : MonoBehaviour
{
    public ThermObject target;

    [SerializeField]
    float radiationTemp = 0f;

    private void FixedUpdate()
    {
        //if (target != null)
        //{
        //    Debug.Log($"target temp is {target.temperature}");
        //    Debug.Log($"radiation temp is {target.RadiationTemperature((transform.position - target.transform.position).sqrMagnitude)}");
        //}
        radiationTemp = target.RadiationTemperature(transform.position);
    }

}
