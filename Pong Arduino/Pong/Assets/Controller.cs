using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class Controller : MonoBehaviour
{
    public GameObject playerOne;
    public GameObject playerTwo;
    public bool controllerActive = false;
    public int commPort = 0;

    private SerialPort serial = null;
    private bool connected = false;
    // Start is called before the first frame update
    void Start()
    {
        ConnectToSerial();
    }

    // Update is called once per frame
    void ConnectToSerial()
    {
        Debug.Log("Attempting Serial: " + commPort);
        serial = new SerialPort("\\\\.\\COM" + commPort, 9600);
        serial.ReadTimeout = 50;
        serial.Open();
    }

    void Update()
    {
        if (controllerActive)
        {
            WriteToArduino("I");
            String value = ReadFromArduino(50);

            if (value != null)
            {
                Debug.Log(value);
                string[] values = value.Split('-');
                if (values.Length == 2)
                {
                    positionPlayers(values);
                }
            }

        }
    }

    void positionPlayers(String[] values)
    {
        if (playerOne != null)
        {
            float yPos = Remap(int.Parse(values[0]), 0, 1023, 0, 10);
            Vector3 newPosition = new Vector3(playerOne.transform.position.x, yPos, playerOne.transform.position.z);
            playerOne.transform.position = newPosition;
        }

        if (playerTwo != null)
        {
            float yPos = Remap(int.Parse(values[0]), 0, 1023, 0, 10);
            Vector3 newPosition = new Vector3(playerTwo.transform.position.x, yPos, playerTwo.transform.position.z);
            playerTwo.transform.position = newPosition;
        }
    }
    void WriteToArduino(string message)
    {
        serial.WriteLine(message);
        serial.BaseStream.Flush();
    }
    public string ReadFromArduino(int timeout = 0)
    {
        serial.ReadTimeout = timeout;
        try
        {
            return serial.ReadLine();
        }
        catch (TimeoutException e)
        {
            return null;
        }
    }
    void OnDestroy()
    {
        Debug.Log("Exiting");
        serial.Close();
    }
    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

