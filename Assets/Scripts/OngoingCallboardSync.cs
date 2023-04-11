using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

public class OngoingCallboardSync : RealtimeComponent<OngoingCallboardModel>
{
    [SerializeField] UpdateAvatar updateAvatar;

    protected override void OnRealtimeModelReplaced(OngoingCallboardModel previousModel, OngoingCallboardModel currentModel)
    {
        if(previousModel != null)
        {
            previousModel.players.modelAdded -= OngoingCallAdded;
            previousModel.players.modelRemoved -= OngoingCallRemoved;
        }

        if(currentModel != null)
        {
            currentModel.players.modelAdded += OngoingCallAdded;
            currentModel.players.modelRemoved += OngoingCallRemoved;
        }
    }

    public void CallStarted(int playerID, int otherPlayerID) 
    {
        AddPlayerToDict((uint)playerID, otherPlayerID);
        AddPlayerToDict((uint)otherPlayerID, playerID);
    }

    public void CallEnded(int playerID, int otherPlayerID)
    {
        RemovePlayerFromDict((uint)playerID);
        RemovePlayerFromDict((uint)otherPlayerID);
    }

    private void AddPlayerToDict(uint playerID, int otherPlayerID)
    {
        OngoingCallModel newOngoingCallModel = new OngoingCallModel();
        newOngoingCallModel.callerID = otherPlayerID;
        model.players.Add(playerID, newOngoingCallModel);
    }

    private void RemovePlayerFromDict(uint playerID)
    {
        model.players.Remove(playerID);
    }

    private void OngoingCallAdded(RealtimeDictionary<OngoingCallModel> dictionary, uint key, OngoingCallModel model, bool remote)
    {
        Debug.Log("Player " + key + " is now in private talk");
        updateAvatar.OnOngoingCallAdded((int)key, model.callerID);
    }

    private void OngoingCallRemoved(RealtimeDictionary<OngoingCallModel> dictionary, uint key, OngoingCallModel model, bool remote)
    {
        Debug.Log("Player " + key + " has ended in private talk");
        updateAvatar.OnOngoingCallRemove((int)key, model.callerID);
    }


}
