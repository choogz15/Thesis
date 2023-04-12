using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using RootMotion.FinalIK;

public class HandPoseAnimator : MonoBehaviour
{
    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public Animator animator;
    public VRIK ik;
    public float avatarModelHeight;


    public InputAction calibrateAction;

    public Recenter recenter;


    public bool isScaled = false;

    private void Awake()
    {
        leftController = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();
        rightController = GameObject.Find("RightHand Controller").GetComponent<ActionBasedController>();
        recenter = GameObject.Find("XR Origin").GetComponent<Recenter>();

        calibrateAction.performed += CalibrateAvatar;

        avatarModelHeight = ik.references.head.position.y - ik.references.root.position.y;
    }

    void Update()
    {
        //Set leftHand Animator Parameters
        animator.SetFloat("Trigger_Left", leftController.activateAction.action.ReadValue<float>());
        animator.SetFloat("Grip_Left", leftController.selectAction.action.ReadValue<float>());

        //Set rightHand Animator Parameters
        animator.SetFloat("Trigger_Right", rightController.activateAction.action.ReadValue<float>());
        animator.SetFloat("Grip_Right", rightController.selectAction.action.ReadValue<float>());

        ////Scale the avatar to user's height
        //if(leftController.activateAction.action.ReadValue<float>() > 0.5 && isScaled == false)
        //{
        //    //ScaleAvatar();          
        //}

    }

    private void OnEnable()
    {
        calibrateAction.Enable();
    }

    private void OnDisable()
    {
        calibrateAction.Disable();
    }

    void CalibrateAvatar(InputAction.CallbackContext context)
    {
        float sizeF = (ik.solver.spine.headTarget.position.y - ik.references.root.position.y) / avatarModelHeight;
        //ik.references.root.localScale *= sizeF;
        ik.references.root.localScale = Vector3.one * sizeF;
        Debug.Log("Scaling avatar");
        recenter.HidePlaceHolders();
    }

    void ScaleAvatar()
    {
        float sizeF = (ik.solver.spine.headTarget.position.y - ik.references.root.position.y) / avatarModelHeight;
        ik.references.root.localScale *= sizeF;
        Debug.Log("Scaling avatar");
        isScaled = true;
    }

    //animator.SetFloat("Trigger_" + whichHand, controller.activateAction.action.ReadValue<float>());
    //animator.SetFloat("Grip_" + whichHand, controller.selectAction.action.ReadValue<float>());
}
