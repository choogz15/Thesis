using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallIconFollow : MonoBehaviour
{
    public Transform localPlayerHead;
    public Transform avatarHead;
    public float verticalOffset = 0.5f;

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
