using UnityEngine;
using System.IO.Ports;
using System.Text;
using System.Collections;

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
    HandThermObject pinky;
    
    SerialPort serialPort;
    byte[] data = new byte[6];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();

        StartCoroutine(MonitorSerial());
    }

    IEnumerator MonitorSerial()
    {
        while (true)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                string line = serialPort.ReadLine();
                Debug.Log(line);
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
     * PXX.XX TXX.XX IXX.XX MXX.XX RXX.XX EXX.XX\n
     * Palm   Thumb  Index  Middle Ring   Pinky
     */
    public void SendData()
    {
        if(serialPort == null || !serialPort.IsOpen)
        {
            Debug.Log("serial port is not open");
            return;
        }

        

        //dataBuilder.AppendFormat("P{0:00.00} ", palm.temperature);
        //dataBuilder.AppendFormat("T{0:00.00} ", thumb.temperature);
        //dataBuilder.AppendFormat("I{0:00.00} ", index.temperature);
        //dataBuilder.AppendFormat("M{0:00.00} ", middle.temperature);
        //dataBuilder.AppendFormat("R{0:00.00} ", ring.temperature);
        //dataBuilder.AppendFormat("E{0:00.00}\n", pinky.temperature);

        //serialPort.Write(data, 0, data.Length);
        serialPort.WriteLine($"30.00");
    }
}
