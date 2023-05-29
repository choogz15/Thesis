using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System;
using UnityEngine.Animations.Rigging;

public class MaleNPC1 : MonoBehaviour
{
    AudioSource audioSource;

    private bool canSpeak = true;
    public float speechInterval = 20f;

    private List<Transform> avatarTransforms;
    public RealtimeAvatarManager realtimeAvatarManager;
    public RealtimeTransform target;

    public MultiAimConstraint constraint;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();


        //realtimeAvatarManager.avatarCreated += AvatarCreated;

        //if (target.isUnownedSelf) target.RequestOwnership();
    }

    //private void AvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    //{
    //    avatarTransforms.Add(avatar.GetComponent<Transform>());    
    //}

    private void EnableSpeak()
    {
        canSpeak = true;
    }

    //private void Update()
    //{
    //    if (!canSpeak) return;

    //    foreach(var player in avatarTransforms)
    //    {
    //        if(Vector3.Distance(player.transform.position, transform.position) < 1)
    //        {
    //            audioSource.Play();
    //            canSpeak = false;
    //            Invoke("EnableSpeak", speechInterval);

    //            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Vector3.up);
    //        }
    //    }
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (!canSpeak) return;

        Debug.Log("On Trigger Enter");
        audioSource.Play();
        canSpeak = false;
        Invoke("EnableSpeak", speechInterval);
        target.GetComponent<HeadAimTarget>().follow = other.transform;
        constraint.weight = 1;
        //transform.LookAt(new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), Vector3.up);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("On Trigger Exit");
        target.GetComponent<HeadAimTarget>().follow = null;
        constraint.weight = 0;
    }
}
