using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using OscJack;

public class OSCSender : MonoBehaviour
{
    public OscClient OC;
    public string IPAddress;
    public int Port;
    public string address;

    public TestListener TL;

    public void SendData(Vector2 Pos, int ID)
    {
        OC.Send(address, Pos.x, Pos.y, ID);
        OscMaster.GetSharedServer(Port);
    }

    private void Start()
    {
        ReadJson();
        OC = new OscClient(IPAddress, Port);
    }

    private void Update()
    {
        GetRawObject();
    }

    private void GetRawObject()
    {
        //foreach (RawObject x in TL.RawData)
        //{
        //    SendData(x._Position, x._ID);
        //}
    }

    private void ReadJson()
    {
        string source;
        try
        {
            source = File.ReadAllText(Application.streamingAssetsPath + "/OSCConfig.txt");
        }
        finally
        {
        }

        IPAddress = JsonUtility.FromJson<OSCConfig>(source).IPAddress;
        Port = JsonUtility.FromJson<OSCConfig>(source).Port;
    }
}

public class OSCConfig
{
    public string IPAddress;
    public int Port;
}