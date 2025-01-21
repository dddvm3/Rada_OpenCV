using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLayer : MonoBehaviour
{
    public GameObject Parent;


    void Start()
    {
        if(this.gameObject.layer != Parent.layer)
        {
            this.gameObject.layer = Parent.layer;
        }
    }

    
}
