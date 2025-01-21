using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

//using System.Linq;

//引入庫
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public enum Mode : int
{
    TouchDown = 0,
    TouchUp = 1,
    TouchMove = 2
}

public class ClientController : MonoBehaviour
{
    [SerializeField] private int Port;

    [SerializeField] private string ClientPC1IPAddress;
    [SerializeField] private string ClientPC2IPAddress;
    [SerializeField] private string ClientPC3IPAddress;

    private IPEndPoint ipEndPointPC1;
    private UdpClient udpClient;
    private byte[] sendByte;

    private IPEndPoint ipEndPointSelf;

    [SerializeField] private pointerData pointerList;

    public void SendCheckPoint()
    {
        SendUDPData("CanSend", ipEndPointPC1);
    }

    public void PointertoJson(string sendsource)
    {
        try
        {
            //Debug.Log(sendsource);

            SendUDPData(sendsource + "#", ipEndPointPC1);
        }
        catch (Exception error)
        {
            Debug.LogError(error.ToString());
        }
    }

    private void Start()
    {
        ReadJson();

        ipEndPointPC1 = new IPEndPoint(IPAddress.Parse(ClientPC1IPAddress), Port);

        //ipEndPointSelf = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);

        udpClient = new UdpClient();
    }

    private void Update()
    {
    }

    private void SendUDPData(string tempData, IPEndPoint ipendpoint)
    {
        sendByte = System.Text.Encoding.UTF8.GetBytes(tempData);
        udpClient.Send(sendByte, sendByte.Length, ipendpoint);
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
        //Debug.Log(source);

        ClientPC1IPAddress = JsonUtility.FromJson<OSCSetting>(source).ClientPC1IPAddress;
        Port = JsonUtility.FromJson<OSCSetting>(source).Port;
    }
}

public class pointerData
{
    public string SensorType;
    public TUIOPointer[] pointerList;
}

[System.Serializable]
public class TUIOPointer
{
    public string data;
    public int Id;
    public Vector3 Position;

    //public Vector3 PrePosition;
    public Mode mode;

    public TUIOPointer(int Id, Vector3 position, Mode mode)
    {
        this.data = Id.ToString() + "_" + position.x.ToString("0.00") + "_" + position.y.ToString("0.00") + "_" + (int)mode;
        this.Id = Id % 100;
        this.Position = position;
        this.mode = mode;
    }
}