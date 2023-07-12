using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAimTarget : MonoBehaviour
{
    public Transform follow;

    // Update is called once per frame
    void Update()
    {
        if(follow != null)
        {
            transform.position = new Vector3(follow.position.x, transform.position.y, follow.position.z);
        }
    }
}
