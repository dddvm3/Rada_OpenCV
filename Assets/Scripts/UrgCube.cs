using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrgCube : MonoBehaviour
{
    public float Origin_Dis;
    public bool isDifferent;
    public Vector3 OriginPos;
    public List<GameObject> InScUrgcube;
    public bool HaveSC;
    [SerializeField] private int Cube_Num;
    private SonicToutch Bird;
    private SonicToutch Dog;

    public void Start()
    {
        //Bird = GameObject.FindGameObjectWithTag("Bird").GetComponent<SonicToutch>();
        //Dog = GameObject.FindGameObjectWithTag("Dog").GetComponent<SonicToutch>();
    }

    public void SetCubeNum(int Num)
    {
        Cube_Num = Num;
    }

    private void Update()
    {
        if (transform.localPosition != Vector3.zero)
        {
            if (OriginPos == Vector3.zero)
            {
                OriginPos = transform.localPosition;
            }
        }
        CheckDistance();
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(OriginPos, transform.localPosition) > 0.5f)
        {
            isDifferent = true;
        }
        else
        {
            isDifferent = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //other.GetComponent<UrgCube>().InScUrgcube.Add(this.gameObject);
            HaveSC = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //other.GetComponent<UrgCube>().InScUrgcube.Remove(gameObject);
            HaveSC = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bird")
        {
            //SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            //sc.radius = 20;
            //sc.isTrigger = true;
            //SC = sc;
            collision.gameObject.GetComponent<SonicToutch>().ToutchObject.Add(gameObject);
            collision.gameObject.GetComponent<SonicToutch>().GetPos(transform.position);
        }
        if (collision.transform.tag == "Dog")
        {
            //SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            //sc.radius = 15;
            //sc.isTrigger = true;
            collision.gameObject.GetComponent<SonicToutch>().ToutchObject.Add(gameObject);
            collision.gameObject.GetComponent<SonicToutch>().GetPos(transform.position);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Bird")
        {
            //Bird.SaveTexture(transform.position);
        }
        if (collision.transform.tag == "Dog")
        {
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Bird")
        {
            //Destroy(SC);
        }
        if (collision.transform.tag == "Dog")
        {
            //Destroy(SC);
        }

        //collision.gameObject.GetComponent<SonicToutch>().ToutchObject.Remove(gameObject);
    }
}