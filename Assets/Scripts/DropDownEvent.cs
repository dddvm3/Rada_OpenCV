using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DropDownEvent : MonoBehaviour, IPointerClickHandler, ICancelHandler
{
    public UnityEvent _OnPointClick;

    public UnityEvent _Deselect;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        _OnPointClick.Invoke();
        //Debug.Log("OnSelect");
    }

    public void OnCancel(BaseEventData data)
    {
        Dropdown dropdown = GetComponent<Dropdown>();
        _Deselect.Invoke();
        if (dropdown)
        {
            Debug.Log("OnCancel");
        }
    }
}