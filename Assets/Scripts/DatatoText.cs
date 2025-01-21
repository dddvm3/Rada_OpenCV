using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatatoText : MonoBehaviour
{
    public ServerController SC;

    public Text IRData;
    public Text RadaData;

    private void Start()
    {
    }

    private void Update()
    {
        if (SC.receiveData.Contains("IR"))
        {
            IRData.text = SC.receiveData;
        }
        else if (SC.receiveData.Contains("Rada"))
        {
            RadaData.text = SC.receiveData;
        }
    }
}