using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

public class CallRequestboardSync : RealtimeComponent<CallRequestboardModel>
{
    [SerializeField] MultiplayerManager multiplayerManager;
    [SerializeField] OngoingCallboardSync ongoingCallboardSync;

    protected override void OnRealtimeModelReplaced(CallRequestboardModel previousModel, CallRequestboardModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.players.modelAdded -= CallRequestAdded;
            previousModel.players.modelRemoved -= CallRequestRemoved;
        }

        if (currentModel != null)
        {
            currentModel.players.modelAdded += CallRequestAdded;
            currentModel.players.modelRemoved += CallRequestRemoved;
        }
    }

    public void MakeCallRequest(int calleeID, int callerID)
    {
        AddPlayerToDict((uint)calleeID, callerID);
    }

    public void CancelCallRequest(int callerID)
    {
        Debug.Log("[CallRequestboardSync]Not yet implemented");
    }

    public void RejectCallRequest(int calleeID)
    {
        Debug.Log("[CallRequestboardSync]Call rejected");
        RemovePlayerFromDict((uint)calleeID);
    }

    public void AnswerCall(int calleeID)
    {
        Debug.Log("[CallRequestboardSync]Call answered");

        int callerID = model.players[(uint)calleeID].callerID;

        ongoingCallboardSync.CallStarted(calleeID, callerID);

        RemovePlayerFromDict((uint)calleeID);
    }

    private void AddPlayerToDict(uint calleeID, int callerID)
    {
        CallRequestModel newCallRequestModel = new CallRequestModel();
        newCallRequestModel.callerID = callerID;
        model.players.Add(calleeID, newCallRequestModel);
    }

    private void RemovePlayerFromDict(uint playerId)
    {
        model.players.Remove(playerId);
    }

    private void CallRequestAdded(RealtimeDictionary<CallRequestModel> dictionary, uint key, CallRequestModel model, bool remote)
    {
        Debug.Log("[CallRequestboardSync]CallRequestAdded");
        multiplayerManager.OnCallRequestAdded((int)key, model.callerID);
    }

    private void CallRequestRemoved(RealtimeDictionary<CallRequestModel> dictionary, uint key, CallRequestModel model, bool remote)
    {
        Debug.Log("[CallRequestboardSync]CallRequestRemoved");
        multiplayerManager.OnCallRequestRemoved((int)key, model.callerID);
    }

}
