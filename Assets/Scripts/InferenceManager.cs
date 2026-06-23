using UnityEngine;

public class InferenceManager : MonoBehaviour
{
    bool runStartup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(runStartup)
        {
            RunStartup();
        }
    }

    public void RunStartup()
    {

    }
}
