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
    private List<MeshRenderer> meshRenderers;
    private List<List<Material>> prevMaterials;
    private List<List<Material>> thermMaterialsList;

    private static readonly int TemperatureId = Shader.PropertyToID("_Temperature");

    private void OnEnable()
    {
        SetRendererMaterials(true);
        //mRenderer.SetMaterials(thermMaterials);
    }

    private void OnDisable()
    {
        SetRendererMaterials(false);
        //mRenderer.SetMaterials(prevMaterial);
    }

    private void Awake()
    {
        thermMaterial = Resources.Load<Material>("Temperature");
        //mRenderer = GetComponent<MeshRenderer>();
        //prevMaterial = new List<Material>();
        //mRenderer.GetMaterials(prevMaterial);
        //thermMaterials = new List<Material>();
        //for(int i = 0; i < prevMaterial.Count; i++)
        //{
        //    thermMaterials.Add(thermMaterial);
        //}
        mPropertyBlock = new MaterialPropertyBlock();
        thermObject = GetComponent<ThermObject>();

        FindRenderers();
    }

    void FindRenderers()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        meshRenderers = new List<MeshRenderer>();
        prevMaterials = new List<List<Material>>();
        thermMaterialsList = new List<List<Material>>();

        foreach (Transform child in children)
        {
            if(child.TryGetComponent<MeshRenderer>(out var renderer))
            {
                //if(!child.TryGetComponent<Collider>(out _))
                //{
                //    continue;
                //}
                meshRenderers.Add(renderer);

                List<Material> materials = new List<Material>();
                renderer.GetMaterials(materials);
                prevMaterials.Add(materials);
                
                List<Material> tempThermList = new List<Material>();
                for (int i = 0; i < materials.Count; i++)
                {
                    tempThermList.Add(thermMaterial);
                }
                thermMaterialsList.Add(tempThermList);

            }
        }
    }

    void SetRendererMaterials(bool useThermMaterials)
    {
        for(int i = 0; i < meshRenderers.Count; i++)
        {
            if(useThermMaterials)
            {

                meshRenderers[i].SetMaterials(thermMaterialsList[i]);
            }
            else
            {
                meshRenderers[i].SetMaterials(prevMaterials[i]);
            }
        }
    }

    void FixedUpdate()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.GetPropertyBlock(mPropertyBlock);
            mPropertyBlock.SetFloat(TemperatureId, thermObject.temperature);
            renderer.SetPropertyBlock(mPropertyBlock);
        }
        //if (mRenderer != null)
        //{
        //    mPropertyBlock.SetFloat(TemperatureId, thermObject.temperature);
        //    mRenderer.SetPropertyBlock(mPropertyBlock);
        //}
    }
}
