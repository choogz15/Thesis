using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        UnityEngine.XR.InputTracking.Recenter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
