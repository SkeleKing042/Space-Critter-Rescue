//Created by Ru McPharlin
//Last Edited by Ru

using UnityEngine;

public class Equipment : MonoBehaviour
{
    //Variables
    #region Variables
    /*
     * To keep in mind
        - LB to toggle holding trap
        - LT to throw trap if holding
     */

    [Header("Camera")]
    [SerializeField]
    private GameObject _playerCamera;

    [Header("Currently Holding")]
    [SerializeField]
    public CurrentlyHolding _currentlyHolding;

    // Trap Components 


    //[Header("Trap Components")]
    //public GameObject _trap;
    //[SerializeField]
    //public GameObject Bubble;

    // trap forces 
    //[SerializeField]
    //private float PickUpRange;
    [Header("Trap Variables")]
    private GameObject _trapGameObject;
    private Trap _trap;
    [SerializeField]
    private GameObject _trapPrefab;
    [SerializeField]
    private float TrapThrowForce;
    private bool _trapDeployed;
    public bool TrapDeployed { get { return _trapDeployed; } }

    private bool _canPickUpTrap;
    public bool CanPickUpTrap { get { return _canPickUpTrap; } }
    //private Transform _trapPos;

    // player Components
    [Header("Player Components")]
    //[SerializeField]
    //private Tablet _tablet;
    //[SerializeField]
    //private Trap _trap;
    [SerializeField]
    private Transform _trapParent;

    //[Header("UI")]
    private UI_Manager _UI_Manager;

    //[Header("Equipment Animator")]
    private Animator _Equipment_Animator;

    

    /// <summary>
    /// players eqipment
    /// </summary>
    public enum CurrentlyHolding
    {
        VC,
        trap,
        detonator
    }
    #endregion

    //Methods
    #region Start & Update
    /// <summary>
    /// set starting values in scene
    /// </summary>
    void Awake()
    {
        _trap = FindObjectOfType<Trap>();
        // get rigidbody

        _UI_Manager = FindObjectOfType<UI_Manager>();

        _Equipment_Animator = GetComponent<Animator>();

        //bring up the VC
        _animation_VC_Up();

        //get the tablet script
        //_tablet = FindObjectOfType<Tablet>();

        // currently holding set to vacuum & set trap parent
        _currentlyHolding = CurrentlyHolding.VC;

    }

    void Update()
    {
        //checks if the player can pick up the trap
        if (_trapDeployed && _trap.Bubble.activeSelf == false)
            _canPickUpTrap = true;
        else
            _canPickUpTrap = false;


        //if the player can pick up the trap
        if(_canPickUpTrap)
        {
            //if the player currently holding the detonator and the ui state is not accurate
            if(_currentlyHolding == CurrentlyHolding.detonator && _UI_Manager.Get_UIState != UI_Manager.UIState.Detonator_PickUpTrap_VC)
            {
                //set the state
                _UI_Manager.SetUIState(UI_Manager.UIState.Detonator_PickUpTrap_VC);
            }
        }

        //if the ui state is pickup trap and the player CANNOT pick up the trap
        if(_UI_Manager.Get_UIState == UI_Manager.UIState.Detonator_PickUpTrap_VC && !_canPickUpTrap)
        {
            //if the state is not accurate
            if(_UI_Manager.Get_UIState != UI_Manager.UIState.Detonator_ActivateTrap_VC && _currentlyHolding == CurrentlyHolding.detonator)
            {
                //update state
                _UI_Manager.SetUIState(UI_Manager.UIState.Detonator_ActivateTrap_VC);
            }
        }
    }
    #endregion

    #region Currently Holding Methods
    /// <summary>
    /// toggle between Vacuum and Trap/Detonator
    /// </summary>
    public void Toggle()
    {
        //dependant on what the player is currently holding
        switch (_currentlyHolding)
        {
            //if the player is holding the VC
            case CurrentlyHolding.VC:
                {
                    //if the trap is not deployed
                    if(!_trapDeployed)
                    {
                        //bring up trap
                        _animation_VCDown_TrapUp();

                        MakeTrap();

                        //update ui
                        _UI_Manager.SetUIState(UI_Manager.UIState.Trap_ThrowTrap_VC);
                    }
                    //if the trap is deployed
                    else
                    {
                        //bring up the detonator
                        _animation_VCDown_DetonatorUp();

                        //IMPLEMENT CHECK FOR IF IT IS PICK UP TRAP OR ACTIVATE TRAP
                        
                        //update UI
                        _UI_Manager.SetUIState(UI_Manager.UIState.Detonator_ActivateTrap_VC);
                    }
                    break;
                }

            //if the player is currently holding the trap
            case CurrentlyHolding.trap:
                {
                    //trap down -> VC Up
                    _animation_TrapDown_VCUp();

                    DestroyTrap();

                    //update ui
                    _UI_Manager.SetUIState(UI_Manager.UIState.VC_Suck_Trap);
                    break;
                }

            //if the player is currently holding the detonator
            case CurrentlyHolding.detonator:
                {
                    //detonator down -> VC Up
                    _animation_DetonatorDown_VCUp();

                    //update UI
                    _UI_Manager.SetUIState(UI_Manager.UIState.VC_Suck_Detonator);
                    break;
                }
        }


        
    }

    //Set currently holding
    public void SetCurrentlyHolding(CurrentlyHolding inputState)
    {
        _currentlyHolding = inputState;
    }

    #endregion

    #region Trap methods
    public void MakeTrap()
    {
        _trapGameObject = Instantiate(_trapPrefab, _trapParent.position, _trapParent.rotation, _playerCamera.transform);
        _trap = _trapGameObject.GetComponent<Trap>();
    }

    public void DestroyTrap()
    {
        Destroy(_trapGameObject);
        _trap = null;
    }



    /// <summary>
    /// of player is currently holding the trap allow to deploy
    ///  if conditions met, add rigidbody, make kinematic false
    /// </summary>
    public void DeployTrap()
    {
        if (_currentlyHolding == CurrentlyHolding.trap)
        {
            // unassign the trap parent
            _trap.transform.parent = null;

            // enable the collider and allow rigidbody physics
            _trap.GetComponent<BoxCollider>().enabled = true;
            _trap.GetComponent<Rigidbody>().isKinematic = false;

            // " throw" the trap out
            _trap.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * TrapThrowForce, ForceMode.Impulse);

            // set to true
            _trapDeployed = true;

            //bring out VC
            _animation_Detonator_Up();

            //update UI
            _UI_Manager.SetUIState(UI_Manager.UIState.Detonator_ActivateTrap_VC);
        }

    }

    /// <summary>
    /// if the player is in rage of the trap object and is not holding the trap
    /// then allow to pick up
    /// - disable box collider, rigidbody is kinematic, change currently holding
    /// </summary>
    /// <param name="trap"></param>
    /// 

    public void pickUpTrap()
    {


        Debug.Log("bubble active");
        // check if the player is within the range, not holding the trap and the trap is not active
        if (!_trap.Bubble.activeInHierarchy)
        {
            MakeTrap();


            /*// deactovate colliders and stop rigidbody physics
            _trap.GetComponent<BoxCollider>().enabled = false;
            _trap.GetComponent<Rigidbody>().isKinematic = true;
            _trap.transform.position = _trapParent.position;*/

            if (_currentlyHolding == CurrentlyHolding.VC)
            {
                _animation_VCDown_TrapUp();
                //update ui
                _UI_Manager.SetUIState(UI_Manager.UIState.Trap_ThrowTrap_VC);
            }
            else if(_currentlyHolding == CurrentlyHolding.detonator)
            {
                _animation_DetonatorDown_TrapUp();

                //update ui
                _UI_Manager.SetUIState(UI_Manager.UIState.Trap_ThrowTrap_VC);
            }



            // set to false
            _trapDeployed = false;
        }
    }

    #endregion

    #region Animation Methods
    //bring up the VC
    public void _animation_VC_Up()
    {
        Debug.Log("VC UP");

        _Equipment_Animator.SetTrigger("VC Up");

        // set currently holding to the vacuum
        SetCurrentlyHolding(CurrentlyHolding.VC);
    }

    //put down the VC
    public void _animation_VC_Down()
    {
        Debug.Log("VC Down");

        _Equipment_Animator.SetTrigger("VC Down");
    }

    //bring up the tablet
    public void _animation_Tablet_Up()
    {
        Debug.Log("Tablet UP");

        _Equipment_Animator.SetTrigger("Tablet Up");
    }

    //put down the tablet
    public void _animation_Tablet_Down()
    {
        Debug.Log("Tablet Down");

        _Equipment_Animator.SetTrigger("Tablet Down");
    }

    //bring up the trap()
    public void _animation_Trap_Up()
    {
        Debug.Log("Trap Up");

        //_Equipment_Animator.SetTrigger("");
        _trap.gameObject.SetActive(true);

        // set parent to the player
        _trap.transform.SetParent(_trapParent);
        _trap.transform.position = Vector3.zero;

        // set currently holding to the trap
        SetCurrentlyHolding(CurrentlyHolding.trap);
    }

    //put down the trap
    public void _animation_Trap_Down()
    {
        Debug.Log("Trap Down");

        //_Equipment_Animator.SetTrigger("");
        _trap.gameObject.SetActive(false);
    }

    //bring out the detonator
    public void _animation_Detonator_Up()
    {
        Debug.Log("Detonator Up");

        _Equipment_Animator.SetTrigger("Detonator Up");

        // set currently holding to the Detonator
        SetCurrentlyHolding(CurrentlyHolding.detonator);
    }

    //put down the detonator
    public void _animation_Detonator_Down()
    {
        Debug.Log("Detonator Down");

        _Equipment_Animator.SetTrigger("Detonator Down");
    }

    //put the VC down then the trap up
    public void _animation_VCDown_TrapUp()
    {
        Debug.Log("VC Down -> Detonator Up");

        _Equipment_Animator.SetTrigger("VCDown_TrapUp");

        //set currently holding trigger
        SetCurrentlyHolding(CurrentlyHolding.trap);
    }

    //put the VC down then the detonator up
    public void _animation_VCDown_DetonatorUp()
    {
        Debug.Log("VC Down -> Detonator Up");

        _Equipment_Animator.SetTrigger("VCDown_DetonatorUp");

        //set currently holding trigger
        SetCurrentlyHolding(CurrentlyHolding.detonator);
    }

    //put the trap down then the VC Up
    public void _animation_TrapDown_VCUp()
    {
        Debug.Log("Trap Down -> VC Up");

        _Equipment_Animator.SetTrigger("TrapDown_VCUp");

        //set currently holding trigger
        SetCurrentlyHolding(CurrentlyHolding.VC);
    }

    //put the trap down then the VC Up
    public void _animation_DetonatorDown_VCUp()
    {
        Debug.Log("Detonator Down -> VC Up");

        _Equipment_Animator.SetTrigger("DetonatorDown_VCUp");

        //set currently holding trigger
        SetCurrentlyHolding(CurrentlyHolding.VC);
    }

    //put the detonator down then the trap Up
    public void _animation_DetonatorDown_TrapUp()
    {
        Debug.Log("Detonator Down -> Trap Up");

        _Equipment_Animator.SetTrigger("DetonatorDown_TrapUp");

        //set currently holding trigger
        SetCurrentlyHolding(CurrentlyHolding.trap);
    }

    //put the tablet up
    public void _animation_TabletUp()
    {
        Debug.Log("Tablet up");

        _Equipment_Animator.SetTrigger("Tablet Up");
    }

    //put the tablet down
    public void _animation_TabletDown()
    {
        Debug.Log("Tablet down");

        _Equipment_Animator.SetTrigger("Tablet down");
    }

    #endregion
}

#region Old Code
/*
         * //swap to trap
        // make sure the player isnt holding the trap & that the trap is still with the player
        if (_currentlyHolding == CurrentlyHolding.VC && _trapDeployed == false && !_tablet.TabletState)
        {
            // change enum state
            _currentlyHolding = CurrentlyHolding.trap;


            return;      // return to avoid toggle loop
        }

        //swap to detonator
        if (_trapDeployed == true && _currentlyHolding == CurrentlyHolding.VC && !_tablet.TabletState)
        {
            *//*PlayerGun.SetActive(false);*//*
            _currentlyHolding = CurrentlyHolding.detonator;



            return;
        }

        //equip vaccuum
        if ((_currentlyHolding == CurrentlyHolding.trap) && _trapDeployed == false && !_tablet.TabletState)
        {
            Trap.SetActive(false);

            _currentlyHolding = CurrentlyHolding.VC;


            return;  // return to avoid toggle loop
        }

        if ((_currentlyHolding == CurrentlyHolding.trap || _currentlyHolding == CurrentlyHolding.detonator) && _trapDeployed == true && !_tablet.TabletState)
        {
            *//*PlayerGun.SetActive(true);*//*
            _currentlyHolding = CurrentlyHolding.VC;


            return;
        }*/
#endregion