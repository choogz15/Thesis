using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System;

public class MaleNPC1 : MonoBehaviour
{
    AudioSource audioSource;

    private bool canSpeak = true;
    public float speechInterval = 10f;

    private List<Transform> avatarTransforms;
    public RealtimeAvatarManager realtimeAvatarManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        realtimeAvatarManager.avatarCreated += AvatarCreated;
    }

    private void AvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        avatarTransforms.Add(avatar.GetComponent<Transform>());    
    }

    private void EnableSpeak()
    {
        canSpeak = true;
    }

    private void Update()
    {
        if (!canSpeak) return;

        foreach(var player in avatarTransforms)
        {
            if(Vector3.Distance(player.transform.position, transform.position) < 1)
            {
                audioSource.Play();
                canSpeak = false;
                Invoke("EnableSpeak", speechInterval);

                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Vector3.up);
            }
        }
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!canSpeak) return;

    //    Debug.Log("On Trigger Enter");
    //    audioSource.Play();
    //    canSpeak = false;
    //    Invoke("EnableSpeak", speechInterval);

    //    transform.LookAt(new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), Vector3.up);
    //}
}
