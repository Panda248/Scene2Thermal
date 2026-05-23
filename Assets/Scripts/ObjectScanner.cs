using UnityEngine;

public class ObjectScanner: MonoBehaviour
{
    public GameObject scanObject;
    private Rotator hRotator, vRotator;

    static ObjectScanner instance;
    public static ObjectScanner  Instance()
    {
        if (instance == null)
        {
            instance = FindAnyObjectByType<ObjectScanner>();
        }
        return instance;
    }

    public void Awake()
    {
        hRotator = transform.GetChild(0).GetComponent<Rotator>();
        vRotator = hRotator.transform.GetChild(0).GetComponent<Rotator>();
    }

    /// <summary>
    /// Performs a scan on the scanObject, taking both isolated and context pictures of the <br></br>
    /// object from 8 angles
    /// </summary>
    public void Scan()
    {
        for (int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                // Capture Context and Iso Image

                // Rotation about X Axis (Pitch)
                vRotator.Rotate();
            }
            // Rotation about Y Axis (Yaw)
            hRotator.Rotate();
        }
    }
}
