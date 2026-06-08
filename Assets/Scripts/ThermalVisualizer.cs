using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(ThermObject))]
public class ThermalVisualizer : MonoBehaviour
{
    private MeshRenderer mRenderer;
    private MaterialPropertyBlock mPropertyBlock;
    private ThermObject thermObject;
    private static readonly int TemperatureId = Shader.PropertyToID("_Temperature");

    void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();
        thermObject = GetComponent<ThermObject>();
        mPropertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (mRenderer != null)
        {
            mRenderer.GetPropertyBlock(mPropertyBlock);
            mPropertyBlock.SetFloat(TemperatureId, thermObject.temperature);
            mRenderer.SetPropertyBlock(mPropertyBlock);
        }
    }
}
