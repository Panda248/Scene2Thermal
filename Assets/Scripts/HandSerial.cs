using UnityEngine;
using System.IO.Ports;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System;
public class HandSerial : MonoBehaviour
{
    public string portName = "COM5";
    public int baudRate = 9600;
    public string logPath = "serial_log.csv";
    public bool logSerialData = false;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (logSerialData)
        {
            Logger.OpenLogFile(logPath, "Timestamp,Temperature");
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
                    if (logSerialData)
                    {
                        Logger.AppendLog(logPath, $"{Time.time},{line}");
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
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
        if(logSerialData)
        {
            Logger.CloseLogFile(logPath);
        }
    }

    public void SendData()
    {
        SendDataBytes(
            palm?.GetData() ?? 20f, 
            thumb?.GetData() ?? 20f, 
            index?.GetData() ?? 20f, 
            middle?.GetData() ?? 20f, 
            ring?.GetData() ?? 20f, 
            little?.GetData() ?? 20f);
    }

    public void SendDataBytes(float palm, float thumb, float index, float middle, float ring, float little)
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

        float palm_clamped = Mathf.Clamp(palm, 15f, 30f);
        float thumb_clamped = Mathf.Clamp(thumb, 15f, 30f);
        float index_clamped = Mathf.Clamp(index, 15f, 30f);
        float middle_clamped = Mathf.Clamp(middle, 15f, 30f);
        float ring_clamped = Mathf.Clamp(ring, 15f, 30f);
        float little_clamped = Mathf.Clamp(little, 15f, 30f);

        short palm_temp = (short)(palm_clamped * 100);
        short thumb_temp = (short)(thumb_clamped * 100);
        short index_temp = (short)(index_clamped * 100);
        short middle_temp = (short)(middle_clamped * 100);
        short ring_temp = (short)(ring_clamped * 100);
        short little_temp = (short)(little_clamped * 100);

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
