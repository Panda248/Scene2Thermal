using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ThermObject : MonoBehaviour
{
    public float conductivity, mass, specificHeat;
    public float temperature, newTemperature;

    //public List<ThermObject> contacts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //if(!GetComponent<Collider>().isTrigger)
        //{
        //    GetComponent<Collider>().isTrigger = true;
        //}
        newTemperature = 0;
    }

    private void OnValidate()
    {
        //if (!GetComponent<Collider>().isTrigger)
        //{
        //    GetComponent<Collider>().isTrigger = true;
        //}
        newTemperature = 0;
    }

    public void SetProperties(JsonClasses.MaterialInference materialInference)
    {
        conductivity = materialInference.thermal_conductivity;
        mass = materialInference.mass;
        specificHeat = materialInference.specific_heat;
        temperature = materialInference.temperature;
    }

    public void SetProperties(JsonClasses.ObjectMaterialInference objectMaterialInference)
    {
        conductivity = objectMaterialInference.thermal_conductivity;
        mass = objectMaterialInference.mass;
        specificHeat = objectMaterialInference.heat_capacity;
        temperature = objectMaterialInference.initial_temperature;
    }
    public void ApplyHeatFlow(float heatFlow)
    {
        float deltaTemp = heatFlow / (mass * specificHeat);
        Debug.Log($"Applying heat flow of {heatFlow} to {name}. {deltaTemp} degrees");
        newTemperature += deltaTemp;
    }
    public void UpdateTemperature()
    {
        temperature += newTemperature;
        newTemperature = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ThermObject other = collision.gameObject.GetComponent<ThermObject>();
        if (other != null && ThermResolver.Instance().graph.GetEdge(this, other) == null)
        {
            bool added = ThermResolver.Instance().graph.AddEdge(this, other);
            if (added)
            {
                Debug.Log("Added edge between " + this.name + " and " + other.name);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        ThermObject other = collision.gameObject.GetComponent<ThermObject>();
        if (other != null)
        {
            bool removed = ThermResolver.Instance().graph.RemoveEdge(this, other);
            if (removed)
            {
                Debug.Log("Removed edge between " + this.name + " and " + other.name);
            }
        }
    }

    

}
