//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System;
using System.ComponentModel;
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
    [Serializable]
    public class HoldEvent
    {
        public UnityEvent _startAction = new UnityEvent();
        public UnityEvent _endAction = new UnityEvent();
        public InputActionReference _inputReference; 
        private InputAction _inputAction;

        //DOES NOT WORK AS A CONSTRUCTOR
        /// <summary>
        /// Sets up the input action and called the action
        /// </summary>
        public void InitializeAction()
        {
            _inputAction = _inputReference.action;
            DoEvent();
        }

        /// <summary>
        /// Invokes the desiered actions.
        /// </summary>
        public void DoEvent()
        {
            Debug.Log("Hold action called.");
            _inputAction.started +=
                context =>
                {
                    Debug.Log("Start action");
                    _startAction.Invoke();
                };
            _inputAction.canceled +=
                _ =>
                {
                    Debug.Log("Ending action.");
                    _endAction.Invoke();
                };
        }
    }

    [Header("Game Settings")]
    [SerializeField] private bool _lockCursor;

    [Header("Movement")]
    [SerializeField] private UnityEvent<Vector2> _movementAction = new UnityEvent<Vector2>();
    [SerializeField] private UnityEvent _sprintAction = new UnityEvent();
    [SerializeField] private UnityEvent _crouchAction = new UnityEvent();
    [SerializeField] private HoldEvent _jumpAction = new HoldEvent();

    [Header("Tool actions")]
    [SerializeField] private UnityEvent _trapInteractionAction = new UnityEvent();
    [SerializeField] private UnityEvent _enableTrapAction = new UnityEvent();
    [SerializeField] private UnityEvent _tabletAction = new UnityEvent();
    [SerializeField] private HoldEvent _fireAction = new HoldEvent();
    [SerializeField] private UnityEvent _altFireAction = new UnityEvent();
    [SerializeField] private UnityEvent _returnToShipAction = new UnityEvent();
    [SerializeField] private UnityEvent _switchToolAction = new UnityEvent();


    private void Awake()
    {
        if(_lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
            
        _jumpAction.InitializeAction();
        _fireAction.InitializeAction();
    }
    void OnJump()
    {
        Debug.Log("OnJump called.");
        _jumpAction.DoEvent();
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
        _fireAction.DoEvent();
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
