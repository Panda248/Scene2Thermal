using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ContactTracker : MonoBehaviour
{
    Collider coll;

    private void Awake()
    {
        coll = GetComponent<Collider>();
        if(!coll.providesContacts)
        {
            Debug.LogWarning($"Collider on {name} does not provide contacts.");
        }
        Physics.ContactEvent += Physics_ContactEvent;
    }

    private void Physics_ContactEvent(PhysicsScene scene, Unity.Collections.NativeArray<ContactPairHeader>.ReadOnly headerArray)
    {
        
    }
}
