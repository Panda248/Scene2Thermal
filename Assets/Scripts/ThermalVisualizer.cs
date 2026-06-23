using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(ThermObject))]
public class ThermalVisualizer : MonoBehaviour
{
    public Material thermMaterial;
    private MeshRenderer mRenderer;
    private List<Material> thermMaterials;
    private MaterialPropertyBlock mPropertyBlock;
    private ThermObject thermObject;
    private List<Material> prevMaterial;
    private static readonly int TemperatureId = Shader.PropertyToID("_Temperature");

    private void OnEnable()
    {
        mRenderer.SetMaterials(thermMaterials);
        mRenderer.GetPropertyBlock(mPropertyBlock);
    }

    private void OnDisable()
    {
        mRenderer.SetMaterials(prevMaterial);
    }

    private void Awake()
    {
        thermMaterial = Resources.Load<Material>("Temperature");
        mRenderer = GetComponent<MeshRenderer>();
        thermObject = GetComponent<ThermObject>();
        prevMaterial = new List<Material>();
        mRenderer.GetMaterials(prevMaterial);
        thermMaterials = new List<Material>();
        for(int i = 0; i < prevMaterial.Count; i++)
        {
            thermMaterials.Add(thermMaterial);
        }
        mPropertyBlock = new MaterialPropertyBlock();
    }
    void Start()
    {
    }

    void FixedUpdate()
    {
        if (mRenderer != null)
        {
            mPropertyBlock.SetFloat(TemperatureId, thermObject.temperature);
            mRenderer.SetPropertyBlock(mPropertyBlock);
        }
    }
}
