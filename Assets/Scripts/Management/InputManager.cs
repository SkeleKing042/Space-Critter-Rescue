//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System;
using System.Collections;
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

    //[Header("Other Scripts")]
    private Tablet _tablet;
    private Equipment _equipment;

    [Header("Movement")]
    public UnityEvent<Vector2> MovementAction = new UnityEvent<Vector2>();
    public UnityEvent SprintAction = new UnityEvent();
    public UnityEvent CrouchAction = new UnityEvent();
    public UnityEvent JumpAction = new UnityEvent();
    public HoldEvent JetPackAction = new HoldEvent();
    //public UnityEvent<Vector2> LookAction = new UnityEvent<Vector2>();

    [Header("Tool actions")]
    public UnityEvent SwitchToolAction = new UnityEvent();

    [Header("Vacuum")]
    public DoWhileDownEvent PullAction = new DoWhileDownEvent();
    [Space]
    public UnityEvent DepositCritters = new UnityEvent();

    [Header("Trap")]
    public UnityEvent ThrowTrapAction = new UnityEvent();
    public UnityEvent DetonateTrapAction = new UnityEvent();
    public UnityEvent PickUpTrapAction = new UnityEvent();

    [Header("Tablet")]
    public UnityEvent TabletAction = new UnityEvent();
    [Space]
    public UnityEvent MoveTabLeft = new UnityEvent();
    public UnityEvent MoveTabRight = new UnityEvent();
    [Space]
    public UnityEvent<Vector2> MoveTeleport = new UnityEvent<Vector2>();
    public UnityEvent SelectTeleport = new UnityEvent();

    [Header("misc")]
    public UnityEvent ReturnToShipAction = new UnityEvent();
    public UnityEvent PopupAction = new UnityEvent();

    /* UNSURE IF NEEDED
     * public UnityEvent EnableTrapAction = new UnityEvent();
     * public UnityEvent AltFireAction = new UnityEvent();
     */

    #region Awake

    private void Awake()
    {
        if (_lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        _tablet = FindObjectOfType<Tablet>();
        if (!_tablet)
        {
            Debug.LogWarning("InputManager couldn't find the tablet.");
            enabled = false;
            return;
        }
        _equipment = FindObjectOfType<Equipment>();
        if (!_equipment)
        {
            Debug.LogWarning("InputManager couldn't find the tablet.");
            enabled = false;
            return;
        }

        JetPackAction.InitializeAction();
        PullAction.InitializeAction();
    }

    #endregion

    #region Movement
    //movement Input
    void OnMove(InputValue value)
    {
        if (!_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("OnMove called.");
            MovementAction.Invoke(value.Get<Vector2>());
        }
    }

    //Sprint Input
    void OnSprint()
    {
        if (!_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("OnSprint called.");
            SprintAction.Invoke();
        }
    }

    //Crouch Movement
    void OnCrouch()
    {
        if (!_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("OnCrouch called.");
            CrouchAction.Invoke();
        }
    }

    //jump action
    void OnJump()
    {
        if (!_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("OnJump called.");
            JumpAction.Invoke();
        }
    }

    //jetpack action
    void OnJetPack()
    {
        if (_sendDebugLogs) Debug.Log("OnJetPack called.");

        if (JetPackAction.DoEvent()) StartCoroutine(JetPackAction.RepeatAction(JetPackAction.ActionDelay));
    }



    #endregion

    #region Tool Actions
    void OnSwitchTool()
    {
        if (!_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("Switching Tools");
            SwitchToolAction.Invoke();
        }
    }

    #endregion

    #region Vaccuum Catcher
    void OnPull()
    {
        if (!_tablet.TabletState && _equipment._currentlyHolding == Equipment.CurrentlyHolding.VC)
        {
            if (_sendDebugLogs) Debug.Log("OnFire called.");
            if (PullAction.DoEvent()) StartCoroutine(PullAction.RepeatAction(PullAction.ActionDelay));
        }
    }

    void OnDepositCritters()
    {
        if (!_tablet.TabletState && _equipment._currentlyHolding == Equipment.CurrentlyHolding.VC)
        {
            if (_sendDebugLogs) Debug.Log("Dropping aliens");
            DepositCritters.Invoke();
        }
    }

    #endregion

    #region Trap
    //throw trap input
    void OnThrowTrap()
    {
        if (!_tablet.TabletState && !_equipment.TrapDeployed && _equipment._currentlyHolding == Equipment.CurrentlyHolding.trap)
        {
            if (_sendDebugLogs) Debug.Log("OnThrowTrapAction called.");
            ThrowTrapAction.Invoke();
        }
    }

    //detonate trap input
    void OnDetonateTrap()
    {
        if (!_tablet.TabletState && _equipment._currentlyHolding == Equipment.CurrentlyHolding.detonator)
        {
            if (_sendDebugLogs) Debug.Log("OnDetonateTrap called.");
            DetonateTrapAction.Invoke();
        }
    }

    //pickup trap input
    void OnPickupTrap()
    {
        if (!_tablet.TabletState && _equipment.TrapDeployed)
        {
            if (_sendDebugLogs) Debug.Log("OnPickupTrap called.");
            PickUpTrapAction.Invoke();
        }
    }

    #endregion

    #region Tablet
    void OnToggleTablet()
    {
        if (_sendDebugLogs) Debug.Log("OnTablet called.");
        TabletAction.Invoke();
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
    void OnMoveTeleport(InputValue value)
    {
        if (_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("Move Teleport Left Called");
            MoveTeleport.Invoke(value.Get<Vector2>());
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

    #endregion

    #region misc
    void OnDisplayPopup()
    {
        if (_sendDebugLogs) Debug.Log("Poping up");
        PopupAction.Invoke();
    }

    void OnReturnToShip()
    {
        if (_sendDebugLogs) Debug.Log("Attempting ship return.");
        ReturnToShipAction.Invoke();
    }

    #endregion
}

#region Old Code

/*void OnEnableTrap()
    {
        if (!_tablet.TabletState)
        {
            if (_sendDebugLogs) Debug.Log("Enabling trap.");
            EnableTrapAction.Invoke();
        }
    }*/


/*void OnFire()
    {
        if (_sendDebugLogs) Debug.Log("OnFire called.");
        if (PullAction.DoEvent()) StartCoroutine(PullAction.RepeatAction(PullAction.ActionDelay));
        ThrowTrapAction.Invoke();
        DetonateTrapAction.Invoke();
    }

    void OnAltFire()
    {
        if (_sendDebugLogs) Debug.Log("OnAltFire called.");
        AltFireAction.Invoke();
    }
    */

#endregion
