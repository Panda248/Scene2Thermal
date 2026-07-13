using System;
using System.Collections.Generic;
using JsonClasses;
using UnityEngine;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof(Rigidbody))]
public class ThermObject : MonoBehaviour
{
    const float SPEED_OF_LIGHT = 299800000;
    const float STEFAN_BOLTZMANN_CONSTANT = 5.670f / 100000000f;

    public float conductivity, mass, specificHeat, generationRate, temperature, temperatureDelta, volume;
    public bool actAsHeatSource;
    //Collider coll;
    public Rigidbody rb;

    void Awake()
    {
        temperatureDelta = 0;
        //coll = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        //volume = coll.bounds.size.x * coll.bounds.size.y * coll.bounds.size.z;
    }

    private void Start()
    {
        ThermResolver.Instance().graph.thermObjects.Add(this);
    }

    private void OnValidate()
    {
        if(specificHeat == 0)
        {
            specificHeat = 1;
        }
        if(mass == 0)
        {
            mass = 1;
        }
    }

    //public void SetProperties(JsonClasses.MaterialInference materialInference)
    //{
    //    conductivity = materialInference.thermal_conductivity;
    //    mass = materialInference.mass;
    //    specificHeat = materialInference.specific_heat;
    //    temperature = materialInference.temperature;
    //}

    //public void SetProperties(JsonClasses.ObjectMaterialInference objectMaterialInference)
    //{
    //    conductivity = objectMaterialInference.thermal_conductivity;
    //    mass = objectMaterialInference.mass;
    //    specificHeat = objectMaterialInference.heat_capacity;
    //    temperature = objectMaterialInference.initial_temperature;
    //}

    public void SetProperties(JsonClasses.ThermObjectProperties properties)
    {
        //Debug.Log($"{name}: SetProperties");
        conductivity = properties.thermal_conductivity;
        mass = properties.mass == 0 ? 1 : properties.mass;
        if(rb != null) rb.mass = mass/1000f;
        specificHeat = properties.heat_capacity == 0 ? 1 : properties.heat_capacity;
        generationRate = properties.heat_generation_rate;
        temperature = properties.initial_temperature;
        actAsHeatSource = properties.initially_on;
    }

    public void ApplyHeatFlow(float heatFlow)
    {
        float deltaTemp = heatFlow / (mass * specificHeat);
        //Debug.Log($"{name}: ApplyHeatFlow heatFlow={heatFlow}, deltaTemp={deltaTemp}");
        temperatureDelta += deltaTemp;
    }

    public virtual void ApplyCooling()
    {
        // Newton's law of cooling: dT/dt = -h * (T - T_environment)
        // where h is the heat transfer coefficient (conductivity)
        float environmentTemperature = ThermEnvironment.Instance().temperature;
        float temperatureDifference = temperature - environmentTemperature;
        float coolingRate = -conductivity * temperatureDifference;
        float deltaTemp = coolingRate * 0.02f / (mass * specificHeat);
        temperatureDelta += deltaTemp;
    }
    public virtual void UpdateTemperature()
    {
        if (actAsHeatSource) {
            //Debug.Log($"{name}: acting as heat source");
            ApplyHeatFlow(generationRate);
        }
        ApplyCooling();
        temperature += temperatureDelta;
        temperatureDelta = 0;
    }

    //void OnCollisionStay(Collision collisionInfo)
    //{
    //    // Debug-draw all contact points and normals
    //    //Debug.Log(collisionInfo.contactCount);
    //    //for (int i = 0; i < collisionInfo.contactCount; i++)
    //    //{
    //    //    ContactPoint contact = collisionInfo.GetContact(i);
    //    //    Debug.DrawRay(contact.point, contact.normal, Color.white);
    //    //}
    //}

    

    private void OnCollisionEnter(Collision collision)
    {
        //ThermObject other = collision.gameObject.GetComponent<ThermObject>();
        ThermObject other = collision.gameObject.GetComponentInParent<ThermObject>();
        if (other != null && ThermResolver.Instance().graph.GetEdge(this, other) == null)
        {
            bool added = ThermResolver.Instance().graph.AddEdge(this, other);
            if (added)
            {
                Debug.Log("Added edge between " + this.name + " and " + other.name);
            }
            //Physics.OverlapBox(transform.position, coll.bounds.extents, transform.rotation);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //ThermObject other = collision.gameObject.GetComponent<ThermObject>();
        ThermObject other = collision.gameObject.GetComponentInParent<ThermObject>();
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
