using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ThermSensor : MonoBehaviour
{
    public ThermObject target;

    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (args.interactableObject.transform.gameObject.TryGetComponent<ThermObject>(out ThermObject obj)) {
            target = obj;
        }
    }

}
