using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabRequest : MonoBehaviour
{
    private RealtimeTransform realtimeTransform;
    private XRGrabInteractable xRGrabInteractable;
    // Start is called before the first frame update
    void Awake()
    {
        realtimeTransform = GetComponent<RealtimeTransform>();
        xRGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        xRGrabInteractable.selectEntered.AddListener(RequestObjectOwnership);
    }


    public void RequestObjectOwnership(SelectEnterEventArgs args)
    {
        realtimeTransform.RequestOwnership();
    }

    private void OnDisable()
    {
        xRGrabInteractable.selectEntered.RemoveListener(RequestObjectOwnership);
    }


}
