using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Normal.Realtime;
using TMPro;
using System;

public class StartMenu : MonoBehaviour
{
    public string roomName;
    public int waitingForPlayers; //number of players before game starts
    public float playerHeight;
    public GameObject[] avatarPanels;
    public GameObject[] avatarPrefabs;
    public GameObject[] xrControllerModels;
    public GameObject defaultPanel;
    public GameObject waitingPanel;
    public TextMeshProUGUI message;

    private int selectedAvatar;
    private Realtime realtime;
    private RealtimeAvatarManager realtimeAvatarManager;

    public AudioSource audio;

    public List<Animator> animators;
    public List<AudioSource> audioSources;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject realtimeVRPlayer = GameObject.Find("Realtime + VR Player");
        realtime = realtimeVRPlayer.GetComponent<Realtime>();
        realtimeAvatarManager = realtime.GetComponent<RealtimeAvatarManager>();
        realtimeAvatarManager.avatarCreated += AvatarCreated;

        //Set first avatar as default selected
        SelectAvatar(0);
    }

    private void AvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        waitingForPlayers--;
        if (waitingForPlayers == 0)
        {
            StartGame();
        }
    }

    public void SelectAvatar(int avatarID)
    {
        selectedAvatar = avatarID;

        for (int i = 0; i < avatarPanels.Length; i++)
        {
            if (i == selectedAvatar)
            {
                avatarPanels[i].SetActive(true);
            }
            else
            {
                avatarPanels[i].SetActive(false);
            }
        }

    }

    public void ConnectToGame()
    {
        playerHeight = GameObject.Find("Main Camera").GetComponent<Transform>().position.y;

        defaultPanel.SetActive(false);
        waitingPanel.SetActive(true);
        realtimeAvatarManager.localAvatarPrefab = avatarPrefabs[selectedAvatar];
        realtime.didConnectToRoom += Connected;
        realtime.Connect(roomName);
    }

    private void Connected(Realtime room)
    {
        message.text = "WAITING FOR OTHER PLAYER";

        foreach (var model in xrControllerModels)
        {
            model.SetActive(false);
        }

    }

    private void StartGame()
    {
        ResetAnimations();
        ResetAudioSources();
        gameObject.SetActive(false);
    }

    //Reset animations when everyone has joined so that all players will see matching animations
    void ResetAnimations()
    {
        foreach (var animator in animators)
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }

    //Reset audio when everyone has joined so that all players will hear matching audio
    void ResetAudioSources()
    {
        foreach (var audioSource in audioSources)
        {
            audioSource.Play();
        }

    }
}
