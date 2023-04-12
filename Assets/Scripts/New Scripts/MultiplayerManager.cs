using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;
using TMPro;

//This script only assumes 2 avatars, user's local avatar and one other player's avatar
public class MultiplayerManager : MonoBehaviour
{

    private RealtimeAvatar localAvatar;
    private RealtimeAvatar otherAvatar;

    [SerializeField] private RealtimeAvatarManager realtimeAvatarManager;

    //[SerializeField] private CallRequestboardSync callRequestboardSync;
    [SerializeField] GameObject incomingCallMenu;
    [SerializeField] GameObject outgoingCallMenu;

    public Button testButton;

    // Start is called before the first frame update
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
            localAvatar = avatar
        }

        else
        {
            otherAvatar = avatar;
            testButton = otherAvatar.GetComponentInChildren<Button>();

            //Assigning MultiplayerManager.CallOtherPlayer as Listener instead of directly assigning CallSync.CallOtherPlayer because the localAvatar might no be instantiated before the remote Avatar.
            testButton.onClick.AddListener(delegate { CallOtherPlayer(avatar.ownerIDInHierarchy); }) ;
            
        }

    }

    public void CallOtherPlayer(int playerID)
    {
        if(localAvatar == null)
        {
            Debug.Log("local avatar is not yet ready");
            return;
        }

        localAvatar.GetComponent<CallSync>().CallOtherPlayer(playerID);
    }

    public void TestFunction(int playerID)
    {
        Debug.Log("[MultiplayerManager:TestFunction()] " + playerID);
    }

    public void OnCallRequestAdded(int calleeID, int callerID)
    {
        //If someone is calling you, show incoming call message
        if (calleeID == localAvatar.ownerIDInHierarchy)
        {
            incomingCallMenu.SetActive(true);
        }

        //If you are calling someone, show outgoing call message
        else if (callerID == localAvatar.ownerIDInHierarchy)
        {
            outgoingCallMenu.SetActive(true);
        }
    }

    public void OnCallRequestRemoved(int calleeID, int callerID)
    {
        Debug.Log("[MultiplayerManager:OnCallRequestRemoved] To be implemented");
    }
}
