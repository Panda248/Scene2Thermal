using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ThermObject : MonoBehaviour
{
    private float conductivity;
    public float Conductivity { get; private set; }
    public float temperature;

    List<ThermObject> contacts;

    //public List<ThermObject> contacts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(!GetComponent<Collider>().isTrigger)
        {
            GetComponent<Collider>().isTrigger = true;
        }
    }

    private void OnValidate()
    {
        if (!GetComponent<Collider>().isTrigger)
        {
            GetComponent<Collider>().isTrigger = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ThermObject other = collision.gameObject.GetComponent<ThermObject>();
        if (other != null)
        {
            contacts.Add(other);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        ThermObject other = collision.gameObject.GetComponent<ThermObject>();
        if (other != null)
        {
            contacts.Remove(other);
        }
    }

    public void ApplyThermalDelta(float delta)
    {
        temperature += delta;
    }

}
