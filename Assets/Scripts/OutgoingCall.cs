using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OutgoingCall : MonoBehaviour
{
    public TextMeshProUGUI notificationMessage;

    public void SetNotificationMessage(int calleeID)
    {
        notificationMessage.SetText("Calling Player " + calleeID + ".");
    }
}
