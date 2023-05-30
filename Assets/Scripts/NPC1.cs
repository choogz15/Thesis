using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.Animations.Rigging;
using System;

public class NPC1 : RealtimeComponent<NPCModel1>
{
    public float speechInterval;
    public RealtimeTransform target;
    public MultiAimConstraint constraint;
    public int targetID;

    private AudioSource audioSource;
    public bool canSpeak = true;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnRealtimeModelReplaced(NPCModel1 previousModel, NPCModel1 currentModel)
    {
        if(previousModel != null)
        {
            previousModel.followConstraintDidChange -= FollowConstraintDidChange;
            previousModel.triggerAudioDidChange -= TriggerAudioDidChange;
        }

        if(currentModel != null)
        {
            currentModel.followConstraintDidChange += FollowConstraintDidChange;
            currentModel.triggerAudioDidChange += TriggerAudioDidChange;
        }
    }

    private void TriggerAudioDidChange(NPCModel1 model, int value)
    {
        Talk();
    }

    private void FollowConstraintDidChange(NPCModel1 model, bool value)
    {
        UpdateConstraintWeight();
    }

    void Talk()
    {
        audioSource.Play();
    }

    void UpdateConstraintWeight()
    {
        constraint.weight = model.followConstraint? 1: 0;
    }

    private void EnableSpeak()
    {
        canSpeak = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canSpeak) return;

        if (realtime.clientID != 0) return; //only client 0 should process logic
        
        model.triggerAudio++; 
        model.followConstraint = true;
        target.GetComponent<HeadAimTarget>().follow = other.transform;
        targetID = other.gameObject.GetComponent<RealtimeTransform>().ownerIDInHierarchy;
        canSpeak = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (realtime.clientID != 0) return;
        
        if(other.GetComponent<RealtimeTransform>().ownerIDInHierarchy == targetID)
        {
            targetID = -1;
            target.GetComponent<HeadAimTarget>().follow = null;
            model.followConstraint = false;
            Invoke("EnableSpeak", speechInterval);
        }
    }
}
