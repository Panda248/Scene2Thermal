using UnityEngine;

public class HandLogger : MonoBehaviour
{
    public HandThermObject target;
    public string logPath = "hand_log.csv";
    
    // Store the handle to prevent modifying the public logPath field
    private string activeLogFile;

    public void Start()
    {
        activeLogFile = Logger.OpenLogFile(logPath, "Timestamp,Temperature, TemperatureDelta, MappedTemperature");
    }

    public void FixedUpdate()
    {
        if (target != null && !string.IsNullOrEmpty(activeLogFile))
        {
            float temperature = target.temperature;
            float temperatureDelta = target.lastTemperatureDelta / Time.fixedDeltaTime;
            float mappedTemperature = target.GetData();
            Logger.AppendLog(activeLogFile, $"{Time.time},{temperature},{temperatureDelta},{mappedTemperature}");
        }
    }

    private void OnDestroy()
    {
        if (!string.IsNullOrEmpty(activeLogFile))
        {
            Logger.CloseLogFile(activeLogFile);
            activeLogFile = null;
        }
    }
}
