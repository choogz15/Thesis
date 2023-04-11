using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPoseAnimator : MonoBehaviour
{

    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public Animator animator;

    private void Awake()
    {
        leftController = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();
        rightController = GameObject.Find("RightHand Controller").GetComponent<ActionBasedController>();
    }

    void Update()
    {
        //Set leftHand Animator Parameters
        animator.SetFloat("Trigger_Left", leftController.activateAction.action.ReadValue<float>());
        animator.SetFloat("Grip_Left", leftController.selectAction.action.ReadValue<float>());

        //Set rightHand Animator Parameters
        animator.SetFloat("Trigger_Right", rightController.activateAction.action.ReadValue<float>());
        animator.SetFloat("Grip_Right", rightController.selectAction.action.ReadValue<float>());

    }

    //animator.SetFloat("Trigger_" + whichHand, controller.activateAction.action.ReadValue<float>());
    //animator.SetFloat("Grip_" + whichHand, controller.selectAction.action.ReadValue<float>());
}
