using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAnimator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Animator _arm_animator;

    [Header("Trigger Strings")]
    [SerializeField]
    private string[] _triggerStrings;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Trigger_ArmDown()
    {
        _arm_animator.SetTrigger(_triggerStrings[0]);
    }

    private void Trigger_ArmUp()
    {
        _arm_animator.SetTrigger(_triggerStrings[1]);
    }

    private void Trigger_Idle()
    {
        _arm_animator.SetTrigger(_triggerStrings[2]);
    }

    private void Trigger_Run()
    {
        _arm_animator.SetTrigger(_triggerStrings[3]);
    }

    private void Trigger_TabletUp()
    {
        _arm_animator.SetTrigger(_triggerStrings[4]);
    }

    private void Trigger_TabletDown()
    {
        _arm_animator.SetTrigger(_triggerStrings[5]);
    }

    private void Trigger_TriggerActivated()
    {
        _arm_animator.SetTrigger(_triggerStrings[6]);
    }

    private void Trigger_TriggerOut()
    {
        _arm_animator.SetTrigger(_triggerStrings[7]);
    }
}
