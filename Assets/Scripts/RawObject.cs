using UnityEngine;

[System.Serializable]
public class RawObject
{
    public int _ID;
    public int _listIndex;
    public Vector2 _Position;
    public Vector3 _WorldPostion;

    public int ObjectCreateID;

    public bool TimeOut;

    public RawObject(Vector2 pos, int id, int listIndex, Vector3 worldPos)
    {
        _Position = pos;
        _ID = id;
        _listIndex = listIndex;
        _WorldPostion = worldPos;
    }

    public void Update(Vector2 pos, int newid)
    {
        _Position = pos;
        _ID = newid;
    }
}