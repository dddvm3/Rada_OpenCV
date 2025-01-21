using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

// Open Sound Control for .NET
// Copyright (c) 2006, Yoshinori Kawasaki
using OSC.NET;

public class SurfaceInputs : MonoBehaviour
{
    //private SurfaceInputs _instance;

    private Thread listenerThread;
    private UdpClient client;
    private IPEndPoint remoteEndpoint;
    private Dictionary<int, FingerInput> surfaceFingers;
    private Dictionary<int, ObjectInput> surfaceObjects;
    private Queue<OSCBundle> packetQueue = new Queue<OSCBundle>();
    private object queueLock = new object();

    public int Port;

    public delegate void TouchReceiveHandler(
        Dictionary<int, FingerInput> surfaceFingers,
        Dictionary<int, ObjectInput> surfaceObjects);

    public event TouchReceiveHandler OnTouch;

    //public static SurfaceInputs Instance
    //{ get { return _instance; } }

    public IEnumerator DelayMethod(float waittime, Action action)
    {
        yield return new WaitForSeconds(waittime);
        action();
    }

    private void Awake()
    {
        //if (_instance != null && _instance != this)
        //{
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    _instance = this;
        //}
        //ReadJson();
    }

    private void Start()
    {
        surfaceFingers = new Dictionary<int, FingerInput>();
        surfaceObjects = new Dictionary<int, ObjectInput>();

        // We will be listening to the Surface on localhost
        remoteEndpoint = new IPEndPoint(IPAddress.Any, Port);
        client = new UdpClient(remoteEndpoint);

        // Run the listener on a separate thread...
        ThreadStart threadStarter = new ThreadStart(Listen);
        listenerThread = new Thread(threadStarter);
        listenerThread.IsBackground = true;
        listenerThread.Start();
    }

    // A few comments on the TUIO protocol:
    // Whether it is a finger or object input depends on the address ("/tuio/2Dcur" or "/tuio/2Dobj" respectively)
    // Information is carried in two types of messages: "alive" and "set".
    // "alive" block lists the touch points that are currently active, there are no explicit messages
    // for touch point removal. This is due to the messages being sent via UDP, so technically unreliable.
    // We are communicating over localhost, so could in fact use pipes and introduce a custom protocol,
    // but not sure if it's worth rewriting now. The amount of touch points at a time is relatively small
    // to cause any data processing bottleneck, so probably no.

    private void Listen()
    {
        Debug.Log("UDP Listener started...");
        while (true)
        {
            try
            {
                byte[] receivedBytes = client.Receive(ref remoteEndpoint);

                if (receivedBytes.Length > 0)
                {
                    lock (queueLock)
                    {
                        OSCBundle packet = (OSCBundle)OSCPacket.Unpack(receivedBytes);

                        packetQueue.Enqueue(packet);
                    }
                }
            }
            catch (Exception error)
            {
                Debug.LogError(error.ToString());
            }
        }
    }

    private void LogState()
    {
        Debug.ClearDeveloperConsole();
        if (surfaceFingers.Count > 0)
        {
            Debug.Log(surfaceFingers.Count + " fingers:");
            foreach (KeyValuePair<int, FingerInput> entry in surfaceFingers)
            {
                Debug.Log(entry.Key + " @ " + entry.Value.position.ToString());
            }
        }

        if (surfaceObjects.Count > 0)
        {
            Debug.Log(surfaceObjects.Count + " objects:");
            foreach (KeyValuePair<int, ObjectInput> entry in surfaceObjects)
            {
                Debug.Log(entry.Key + ", tag: " + entry.Value.tagValue + " @ " + entry.Value.position.ToString());
            }
        }
    }

    private void ProcessCursorMessage(OSCMessage msg)
    {
        string msgType = msg.Values[0].ToString(); //   source / alive / set / fseq

        switch (msgType)
        {
            case "alive":
                {
                    List<int> ids = new List<int>(surfaceFingers.Keys);
                    FingerInput surfaceFinger;
                    foreach (int id in ids)
                    {
                        if (!msg.Values.Contains(id))
                        {
                            if (surfaceFingers.TryGetValue(id, out surfaceFinger))
                            {
                                surfaceFinger.UpdateProps(Mode.TouchUp);
                            }
                            StartCoroutine(DelayMethod(0.1f, () => surfaceFingers.Remove(id)));
                        }
                    }
                    break;
                }
            case "set":
                {
                    int id = (int)msg.Values[1];

                    float x = (float)msg.Values[2];
                    float y = (float)msg.Values[3];
                    Vector2 position = new Vector2(x, y);

                    float xVel = (float)msg.Values[4];
                    float yVel = (float)msg.Values[5];
                    Vector2 velocity = new Vector2(xVel, yVel);

                    float acc = (float)msg.Values[6];

                    FingerInput surfaceFinger;
                    if (surfaceFingers.TryGetValue(id, out surfaceFinger))
                    {
                        surfaceFinger.UpdateProps(position, velocity, acc, Mode.TouchMove);
                    }
                    else
                    {
                        surfaceFinger = new FingerInput(id, position, velocity, acc, Mode.TouchDown);

                        surfaceFingers.Add(id, surfaceFinger);
                    }
                    break;
                }
        }
    }

    private void ProcessObjectMessage(OSCMessage msg)
    {
        string msgType = msg.Values[0].ToString(); //   source / alive / set / fseq

        switch (msgType)
        {
            case "alive":
                {
                    List<int> ids = new List<int>(surfaceObjects.Keys);
                    foreach (int id in ids)
                    {
                        if (!msg.Values.Contains(id))
                        {
                            surfaceObjects.Remove(id);
                        }
                    }
                    break;
                }
            case "set":
                {
                    int id = (int)msg.Values[1];
                    int tagValue = (int)msg.Values[2];

                    float x = (float)msg.Values[3];
                    float y = (float)msg.Values[4];
                    Vector2 position = new Vector2(x, y);

                    float orientation = (float)msg.Values[5];

                    float xVel = (float)msg.Values[6];
                    float yVel = (float)msg.Values[7];
                    Vector2 velocity = new Vector2(xVel, yVel);

                    float angularVel = (float)msg.Values[8];
                    float acc = (float)msg.Values[9];
                    float angularAcc = (float)msg.Values[10];

                    ObjectInput surfaceObject;
                    if (surfaceObjects.TryGetValue(id, out surfaceObject))
                    {
                        surfaceObject.UpdateProps(position, orientation, velocity, acc, angularVel, angularAcc);
                    }
                    else
                    {
                        surfaceObject = new ObjectInput(id, tagValue, position, orientation, velocity, acc, angularVel, angularAcc);
                        surfaceObjects.Add(id, surfaceObject);
                    }
                    break;
                }
        }
    }

    private void OnApplicationQuit()
    {
        listenerThread.Abort();
        client.Close();
    }

    // Update is called once per frame
    private void Update()
    {
        if (packetQueue.Count > 0)
        {
            lock (packetQueue)
            {
                if (packetQueue.Count > 0)
                {
                    foreach (OSCBundle packet in packetQueue)
                    {
                        foreach (OSCMessage msg in packet.Values)
                        {
                            if (msg.Address.Equals("/tuio/2Dobj"))
                            {
                                ProcessObjectMessage(msg);
                            }
                            else if (msg.Address.Equals("/tuio/2Dcur"))
                            {
                                ProcessCursorMessage(msg);
                            }
                            // there's also /tuio/2Dblb
                            // but we don't really need it
                        }
                    }
                }
            }
            OnTouch(surfaceFingers, surfaceObjects);
            // LogState();
        }
    }

    private void ReadJson()
    {
        string source;

        try
        {
            source = File.ReadAllText(Application.streamingAssetsPath + "/IRUDPPort.txt");
        }
        finally
        {
        }
        Debug.Log(source);
        Port = JsonUtility.FromJson<IRUDPPort>(source).Port;
    }
}

public class IRUDPPort
{
    public int Port;
}