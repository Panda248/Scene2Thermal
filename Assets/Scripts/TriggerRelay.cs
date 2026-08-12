using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerRelay : MonoBehaviour
{
    [SerializeField] 
    RelayGroup targetHandler;


    private void OnTriggerEnter(Collider other)
    {
        if (targetHandler != null)
        {
            Debug.Log($"{name} Enter, propogate to {targetHandler.name}");
            targetHandler.BubbleEnter(this.GetEntityId(), other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetHandler != null)
        {
            Debug.Log($"{name} Exit, propogate to {targetHandler.name}");
            targetHandler.BubbleExit(this.GetEntityId(), other);
        }
    }
}


