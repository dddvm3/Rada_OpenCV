using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendPos : MonoBehaviour
{
    public ClientController CC;

    //public string sensorType;
    public TestListener_Filter Rada_TL;

    public TestListener IR_TL;
    public string Rada_sendsource;
    public string IR_sendsource;

    public Text IR_UDPData;
    public Text Rada_UDPData;

    private void Start()
    {
    }

    private void Update()
    {
        sendsourceToJson();
    }

    private void sendsourceToJson()
    {
        pointerData Rada_PD = new pointerData();
        Rada_PD.SensorType = "Rada";
        Rada_PD.pointerList = new TUIOPointer[Rada_TL.pointerList.Count];
        Rada_TL.pointerList.CopyTo(Rada_PD.pointerList);

        Rada_sendsource = JsonUtility.ToJson(Rada_PD);

        CC.PointertoJson(Rada_sendsource);
        Rada_UDPData.text = Rada_sendsource;

        pointerData IR_PD = new pointerData();
        IR_PD.SensorType = "IR";
        IR_PD.pointerList = new TUIOPointer[IR_TL.pointerList.Count];
        IR_TL.pointerList.CopyTo(IR_PD.pointerList);

        IR_sendsource = JsonUtility.ToJson(IR_PD);

        CC.PointertoJson(IR_sendsource);
        IR_UDPData.text = IR_sendsource;
    }
}