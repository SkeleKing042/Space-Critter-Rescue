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

        [SerializeField] bool _sendDebugLogs = true;

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
                    if (_sendDebugLogs) Debug.Log("Starting action");
                    _doAction = true;
                };
            _inputAction.canceled +=
                _e =>
                {
                    if (_sendDebugLogs) Debug.Log("Ending action.");
                    _doAction = false;
                };

            return _doAction;
        }

        public IEnumerator RepeatAction(float delay)
        {
            while (_doAction)
            {
                yield return new WaitForSeconds(delay);
                if (_sendDebugLogs) Debug.Log("Doing action");
                Action.Invoke();
            }
        }
    }
    [System.Serializable]
    public class WhileDownEvent
    {
        public UnityEvent FirstAction = new UnityEvent();
        public UnityEvent SecondAction = new UnityEvent();
        public InputActionReference InputReference;
        private InputAction _inputAction;
        private bool _doAction;

        [SerializeField] bool _sendDebugLogs = true;
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
        public void DoEvent()
        {
            //Debug.Log("Hold action called.");
            _inputAction.started +=
                _s =>
                {
                    if (_sendDebugLogs) Debug.Log("Starting action");
                    FirstAction.Invoke();
                };
            _inputAction.canceled +=
                _e =>
                {
                    if (_sendDebugLogs) Debug.Log("Ending action.");
                    SecondAction.Invoke();
                };
        }
    }
    [System.Serializable]
    public class DoWhileDownEvent
    {
        public UnityEvent FirstAction = new UnityEvent();
        public UnityEvent SecondAction = new UnityEvent();
        public InputActionReference InputReference;
        public float ActionDelay = 0.01f;
        private InputAction _inputAction;
        private bool _doAction;

        [SerializeField] bool _sendDebugLogs = true;
        //DOES NOT WORK AS A CONSTRUCTOR
        /// <summary>
        /// Sets up the input action and called the action
        /// </summary>
        public void InitializeAction()
        {
            _inputAction = InputReference.action;
            DoEvent();
        }
        public bool DoEvent()
        {
            _inputAction.started +=
                _s =>
                {
                    if(_sendDebugLogs) Debug.Log("Starting action");
                    _doAction = true;
                };
            _inputAction.canceled +=
                _e =>
                {
                    if (_sendDebugLogs) Debug.Log("Ending action.");
                    SecondAction.Invoke();
                    _doAction = false;
                };
            return _doAction;
        }

        public IEnumerator RepeatAction(float delay)
        {
            while (_doAction)
            {

                if (_sendDebugLogs) Debug.Log("Doing action");
                FirstAction.Invoke();
                yield return new WaitForSeconds(delay);
            }
        }
    }

    [Header("Game Settings")]
    [SerializeField] private bool _lockCursor;
    [SerializeField] private bool _sendDebugLogs = true;

    [Header("Other Scripts")]
    [SerializeField] private Tablet _tablet;
    [SerializeField] private Equipment _equipment;

    [Header("Movement")]
    public UnityEvent<Vector2> MovementAction = new UnityEvent<Vector2>();
    public UnityEvent SprintAction = new UnityEvent();
    public UnityEvent CrouchAction = new UnityEvent();
    public UnityEvent JumpAction = new UnityEvent();
    public HoldEvent JetPackAction = new HoldEvent();
    public UnityEvent<Vector2> LookAction = new UnityEvent<Vector2>();

    [Header("Tool actions")]
    public UnityEvent TrapInteractionAction = new UnityEvent();
    public UnityEvent EnableTrapAction = new UnityEvent();
    public UnityEvent TabletAction = new UnityEvent();
    public DoWhileDownEvent PullAction = new DoWhileDownEvent();
    public UnityEvent ThrowTrapAction = new UnityEvent();
    public UnityEvent DetonateAction = new UnityEvent();
    public UnityEvent AltFireAction = new UnityEvent();
    public UnityEvent ReturnToShipAction = new UnityEvent();
    public UnityEvent SwitchToolAction = new UnityEvent();
    public UnityEvent DropAliensToShip = new UnityEvent();

    [Header("UI actions")]
    public UnityEvent MoveTabLeft = new UnityEvent();
    public UnityEvent MoveTabRight = new UnityEvent();
    public UnityEvent MoveTeleportLeft = new UnityEvent();
    public UnityEvent MoveTeleportRight = new UnityEvent();
    public UnityEvent SelectTeleport = new UnityEvent();

    public UnityEvent PopupAction = new UnityEvent();


    private void Awake()
    {
        if (_lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        JetPackAction.InitializeAction();
        PullAction.InitializeAction();
    }
    void OnJump()
    {
        if (!_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("OnJump called.");
            JumpAction.Invoke();
        }
    }
    void OnJetPack()
    {
        if (_sendDebugLogs) Debug.Log("OnJetPack called.");

        if (JetPackAction.DoEvent()) StartCoroutine(JetPackAction.RepeatAction(JetPackAction.ActionDelay));
    }
    void OnSprint()
    {
        if (_sendDebugLogs) Debug.Log("OnSprint called.");
        SprintAction.Invoke();
    }
    void OnEnableTrap()
    {
        if (!_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("Enabling trap.");
            EnableTrapAction.Invoke();
        }
    }
    void OnPickupTrap()
    {
        if (!_tablet.TabletState && _equipment.TrapDeployed)
        {
            if (_sendDebugLogs) Debug.Log("OnPickupTrap called.");
            TrapInteractionAction.Invoke();
        }
    }

    void OnMove(InputValue value)
    {
        if (_sendDebugLogs) Debug.Log("OnMove called.");
        MovementAction.Invoke(value.Get<Vector2>());
    }

    void OnCrouch()
    {
        if (!_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("OnCrouch called.");
            CrouchAction.Invoke();
        }
    }

    void OnToggleTablet()
    {
        if (_sendDebugLogs) Debug.Log("OnTablet called.");
        TabletAction.Invoke();
    }

    void OnFire()
    {
        if (_sendDebugLogs) Debug.Log("OnFire called.");
        if (PullAction.DoEvent()) StartCoroutine(PullAction.RepeatAction(PullAction.ActionDelay));
        ThrowTrapAction.Invoke();
        DetonateAction.Invoke();
    }

    void OnAltFire()
    {
        if (_sendDebugLogs) Debug.Log("OnAltFire called.");
        AltFireAction.Invoke();
    }

    void OnReturnToShip()
    {
        if (_sendDebugLogs) Debug.Log("Attempting ship return.");
        ReturnToShipAction.Invoke();
    }

    void OnSwitchTool()
    {
        if (_sendDebugLogs) Debug.Log("Switching Tools");
        SwitchToolAction.Invoke();
    }

    void OnMoveTabLeft()
    {
        if (_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("Move tab left called");
            MoveTabLeft.Invoke();
        }
    }
    void OnMoveTabRight()
    {
        if (_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("Move tab right called");
            MoveTabRight.Invoke();
        }
    }
    void OnMoveTeleportLeft()
    {
        if (_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("Move Teleport Left Called");
            MoveTeleportLeft.Invoke();
        }
    }
    void OnMoveTeleportRight()
    {
        if (_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("Move Teleport Right Called");
            MoveTeleportRight.Invoke();
        }
    }
    void OnSelectTeleport()
    {
        if (_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("On select teleport called");
            SelectTeleport.Invoke();
        }
    }

    void OnAlienDrop()
    {
        if (_sendDebugLogs) Debug.Log("Droping aliens");
        DropAliensToShip.Invoke();
    }
    void OnDisplayPopup()
    {
        if (_sendDebugLogs) Debug.Log("Poping up");
        PopupAction.Invoke();
    }
}
