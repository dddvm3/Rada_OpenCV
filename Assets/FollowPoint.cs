using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPoint : MonoBehaviour
{
    public FollowObjectControl FOC;
    public bool haveObject;

    public void haveMaster()
    {
        haveObject = !haveObject;
    }

    public void OnDestroy()
    {
        if (FOC != null)
        {
            FOC._FollowPointArray.Remove(gameObject.name);
        }
    }
}