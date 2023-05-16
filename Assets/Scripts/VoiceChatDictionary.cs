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
        }

        if (currentModel != null)
        {
            currentModel.players.modelAdded += voiceChatAdded;
            currentModel.players.modelRemoved += voiceChatRemoved;
        }
    }


    private void voiceChatAdded(RealtimeDictionary<VoiceChatModel> dictionary, uint key, VoiceChatModel model, bool remote)
    {
        //Mute other players if they are in private voice chat mode
        if(key != realtimeAvatarManager.localAvatar.ownerIDInHierarchy && model.pairID !=  realtimeAvatarManager.localAvatar.ownerIDInHierarchy)
        {
            realtimeAvatarManager.avatars[(int)key].GetComponentInChildren<RealtimeAvatarVoice>().mute = true;
            realtimeAvatarManager.avatars[model.pairID].GetComponentInChildren<RealtimeAvatarVoice>().mute = true;
        }

        if(key == realtimeAvatarManager.localAvatar.ownerIDInHierarchy || model.pairID == realtimeAvatarManager.localAvatar.ownerIDInHierarchy)
        {
            PrivateVoiceChatStarted();
        }

    }

    private void voiceChatRemoved(RealtimeDictionary<VoiceChatModel> dictionary, uint key, VoiceChatModel model, bool remote)
    {
        if (key != realtimeAvatarManager.localAvatar.ownerIDInHierarchy && model.pairID != realtimeAvatarManager.localAvatar.ownerIDInHierarchy)
        {
            realtimeAvatarManager.avatars[(int)key].GetComponentInChildren<RealtimeAvatarVoice>().mute = false;
            realtimeAvatarManager.avatars[model.pairID].GetComponentInChildren<RealtimeAvatarVoice>().mute = false;
        }

        if (key == realtimeAvatarManager.localAvatar.ownerIDInHierarchy || model.pairID == realtimeAvatarManager.localAvatar.ownerIDInHierarchy)
        {
            PrivateVoiceChatEnded();
        }
    }

    public void PrivateVoiceChatStarted()
    {
        ShowOngoingCallmenu();
    }

    public void PrivateVoiceChatEnded()
    {
        HideOngoingCallMenu();
    }

    public void MakeOutgoingCall(int playerID)
    {
        RequestVoiceChat(realtimeAvatarManager.localAvatar.ownerIDInHierarchy, playerID);
    }

    public void RequestVoiceChat(int senderID, int receiverID)
    {
        //Make check if there is already an outgoing call
        //Make check if there is already an incoming call
        AddPlayerToDict(senderID, receiverID, 0);
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
        int key = GetKeyToRemove(playerID);
        if(key == -1)
        {
            return;
        }

        model.players.Remove((uint)key);
    }

    private int GetKeyToRemove(int playerID)
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
