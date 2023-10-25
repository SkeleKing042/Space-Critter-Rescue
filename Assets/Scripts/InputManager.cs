//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UIElements;

/// <summary>
/// Interperates the new unity input manager into listeners
/// </summary>
public class InputManager : MonoBehaviour
//OK, so I'm using listeners here as it makes it posible to change the invoked function in the
//editor so the other disciplines can make changes easier.
{
    [Header("Movement")]
    [SerializeField] private UnityEvent<Vector2> MovementAction = new UnityEvent<Vector2>();
    [SerializeField] private UnityEvent SprintAction = new UnityEvent();
    [SerializeField] private UnityEvent CrouchAction = new UnityEvent();
    private Vector2 movementAxis;
    [Header("Jump actions")]
    [SerializeField] private UnityEvent JumpHoldAction = new UnityEvent();
    [SerializeField] private UnityEvent JumpAction = new UnityEvent();
    [SerializeField] private InputActionReference JumpActRef;

    [Header("Tool actions")]
    [SerializeField] private UnityEvent TrapInteractionAction = new UnityEvent();
    [SerializeField] private UnityEvent TabletAction = new UnityEvent();
    [SerializeField] private UnityEvent FireAction = new UnityEvent();
    [SerializeField] private UnityEvent AltFireAction = new UnityEvent();

    void OnJump()
    {
        InputAction jumpAct = JumpActRef.action;

        jumpAct.started +=
            context =>
            {
                if (context.interaction is HoldInteraction)
                {
                    Debug.Log("OnJump Tapped");
                    JumpHoldAction.Invoke();
                }
            };

        jumpAct.performed +=
            context =>
            {
                if (context.interaction is HoldInteraction)
                {
                    Debug.Log("OnJump Held");
                    JumpHoldAction.Invoke();
                }
                /*else
                {
                    Debug.Log("OnJump tapped");
                    JumpAction.Invoke();

                }*/
            };
        //jumpAct.canceled +=
            //_ => endFunct();
    }
    void OnSprint()
    {
        Debug.Log("OnSprint called.");
        SprintAction.Invoke();
    }
    void OnPickupTrap()
    {
        Debug.Log("OnPickupTrap called.");
        TrapInteractionAction.Invoke();
    }
    void OnMove(InputValue value)
    {
        Debug.Log("OnMove called.");
        MovementAction.Invoke(value.Get<Vector2>());
    }
    void OnCrouch()
    {
        Debug.Log("OnCrouch called.");
        CrouchAction.Invoke();
    }
    void OnLook()
    {
        //Debug.Log("OnLook called.");
    }
    void OnTablet()
    {
        Debug.Log("OnTablet called.");
        TabletAction.Invoke();
    }
    void OnFire()
    {
        Debug.Log("OnFire called.");
        FireAction.Invoke();
    }
    void OnAltFire()
    {
        Debug.Log("OnAltFire called.");
        AltFireAction.Invoke();
    }
}
