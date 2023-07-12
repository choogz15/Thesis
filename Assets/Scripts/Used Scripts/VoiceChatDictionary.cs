using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;
using UnityEngine.UI;
using TMPro;
using System;

public class VoiceChatDictionary : RealtimeComponent<VoiceChatDictionaryModel>
{
    public RealtimeAvatarManager realtimeAvatarManager;

    //Incoming Call Menu
    [SerializeField] GameObject incomingCallMenu;
    [SerializeField] Button incomingCallMenuAcceptButton;
    [SerializeField] Button incomingCallMenuRejectButton;

    //Outgoing Call Menu
    [SerializeField] GameObject outgoingCallMenu;
    [SerializeField] Button outgoingCallMenuCancelButton;

    //Ongoing Call Menu
    [SerializeField] GameObject ongoingCallMenu;
    [SerializeField] Button ongoingCallMenuEndButton;

    private void Awake()
    {
        realtimeAvatarManager.avatarCreated += AvatarCreated;
    }

    private void AvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        if (isLocalAvatar)
        {
            Debug.Log("");
        }

        else
        {
            avatar.GetComponent<TestSync>().callButton.onClick.AddListener(delegate { MakeOutgoingCall(avatar.ownerIDInHierarchy); });
        }
    }

    protected override void OnRealtimeModelReplaced(VoiceChatDictionaryModel previousModel, VoiceChatDictionaryModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.players.modelAdded -= voiceChatAdded;
            previousModel.players.modelRemoved -= voiceChatRemoved;
            previousModel.players.modelReplaced -= voiceChatReplaced;
        }

        if (currentModel != null)
        {
            currentModel.players.modelAdded += voiceChatAdded;
            currentModel.players.modelRemoved += voiceChatRemoved;
            currentModel.players.modelReplaced += voiceChatReplaced;
        }
    }

    private void voiceChatReplaced(RealtimeDictionary<VoiceChatModel> dictionary, uint key, VoiceChatModel oldModel, VoiceChatModel newModel, bool remote)
    {
        //Mute other players if they are in private voice chat mode
        if (key != realtimeAvatarManager.localAvatar.ownerIDInHierarchy && newModel.pairID != realtimeAvatarManager.localAvatar.ownerIDInHierarchy && newModel.status == 1)
        {
            realtimeAvatarManager.avatars[(int)key].GetComponentInChildren<RealtimeAvatarVoice>().mute = true;
            realtimeAvatarManager.avatars[newModel.pairID].GetComponentInChildren<RealtimeAvatarVoice>().mute = true;
        }

        if ((key == realtimeAvatarManager.localAvatar.ownerIDInHierarchy || newModel.pairID == realtimeAvatarManager.localAvatar.ownerIDInHierarchy) && newModel.status == 1)
        {
            PrivateVoiceChatStarted();
        }
    }

    private void voiceChatAdded(RealtimeDictionary<VoiceChatModel> dictionary, uint key, VoiceChatModel model, bool remote)
    {
        //Mute other players if they are in private voice chat mode
        if(key != realtimeAvatarManager.localAvatar.ownerIDInHierarchy && model.pairID !=  realtimeAvatarManager.localAvatar.ownerIDInHierarchy && model.status == 1)
        {
            realtimeAvatarManager.avatars[(int)key].GetComponentInChildren<RealtimeAvatarVoice>().mute = true;
            realtimeAvatarManager.avatars[model.pairID].GetComponentInChildren<RealtimeAvatarVoice>().mute = true;
        }

        if(key == realtimeAvatarManager.localAvatar.ownerIDInHierarchy || model.pairID == realtimeAvatarManager.localAvatar.ownerIDInHierarchy)
        {
            if(model.status == 0)
            {
                if(key == realtimeAvatarManager.localAvatar.ownerIDInHierarchy)
                {
                    OutgoingCallRequest();
                }

                else if(model.pairID == realtimeAvatarManager.localAvatar.ownerIDInHierarchy)
                {
                    Debug.Log("Incoming call request");
                    IncomingCallRequest();
                }
            }

            if(model.status == 1)
            {
                PrivateVoiceChatStarted();

            }
        }

    }

    private void voiceChatRemoved(RealtimeDictionary<VoiceChatModel> dictionary, uint key, VoiceChatModel model, bool remote)
    {
        if (key != realtimeAvatarManager.localAvatar.ownerIDInHierarchy && model.pairID != realtimeAvatarManager.localAvatar.ownerIDInHierarchy)
        {
            realtimeAvatarManager.avatars[(int)key].GetComponentInChildren<RealtimeAvatarVoice>().mute = false;
            realtimeAvatarManager.avatars[model.pairID].GetComponentInChildren<RealtimeAvatarVoice>().mute = false;
        }

        if(model.status == 0)
        {
            if (key == realtimeAvatarManager.localAvatar.ownerIDInHierarchy)
            {
                OutgoingCallCancelled();
            }

            else if (model.pairID == realtimeAvatarManager.localAvatar.ownerIDInHierarchy)
            {
                IncomingCallCancelled();
            }
        }

        else if (model.status == 1)
        {
            PrivateVoiceChatEnded();
        }

    }

    public void PrivateVoiceChatStarted()
    {
        HideIncomingCallMenu();
        HideOutgoingCallMenu();
        ShowOngoingCallmenu();
    }

    public void PrivateVoiceChatEnded()
    {
        HideOngoingCallMenu();
    }

    public void OutgoingCallRequest()
    {
        ShowOutgoingCallMenu();
    }

    public void IncomingCallRequest()
    {
        ShowIncomingCallMenu();
    }

    public void OutgoingCallCancelled()
    {
        HideOutgoingCallMenu();
    }

    public void IncomingCallCancelled()
    {
        HideIncomingCallMenu();
    }

    public void MakeOutgoingCall(int playerID)
    {
        if(incomingCallMenu.activeSelf || outgoingCallMenu.activeSelf || ongoingCallMenu.activeSelf)
        {
            return;
        }
        CreateCallRequest(realtimeAvatarManager.localAvatar.ownerIDInHierarchy, playerID);
    }

    public void CreateCallRequest(int senderID, int receiverID)
    {
        //Make check if there is already an outgoing call
        //Make check if there is already an incoming call
        AddPlayerToDict(senderID, receiverID, 0);
    }

    public void MasterControlStartCall()
    {
        List<int> otherPlayers = GetOtherPlayersID();

        int key = GetKey(otherPlayers[0]);

        //If players are not yet in a call or no ongoing request
        if (key == -1)
        {
            AddPlayerToDict(otherPlayers[0], otherPlayers[1],1);
        }

        else
        {
            VoiceChatModel voiceChat = model.players[(uint)key];

            //If there is an ongoing request
            if(voiceChat.status == 0)
            {
                VoiceChatModel newVoiceChat = new VoiceChatModel();
                newVoiceChat.pairID = voiceChat.pairID;
                newVoiceChat.status = 1;
                model.players[(uint)key] = newVoiceChat;

            }

            //If they are already in a call
            else if (voiceChat.status == 1)
            {
                return;
            }
        }

    }

    public void MasterControlEndCall()
    {
        List<int> otherPlayers = GetOtherPlayersID();

        int key = GetKey(otherPlayers[0]);

        if (key == -1)
        {
            return;
        }

        else
        {
            RemovePlayerFromDict(otherPlayers[0]);
        }
    }

    private List<int> GetOtherPlayersID()
    {
        List<int> otherPlayers = new List<int>();

        foreach (var avatar in realtimeAvatarManager.avatars)
        {
            if (!avatar.Value.isOwnedLocallyInHierarchy)
            {
                otherPlayers.Add(avatar.Value.ownerIDInHierarchy);
            }
        }

        return otherPlayers;
    }

    public void AcceptIncomingCall()
    {  
        int key = GetKey(realtimeAvatarManager.localAvatar.ownerIDInHierarchy);
        if (key == -1)
        {
            return;
        }

        VoiceChatModel voiceChat = new VoiceChatModel();
        voiceChat.pairID = realtimeAvatarManager.localAvatar.ownerIDInHierarchy;
        voiceChat.status = 1;
        model.players[(uint)key] = voiceChat;
    }

    public void RejectIncomingCall()
    {
        RemovePlayerFromDict(realtimeAvatarManager.localAvatar.ownerIDInHierarchy);
    }

    public void CancelOutgoingCall()
    {
        RemovePlayerFromDict(realtimeAvatarManager.localAvatar.ownerIDInHierarchy);
    }

    public void EndOngoingCall()
    {
        RemovePlayerFromDict(realtimeAvatarManager.localAvatar.ownerIDInHierarchy);
    }

    private void AddPlayerToDict(int senderID, int receiverID, int status)
    {
        VoiceChatModel voiceChat = new VoiceChatModel();
        voiceChat.pairID = receiverID;
        voiceChat.status = status;
        model.players.Add((uint)senderID, voiceChat);
    }

    private void RemovePlayerFromDict(int playerID)
    {
        int key = GetKey(playerID);
        if(key == -1)
        {
            return;
        }

        model.players.Remove((uint)key);
    }

    private int GetKey(int playerID)
    {
        if(model.players.ContainsKey((uint)playerID))
        {
            return playerID;
        }

        else
        {
            foreach(var entry in model.players)
            {
                if(entry.Value.pairID == playerID)
                {
                    return (int)entry.Key;
                }
            }
        }

        return -1;
    }

    public void ShowIncomingCallMenu()
    {
        incomingCallMenu.SetActive(true);
    }

    public void HideIncomingCallMenu()
    {
        incomingCallMenu.SetActive(false);
    }

    public void ShowOutgoingCallMenu()
    {
        outgoingCallMenu.SetActive(true);
    }

    public void HideOutgoingCallMenu()
    {
        outgoingCallMenu.SetActive(false);
    }

    public void ShowOngoingCallmenu()
    {
        ongoingCallMenu.SetActive(true);
    }

    public void HideOngoingCallMenu()
    {
        ongoingCallMenu.SetActive(false);
    }
}
