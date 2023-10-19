//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Interperates the new unity input manager into listeners
/// </summary>
public class InputManager : MonoBehaviour
//OK, so I'm using listeners here as it makes it posible to change the invoked function in the
//editor so the other disciplines can make changes easier.
{
    [Header("Movement")]
    [Tooltip("Called when the \'D\' key is pressed")]
    public UnityEvent PositiveHorizontalMoveAction = new UnityEvent();
    public UnityEvent NeutralHorizontalMoveAction = new UnityEvent();
    [Tooltip("Called when the \'A\' key is pressed")]
    public UnityEvent NegativeHorizontalMoveAction = new UnityEvent();
    [Tooltip("Called when the \'W\' key is pressed")]
    public UnityEvent PositiveVerticalMoveAction = new UnityEvent();
    public UnityEvent NeutralVerticalMoveAction = new UnityEvent();
    [Tooltip("Called when the \'S\' key is pressed")]
    public UnityEvent NegativeVerticalMoveAction = new UnityEvent();
    [Tooltip("Called when the \'LMB\' is pressed")]
    public UnityEvent FireAction = new UnityEvent();
    [Tooltip("Called when the \'RMB\' is pressed")]
    public UnityEvent AltFireAction = new UnityEvent();
    public UnityEvent JumpAction = new UnityEvent();

    void OnFire()
    {
        FireAction.Invoke();
    }
    void OnAltFire()
    {
        AltFireAction.Invoke();
    }
    void OnMove(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();

        if (v.y > 0) PositiveHorizontalMoveAction.Invoke();
        else if (v.y == 0) NeutralHorizontalMoveAction.Invoke();
        else if (v.y < 0) NegativeHorizontalMoveAction.Invoke();

        if (v.x > 0) PositiveVerticalMoveAction.Invoke();
        else if (v.x == 0) NeutralVerticalMoveAction.Invoke();
        else if (v.x < 0) NegativeVerticalMoveAction.Invoke();
    }
    void OnJump()
    {
        JumpAction.Invoke();
    }
}
