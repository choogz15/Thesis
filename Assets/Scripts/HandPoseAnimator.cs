using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPoseAnimator : MonoBehaviour
{

    public ActionBasedController controller;
    public Animator animator;
    public string whichHand = "";

    void Update()
    {
        animator.SetFloat("Trigger_" + whichHand, controller.activateAction.action.ReadValue<float>());
        animator.SetFloat("Grip_" + whichHand, controller.selectAction.action.ReadValue<float>());
    }
}
