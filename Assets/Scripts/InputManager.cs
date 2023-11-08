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
    public UnityEvent TabletAction = new UnityEvent();
    public HoldEvent FireAction = new HoldEvent();
    public UnityEvent AltFireAction = new UnityEvent();
    public UnityEvent ReturnToShipAction = new UnityEvent();
    public UnityEvent SwitchToolAction = new UnityEvent();
    public UnityEvent DropAliensToShip = new UnityEvent();

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
        Debug.Log("OnJump called.");
        JumpAction.Invoke();
    }
    void OnJetPack()
    {
        Debug.Log("OnJetPack called.");
        if (JetPackAction.DoEvent()) StartCoroutine(JetPackAction.RepeatAction(JetPackAction.ActionDelay));
    }
    void OnSprint()
    {
        Debug.Log("OnSprint called.");
        SprintAction.Invoke();
    }
    void OnEnableTrap()
    {
        Debug.Log("Enabling trap.");
        EnableTrapAction.Invoke();
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
    void OnTablet()
    {
        Debug.Log("OnTablet called.");
        TabletAction.Invoke();
    }
    void OnFire()
    {
        Debug.Log("OnFire called.");
        if(FireAction.DoEvent()) StartCoroutine(FireAction.RepeatAction(FireAction.ActionDelay));
    }
    void OnAltFire()
    {
        Debug.Log("OnAltFire called.");
        AltFireAction.Invoke();
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
    void OnAlienDrop()
    {
        Debug.Log("Dropping of aliens...");
        DropAliensToShip.Invoke();
    }
}
