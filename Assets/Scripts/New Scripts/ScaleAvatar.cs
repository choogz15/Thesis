using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScaleAvatar : MonoBehaviour
{
    public InputAction calibrate;
    // Start is called before the first frame update

    private void Awake()
    {
        calibrate.performed += CalibrateAvatar;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        calibrate.Enable();
    }

    private void OnDisable()
    {
        calibrate.Disable();
    }

    void CalibrateAvatar(InputAction.CallbackContext context)
    {
        Debug.Log("Calibrating avatar height");
    }
}
