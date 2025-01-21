using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectControl : MonoBehaviour
{
    public GameObject Origintrail;

    public GameObject[] FollowPointArray;
    public List<string> _FollowPointArray;

    public float OnePerson;

    public int DetectObjectCount;
    [SerializeField] private int m_DetectObjectCount;

    public void PersonPos()
    {
        if (m_DetectObjectCount < FollowPointArray.Length)
        {
            for (int i = 0; i < FollowPointArray.Length; i++)
            {
                for (int j = 0; j < FollowPointArray.Length; j++)
                {
                    if (!_FollowPointArray.Contains(FollowPointArray[i].name))
                    {
                        if (!_FollowPointArray.Contains(FollowPointArray[j].name))
                        {
                            if (i != j)
                            {
                                if (Vector3.Distance(FollowPointArray[i].transform.position, FollowPointArray[j].transform.position) < OnePerson)
                                {
                                    //Debug.Log(Vector3.Distance(FollowPointArray[i].transform.position, FollowPointArray[j].transform.position));
                                    _FollowPointArray.Add(FollowPointArray[i].name);
                                    _FollowPointArray.Add(FollowPointArray[j].name);
                                    FollowPointArray[i].GetComponent<FollowPoint>().FOC = this;
                                    FollowPointArray[j].GetComponent<FollowPoint>().FOC = this;

                                    float x = (FollowPointArray[i].transform.position.x + FollowPointArray[j].transform.position.x) / 2;
                                    float y = (FollowPointArray[i].transform.position.y + FollowPointArray[j].transform.position.y) / 2;
                                    float z = (FollowPointArray[i].transform.position.z + FollowPointArray[j].transform.position.z) / 2;

                                    var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    go.transform.position = new Vector3(x, y, z);
                                    go.transform.localScale = Vector3.one * 200;
                                    go.tag = "FollowObject";
                                    go.GetComponent<SphereCollider>().isTrigger = true;
                                    go.AddComponent<MyLine>();
                                    go.AddComponent<FollowObject>();
                                    go.GetComponent<FollowObject>().Feet_1 = FollowPointArray[i].transform;
                                    go.GetComponent<FollowObject>().Feet_2 = FollowPointArray[j].transform;
                                    go.GetComponent<FollowObject>().Feet_1FP = FollowPointArray[i].GetComponent<FollowPoint>();
                                    go.GetComponent<FollowObject>().Feet_2FP = FollowPointArray[j].GetComponent<FollowPoint>();
                                    go.GetComponent<FollowObject>().Feet_2Name = FollowPointArray[i].name;
                                    go.GetComponent<FollowObject>().Feet_2Name = FollowPointArray[j].name;
                                    go.GetComponent<FollowObject>().FOC = this;
                                    FollowPointArray[i].GetComponent<FollowPoint>().haveMaster();
                                    FollowPointArray[j].GetComponent<FollowPoint>().haveMaster();
                                }
                            }
                        }
                    }
                }
                m_DetectObjectCount += 1;
            }
        }
        else if (FollowPointArray.Length != m_DetectObjectCount)
        {
            m_DetectObjectCount = 0;
        }
    }

    private void GetFollowObject()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("FollowPoint");
        FollowPointArray = go;
    }

    private void Start()
    {
    }

    private void Update()
    {
        GetFollowObject();
        PersonPos();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            _FollowPointArray.Clear();
        }
    }
}