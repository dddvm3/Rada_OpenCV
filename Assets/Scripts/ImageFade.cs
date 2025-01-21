using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    public Image _image;

    public IEnumerator FadeIn()
    {
        for (float a = _image.color.a; a < 1; a += Time.deltaTime)
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, a);
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        for (float a = _image.color.a; a > 0; a -= Time.deltaTime)
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, a);
            yield return null;
        }
    }

    public void CallFadeIn()
    {
        StartCoroutine("FadeIn");
    }

    public void CallFadeOut()
    {
        StartCoroutine("FadeOut");
    }

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    StartCoroutine("FadeIn");
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    StartCoroutine("FadeOut");
        //}
    }
}