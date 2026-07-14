using UnityEngine;

public class ThermEnvironment : MonoBehaviour
{
    Bounds bounds;
    public float temperature = 20f;

    static ThermEnvironment instance;
    public static ThermEnvironment Instance()
    {
        instance = instance == null ? FindAnyObjectByType<ThermEnvironment>() : instance;
        return instance;
    }

    void Start()
    {
        bounds = new Bounds(transform.position, transform.localScale);

        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if(child.TryGetComponent<Collider>(out Collider coll))
            {
                bounds.Encapsulate(coll.bounds);
            }
        }
    }
}
