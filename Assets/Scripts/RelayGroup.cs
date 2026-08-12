using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class RelayGroup : MonoBehaviour
{
    [SerializeField]
    HandThermObject targetHandler;
    public List<Collider> colliders;
    bool collided = false;
    NativeHashMap<EntityId, bool> colliderStates;

    public void Awake()
    {
        colliderStates = new (colliders.Count, Allocator.Persistent);
        foreach (Collider collider in colliders)
        {
            if( !colliderStates.TryAdd(collider.GetEntityId(), false))
            {
                Debug.LogWarning($"Failed to add collider with EntityId {collider.GetEntityId()} to colliderStates.");
            }
        }
    }

    public void BubbleEnter(EntityId sourceId, Collider other)
    {
        colliderStates[sourceId] = true;
        if (!collided)
        {
            Debug.Log($"One collider entered, propagating enter to {targetHandler.name}");

            collided = true;
            targetHandler.OnTriggerEnter(other);
            //targetHandler.SendMessage("OnTriggerEnter", other, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void BubbleExit(EntityId sourceId, Collider other)
    {

        colliderStates[sourceId] = false;
        if (colliderStates.GetValueArray(Allocator.Temp).All(state => !state))
        {
            Debug.Log($"All colliders exited, propagating exit to {targetHandler.name}");
            collided = false;
            targetHandler.OnTriggerExit(other);
            //targetHandler.SendMessage("OnTriggerExit", other, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void OnDestroy()
    {
        colliderStates.Dispose();
    }
}

