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
    [SerializeField] private UnityEvent<Vector2> _movementAction = new UnityEvent<Vector2>();
    [SerializeField] private UnityEvent _sprintAction = new UnityEvent();
    [SerializeField] private UnityEvent _crouchAction = new UnityEvent();

    [Header("Jump actions")]
    [SerializeField] private UnityEvent _jumpAction = new UnityEvent();
    [SerializeField] private UnityEvent _endJumpAction = new UnityEvent();
    [SerializeField] private InputActionReference _jumpActRef;
    private InputAction _jumpInputAct;

    [Header("Tool actions")]
    [SerializeField] private UnityEvent _trapInteractionAction = new UnityEvent();
    [SerializeField] private UnityEvent _enableTrapAction = new UnityEvent();
    [SerializeField] private UnityEvent _tabletAction = new UnityEvent();
    [SerializeField] private UnityEvent _fireAction = new UnityEvent();
    [SerializeField] private UnityEvent _endFireAction = new UnityEvent();
    [SerializeField] private InputActionReference _fireActRef;
    private InputAction _fireInputAct;
    [SerializeField] private UnityEvent _altFireAction = new UnityEvent();
    [SerializeField] private UnityEvent _returnToShipAction = new UnityEvent();
    [SerializeField] private UnityEvent _switchToolAction = new UnityEvent();

    private void Awake()
    {
        _jumpInputAct = _jumpActRef.action;
        _fireInputAct = _fireActRef.action;
    }
    void OnJump()
    {
        Debug.Log("Jump input recived.");
        _jumpInputAct.started +=
            context =>
            {
                Debug.Log("Jump started");
                _jumpAction.Invoke();
            };
        _jumpInputAct.canceled +=
            _ =>
            {
                Debug.Log("Jump ended");
                _endJumpAction.Invoke();
            };
    }
    void OnSprint()
    {
        Debug.Log("OnSprint called.");
        _sprintAction.Invoke();
    }
    void OnEnableTrap()
    {
        Debug.Log("Enabling trap.");
        _enableTrapAction.Invoke();
    }
    void OnPickupTrap()
    {
        Debug.Log("OnPickupTrap called.");
        _trapInteractionAction.Invoke();
    }
    void OnMove(InputValue value)
    {
        Debug.Log("OnMove called.");
        _movementAction.Invoke(value.Get<Vector2>());
    }
    void OnCrouch()
    {
        Debug.Log("OnCrouch called.");
        _crouchAction.Invoke();
    }
    void OnTablet()
    {
        Debug.Log("OnTablet called.");
        _tabletAction.Invoke();
    }
    void OnFire()
    {
        Debug.Log("OnFire called.");
        _fireInputAct.started +=
            context =>
            {
                Debug.Log("Fire started");
                _fireAction.Invoke();
            };
        _fireInputAct.canceled +=
            _ =>
            {
                Debug.Log("Stopped firing.");
                _endFireAction.Invoke();
            };
    }
    void OnAltFire()
    {
        Debug.Log("OnAltFire called.");
        _altFireAction.Invoke();
    }
    void OnReturnToShip()
    {
        Debug.Log("Attempting ship return.");
        _returnToShipAction.Invoke();
    }
    void OnSwitchTool()
    {
        Debug.Log("Switching Tools");
        _switchToolAction.Invoke();
    }
}
