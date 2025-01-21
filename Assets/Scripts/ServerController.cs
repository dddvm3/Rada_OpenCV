using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//引入庫
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ServerController : MonoBehaviour
{
    [SerializeField] private string ClientPC1IPAddress;
    [SerializeField] private int Port;

    private IPEndPoint ipEndPoint;
    private UdpClient udpClient;
    private Thread receiveThread;
    private byte[] receiveByte;
    public string receiveData = "";

    public void GetUDPData()
    {
        List<string> source = new List<string>();
    }

    private void Start()
    {
        ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
        udpClient = new UdpClient(ipEndPoint.Port);

        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void Update()
    {
    }

    private void ReceiveData()
    {
        while (true)
        {
            receiveByte = udpClient.Receive(ref ipEndPoint);
            receiveData = System.Text.Encoding.UTF8.GetString(receiveByte);

            if (receiveData.Contains("IR"))
            {
            }

            //SC.UDP_string = receiveData;

            Debug.Log("接收到：" + receiveData);
        }
    }

    private void OnDisable()
    {
        udpClient.Close();
        receiveThread.Join();
        receiveThread.Abort();
    }

    private void OnApplicationQuit()
    {
        receiveThread.Abort();
    }

    private void ReadJson()
    {
        string source;
        try
        {
            source = File.ReadAllText(Application.streamingAssetsPath + "/config/OSCSetting.txt");
        }
        finally
        {
        }
        Debug.Log(source);

        Port = JsonUtility.FromJson<OSCSetting>(source).Port;
    }
}

public class OSCSetting
{
    public string SensorType;

    public string ClientPC1IPAddress;
    public string ClientPC2IPAddress;
    public string ClientPC3IPAddress;

    public int Port;
}