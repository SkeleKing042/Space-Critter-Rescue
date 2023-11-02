//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System;
using System.Collections;
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
        public UnityEvent Action = new UnityEvent();
        public InputActionReference InputReference;
        public float ActionDelay = 0.01f;
        private InputAction _inputAction;
        private bool _doAction;

        //DOES NOT WORK AS A CONSTRUCTOR
        /// <summary>
        /// Sets up the input action and called the action
        /// </summary>
        public void InitializeAction()
        {
            _inputAction = InputReference.action;
            DoEvent();
        }

        /// <summary>
        /// Invokes the desiered actions.
        /// </summary>
        public bool DoEvent()
        {
            //Debug.Log("Hold action called.");
            _inputAction.started +=
                _s =>
                {
                    Debug.Log("Starting action");
                    _doAction = true;
                };
            _inputAction.canceled +=
                _e =>
                {
                    Debug.Log("Ending action.");
                    _doAction = false;
                };

            return _doAction;
        }

        public IEnumerator RepeatAction(float delay)
        {
            while(_doAction)
            {
                yield return new WaitForSeconds(delay);
                Debug.Log("Doing action");
                Action.Invoke();
            }
        }
    }

    [Header("Tablet Script")]
    [SerializeField] Tablet _tabletScript;

    [Header("Game Settings")]
    [SerializeField] private bool _lockCursor;

    [Header("Movement")]
    public UnityEvent<Vector2> MovementAction = new UnityEvent<Vector2>();
    public UnityEvent SprintAction = new UnityEvent();
    public UnityEvent CrouchAction = new UnityEvent();
    public UnityEvent JumpAction = new UnityEvent();
    public HoldEvent JetPackAction = new HoldEvent();

    [Header("Tool actions")]
    public UnityEvent TrapInteractionAction = new UnityEvent();
    public UnityEvent EnableTrapAction = new UnityEvent();
    public HoldEvent FireAction = new HoldEvent();
    public UnityEvent AltFireAction = new UnityEvent();
    public UnityEvent ReturnToShipAction = new UnityEvent();
    public UnityEvent SwitchToolAction = new UnityEvent();

    [Header("UI Actions")]
    public UnityEvent ToggleTablet = new UnityEvent();
    public UnityEvent MoveTabLeft = new UnityEvent();
    public UnityEvent MoveTabRight = new UnityEvent();
    public UnityEvent MoveTeleportLeft = new UnityEvent();
    public UnityEvent MoveTeleportRight = new UnityEvent();
    public UnityEvent SelectTeleport = new UnityEvent();




    private void Awake()
    {
        if(_lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
            
        JetPackAction.InitializeAction();
        FireAction.InitializeAction();
    }
    void OnJump()
    {
        if (_tabletScript.tabletState == Tablet.TabletState.off)
        {
            Debug.Log("OnJump called.");
            JumpAction.Invoke();
        }
        else
        {
            return;
        }
    }
    void OnJetPack()
    {
        if (_tabletScript.tabletState == Tablet.TabletState.off)
        {
            Debug.Log("OnJetPack called.");
            if (JetPackAction.DoEvent()) StartCoroutine(JetPackAction.RepeatAction(JetPackAction.ActionDelay));
        }
        else
        {
            return;
        }
    }
    void OnSprint()
    {
        if (_tabletScript.tabletState == Tablet.TabletState.off)
        {
            Debug.Log("OnSprint called.");
            SprintAction.Invoke();
        }
        else
        {
            return;
        }
    }
    void OnEnableTrap()
    {
        if (_tabletScript.tabletState == Tablet.TabletState.off)
        {
            Debug.Log("Enabling trap.");
            EnableTrapAction.Invoke();
        }
        else
        {
            return;
        }
    }
    void OnPickupTrap()
    {
        if (_tabletScript.tabletState == Tablet.TabletState.off)
        {
            Debug.Log("OnPickupTrap called.");
            TrapInteractionAction.Invoke();
        }
        else
        {
            return;
        }
    }
    void OnMove(InputValue value)
    {
        if (_tabletScript.tabletState == Tablet.TabletState.off)
        {
            Debug.Log("OnMove called.");
            MovementAction.Invoke(value.Get<Vector2>());
        }
        else
        {
            return;
        }
    }
    void OnCrouch()
    {
        if (_tabletScript.tabletState == Tablet.TabletState.off)
        {
            Debug.Log("OnCrouch called.");
            CrouchAction.Invoke();
        }
        else
        {
            return;
        }
    }
    void OnToggleTablet()
    {
        Debug.Log("Toggle Tablet called");
        ToggleTablet.Invoke();
    }
    void OnFire()
    {
        if (_tabletScript.tabletState == Tablet.TabletState.off)
        {
            Debug.Log("OnFire called.");
            if (FireAction.DoEvent()) StartCoroutine(FireAction.RepeatAction(FireAction.ActionDelay));
        }
        else
        {
            return;
        }
    }
    void OnAltFire()
    {
        if (_tabletScript.tabletState == Tablet.TabletState.off)
        {
            Debug.Log("OnAltFire called.");
            AltFireAction.Invoke();
        }
        else
        {
            return;
        }
    }
    void OnReturnToShip()
    {
        Debug.Log("Attempting ship return.");
        ReturnToShipAction.Invoke();
    }
    void OnSwitchTool()
    {
        Debug.Log("Switching Tools");
        SwitchToolAction.Invoke();
    }

    void OnMoveTabLeft()
    {
        Debug.Log("Move tab left called");
        MoveTabLeft.Invoke();
    }
    void OnMoveTabRight()
    {
        Debug.Log("Move tab right called");
        MoveTabRight.Invoke();
    }
    void OnMoveTeleportLeft()
    {
        Debug.Log("Move Teleport Left Called");
        MoveTeleportLeft.Invoke();
    }
    void OnMoveTeleportRight()
    {
        Debug.Log("Move Teleport Right Called");
        MoveTeleportRight.Invoke();
    }
    void OnSelectTeleport()
    {
        Debug.Log("On select teleport called");
        SelectTeleport.Invoke();
    }




}
