using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLine : MonoBehaviour
{
    public GameObject _myLine;
    public int NowWhen;
    public int ControlNum;
    public Draw _draw;
    public float touchTime;
    public float DrawingTime = 30;
    [SerializeField] private LineRenderer LR;

    public void SwitchLR()
    {
        LR.enabled = !LR.enabled;
    }

    public void LRRest()
    {
        Debug.Log("LR = null");
        //Destroy(LR.gameObject);
        _myLine = null;
        LR = null;
    }

    public void CloseDraw()
    {
        _draw.FinishLine();
        _draw.enabled = false;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (LR == null)
        {
            if (_myLine != null)
            {
                LR = _myLine.GetComponent<LineRenderer>();
                _draw = LR.GetComponent<Draw>();
            }
        }
        //if (touchTime != 0)
        //{
        //    string time = (DrawingTime - (Time.time - touchTime)).ToString();
        //}
    }
}