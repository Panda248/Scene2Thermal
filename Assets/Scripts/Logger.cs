using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger
{
    public static string logsPath = "Logs";
    static Dictionary<string, StreamWriter> logWriters = new();
    public static void OpenLogFile(string name, string header)
    {
        string logFilePath = Path.Combine(Application.dataPath, logsPath, name);
        if (!File.Exists(logFilePath))
        {
            File.WriteAllText(logFilePath, header + '\n');
        }
        StreamWriter writer = new(logFilePath, true);
        logWriters[name] = writer;
    }

    public static void AppendLog(string name, string logEntry)
    {
        logWriters[name].WriteLine(logEntry);
    }

    public static void CloseLogFile(string name)
    {
        logWriters[name].Close();
        logWriters.Remove(name);
    }

}
