using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class CallSync : RealtimeComponent<CallSyncModel>
{

    public GameObject busyIndicator;

    //The button on top of the player's avatar to be clicked to initiate a private talk
    public Button callButton;

    [Serializable] public class DialingPlayerChangeEvent : UnityEvent<CallSyncModel, int, int> { }     //<model, calleeID, callerID>
    [Serializable] public class TalkingPlayerChangeEvent : UnityEvent<CallSyncModel, int, int> { }


    public DialingPlayerChangeEvent dialingPlayerChangedEvent;
    public TalkingPlayerChangeEvent talkingPlayerChangedEvent;

    //Just to see in inspector
    public Color dialingPlayerColor;
    public Color dialerPlayerColor;
    public Color talkingPlayerColor;

    public int talkingWithPlayer;

    protected override void OnRealtimeModelReplaced(CallSyncModel previousModel, CallSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.dialingPlayerDidChange -= DialingPlayerDidChange;
            previousModel.dialerPlayerDidChange -= DialerPlayerDidChange;

            previousModel.talkingPlayerDidChange -= TalkingPlayerDidChange;

        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.dialingPlayer = -1;
                currentModel.dialerPlayer = -1;

                currentModel.talkingPlayer = -1;
            }
            UpdateDialerIndicator();
            UpdateTalkingIndicator();

            currentModel.dialingPlayerDidChange += DialingPlayerDidChange;
            currentModel.dialerPlayerDidChange += DialerPlayerDidChange;
            currentModel.talkingPlayerDidChange += TalkingPlayerDidChange;
        }
    }

    private void TalkingPlayerDidChange(CallSyncModel model, int value)
    {
        UpdateTalkingIndicator();
        talkingPlayerChangedEvent.Invoke(model, value, ownerIDInHierarchy);
    }

    private void UpdateTalkingIndicator()
    {
        talkingWithPlayer = model.talkingPlayer;    //Just for debugging

        if (model.talkingPlayer >= 0) busyIndicator.GetComponent<MeshRenderer>().material.color = talkingPlayerColor;
        UpdateBusyIndicator();

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
        if (model.dialingPlayer >= 0) busyIndicator.GetComponent<MeshRenderer>().material.color = dialingPlayerColor;
        UpdateBusyIndicator();
    }

    private void UpdateDialerIndicator()
    {
        if (model.dialerPlayer >= 0) busyIndicator.GetComponent<MeshRenderer>().material.color = dialerPlayerColor;
        UpdateBusyIndicator();
    }

    private void UpdateBusyIndicator()
    {
        bool showIndicator = model.dialerPlayer >= 0 || model.dialingPlayer >= 0 || model.talkingPlayer >= 0;
        busyIndicator.SetActive(showIndicator);
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
        SetDialerPlayer(-1);

    }

    public void RejectCall()
    {
        model.dialerPlayer = -1;
        SetDialerPlayer(-1);
    }

    public void CallAccepted()
    {

    }

    public void CallRejected()
    {
    }
}
