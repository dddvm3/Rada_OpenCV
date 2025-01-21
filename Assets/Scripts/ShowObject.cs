using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowObject : MonoBehaviour
{
    public Sprite WhiteGrid;
    public Sprite BlackGrid;

    public Image BG;

    public void ShowGrid()
    {
        BG.enabled = !BG.enabled;
    }

    //public void HideGrid()
    //{
    //    BG.enabled = false;
    //}

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetBlackGrid();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SetWhiteGrid();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
        }
    }

    private void SetBlackGrid()
    {
        BG.sprite = BlackGrid;
    }

    private void SetWhiteGrid()
    {
        BG.sprite = WhiteGrid;
    }
}