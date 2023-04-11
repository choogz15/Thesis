using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IncomingCall : MonoBehaviour
{
    public TextMeshProUGUI notificationMessage;

    public void SetNotificationMessage(int callerID)
    {
        notificationMessage.SetText("Player " + callerID + " wants to talk to you.");
    }
}
