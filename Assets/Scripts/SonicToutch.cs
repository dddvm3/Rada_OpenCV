using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicToutch : MonoBehaviour
{
    public Camera FollowCam;
    public int triggerCount;
    public AudioSource[] AnimalSoundList;
    public List<UrgObject> _urgObjects;
    public List<Vector3> UrgCubePos;
    public List<GameObject> ToutchObject;
    public Vector3 triggerPos_1;
    public Vector3 triggerPos_2;
    private Transform _selftransform;
    private bool MusicOn;
    [SerializeField] private Vector3 ScreenPos;
    [SerializeField] private float GetposTime;
    [SerializeField] private Texture2D tex;
    [SerializeField] private Color texPixalColor;

    [SerializeField] private int PosinCount;

    public void SonicDiffusion()
    {
        if (gameObject.activeSelf)
        {
        }
    }

    public void GetPos(Vector3 pos)
    {
        if (triggerPos_1 == Vector3.zero)
        {
            triggerPos_1 = pos;
            MusicOn = true;
            GetposTime = Time.time;
        }
        else
        {
            triggerPos_2 = pos;
        }
    }

    public void SaveTexture(Vector3 pos)
    {
        //RenderTexture rt = new RenderTexture(1920, 1200, -1);
        //FollowCam.targetTexture = rt;
        FollowCam.Render();
        RenderTexture.active = FollowCam.targetTexture;

        tex.ReadPixels(new Rect(0, 0, 1920, 1200), 0, 0);
        tex.Apply();

        Vector3 screenpos = Camera.main.WorldToViewportPoint(pos);
        texPixalColor = tex.GetPixel((int)screenpos.x, (int)screenpos.y);
        if (texPixalColor == Color.black)
        {
            triggerCount += 1;
            MusicOn = true;
            texPixalColor = Color.white;
        }
    }

    public void CheckURGCube()
    {
        //triggerCount += 1;
        MusicOn = true;
    }

    private void Start()
    {
        _selftransform = GetComponent<Transform>();
        tex = new Texture2D(1920, 1200, TextureFormat.ARGB32, false);
    }

    private void Update()
    {
        //if (triggerPos != null) ScreenPos = Camera.main.WorldToViewportPoint(triggerPos);

        if (Input.GetKeyDown(KeyCode.H))
        {
            //SaveTexture();
        }

        if (MusicOn)
        {
            triggerCount += 1;
            AnimalSoundList[triggerCount - 1].Play();
            MusicOn = false;
            if (triggerCount >= AnimalSoundList.Length)
            {
                triggerCount = 0;
            }
        }
        if (ToutchObject.Count != 0)
        {
            //Debug.Log("Vector3.Distance(triggerPos_1, triggerPos_2) : " + Vector3.Distance(triggerPos_1, triggerPos_2));
            if (Vector3.Distance(triggerPos_1, triggerPos_2) > 2)
            {
                triggerPos_1 = triggerPos_2;
                MusicOn = true;
                GetposTime = Time.time;
                PosinCount = ToutchObject.Count;
            }
            //else if (triggerPos_2 != null)
            //{
            //    if (Time.time - GetposTime > 2)
            //    {
            //        triggerPos_1 = triggerPos_2;
            //        MusicOn = true;
            //        GetposTime = Time.time;
            //    }
            //    else
            //    {
            //        if (ToutchObject.Count <= PosinCount)
            //        {
            //            triggerPos_2 = Vector3.zero;
            //            GetposTime = Time.time;
            //        }
            //        else
            //        {
            //        }
            //    }
            //}
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //if (triggerPos != Vector3.zero)
            //{
            //    triggerPos = collision.transform.position;
            //}
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //if (tex.GetPixels(ScreenPos.x, ScreenPos.y, 1920, 1200))
        //{
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        //if (triggerCount >= AnimalSoundList.Length)
        //{
        //    triggerCount = 0;
        //}
    }
}

[System.Serializable]
public class UrgObject
{
    public Vector3 Pos;
    public bool success;
}