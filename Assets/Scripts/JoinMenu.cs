using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

public class JoinMenu : MonoBehaviour
{
    public Transform privateRoomTeleportArea;
    public Transform publicRoomTeleportArea;
    public bool isInPublicRoom = true;
    public XROrigin xrOrigin;
    public TextMeshProUGUI message;
    public InputAction teleportAction;
    public GameObject panel;
    public Transform head;
    public float spawnDistance = 1;

    void Awake()
    {
        privateRoomTeleportArea = GameObject.Find("Private Room Teleport Area").GetComponent<Transform>();
        publicRoomTeleportArea = GameObject.Find("Public Room Teleport Area").GetComponent<Transform>();
        xrOrigin = GameObject.Find("XR Origin").GetComponent<XROrigin>();
        head = GameObject.Find("Main Camera").GetComponent<Transform>();

        teleportAction.performed += ToggleVisible;
    }

    private void OnEnable()
    {
        teleportAction.Enable();
    }

    private void OnDisable()
    {
        teleportAction.Disable();
    }

    public void ToggleVisible(InputAction.CallbackContext context)
    {
        panel.SetActive(!panel.activeSelf);
    }

    private void Update()
    {
        transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        transform.LookAt(new Vector3(head.position.x, transform.position.y, head.position.z));
        transform.forward *= -1;

    }

    // Update is called once per frame
    public void Teleport()
    {
        if (isInPublicRoom)
        {
            xrOrigin.transform.position = privateRoomTeleportArea.position;
            //xrOrigin.MoveCameraToWorldLocation(privateRoomTeleportArea.position);
            xrOrigin.MatchOriginUpCameraForward(privateRoomTeleportArea.up, privateRoomTeleportArea.forward);
            message.text = "Join Public Room";
        }

        else
        {
            xrOrigin.transform.position = publicRoomTeleportArea.position;
            //xrOrigin.MoveCameraToWorldLocation(publicRoomTeleportArea.position);
            xrOrigin.MatchOriginUpCameraForward(publicRoomTeleportArea.up, publicRoomTeleportArea.forward);
            message.text = "Join Private Room";
        }

        isInPublicRoom = !isInPublicRoom;
        panel.SetActive(false);

    }
}
