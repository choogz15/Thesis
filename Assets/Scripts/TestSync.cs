using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using RootMotion.FinalIK;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class TestSync : RealtimeComponent<TestModel>
{
    public RealtimeAvatarVoice voice;
    public VoiceChatDictionary voiceChatDictionary;

    public Button callButton;

    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public Animator animator;

    private void Awake()
    {
        voice = GetComponentInChildren<RealtimeAvatarVoice>();
        voiceChatDictionary = GameObject.Find("VoiceChatDictionary").GetComponent<VoiceChatDictionary>();

        leftController = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();
        rightController = GameObject.Find("RightHand Controller").GetComponent<ActionBasedController>();

    }


    protected override void OnRealtimeModelReplaced(TestModel previousModel, TestModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.triggerLeftDidChange -= TriggerLeftDidChange;
            previousModel.gripLeftDidChange -= GripLeftDidChange;
            previousModel.triggerRightDidChange -= TriggerRightDidChange;
            previousModel.gripRightDidChange -= GripRightDidChange;
        }

        if (currentModel != null)
        {
            currentModel.triggerLeftDidChange += TriggerLeftDidChange;
            currentModel.gripLeftDidChange += GripLeftDidChange;
            currentModel.triggerRightDidChange += TriggerRightDidChange;
            currentModel.gripRightDidChange += GripRightDidChange;
        }
    }

    private void GripRightDidChange(TestModel model, float value)
    {
        UpdateRightGrip();
    }

    private void TriggerRightDidChange(TestModel model, float value)
    {
        UpdateRightTrigger();
    }

    private void GripLeftDidChange(TestModel model, float value)
    {
        UpdateLeftGrip();
    }

    private void TriggerLeftDidChange(TestModel model, float value)
    {
        UpdateLeftTrigger();
    }


    private void Update()
    {
        if (isOwnedLocallyInHierarchy)
        {
            model.triggerLeft = leftController.activateAction.action.ReadValue<float>();
            model.gripLeft = leftController.selectAction.action.ReadValue<float>();
            model.triggerRight = rightController.activateAction.action.ReadValue<float>();
            model.gripRight = rightController.selectAction.action.ReadValue<float>();
        }
    }

    private void UpdateLeftTrigger()
    {
        animator.SetFloat("Trigger_Left", model.triggerLeft);
    }

    private void UpdateLeftGrip()
    {
        animator.SetFloat("Grip_Left", model.gripLeft);
    }

    private void UpdateRightTrigger()
    {
        animator.SetFloat("Trigger_Right", model.triggerRight);
    }

    private void UpdateRightGrip()
    {
        animator.SetFloat("Grip_Right", model.gripRight);
    }

}
