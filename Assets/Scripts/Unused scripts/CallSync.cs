using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CallSync : RealtimeComponent<CallSyncModel>
{

    public GameObject busyIndicator;

    //The button on top of the player's avatar to be clicked to initiate a private talk
    public Button callButton;

    [Serializable] public class CallEvent : UnityEvent<CallSyncModel, int> { }

    public CallEvent incomingCallAccepted;
    public CallEvent incomingCallRejected;
    public CallEvent outgoingCallCancelled;
    public CallEvent outgoingCallRequested;
    public CallEvent incomingCallRequested;
    public CallEvent incomingCallCancelled;

    //For debugging in inspector
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
        if (value >= 0)
        {
            if (model.dialingPlayer > 0)
            {
                model.dialingPlayer = -1;
            }
            else
            {
                incomingCallAccepted.Invoke(model, value);
            }
        }
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
        if (value >= 0) outgoingCallRequested.Invoke(model, value);
        else if (value < 0 && model.talkingPlayer < 0) outgoingCallCancelled.Invoke(model, value);
    }

    private void DialerPlayerDidChange(CallSyncModel model, int value)
    {
        UpdateDialerIndicator();
        if (value >= 0) incomingCallRequested.Invoke(model, value);
        else incomingCallCancelled.Invoke(model, value);
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

        if (showIndicator)
        {
            DisableCallButton();
        }
        else
        {
            EnableCallButton();
        }
    }

    private void DisableCallButton()
    {
        callButton.GetComponentInChildren<TextMeshProUGUI>().text = "Busy";
        callButton.interactable = false;
    }

    private void EnableCallButton()
    {
        callButton.GetComponentInChildren<TextMeshProUGUI>().text = "Call";
        callButton.interactable = true;
    }


    public void MakeOutgoingCall(int playerID)
    {
        model.dialingPlayer = playerID;
    }

    public void CancelOutgoingCall()
    {
        model.dialingPlayer = -1;
    }

    public void IncomingCallReceived(int playerID)
    {
        model.dialerPlayer = playerID;
    }

    public void IncomingCallCancelled()
    {
        model.dialerPlayer = -1;
    }

    public void OutgoingCallAccepted()
    {
        model.talkingPlayer = model.dialingPlayer;
        model.dialingPlayer = -1;
    }

    public void OutgoingCallRejected()
    {
        model.dialingPlayer = -1;
    }

    public void AcceptIncomingCall()
    {
        model.talkingPlayer = model.dialerPlayer;
    }

    public void RejectIncomingCall()
    {
        model.dialerPlayer = -1;
    }

    public void EndOnGoingCall()
    {
        model.talkingPlayer = -1;
    }

}
