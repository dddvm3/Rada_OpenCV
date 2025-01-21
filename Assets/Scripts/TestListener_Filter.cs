using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestListener_Filter : MonoBehaviour
{
    public SurfaceInputs SI;

    public List<TUIOPointer> pointerList;

    public List<ObjectData> ObjectList = new List<ObjectData>();

    public int CCVHeight, CCVWidth;
    public int _Height, _Width;
    [SerializeField] private float Remake;
    public bool show;

    //public void CreateObject(RawObject rawobject)
    //{
    //    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    //go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    //    go.AddComponent<ObjectData>();
    //    //go.GetComponent<ObjectData>()._Point = new RawObject(rawobject._Position, rawobject._ID, rawobject._listIndex,);
    //    go.GetComponent<ObjectData>().InvertY = true;
    //    //go.GetComponent<ObjectData>().TL = this;

    //    ObjectList.Add(go.GetComponent<ObjectData>());
    //}

    public Vector3 ScreentoWorldPoint(Vector3 pos)
    {
        float xPos = pos.x;
        float yPos = Mathf.InverseLerp(0, Remake, pos.y);

        Vector3 newPos = new Vector3(xPos, (1 - yPos), 10);

        return Camera.main.ViewportToWorldPoint(newPos);
    }

    private void Start()
    {
        SI.OnTouch += OnTouchReceive;
        float widthRemake = _Width / CCVWidth;
        float heightRemake = _Height / CCVHeight;
        Remake = widthRemake / heightRemake;
    }

    private void OnTouchReceive(Dictionary<int, FingerInput> surfaceFingers, Dictionary<int, ObjectInput> surfaceObjects)
    {
        Debug.ClearDeveloperConsole();
        pointerList = new List<TUIOPointer>();

        if (surfaceFingers.Count > 0)
        {
            for (int i = 0; i < surfaceFingers.Count; i++)
            {
                TUIOPointer pointer = new TUIOPointer(surfaceFingers[i].id, surfaceFingers[i].position, surfaceFingers[i].mode);

                pointerList.Add(pointer);
            }

            //Debug.Log(surfaceFingers.Count + " fingers:");
            foreach (KeyValuePair<int, FingerInput> entry in surfaceFingers)
            {
                //Debug.Log(entry.Key+"ID"+ entry.Value.id + " @ " + entry.Value.position.x + ";" + entry.Value.position.y);
                //Vector3 Pos = ScreentoWorldPoint(entry.Value.position);

                //TUIOPointer pointer = new TUIOPointer(entry.Value.id, entry.Value.position, entry.Value.mode);

                //pointerList.Add(pointer);

                //if (surfaceObjects.Count > 0)
                //{
                //    Debug.Log(surfaceObjects.Count + " objects:");
                //    foreach (KeyValuePair<int, ObjectInput> entry in surfaceObjects)
                //    {
                //        Debug.Log(entry.Key + ", tag: " + entry.Value.tagValue + " @ " + entry.Value.position.x + ";" + entry.Value.position.y);
                //    }
                //}
            }
        }
        else if (surfaceFingers.Count == 0 & pointerList.Count != 0)
        {
            pointerList = new List<TUIOPointer>();
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F12))
        //{
        //    show = !show;
        //}
    }
}