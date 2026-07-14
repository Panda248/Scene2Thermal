using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleAnalyzer : MonoBehaviour
{
    public InputActionReference inputAction;
    public MeshRenderer mRenderer;

    void Start()
    {
        inputAction.action.performed += ctx => ToggleAnalyzerComponent();
        inputAction.action.Enable();
    }

    private void ToggleAnalyzerComponent()
    {
        mRenderer.enabled = !mRenderer.enabled;
    }
}
