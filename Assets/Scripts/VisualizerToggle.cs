using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class VisualizerToggle : MonoBehaviour
{
    public InputActionReference toggleThermsRef;
    List<ThermalVisualizer> thermalVisualizers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thermalVisualizers = FindObjectsByType<ThermalVisualizer>().ToList();
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
