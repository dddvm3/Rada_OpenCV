using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public Transform FollowObject;
    public LineRenderer LR;
    private Coroutine Drawing;
    private Vector3 position;

    [SerializeField] private bool _Drawing;

    public void Switch()
    {
        _Drawing = !_Drawing;
    }

    public void FinishLine()
    {
        if (Drawing != null)
        {
            StopCoroutine(Drawing);
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (_Drawing)
        {
            StartLine();
        }
        if (!_Drawing)
        {
            FinishLine();
        }
        if (FollowObject == null)
        {
            Destroy(this.gameObject);
        }
    }

    private void StartLine()
    {
        if (Drawing != null)
        {
            StopCoroutine(Drawing);
        }
        Drawing = StartCoroutine(DrawLine());
    }

    private IEnumerator DrawLine()
    {
        //GameObject go = Instantiate(Resources.Load("Line") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
        //LineRenderer line = go.GetComponent<LineRenderer>();

        if (FollowObject != null)
        {
            position = FollowObject.position;
        }

        position.z = 0;
        //Debug.Log(Vector3.Distance(LR.GetPosition(LR.positionCount - 1), new Vector3(position.x, position.y, 0)));
        if (Vector3.Distance(LR.GetPosition(LR.positionCount - 1), new Vector3(position.x, position.y, 0)) > 50)
        {
            LR.positionCount = LR.positionCount + 1;

            LR.SetPosition(LR.positionCount - 1, position);
        }
        yield return null;
    }
}