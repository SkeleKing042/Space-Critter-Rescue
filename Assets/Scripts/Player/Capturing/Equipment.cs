using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Equipment : MonoBehaviour
{
    /*
     * To keep in mind
        - LB to toggle holding trap
        - LT to throw trap if holding
     */
    [Header("Currently Holding")]
    [SerializeField]
    public CurrentlyHolding currentlyHolding;

    // inputs
    [Header("Inputs")]
    public PlayerInput Input;
    private InputAction _PickUp;

    // Trap Components 
    [Header("Trap Components")]
    public GameObject Trap;
    private Rigidbody _trapRigid;
    public GameObject Bubble;

    // trap forces 
    [Header("Trap Variables")]
    public float PickUpRange;
    public float TrapThrowForce;
    private bool _trapDeployed;
    public bool TrapDeployed { get { return _trapDeployed; } }
    private float _distance;
    private bool _canPickUpTrap;
    public bool CanPickUpTrap { get { return _canPickUpTrap; } }

    // player Components
    [Header("Player Components")]
    private Tablet _tablet;

    [Header("UI")]
    public Animator _UI_animator;

    [Header("Equipment Animator")]
    public Animator _Equipment_Animator;

    /// <summary>
    /// players eqipment
    /// </summary>
    public enum CurrentlyHolding
    {
        VC,
        trap,
        detonator
    }

    
    /// <summary>
    /// set starting values in scene
    /// </summary>
    void Start()
    {
        //get the tablet script
        _tablet = FindObjectOfType<Tablet>();

        // currently holding set to vacuum & set trap parent
        currentlyHolding = CurrentlyHolding.VC;

        // get rigidbody
        _trapRigid = Trap.GetComponent<Rigidbody>();

        //get the UI animator
        _UI_animator = GameObject.FindGameObjectWithTag("UI").GetComponent<Animator>();

    }
    void Update()
    {
        if ((_trapDeployed == false) && currentlyHolding == CurrentlyHolding.VC || Trap.transform.parent != null)
        {
            // match the vacuums position and rotation
/*            Trap.transform.position = PlayerGun.transform.position;
            Trap.transform.rotation = PlayerGun.transform.rotation;*/
        }

        if (_trapDeployed && Bubble.activeSelf == false)
        {
            _distance = Mathf.Abs(Vector3.Distance(Trap.transform.position, transform.position));

            if (PickUpRange >= _distance)
                _canPickUpTrap = true;
            else
                _canPickUpTrap = false;
        }
    }
    /// <summary>
    /// toggle between Vacuum and Trap
    /// </summary>
    public void Toggle()
    {
        //swap to trap
        // make sure the player isnt holding the trap & that the trap is still with the player
        if (currentlyHolding == CurrentlyHolding.VC && _trapDeployed == false && !_tablet.TabletState)
        {
            // change enum state
            currentlyHolding = CurrentlyHolding.trap;

            _UI_animator.SetTrigger("UI_Trap");
            return;      // return to avoid toggle loop
        }

        //swap to detonator
        if (_trapDeployed == true && currentlyHolding == CurrentlyHolding.VC && !_tablet.TabletState)
        {
            /*PlayerGun.SetActive(false);*/
            currentlyHolding = CurrentlyHolding.detonator;

            _UI_animator.SetTrigger("UI_Trap");

            return;
        }

        //equip vaccuum
        if ((currentlyHolding == CurrentlyHolding.trap) && _trapDeployed == false && !_tablet.TabletState)
        {
            Trap.SetActive(false);

            currentlyHolding = CurrentlyHolding.VC;

            _UI_animator.SetTrigger("UI_Vacuum");
            return;  // return to avoid toggle loop
        }

        if ((currentlyHolding == CurrentlyHolding.trap || currentlyHolding == CurrentlyHolding.detonator) && _trapDeployed == true && !_tablet.TabletState)
        {
            /*PlayerGun.SetActive(true);*/
            currentlyHolding = CurrentlyHolding.VC;

            _UI_animator.SetTrigger("UI_Vacuum");
            return;
        }
    }

    

    /// <summary>
    /// of player is currently holding the trap allow to deploy
    ///  if conditions met, add rigidbody, make kinematic false
    /// </summary>
    public void DeployTrap()
    {
        if (currentlyHolding == CurrentlyHolding.trap && !_tablet.TabletState)
        {

            // unassign the trap parent
            Trap.transform.parent = null;

            // enable the collider and allow rigidbody physics
            Trap.GetComponent<BoxCollider>().enabled = true;
            Trap.GetComponent<Rigidbody>().isKinematic = false;

            // " throw" the trap out
            _trapRigid.AddForce(Camera.main.transform.forward * TrapThrowForce, ForceMode.Impulse);

            // set currently holding to the vacuum
            currentlyHolding = CurrentlyHolding.detonator;

            // set to true
            _trapDeployed = true;

        }

    }

    /// <summary>
    /// if the player is in rage of the trap object and is not holding the trap
    /// then allow to pick up
    /// - disable box collider, rigidbody is kinematic, change currently holding
    /// </summary>
    /// <param name="trap"></param>
    /// 

    public void pickUp(bool DoDistace)
    {
        //if(DoDistace)
        // {
        _distance = Vector3.Distance(Trap.transform.position, transform.position);
        //find the distance between the player and the trap
        Debug.Log( "distance frfom trap: " + _distance);
        _distance = Mathf.Abs(_distance);
        // }

        // check if the player is within the range, not holding the trap and the trap is not active
        if ((PickUpRange >= _distance || DoDistace) && currentlyHolding != CurrentlyHolding.trap && Bubble.activeSelf == false)
        {
            // set parent to the player
            Trap.transform.SetParent(transform);

            // deactovate colliders and stop rigidbody physics
            Trap.GetComponent<BoxCollider>().enabled = false;
            Trap.GetComponent<Rigidbody>().isKinematic = true;

            // set enum state
            currentlyHolding = CurrentlyHolding.trap;

            // set game objects apprpriatly
            /*PlayerGun.SetActive(false);*/
            Trap.SetActive(true);

            // set to false
            _trapDeployed = false;
        }

    }
}