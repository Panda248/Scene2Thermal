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

    List<HandThermObject> handThermObjects;
    SerialPort serialPort;
    StringBuilder dataBuilder;
    byte[] data = new byte[12];  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        handThermObjects = new List<HandThermObject> { palm, thumb, index, middle, ring, little };

        dataBuilder = new StringBuilder();

        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
        //serialPort.write
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
                }
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //float time = Time.realtimeSinceStartup;

        SendDataBytes();

        //time = Time.realtimeSinceStartup - time;
        //Debug.Log($"Cost: {time}s");
    }

    private void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    /*
     * Format for sending temperature data:
     * PXX.XX TXX.XX IXX.XX MXX.XX RXX.XX LXX.XX\n
     * Palm   Thumb  Index  Middle Ring   Pinky/Little
     */
    public void SendDataString()
    {
        if(serialPort == null || !serialPort.IsOpen)
        {
            //Debug.Log("serial port is not open");
            return;
        }
        dataBuilder.Clear();

        dataBuilder.AppendFormat("P{0:00.00} ", palm.temperature);
        dataBuilder.AppendFormat("T{0:00.00} ", thumb.temperature);
        dataBuilder.AppendFormat("I{0:00.00} ", index.temperature);
        dataBuilder.AppendFormat("M{0:00.00} ", middle.temperature);
        dataBuilder.AppendFormat("R{0:00.00} ", ring.temperature);
        dataBuilder.AppendFormat("L{0:00.00}\n", little.temperature);

        serialPort.Write(dataBuilder.ToString());

        //serialPort.WriteLine($"{sendTemperature:00.00}");
    }

    public void SendDataBytes()
    {
        if (serialPort == null || !serialPort.IsOpen)
        {
            //Debug.Log("serial port is not open");
            return;
        }
        short palm_temp = (short)(palm.temperature * 100);
        short thumb_temp = (short)(thumb.temperature * 100);
        short index_temp = (short)(index.temperature * 100);
        short middle_temp = (short)(middle.temperature * 100);
        short ring_temp = (short)(ring.temperature * 100);
        short little_temp = (short)(little.temperature * 100);

        Debug.Log($"Sending temperatures: Palm={palm_temp}, Thumb={thumb_temp}, Index={index_temp}, Middle={middle_temp}, Ring={ring_temp}, Little={little_temp}");

        BitConverter.GetBytes(palm_temp).CopyTo(data, 0);
        BitConverter.GetBytes(thumb_temp).CopyTo(data, 2);
        BitConverter.GetBytes(index_temp).CopyTo(data, 4);
        BitConverter.GetBytes(middle_temp).CopyTo(data, 6);
        BitConverter.GetBytes(ring_temp).CopyTo(data, 8);
        BitConverter.GetBytes(little_temp).CopyTo(data, 10);
        byte[] byteArray = new byte[2];
        BitConverter.GetBytes(palm_temp).CopyTo(byteArray, 0);
        short test = BitConverter.ToInt16(byteArray, 0);
        //Debug.Log($"Test conversion: {test}");
        serialPort.Write("S");
        serialPort.Write(data, 0, data.Length);
    }
}
