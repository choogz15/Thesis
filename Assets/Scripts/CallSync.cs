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


    protected override void OnRealtimeModelReplaced(CallSyncModel previousModel, CallSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.dialingPlayerDidChange -= DialingPlayerDidChange;
            previousModel.dialerPlayerDidChange -= DialerPlayerDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.dialingPlayer = -1;
                currentModel.dialerPlayer = -1;
            }


            UpdateDialingIndicator();
            UpdateCallButton();

            currentModel.dialingPlayerDidChange += DialingPlayerDidChange;
            currentModel.dialerPlayerDidChange += DialerPlayerDidChange;
        }
    }


    private void DialingPlayerDidChange(CallSyncModel model, int value)
    {
        UpdateDialingIndicator();
        dialingPlayerChangedEvent.Invoke(model, value, ownerIDInHierarchy);
    }

    private void DialerPlayerDidChange(CallSyncModel model, int value)
    {
        UpdateCallButton();
    }

    private void UpdateDialingIndicator()
    {
        bool showIndicator = model.dialingPlayer < 0 ? false : true;
        dialingIndicator.SetActive(showIndicator);
    }

    private void UpdateCallButton()
    {
        if (model.dialerPlayer < 0) EnableCallButton();
        else DisableCallButton();
    }

    private void DisableCallButton()
    {
        callButton.GetComponentInChildren<Text>().text = "Busy";
        callButton.interactable = false;
    }

    private void EnableCallButton()
    {
        callButton.GetComponentInChildren<Text>().text = "Call";
        callButton.interactable = true;
    }

    public void CallOtherPlayer(int playerID)
    {
        model.dialingPlayer = playerID;
    }

    public void SetDialerPlayer(int playerID)
    {
        model.dialerPlayer = playerID;
    }

    public void AcceptCall()
    {
        model.talkingPlayer = model.dialerPlayer;
        model.dialerPlayer = -1;
    }

    public void RejectCall()
    {
        model.dialerPlayer = -1;
    }
}
