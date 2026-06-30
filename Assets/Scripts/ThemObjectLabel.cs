using TMPro;
using UnityEngine;

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
        tmp.text = $"{thermObject.name}\n{thermObject.temperature:F2}°C";
    }
}
