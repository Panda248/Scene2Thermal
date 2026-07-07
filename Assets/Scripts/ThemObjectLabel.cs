using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

//[RequireComponent(typeof(ThermObject))]
public class ThemObjectLabel : MonoBehaviour
{
    public TextMeshPro tmp;
    public ThermObject thermObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(thermObject == null)
        {
            thermObject = GetComponentInParent<ThermObject>();
        }
        if(tmp == null)
        {
            tmp = GetComponentInChildren<TextMeshPro>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(thermObject == null)
        {
            tmp.text = "No object being measured";
        }
        else
        {
            tmp.text = $"{thermObject.name}\n{thermObject.temperature:F2}°C";
        }
    }

    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (args.interactableObject.transform.gameObject.TryGetComponent<ThermObject>(out ThermObject obj))
        {
            thermObject = obj;
        }
    }
}
