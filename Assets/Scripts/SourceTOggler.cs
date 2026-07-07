using UnityEngine;

public class SourceTOggler : MonoBehaviour
{
    public ThermObject thermObject;
    public void ToggleHeat()
    {
        thermObject.actAsHeatSource = !thermObject.actAsHeatSource;
    }
}
