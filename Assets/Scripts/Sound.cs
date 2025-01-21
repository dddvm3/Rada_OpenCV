using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public List<AudioSource> ASList;
    public List<AudioSource> _ASList;

    public int audioNum;

    public void PlaySound()
    {
        //audioNum
        if (ASList.Count != 0)
        {
            int rand = Random.Range(0, ASList.Count - 1);
            audioNum = rand;
            ASList[audioNum].Play();
            AudioSource x = ASList[audioNum];
            _ASList.Add(x);
            ASList.Remove(x);
        }
        else if (ASList.Count == 0)
        {
            int rand = Random.Range(0, _ASList.Count - 1);
            audioNum = rand;
            _ASList[audioNum].Play();
            AudioSource x = _ASList[audioNum];
            ASList.Add(x);
            _ASList.Remove(x);
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}