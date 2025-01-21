using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    public TestListener TL;
    public RawObject _Point;

    public int CCVHeight = 320, CCVWidth = 240;
    public int _Height = 1920, _Width = 1200;

    public bool InvertX = false;
    public bool InvertY = false;
    public float therhlod;

    public Vector3 ScreentoWorldPoint(Vector3 pos)
    {
        float xPos = pos.x;
        float yPos = pos.y;

        float Remake = (_Width / CCVWidth) / (_Height / CCVHeight);

        if (this.InvertX) xPos = 1 - xPos;
        if (this.InvertY) yPos = 1 - yPos;

        Vector3 newPos = new Vector3(xPos, yPos * 1.14f * Remake, 10);
        return Camera.main.ViewportToWorldPoint(newPos);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(ScreentoWorldPoint(_Point._Position), new Vector3(1, 1, 1));
    }

    private void OnEnable()
    {
    }

    private void Update()
    {
        Debug.Log((_Width / CCVWidth) / (_Height / CCVHeight));
        //if (TL.RawData.Count > _Point._listIndex)
        //{
        //    _Point._Position = TL.ObjectList[_Point._listIndex]._Point._Position;
        //}

        //if (Vector3.Distance(transform.localPosition, ScreentoWorldPoint(_Point._Position)) > therhlod)
        //{
        //    transform.localPosition = ScreentoWorldPoint(_Point._Position);
        //}
    }
}