using UnityEngine;

public class SceneScanner : MonoBehaviour
{
    Rotator rotator;

    private void Awake()
    {
        rotator = GetComponent<Rotator>();
    }

    static SceneScanner instance;
    public static SceneScanner Instance()
    {
        if (instance == null)
        {
            instance = FindAnyObjectByType<SceneScanner>();
        }
        return instance;
    }
    
    public void Scan()
    {
        for (int i = 0; i < 4; i++)
        {
            // Scan

            // Rotation about Y Axis (Yaw)
            rotator.Rotate();
        }
    }
}
