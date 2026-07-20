using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CullUtility
{
    // Culls based on 5 rays arranged in a plus sign
    // Returns a list of game objects that are hit by the rays
    public static List<GameObject> Cull(Vector3 origin, Vector3 dest, GameObject blacklist)
    {
        HashSet<GameObject> culledObjects = new HashSet<GameObject>();
        Vector3 direction = dest - origin;
        float distance = direction.magnitude;
        direction.Normalize();
        List<Ray> rays = new List<Ray>();
        rays.Add(new Ray(origin, direction));
        rays.Add(new Ray(origin + Vector3.up * 0.1f, direction));
        rays.Add(new Ray(origin + Vector3.down * 0.1f, direction));
        rays.Add(new Ray(origin + Vector3.left * 0.1f, direction));
        rays.Add(new Ray(origin + Vector3.right * 0.1f, direction));
        foreach (Ray ray in rays)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, distance);
            foreach (RaycastHit hit in hits)
            {
                if(culledObjects.Contains(hit.collider.gameObject) || hit.collider.gameObject == blacklist) continue;
                culledObjects.Add(hit.collider.gameObject);
                //hit.transform.GetComponent<Renderer>().enabled = false;
                hit.collider.gameObject.SetActive(false);
            }
        }

        return culledObjects.ToList();
    }

    public static List<GameObject> Cull(Vector3 origin, GameObject target)
    {
        Vector3 center = target.transform.position;
        if (target.TryGetComponent<Renderer>(out Renderer renderer))
        {
            Debug.Log("Adding center offset");
            center = renderer.bounds.center;
        }
        return Cull(origin, center, target);
    }

    public static void UnCull(List<GameObject> culledObjects)
    {
        foreach (GameObject obj in culledObjects)
        {
            //obj.GetComponent<Renderer>().enabled = true;
            obj.SetActive(true);
        }
    }

    public static float GetTargetDistance(float fov, Bounds bounds, float ratio)
    {
        float size = (bounds.max - bounds.min).magnitude;
        float sine = Mathf.Sin(fov * Mathf.Deg2Rad);
        float distance = size / (sine * ratio);
        return distance;
    }
}
