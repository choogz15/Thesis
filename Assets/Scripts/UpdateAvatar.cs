using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;
using TMPro;

public class UpdateAvatar : MonoBehaviour
{
    private CustomAvatarManager realtimeAvatarManager;
    private RealtimeAvatar realtimeAvatar;
    private RealtimeAvatar otherAvatar;
    private string localPlayerName;
    [SerializeField] private ScoreboardSync scoreboardSync;

    [SerializeField] private Button muteButton;
    [SerializeField] private Button ringButton;
    [SerializeField] private Button unringButton;

    [SerializeField] private CallRequestboardSync callRequestboardSync;
    [SerializeField] GameObject incomingCallMenu;
    [SerializeField] GameObject outgoingCallMenu;
    [SerializeField] GameObject playersboard;

    private void Awake()
    {
        realtimeAvatarManager = GetComponent<CustomAvatarManager>();
    }

    private void OnEnable()
    {
        realtimeAvatarManager.avatarCreated += AvatarCreated;
    }

    private void OnDisable()
    {
        realtimeAvatarManager.avatarCreated -= AvatarCreated;
    }

    //TODO: fix for more than 2 players
    private void AvatarCreated(CustomAvatarManager avatarmanager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        if (isLocalAvatar)
        {
            realtimeAvatar = avatar;
            realtimeAvatar.GetComponentInChildren<NameSync>().SetText("Player " + realtimeAvatar.ownerIDInHierarchy);
        }

        else
        {
            otherAvatar = avatar;
            playersboard.GetComponent<Playersboard>().SetOtherPlayerName("Player " + otherAvatar.ownerIDInHierarchy);
        }

    }

    // Update is called once per frame
    public void SaveLocalPlayerName(TMP_InputField nameField)
    {
        localPlayerName = nameField.text;
        realtimeAvatar.GetComponentInChildren<NameSync>().SetText(localPlayerName);
    }

    public void StartRing()
    {
        realtimeAvatar.GetComponentInChildren<PlayerSync>().setIsRinging(true);
        otherAvatar.GetComponentInChildren<PlayerSync>().setIsRinging(true);
    }

    public void StopRing()
    {
        realtimeAvatar.GetComponentInChildren<PlayerSync>().setIsRinging(false);
        otherAvatar.GetComponentInChildren<PlayerSync>().setIsRinging(false);
    }

    public void AddScore()
    {
        scoreboardSync.IncrementScore((uint)realtimeAvatar.ownerIDInHierarchy);
    }

    public void MinusScore()
    {
        scoreboardSync.RemovePlayerFromDict((uint)realtimeAvatar.ownerIDInHierarchy);
    }

    public void DisplayScore()
    {
        if (realtimeAvatar != null)
        {
            Debug.Log("Local player score is: " + scoreboardSync.GetScore((uint)realtimeAvatar.ownerIDInHierarchy));
        }
        if (otherAvatar != null)
        {
            Debug.Log("Remote player score is: " + scoreboardSync.GetScore((uint)otherAvatar.ownerIDInHierarchy));
        }
    }

    public void CallOtherPlayer()
    {
        if (realtimeAvatar == null || otherAvatar == null)
        {
            Debug.Log("(ERROR) One of the avatars is equal to null");
            return;
        }

        callRequestboardSync.MakeCallRequest(otherAvatar.ownerIDInHierarchy, realtimeAvatar.ownerIDInHierarchy);
        Debug.Log("(UpdateAvatar: " + realtimeAvatar.ownerIDInHierarchy + " is trying to call " + otherAvatar.ownerIDInHierarchy);
    }

    public void AcceptCall()
    {
        callRequestboardSync.AnswerCall(realtimeAvatar.ownerIDInHierarchy);
    }

    public void RejectCall()
    {
        callRequestboardSync.RejectCallRequest(realtimeAvatar.ownerIDInHierarchy);
    }

    public void OnCallRequestAdded(int calleeID, int callerID)
    {
        //Debug.Log("(UpdateAvatar: " + callerID + " is trying to call " + calleeID);
        if (calleeID == realtimeAvatar.ownerIDInHierarchy)
        {
            incomingCallMenu.GetComponent<IncomingCall>().SetNotificationMessage(callerID);
            incomingCallMenu.SetActive(true);
        }

        else if (callerID == realtimeAvatar.ownerIDInHierarchy)
        {
            outgoingCallMenu.SetActive(true);
            outgoingCallMenu.GetComponent<OutgoingCall>().SetNotificationMessage(calleeID);
        }
    }

    public void OnCallRequestRemoved(int calleeID, int callerId)
    {
        if (calleeID == realtimeAvatar.ownerIDInHierarchy)
        {
            incomingCallMenu.SetActive(false);
        }

        else if (callerId == realtimeAvatar.ownerIDInHierarchy)
        {
            outgoingCallMenu.SetActive(false);
        }
    }

    public void OnOngoingCallAdded(int playerID, int otherPlayerID)
    {
        if (playerID == realtimeAvatar.ownerIDInHierarchy || otherPlayerID == realtimeAvatar.ownerIDInHierarchy)
        {
            return;
        }
        
        realtimeAvatarManager.avatars[playerID].GetComponentInChildren<RealtimeAvatarVoice>().mute = true;
    }

    public void OnOngoingCallRemove(int playerID, int otherPlayerID)
    {
        realtimeAvatarManager.avatars[playerID].GetComponentInChildren<RealtimeAvatarVoice>().mute = false;
    }

}
