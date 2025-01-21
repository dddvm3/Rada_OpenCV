using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootObject : MonoBehaviour
{
    public float ForceMulValue;

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.tag == "Ball")
        //{
        //    Vector3 pos = Vector3.Reflect(transform.position, collision.contacts[0].normal);
        //    collision.rigidbody.velocity = pos * ForceMulValue;
        //    Debug.Log("Contacts : " + collision.contacts[0].normal + "Reflect : " + pos);
        //}
    }
}