using UnityEngine;

public class SourceToggler : MonoBehaviour
{
    public ThermObject thermObject;
    public void ToggleHeat()
    {
        thermObject.actAsHeatSource = !thermObject.actAsHeatSource;
    }
}
