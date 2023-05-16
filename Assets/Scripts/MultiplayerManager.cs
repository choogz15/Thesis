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

    [SerializeField] private RealtimeAvatarManager realtimeAvatarManager;

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
        incomingCallMenuAcceptButton.onClick.AddListener(AcceptIncomingCall);
        incomingCallMenuRejectButton.onClick.AddListener(RejectIncomingCall);
        outgoingCallMenuCancelButton.onClick.AddListener(CancelOutgoingCall);
        //ongoingCallMenuEndButton.onClick.AddListener(EndOngoingCall);
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

            avatar.GetComponent<CallSync>().outgoingCallRequested.AddListener(LocalAvatarOnOutgoingCallRequested);
            avatar.GetComponent<CallSync>().outgoingCallCancelled.AddListener(LocalAvatarOnOutgoingCallCancelled);
            avatar.GetComponent<CallSync>().incomingCallRequested.AddListener(LocalAvatarOnIncomingCallRequested);
            avatar.GetComponent<CallSync>().incomingCallCancelled.AddListener(LocalAvatarOnIncomingCallCancelled);
            avatar.GetComponent<CallSync>().incomingCallAccepted.AddListener(LocalAvatarOnIncomingCallAccepted);
        }

        else
        {
            avatar.GetComponent<CallSync>().outgoingCallRequested.AddListener(RemoteAvatarOnOutgoingCallRequested);
            avatar.GetComponent<CallSync>().incomingCallAccepted.AddListener(RemoteAvatarOnIncomingCallAccepted);
            
            //Assigning MultiplayerManager.CallOtherPlayer as Listener instead of directly assigning CallSync.CallOtherPlayer because the localAvatar might no be instantiated before the remote Avatar.
            avatar.GetComponent<CallSync>().callButton.onClick.AddListener(delegate { MakeOutgoingCall(avatar.ownerIDInHierarchy); });
        }

    }

    private void LocalAvatarOnIncomingCallAccepted(CallSyncModel arg0, int arg1)
    {
        ShowOngoingCallmenu();
    }

    private void RemoteAvatarOnIncomingCallCancelled(CallSyncModel model, int arg1)
    {
        localAvatar.GetComponent<CallSync>().OutgoingCallRejected();

    }



    //LOCAL AVATAR LISTENERS
    private void LocalAvatarOnIncomingCallCancelled(CallSyncModel model, int arg1)  //arg1 not used. Will be equal to -1 when cancelled
    {
        Debug.Log("Incoming Call cancelled");
        HideIncomingCallMenu();
    }

    private void LocalAvatarOnIncomingCallRequested(CallSyncModel model, int callerID)
    {
        Debug.Log("Incoming Call requested");
        ShowIncomingCallMenu();
        
    }

    private void LocalAvatarOnOutgoingCallRequested(CallSyncModel model, int calleeID)
    {
        ShowOutgoingCallMenu();
        realtimeAvatarManager.avatars[calleeID].GetComponent<CallSync>().incomingCallCancelled.AddListener(RemoteAvatarOnIncomingCallCancelled);

    }
    private void LocalAvatarOnOutgoingCallCancelled(CallSyncModel model, int arg1)  //arg1 not used. Will be equal to -1 when cancelled
    {
        HideOutgoingCallMenu();
    }

    
    //REMOTE AVATAR LISTENERS
    private void RemoteAvatarOnOutgoingCallRequested(CallSyncModel model, int calleeID)
    {
        if (localAvatar != null && localAvatar.ownerIDInHierarchy == calleeID)
        {
            localAvatar.GetComponent<CallSync>().IncomingCallReceived(model.ownerIDInHierarchy);
            realtimeAvatarManager.avatars[model.ownerIDInHierarchy].GetComponent<CallSync>().outgoingCallCancelled.AddListener(RemoteAvatarOnOutgoingCallCancelled);
        }
    }

    private void RemoteAvatarOnOutgoingCallCancelled(CallSyncModel model, int calleeID) //arg1 not used. Will be equal to -1 when cancelled
    {
        localAvatar.GetComponent<CallSync>().IncomingCallCancelled();
        realtimeAvatarManager.avatars[model.ownerIDInHierarchy].GetComponent<CallSync>().outgoingCallCancelled.RemoveListener(RemoteAvatarOnOutgoingCallCancelled);
    }
    private void RemoteAvatarOnIncomingCallAccepted(CallSyncModel model, int callerID)
    {
        if (localAvatar != null && localAvatar.ownerIDInHierarchy == callerID)
        {
            ShowOngoingCallmenu();
            localAvatar.GetComponent<CallSync>().OutgoingCallAccepted();
        }
    }

    public void MakeOutgoingCall(int playerID)
    {
        if (localAvatar == null)
        {
            Debug.Log("local avatar is null");
            return;
        }

        localAvatar.GetComponent<CallSync>().MakeOutgoingCall(playerID);
    }

    public void CancelOutgoingCall()
    {
        if (localAvatar == null)
        {
            Debug.Log("local avatar is null");
            return;
        }
        localAvatar.GetComponent<CallSync>().CancelOutgoingCall();
    }

    public void AcceptIncomingCall()
    {
        localAvatar.GetComponent<CallSync>().AcceptIncomingCall();
    }

    public void RejectIncomingCall()
    {
        localAvatar.GetComponent<CallSync>().RejectIncomingCall();
    }
    private void EndOngoingCall()
    {
        localAvatar.GetComponent<CallSync>().EndOnGoingCall();
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
