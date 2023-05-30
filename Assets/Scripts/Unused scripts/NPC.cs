using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System;


public class NPC : RealtimeComponent<NPCModel>
{
    public Animator animator;
    public string animationClip;
    public double animationStartTime;
    public int stateNameHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        realtime.didConnectToRoom += DidConnectToRoom;
    }

    protected override void OnRealtimeModelReplaced(NPCModel previousModel, NPCModel currentModel)
    {
        if (previousModel != null)
        {
            model.triggerDidChange -= TriggerDidChange;
        }

        if (currentModel != null)
        {
            model.triggerDidChange += TriggerDidChange;
        }
    }

    private void TriggerDidChange(NPCModel model, int value)
    {
        animator.Rebind();
        animator.Update(0f);
    }

    void DidConnectToRoom(Realtime room)
    {
        model.trigger++;
    }

}
