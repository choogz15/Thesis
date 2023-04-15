using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;
using TMPro;
using System;

//This script only assumes 2 avatars, user's local avatar and one other player's avatar
public class MultiplayerManager : MonoBehaviour
{
    private RealtimeAvatar localAvatar;
    private RealtimeAvatar otherAvatar;

    [SerializeField] private RealtimeAvatarManager realtimeAvatarManager;

    //Incoming Call Menu
    [SerializeField] GameObject incomingCallMenu;
    [SerializeField] Button incomingCallMenuAcceptButton;
    [SerializeField] Button incomingCallMenuRejectButton;

    //Outgoing Call Menu
    [SerializeField] GameObject outgoingCallMenu;
    [SerializeField] Button outgoingCallMenuCancelButton;


    private void Awake()
    {
        incomingCallMenuAcceptButton.onClick.AddListener(AcceptCall);
        incomingCallMenuRejectButton.onClick.AddListener(RejectCall);
    }

    private void OnEnable()
    {
        realtimeAvatarManager.avatarCreated += AvatarCreated;
    }

    private void OnDisable()
    {
        realtimeAvatarManager.avatarCreated -= AvatarCreated;
    }

    private void AvatarCreated(RealtimeAvatarManager avatarmanager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        if (isLocalAvatar)
        {
            localAvatar = avatar;

            avatar.GetComponent<CallSync>().dialingPlayerChangedEvent.AddListener(LocalAvatarDialingPlayerDidChange);
        }

        else
        {
            otherAvatar = avatar;

            avatar.GetComponent<CallSync>().dialingPlayerChangedEvent.AddListener(RemoteAvatarDialingPlayerDidChange);

            //Assigning MultiplayerManager.CallOtherPlayer as Listener instead of directly assigning CallSync.CallOtherPlayer because the localAvatar might no be instantiated before the remote Avatar.
            avatar.GetComponent<CallSync>().callButton.onClick.AddListener(delegate { CallOtherPlayer(avatar.ownerIDInHierarchy); });

        }

    }


    public void LocalAvatarDialingPlayerDidChange(CallSyncModel model, int calleeID, int callerID)
    {
        //Debug.Log("LocalAvatarDialingPlayerDidChange");
        if (calleeID >= 0)
        {
            ShowOutgoingCallMenu();
        }
        else
        {
            HideOutgoingCallMenu();
        }
    }

    public void RemoteAvatarDialingPlayerDidChange(CallSyncModel model, int calleeID, int callerID)
    {
        if (localAvatar == null) return;

        if (calleeID == localAvatar.ownerIDInHierarchy)
        {
            ShowIncomingCallMenu();
            localAvatar.GetComponent<CallSync>().SetDialerPlayer(callerID);
        }
    }

    public void CallOtherPlayer(int playerID)
    {
        if (localAvatar == null)
        {
            Debug.Log("local avatar is not yet ready");
            return;
        }

        localAvatar.GetComponent<CallSync>().CallOtherPlayer(playerID);
    }

    public void AcceptCall()
    {
        localAvatar.GetComponent<CallSync>().AcceptCall();
    }

    public void RejectCall()
    {

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
}
