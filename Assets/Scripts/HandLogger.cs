using UnityEngine;

public class HandLogger : MonoBehaviour
{
    public HandThermObject target;
    public string logPath = "hand_log.csv";
    public void Start()
    {
        Logger.OpenLogFile(logPath, "Timestamp,Temperature, TemperatureDelta, MappedTemperature");
    }

    public void FixedUpdate()
    {
        if (target != null)
        {
            float temperature = target.temperature;
            float temperatureDelta = target.lastTemperatureDelta;
            float mappedTemperature = target.GetData();
            Logger.AppendLog(logPath, $"{Time.time},{temperature},{temperatureDelta},{mappedTemperature}");
        }
    }

    private void OnDestroy()
    {
        Logger.CloseLogFile(logPath);
    }
}
