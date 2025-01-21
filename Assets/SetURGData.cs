using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Urg;

public class SetURGData : MonoBehaviour
{
    public UrgSensor _urgSensor;
    public EthernetTransport _ethernetTransport;
    public GameObject meshJob_Update;
    public GameObject meshJob_Object;
    public DrawMesh_Job drawMeshUpdate;
    public DrawMesh_Job drawMeshObject;
    public Transform Sector;

    public string _IP_address;
    public float _Degrees;
    public Vector3 _Pos;
    public Vector3 _Rot;
    public Bounds _sensingArea;
    public float DistanceLimit;
    public float Offset;
    public float[] Unman_Distance;

    public void SetIPAddress(string IP_address)
    {
        _ethernetTransport.ipAddress = IP_address;
        _IP_address = _ethernetTransport.ipAddress;
    }

    public void SetDegrees(float Degrees)
    {
        _urgSensor.offsetDegrees = Degrees;
        _Degrees = _urgSensor.offsetDegrees;
    }

    public void SettransformPosition(Vector3 Pos)
    {
        transform.position = Pos;
        _Pos = Pos;
    }

    public void SettransformRotation(Vector3 Rot)
    {
        transform.rotation = Quaternion.Euler(Rot);
        _Rot = Rot;
    }

    public void SetBounds(Bounds bounds)
    {
        //drawMeshObject.transform.localPosition = bounds.center;
        drawMeshObject.sensingArea.extents = bounds.extents;
    }

    public void SetDistanceLimit(float value)
    {
        //drawMeshObject.distanceMinLimit = value;
        DistanceLimit = value;
    }

    public void SetOffset(float value)
    {
        Offset = value;
        drawMeshUpdate.Offset = value;
    }

    public void SetUnmanDis(List<float> data)
    {
        drawMeshUpdate.Unman_DistanceArray = data;
    }

    public void GetUnmanData(List<float> data)
    {
        drawMeshUpdate.GetUnmanDistance();
        //data = drawMeshUpdate.Unman_DistanceArray;
        //data = new List<float>(drawMeshUpdate.Unman_DistanceArray);
        data.Clear();
        for (int i = 0; i < drawMeshUpdate.DistanceArray.Length; i++)
        {
            data.Add(drawMeshUpdate.DistanceArray[i]);
        }
    }

    public void ClearUnmanData(List<float> data)
    {
        drawMeshUpdate.Unman_DistanceArray.Clear();
        drawMeshUpdate.Unman_DistanceArray = new List<float>(1081);
        data.Clear();
        data = new List<float>(1081);
    }

    public void ReConnect()
    {
        _urgSensor.Open();
    }

    public void DisConnect()
    {
        _urgSensor.Close();
    }

    public void SetUpdateMesh()
    {
        meshJob_Update.SetActive(!meshJob_Update.activeSelf);
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}