using System.IO.Ports;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class HandSerial : MonoBehaviour
{
    public string portName = "COM5";
    public int baudRate = 9600;
    public string logPath = "serial_log.csv";
    public bool logSerialData = false;

    // Track the currently open log file uniquely
    private string activeLogFile;

    [SerializeField]
    HandThermObject palm;
    [SerializeField]
    HandThermObject thumb;
    [SerializeField]
    HandThermObject index;
    [SerializeField]
    HandThermObject middle;
    [SerializeField]
    HandThermObject ring;
    [SerializeField]
    HandThermObject little;

    SerialPort serialPort;
    byte[] data = new byte[12];  

    void OnEnable()
    {
        if (logSerialData)
        {
            // Store the timestamped log file name
            activeLogFile = Logger.OpenLogFile(logPath, "Timestamp,Temperature");
        }
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
        if(serialPort.IsOpen)
        {
            Debug.Log($"Serial port {portName} opened at {baudRate} baud.");
            StartCoroutine(MonitorSerial());
        }
        else
        {
            Debug.LogError($"Failed to open serial port {portName}.");
        }
    }

    IEnumerator MonitorSerial()
    {
        while (true)
        {
            if (serialPort != null && serialPort.IsOpen && serialPort.BytesToRead != 0)
            {
                string line = serialPort.ReadLine();
                if (line.Length > 0)
                {
                    Debug.Log(line);
                    if (logSerialData && !string.IsNullOrEmpty(activeLogFile))
                    {
                        Logger.AppendLog(activeLogFile, $"{Time.time},{line}");
                    }
                }
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //float time = Time.realtimeSinceStartup;

        SendData();

        //time = Time.realtimeSinceStartup - time;
        //Debug.Log($"Cost: {time}s");
    }

    private void OnDestroy()
    {
        CloseSerialAndLogs();
    }

    private void OnDisable()
    {
        CloseSerialAndLogs();
    }

    private void CloseSerialAndLogs()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
        if (logSerialData && !string.IsNullOrEmpty(activeLogFile))
        {
            Logger.CloseLogFile(activeLogFile);
            activeLogFile = null;
        }
    }

    public void SendData()
    {
        SendBytes(
            palm?.GetData() ?? 27f, 
            thumb?.GetData() ?? 27f, 
            index?.GetData() ?? 27f, 
            middle?.GetData() ?? 27f, 
            ring?.GetData() ?? 27f, 
            little?.GetData() ?? 27f);
    }

    public void SendBytes(float palm, float thumb, float index, float middle, float ring, float little)
    {
        if (serialPort == null || !serialPort.IsOpen)
        {
            //Debug.Log("serial port is not open");
            return;
        }

        for(int i = 0; i < data.Length; i++)
        {
            data[i] = 0;
        }

        short palm_temp = (short)(palm * 100);
        short thumb_temp = (short)(thumb * 100);
        short index_temp = (short)(index * 100);
        short middle_temp = (short)(middle * 100);
        short ring_temp = (short)(ring * 100);
        short little_temp = (short)(little * 100);

        //Debug.Log($"Sending temperatures: Palm={palm_temp}, Thumb={thumb_temp}, Index={index_temp}, Middle={middle_temp}, Ring={ring_temp}, Little={little_temp}");

        BitConverter.GetBytes(palm_temp).CopyTo(data, 0);
        BitConverter.GetBytes(thumb_temp).CopyTo(data, 2);
        BitConverter.GetBytes(index_temp).CopyTo(data, 4);
        BitConverter.GetBytes(middle_temp).CopyTo(data, 6);
        BitConverter.GetBytes(ring_temp).CopyTo(data, 8);
        BitConverter.GetBytes(little_temp).CopyTo(data, 10);

        serialPort.Write("S");
        serialPort.Write(data, 0, data.Length);
    }
}
