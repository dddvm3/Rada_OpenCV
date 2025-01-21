using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColiderCheck : MonoBehaviour
{
    public SonicToutch Bird;
    public SonicToutch Dog;

    public UnityEvent PersonIn;
    public UnityEvent PersonOut;
    public int Check_ObjectNum;
    [SerializeField] private float InStayTime;
    [SerializeField] private bool One;
    [SerializeField] private bool isOn;

    [SerializeField] private List<GameObject> InSideObject;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            isOn = true;
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isOn = false;
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    private void Update()
    {
        if (isOn)
        {
            if (!One)
            {
                PersonIn.Invoke();
                Debug.Log("IN");
                One = true;
            }
        }
        else
        {
            if (One)
            {
                PersonOut.Invoke();
                One = false;
            }
        }
    }
}