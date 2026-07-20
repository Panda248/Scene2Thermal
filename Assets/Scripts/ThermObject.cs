using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class ThermObject : MonoBehaviour
{
    //const float SPEED_OF_LIGHT = 299800000;
    //const float STEFAN_BOLTZMANN_CONSTANT = 5.670f / 100000000f;

    public float conductivity, mass, specificHeat, generationRate, temperature, temperatureDelta, volume;
    public bool actAsHeatSource, isHand;
    //Collider coll;
    Rigidbody rb;

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

    public virtual void UpdateTemperature()
    {
        if (actAsHeatSource) {
            //Debug.Log($"{name}: acting as heat source");
            ApplyHeatFlow(generationRate * 0.01f);
        }
        temperature += temperatureDelta;
        temperatureDelta = 0;
    }

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
    public void OnTriggerEnter(Collider other)
    {
        ThermObject thermObject = other.gameObject.GetComponentInParent<ThermObject>();
        if (thermObject != null)
        {
            ThermResolver.Instance().graph.AddEdge(this, thermObject);
            Debug.Log("Hand edge added");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        ThermObject thermObject = other.gameObject.GetComponentInParent<ThermObject>();
        if (thermObject != null)
        {
            ThermResolver.Instance().graph.RemoveEdge(this, thermObject);
            Debug.Log("hand edge removed");
        }
    }

}
