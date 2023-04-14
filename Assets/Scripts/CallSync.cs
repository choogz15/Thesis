using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class CallSync : RealtimeComponent<CallSyncModel>
{

    public GameObject dialingIndicator;

    //The button on top of the player's avatar to be clicked to initiate a private talk
    public Button callButton;


    [Serializable]
    public class DialingPlayerChangeEvent : UnityEvent<CallSyncModel, int, int> { }     //<model, calleeID, callerID>
    public DialingPlayerChangeEvent dialingPlayerChangedEvent;

    public int dialerPlayer;    //It seems that this doesnt need to be networked.


    protected override void OnRealtimeModelReplaced(CallSyncModel previousModel, CallSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.dialingPlayerDidChange -= DialingPlayerDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.dialingPlayer = -1;
            }


            UpdateDialingIndicator();

            currentModel.dialingPlayerDidChange += DialingPlayerDidChange;

        }
    }

    private void DialerPlayerDidChange(CallSyncModel model, int value)
    {
        throw new NotImplementedException();
    }

    private void DialingPlayerDidChange(CallSyncModel model, int value)
    {
        UpdateDialingIndicator();
        dialingPlayerChangedEvent.Invoke(model, value, ownerIDInHierarchy);
    }

    private void UpdateDialingIndicator()
    {
        bool showIndicator = model.dialingPlayer < 0 ? false : true;
        dialingIndicator.SetActive(showIndicator);
    }

    public void CallOtherPlayer(int playerID)
    {
        model.dialingPlayer = playerID;
    }

    public void AcceptCall()
    {
        model.talkingPlayer = dialerPlayer;
        dialerPlayer = -1;
    }

    public void RejectCall()
    {
        dialerPlayer = -1;
    }
}
