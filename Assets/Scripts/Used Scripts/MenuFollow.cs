using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class MenuFollow : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Transform head;
    public float spawnDistance = 1;

    private void Awake()
    {
        xrOrigin = GameObject.Find("XR Origin").GetComponent<XROrigin>();
        head = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    private void Update()
    {
        transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        transform.LookAt(new Vector3(head.position.x, transform.position.y, head.position.z));
        transform.forward *= -1;
    }
}
