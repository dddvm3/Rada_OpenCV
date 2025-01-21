using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public FollowObjectControl FOC;
    public Transform Feet_1;
    public Transform Feet_2;
    public Vector3 Feet_1Pos;
    public Vector3 Feet_2Pos;
    public FollowPoint Feet_1FP;
    public FollowPoint Feet_2FP;

    public string Feet_1Name;
    public string Feet_2Name;

    private Vector3 thisObjectPos;
    private float _centerX, _centerY, _centerZ;

    [SerializeField] private float StartTime;
    [SerializeField] private Vector3 lastPos;
    [SerializeField] private Vector3 NowPos;

    #region Follow

    public void FollowPos()
    {
        if (Feet_1 != null & Feet_2 != null)
        {
            if (Vector3.Distance(Feet_1.position, Feet_2.position) > 800)
            {
                GameObject.Destroy(this.gameObject);
            }
            else
            {
                _centerX = (Feet_1.position.x + Feet_2.position.x) / 2;
                _centerY = (Feet_1.position.y + Feet_2.position.y) / 2;
            }
        }
        else if (Feet_1 == null & Feet_2 == null)
        {
            GameObject.Destroy(this.gameObject, 2);
        }
        else
        {
            _centerX = Feet_1 ? Feet_1.position.x : Feet_2.position.x;
            _centerY = Feet_1 ? Feet_1.position.y : Feet_2.position.y;
        }

        transform.position = Vector3.Lerp(transform.position, new Vector3(_centerX, _centerY, 0), 0.1f);
        thisObjectPos = transform.position;
    }

    public void MissObject()
    {
        if (Feet_1 == null | Feet_2 == null)
        {
            //Debug.Log("12324");
            for (int i = 0; i < FOC.FollowPointArray.Length; i++)
            {
                if (FOC.FollowPointArray[i].GetComponent<FollowPoint>().haveObject == false)
                {
                    if (Feet_1 == null)
                    {
                        if (Vector3.Distance(Feet_2Pos, FOC.FollowPointArray[i].transform.position) > 100 & Vector3.Distance(Feet_2Pos, FOC.FollowPointArray[i].transform.position) < 500)
                        {
                            if (Feet_1 == null)
                            {
                                Feet_1 = FOC.FollowPointArray[i].transform;
                                FOC._FollowPointArray.Add(Feet_1.name);
                                Feet_1Name = Feet_1.name;
                                Feet_1FP = Feet_1.GetComponent<FollowPoint>();
                                Feet_1FP.haveMaster();
                            }
                        }
                    }

                    if (Feet_2 == null)
                    {
                        if (Vector3.Distance(Feet_1Pos, FOC.FollowPointArray[i].transform.position) > 100 & Vector3.Distance(Feet_1Pos, FOC.FollowPointArray[i].transform.position) < 500)
                        {
                            if (Feet_2 == null)
                            {
                                Feet_2 = FOC.FollowPointArray[i].transform;
                                FOC._FollowPointArray.Add(Feet_2.name);
                                Feet_2Name = Feet_2.name;
                                Feet_2FP = Feet_2.GetComponent<FollowPoint>();
                                Feet_2FP.haveMaster();
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Feet_1Pos = Feet_1.position;
            Feet_2Pos = Feet_2.position;
        }
    }

    #endregion Follow

    private void ResTime()
    {
        lastPos = NowPos;
        NowPos = transform.position;

        //Debug.Log(lastPos - NowPos);
    }

    private void OnEnable()
    {
        StartTime = Time.time;
    }

    private void Start()
    {
    }

    private void Update()
    {
        FollowPos();
        MissObject();
        ResTime();
    }

    private void OnDestroy()
    {
        if (Feet_1FP != null) { Feet_1FP.haveMaster(); }
        if (Feet_2FP != null) { Feet_2FP.haveMaster(); }
        FOC._FollowPointArray.Remove(Feet_1Name);
        FOC._FollowPointArray.Remove(Feet_2Name);
    }
}