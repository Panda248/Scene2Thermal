using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger
{
    public static string logsPath = "Logs";
    static Dictionary<string, StreamWriter> logWriters = new();

    public static string OpenLogFile(string name, string header)
    {
        string directoryPath = Path.Combine(Application.dataPath, logsPath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name);
        string extension = Path.GetExtension(name);
        
        // Append a timestamp to make the filename unique and chronological
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string logFileName = $"{fileNameWithoutExtension}_{timestamp}{extension}";
        
        string logFilePath = Path.Combine(directoryPath, logFileName);

        File.WriteAllText(logFilePath, header + '\n');

        StreamWriter writer = new(logFilePath, true);
        logWriters[logFileName] = writer;

        return logFileName;
    }

    public static void AppendLog(string name, string logEntry)
    {
        if (logWriters.TryGetValue(name, out StreamWriter writer))
        {
            writer.WriteLine(logEntry);
        }
    }

    public static void CloseLogFile(string name)
    {
        if (logWriters.TryGetValue(name, out StreamWriter writer))
        {
            writer.Close();
            logWriters.Remove(name);
        }
    }
}
