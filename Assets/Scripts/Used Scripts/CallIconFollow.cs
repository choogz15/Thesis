using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallIconFollow : MonoBehaviour
{
    //Make the remote avatars' call icon face the local player all the time

    public Transform avatarHead;
    private Transform localPlayerHead;   //main camera of the XR Origin
    private float verticalOffset = 0.5f; //distance from call icon to avatar head

    private void Awake()
    {
        localPlayerHead = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(localPlayerHead.position.x, transform.position.y, localPlayerHead.position.z));
        transform.position = avatarHead.position + Vector3.up * verticalOffset;

    }
}
