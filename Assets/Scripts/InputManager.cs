using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputActionReference toggleThermsRef;
    List<ThermalVisualizer> thermalVisualizers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thermalVisualizers = new List<ThermalVisualizer>();
        foreach (Transform child in ThermEnvironment.Instance().transform)
        {
            if(child.TryGetComponent<ThermalVisualizer>(out ThermalVisualizer viz))
            {
                thermalVisualizers.Add(viz);
            }
        }
        //Debug.Log($"{thermalVisualizers.Count} thermal Visualizers");

        toggleThermsRef.action.performed += ctx => OnToggleThermals();
        toggleThermsRef.action.Enable();
    }


    public void OnToggleThermals()
    {
        //Debug.Log("printing");

        foreach (ThermalVisualizer visualizer in thermalVisualizers)
        {
            visualizer.enabled = !visualizer.enabled;
        }
    }
}
