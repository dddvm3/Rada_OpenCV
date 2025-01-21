using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[System.Serializable]
public class RaydaPrefab
{
    public string Name;
    public string IPAddress;
    public float degress;
    public Vector3 Position;
    public Vector3 Rotation;
    public Bounds SensingArea;
    public List<float> urgDistance;
}

public class Setting : MonoBehaviour
{
    public GameObject mainCamObject;
    public Camera mainCam;
    public GameObject SettingCanvas;

    public Urg.UrgSensor US;
    public GameObject URG;

    public Camera RayDaCamera;

    [Header("Orthographic Camera Setting")]
    public Vector3 _CameraPosition;

    public float _CameraSize;

    [Header("SelectRayDa")]
    public int _SelectRayDaNum;

    public int RayDaCount;
    public string _RayDaIPAddress;
    public float _RayDaDegrees;
    public Vector3 _RayDaPosition;
    public Vector3 _RayDaRotation;
    public Bounds _RayDaSensingArea;
    public Vector3 Center;
    public Vector3 Extents;
    public float DistanceLimit;
    public float Offset;

    public List<RaydaPrefab> _RayDaList;
    public List<SetURGData> URGDataList;
    public List<GameObject> RayDaObject;

    [Header("UIObject")]
    public Dropdown _RaydaListDropdown;

    public InputField _RaydaIPAddress;
    public InputField _RaydaDegrees;
    public InputField _RaydaPosition_X, _RaydaPosition_Y, _RaydaPosition_Z;
    public InputField _RaydaRotation_X, _RaydaRotation_Y, _RaydaRotation_Z;
    public InputField _RaydaBoundsCenter_X, _RaydaBoundsCenter_Y, _RaydaBoundsCenter_Z;
    public InputField _RaydaBoundsExtents_X, _RaydaBoundsExtents_Y, _RaydaBoundsExtents_Z;
    public InputField _CameraPosition_X, _CameraPosition_Y, _CameraPosition_Z;
    public InputField _Camera_Size;
    public GameObject RT;
    public GameObject RaydaRT;

    public Text text_RayDaDistanceLimit;
    public Text text_RayDaOffset;
    public Slider _RaydaDistanceLimit;
    public Slider _RaydaOffset;

    public List<RawImage> URGrawImage;
    public List<Text> URGNameText;
    public GameObject NDIURG_rawImageList;

    public bool selectOption;

    private ShowObject SO;

    [SerializeField] private bool OneRayda;

    #region 雷達設定

    public string CameraPosition_x
    {
        set
        {
            //if (!selectOption)
            //{
            float.TryParse(value, out _CameraPosition.x);
            SetCamPos();
            //}
        }
    }

    public string CameraPosition_y
    {
        set
        {
            //if (!selectOption)
            //{
            float.TryParse(value, out _CameraPosition.y);
            SetCamPos();
            //}
        }
    }

    public string CameraPosition_z
    {
        set
        {
            //if (!selectOption)
            //{
            float.TryParse(value, out _CameraPosition.z);
            SetCamPos();
            //}
        }
    }

    public string CameraSize
    {
        set
        {
            //if (!selectOption)
            //{
            float.TryParse(value, out _CameraSize);
            SetCamSize();
            //}
        }
    }

    public int SelectRayDa
    {
        set
        {
            //URGDataList[_SelectRayDaNum].drawMeshObject.SetMaterialColor(Color.white);
            URGDataList[_SelectRayDaNum].drawMeshUpdate.SetUpdateMaterialColor(Color.black);
            _SelectRayDaNum = value;
            SelectRayda();

            //if (!selectOption)
            //{
            //
            //}
        }
    }

    public string RayDaIPAddress
    {
        set
        {
            //if (!selectOption)
            //{
            _RayDaIPAddress = value;
            SetIP_Addes();
            //}
        }
    }

    public string RayDaDegrees
    {
        set
        {
            //if (!selectOption)
            //{
            float.TryParse(value, out _RayDaDegrees);
            SetDegrees();
            //}
        }
    }

    public string RayDaPosition_x
    {
        set
        {
            //if (!selectOption)
            //{
            float.TryParse(value, out _RayDaPosition.x);
            Pos();
            //}
        }
    }

    public string RayDaPosition_y
    {
        set
        {
            //if (!selectOption)
            //{
            float.TryParse(value, out _RayDaPosition.y);
            Pos();
            //}
        }
    }

    public string RayDaPosition_z
    {
        set
        {
            //if (!selectOption)
            //{
            float.TryParse(value, out _RayDaPosition.z);
            Pos();
            //}
        }
    }

    public string RayDaRotation_x
    {
        set
        {
            //if (!selectOption)
            //{
            float.TryParse(value, out _RayDaRotation.x);
            Rot();
            //}
        }
    }

    public string RayDaRotation_y
    {
        set
        {
            //if (!selectOption)
            //{
            float.TryParse(value, out _RayDaRotation.y);
            Rot();
            //}
        }
    }

    public string RayDaRotation_z
    {
        set
        {
            //if (!selectOption)
            //{
            float.TryParse(value, out _RayDaRotation.z);
            Rot();
            //}
        }
    }

    public string RayDaSensingArea_CenterX
    {
        set
        {
            float.TryParse(value, out Center.x);
        }
    }

    public string RayDaSensingArea_CenterY
    {
        set
        {
            float.TryParse(value, out Center.y);
        }
    }

    public string RayDaSensingArea_CenterZ
    {
        set
        {
            float.TryParse(value, out Center.z);
        }
    }

    public string RayDaSensingArea_ExtentsX
    {
        set
        {
            float.TryParse(value, out Extents.x);
        }
    }

    public string RayDaSensingArea_ExtentsY
    {
        set
        {
            float.TryParse(value, out Extents.y);
        }
    }

    public string RayDaSensingArea_ExtentsZ
    {
        set
        {
            float.TryParse(value, out Extents.z);
        }
    }

    public float RayDaDistanceLimit
    {
        set
        {
            //if (!selectOption)
            //{
            text_RayDaDistanceLimit.text = value.ToString("0.##");
            DistanceLimit = value;
            SetDistanceLimit();
            //}
        }
    }

    public float RayDaOffset
    {
        set
        {
            text_RayDaOffset.text = value.ToString("0.##");
            Offset = value;
            SetOffset();
        }
    }

    public void SetRaydaRT()
    {
        RaydaRT.SetActive(!RaydaRT.activeSelf);
    }

    public void SetOption()
    {
        selectOption = !selectOption;
    }

    public void SetIP_Addes()
    {
        URGDataList[_SelectRayDaNum].SetIPAddress(_RayDaIPAddress);
        _RayDaList[_SelectRayDaNum].IPAddress = _RayDaIPAddress;
    }

    public void SetDegrees()
    {
        URGDataList[_SelectRayDaNum].SetDegrees(_RayDaDegrees);
        _RayDaList[_SelectRayDaNum].degress = _RayDaDegrees;
    }

    public void Pos()
    {
        URGDataList[_SelectRayDaNum].SettransformPosition(_RayDaPosition);
        _RayDaList[_SelectRayDaNum].Position = _RayDaPosition;
    }

    public void Rot()
    {
        URGDataList[_SelectRayDaNum].SettransformRotation(_RayDaRotation);
        _RayDaList[_SelectRayDaNum].Rotation = _RayDaRotation;
    }

    public void SetBounds()
    {
        URGDataList[_SelectRayDaNum].SetBounds(_RayDaSensingArea);
        _RayDaList[_SelectRayDaNum].SensingArea = _RayDaSensingArea;
    }

    public void SetDistanceLimit()
    {
        for (int i = 0; i < URGDataList.Count; i++)
            URGDataList[i].SetDistanceLimit(DistanceLimit);
    }

    public void SetOffset()
    {
        URGDataList[_SelectRayDaNum].SetOffset(Offset);
    }

    public void ReFrash()
    {
        _RaydaListDropdown.ClearOptions();
        for (int i = 0; i < _RayDaList.Count; i++)
        {
            _RaydaListDropdown.options.Add(new Dropdown.OptionData()
            {
                text = _RayDaList[i].Name
            });
        }
        _RaydaListDropdown.RefreshShownValue();

        _SelectRayDaNum = 0;
        SelectRayda();
    }

    public void RafrashDropdown()
    {
        _RaydaListDropdown.ClearOptions();
        for (int i = 0; i < _RayDaList.Count; i++)
        {
            _RaydaListDropdown.options.Add(new Dropdown.OptionData()
            {
                text = _RayDaList[i].Name
            });
        }
        _RaydaListDropdown.RefreshShownValue();
        SelectRayda();
    }

    public void SelectRayda()
    {
        OneRayda = false;
        ResetInputFeild();
        _RayDaIPAddress = _RayDaList[_SelectRayDaNum].IPAddress;
        _RayDaDegrees = _RayDaList[_SelectRayDaNum].degress;
        _RayDaPosition = _RayDaList[_SelectRayDaNum].Position;
        _RayDaRotation = _RayDaList[_SelectRayDaNum].Rotation;
        Center = _RayDaList[_SelectRayDaNum].SensingArea.center;
        Extents = _RayDaList[_SelectRayDaNum].SensingArea.extents;

        _RaydaIPAddress.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].IPAddress;
        _RaydaDegrees.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].degress.ToString();
        _RaydaPosition_X.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].Position.x.ToString();
        _RaydaPosition_Y.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].Position.y.ToString();
        _RaydaPosition_Z.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].Position.z.ToString();
        _RaydaRotation_X.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].Rotation.x.ToString();
        _RaydaRotation_Y.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].Rotation.y.ToString();
        _RaydaRotation_Z.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].Rotation.z.ToString();
        _RaydaBoundsCenter_X.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].SensingArea.center.x.ToString();
        _RaydaBoundsCenter_Y.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].SensingArea.center.y.ToString();
        _RaydaBoundsCenter_Z.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].SensingArea.center.z.ToString();
        _RaydaBoundsExtents_X.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].SensingArea.extents.x.ToString();
        _RaydaBoundsExtents_Y.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].SensingArea.extents.y.ToString();
        _RaydaBoundsExtents_Z.placeholder.GetComponent<Text>().text = _RayDaList[_SelectRayDaNum].SensingArea.extents.z.ToString();
        _RaydaDistanceLimit.value = DistanceLimit;
        _RaydaOffset.value = Offset;
        text_RayDaDistanceLimit.text = DistanceLimit.ToString();
        text_RayDaOffset.text = Offset.ToString();

        if (SettingCanvas.activeSelf)
        {
            URGDataList[_SelectRayDaNum].drawMeshUpdate.SetUpdateMaterialColor(Color.blue);
        }
        else
        {
            URGDataList[_SelectRayDaNum].drawMeshUpdate.SetUpdateMaterialColor(Color.black);
        }
    }

    public void SetCameraData()
    {
        _CameraPosition_X.placeholder.GetComponent<Text>().text = _CameraPosition.x.ToString();
        _CameraPosition_Y.placeholder.GetComponent<Text>().text = _CameraPosition.y.ToString();
        _CameraPosition_Z.placeholder.GetComponent<Text>().text = _CameraPosition.z.ToString();

        _Camera_Size.placeholder.GetComponent<Text>().text = _CameraSize.ToString();
        SetCamPos();
        SetCamSize();
    }

    public void ResetInputFeild()
    {
        ClearText(_RaydaIPAddress);
        ClearText(_RaydaDegrees);
        ClearText(_RaydaPosition_X);
        ClearText(_RaydaPosition_Y);
        ClearText(_RaydaPosition_Z);
        ClearText(_RaydaRotation_X);
        ClearText(_RaydaRotation_Y);
        ClearText(_RaydaRotation_Z);
        ClearText(_RaydaBoundsCenter_X);
        ClearText(_RaydaBoundsCenter_Y);
        ClearText(_RaydaBoundsCenter_Z);
        ClearText(_RaydaBoundsExtents_X);
        ClearText(_RaydaBoundsExtents_Y);
        ClearText(_RaydaBoundsExtents_Z);
    }

    public void AddNewRayDa()
    {
        _RayDaList.Add(
            new RaydaPrefab
            {
                Name = "192.168.0.10",
                IPAddress = "192.168.0.10",
                degress = -135,
                Position = Vector3.zero,
                Rotation = Vector3.zero,
                SensingArea = new Bounds { center = new Vector3(0, 1, 0), extents = new Vector3(2, 1, 0) }
            });
        RafrashDropdown();
        GameObject go = Instantiate(URG);
        go.name = _RayDaList[_RayDaList.Count - 1].Name;
        go.GetComponent<SetURGData>().SetIPAddress(_RayDaList[_RayDaList.Count - 1].IPAddress);
        go.GetComponent<SetURGData>().SetDegrees(_RayDaList[_RayDaList.Count - 1].degress);
        go.GetComponent<SetURGData>().SettransformPosition(_RayDaList[_RayDaList.Count - 1].Position);
        go.GetComponent<SetURGData>().SettransformRotation(_RayDaList[_RayDaList.Count - 1].Rotation);
        go.GetComponent<SetURGData>().SetBounds(_RayDaList[_RayDaList.Count - 1].SensingArea);

        go.SetActive(true);
        URGDataList.Add(go.GetComponent<SetURGData>());
        RayDaObject.Add(go);
    }

    public void SubRayDa()
    {
        Debug.Log(_SelectRayDaNum);
        _RayDaList.RemoveAt(_SelectRayDaNum);
        URGDataList.Remove(URGDataList[_SelectRayDaNum]);
        RayDaObject.RemoveAt(_SelectRayDaNum);
        ReFrash();
    }

    public void ClearText(InputField inputField)
    {
        inputField.text = "";
    }

    public void URGReConnect()
    {
        URGDataList[_SelectRayDaNum].ReConnect();
    }

    public void URGallReConnect()
    {
        for (int i = 0; i < URGDataList.Count; i++)
        {
            URGDataList[i].ReConnect();
        }
    }

    public void URGallDisConnect()
    {
        for (int i = 0; i < URGDataList.Count; i++)
        {
            URGDataList[i].DisConnect();
        }
    }

    public void URGDisConnect()
    {
        URGDataList[_SelectRayDaNum].DisConnect();
    }

    public void SetUpdateMesh()
    {
        for (int i = 0; i < URGDataList.Count; i++)
        {
            URGDataList[i].drawMeshUpdate.SetUpdateMaterialColor(Color.black);
        }
    }

    private void SetCamPos()
    {
        RayDaCamera.transform.position = _CameraPosition;
    }

    private void SetCamSize()
    {
        RayDaCamera.GetComponent<Camera>().orthographicSize = _CameraSize;
    }

    private void SetURG()
    {
        for (int i = 0; i < RayDaCount; i++)
        {
            GameObject go = Instantiate(URG);
            go.name = _RayDaList[i].IPAddress;
            go.layer = LayerMask.NameToLayer("URG" + (i + 1).ToString());
            // URGNameText[i].text = _RayDaList[i].IPAddress;
            go.GetComponent<SetURGData>().SetIPAddress(_RayDaList[i].IPAddress);
            go.GetComponent<SetURGData>().SetDegrees(_RayDaList[i].degress);
            go.GetComponent<SetURGData>().SettransformPosition(_RayDaList[i].Position);
            go.GetComponent<SetURGData>().SettransformRotation(_RayDaList[i].Rotation);
            go.GetComponent<SetURGData>().SetBounds(_RayDaList[i].SensingArea);
            go.GetComponent<SetURGData>().SetDistanceLimit(DistanceLimit);
            go.GetComponent<SetURGData>().SetOffset(Offset);
            go.GetComponent<SetURGData>().SetUnmanDis(_RayDaList[i].urgDistance);

            URGDataList.Add(go.GetComponent<SetURGData>());

            go.SetActive(true);
        }
    }

    public void URGMeshColor()
    {
        if (!OneRayda)
        {
            for (int i = 0; i < _RayDaList.Count; i++)
            {
                if (i == _SelectRayDaNum)
                {
                    URGDataList[i].drawMeshUpdate.SetUpdateMaterialColor(Color.blue);
                }
                else
                {
                    URGDataList[i].drawMeshUpdate.SetUpdateMaterialColor(Color.clear);
                }
            }
            OneRayda = true;
        }
        else
        {
            for (int i = 0; i < _RayDaList.Count; i++)
            {
                if (i == _SelectRayDaNum)
                {
                    URGDataList[i].drawMeshUpdate.SetUpdateMaterialColor(Color.blue);
                }
                else
                {
                    URGDataList[i].drawMeshUpdate.SetUpdateMaterialColor(Color.black);
                }
            }
            OneRayda = false;
        }
    }

    #endregion 雷達設定

    #region 攝影機設置

    public void OpenCullingMask()
    {
        mainCam.cullingMask = -1;
    }

    public void CloseCullingMask()
    {
        mainCam.cullingMask &= ~(1 << 7) + (1 << 8) + (1 << 9);
    }

    public void MainCamControl()
    {
        mainCamObject.SetActive(!mainCamObject.activeSelf);
    }

    #endregion 攝影機設置

    #region UI設置

    public void NDIURG(RawImage raw)
    {
        raw.enabled = !raw.enabled;
    }

    private void SetURGRawImage()
    {
        for (int i = 0; i < NDIURG_rawImageList.transform.childCount; i++)
        {
            NDIURG_rawImageList.transform.GetChild(i).GetComponentInChildren<RawImage>().enabled = false;
        }
    }

    #endregion UI設置

    public void CleanMemery()
    {
        Resources.UnloadUnusedAssets();
        Invoke("CleanMemery", 10);
    }

    private void GetUnmanData()
    {
        for (int i = 0; i < _RayDaList.Count; i++)
        {
            URGDataList[i].GetUnmanData(_RayDaList[i].urgDistance);
        }
    }

    private void ClearUnmanData()
    {
        for (int i = 0; i < _RayDaList.Count; i++)
        {
            URGDataList[i].ClearUnmanData(_RayDaList[i].urgDistance);
        }
    }

    private void Awake()
    {
        LoadJson();
        SetURG();
    }

    private void Start()
    {
        SetCameraData();
        CleanMemery();
        SetURGRawImage();
        SO = GetComponent<ShowObject>();
    }

    private void Update()
    {
        if (_RayDaSensingArea.extents != Extents)
        {
            _RayDaSensingArea.extents = Extents;
            SetBounds();
        }

        if (_RayDaSensingArea.center != Center)
        {
            _RayDaSensingArea.center = Center;
            SetBounds();
        }

        if (Input.GetKey(KeyCode.LeftControl) | Input.GetKeyDown(KeyCode.R))
        {
            URGallReConnect();
        }
        if (Input.GetKey(KeyCode.LeftControl) | Input.GetKeyDown(KeyCode.D))
        {
            URGallDisConnect();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SettingCanvas.SetActive(!SettingCanvas.activeSelf);
            SetUpdateMesh();
            //MainCamControl();
            SO.ShowGrid();
            RafrashDropdown();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            GetUnmanData();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            ClearUnmanData();
        }

        if (SettingCanvas.activeSelf)
        {
            OpenCullingMask();
        }
        else
        {
            CloseCullingMask();
        }

        if (SettingCanvas.activeSelf)
        {
            if (!Input.GetKey(KeyCode.LeftControl) & Input.GetKey(KeyCode.UpArrow))
            {
                _RaydaPosition_Y.text = (_RayDaPosition.y + 0.001f).ToString();
                _RayDaPosition.y += 0.001f;
                Pos();
            }
            else if (!Input.GetKey(KeyCode.LeftControl) & Input.GetKey(KeyCode.DownArrow))
            {
                _RaydaPosition_Y.text = (_RayDaPosition.y - 0.001f).ToString();
                _RayDaPosition.y -= 0.001f;
                Pos();
            }
            else if (!Input.GetKey(KeyCode.LeftControl) & Input.GetKey(KeyCode.LeftArrow))
            {
                _RaydaPosition_X.text = (_RayDaPosition.x - 0.001f).ToString();
                _RayDaPosition.x -= 0.001f;
                Pos();
            }
            else if (!Input.GetKey(KeyCode.LeftControl) & Input.GetKey(KeyCode.RightArrow))
            {
                _RaydaPosition_X.text = (_RayDaPosition.x + 0.001f).ToString();
                _RayDaPosition.x += 0.001f;
                Pos();
            }
            else if (Input.GetKey(KeyCode.LeftShift) & Input.GetKey(KeyCode.W))
            {
                //Debug.Log("Rot UP");
                _RaydaRotation_Z.text = (_RayDaRotation.z + 0.001f).ToString();
                _RayDaRotation.z += 0.001f;
                Rot();
            }
            else if (Input.GetKey(KeyCode.LeftShift) & Input.GetKey(KeyCode.S))
            {
                //Debug.Log("Rot DOWN");
                _RaydaRotation_Z.text = (_RayDaRotation.z - 0.001f).ToString();
                _RayDaRotation.z -= 0.001f;
                Rot();
            }
        }
    }

    #region Json

    [ContextMenu("SJ")]
    public void SaveJson()
    {
        RaydaSetting RSS = new RaydaSetting();
        RSS.CameraPos = _CameraPosition;
        RSS.CameraSize = _CameraSize;
        RSS.URGCount = _RayDaList.Count;
        RSS.RaydaDistanceLimit = DistanceLimit;
        RSS.Offset = Offset;

        RSS.RaydaPrefabs = new RaydaPrefab[_RayDaList.Count];
        _RayDaList.CopyTo(RSS.RaydaPrefabs);

        string savesource = JsonUtility.ToJson(RSS);
        //Debug.Log(savesource);
        StreamWriter file = new StreamWriter(Application.streamingAssetsPath + "/RaydaList.txt");
        file.Write(savesource);
        file.Close();
    }

    private void LoadJson()
    {
        string SettingSource;
        try
        {
            SettingSource = File.ReadAllText(Application.streamingAssetsPath + "/RaydaList.txt");
            Debug.Log(SettingSource);
        }
        finally
        {
        }
        _CameraPosition = JsonUtility.FromJson<RaydaSetting>(SettingSource).CameraPos;
        _CameraSize = JsonUtility.FromJson<RaydaSetting>(SettingSource).CameraSize;
        RayDaCount = JsonUtility.FromJson<RaydaSetting>(SettingSource).URGCount;
        DistanceLimit = JsonUtility.FromJson<RaydaSetting>(SettingSource).RaydaDistanceLimit;
        Offset = JsonUtility.FromJson<RaydaSetting>(SettingSource).Offset;

        for (int i = 0; i < JsonUtility.FromJson<RaydaSetting>(SettingSource).URGCount; i++)
        {
            _RayDaList.Add(JsonUtility.FromJson<RaydaSetting>(SettingSource).RaydaPrefabs[i]);
        }
    }

    public class RaydaSetting
    {
        public Vector3 CameraPos;
        public float CameraSize;
        public int URGCount;
        public float RaydaDistanceLimit;
        public float Offset;

        public RaydaPrefab[] RaydaPrefabs;
    }

    #endregion Json
}