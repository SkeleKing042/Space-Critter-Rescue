using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TrapDeploy : MonoBehaviour
{
    /*
     * To keep in mind
        - LB to toggle holding trap
        - LT to throw trap if holding
     */

    // inputs
    public PlayerInput Input;
    private InputAction _PickUp;


    // Trap Components 
    public GameObject Trap;
    private Rigidbody _trapRigid;
    public GameObject Bubble;

    // player Components
    public GameObject PlayerGun;
    public GameObject Player;

    // trap forces 
    public float PickUpRange = 10;
    public float TrapThrowForce;

    // misc
    private bool trapDeployed;


    /// <summary>
    /// players eqipment
    /// </summary>
    public enum CurrentlyHolding
    {
        vacuum,
        trap
    }

    [SerializeField]
    public CurrentlyHolding currentlyHolding;

    /// <summary>
    /// set starting values in scene
    /// </summary>
    void Start()
    {

        // input declaration
        Input = new PlayerInput();
        _PickUp = Input.Player.AltFire;
        _PickUp.Enable();

        // currently holding set to vacuum & set trap parent
        currentlyHolding = CurrentlyHolding.vacuum;
        Trap.transform.SetParent(transform);
        Trap.SetActive(false);

        // get rigodbody
        _trapRigid = Trap.GetComponent<Rigidbody>();

    }
    void Update()
    {
        if (currentlyHolding == CurrentlyHolding.vacuum || Trap.transform.parent !=null)
        {
            // match the vacuums position and rotation
            Trap.transform.position = PlayerGun.transform.position;
            Trap.transform.rotation = PlayerGun.transform.rotation;
        }

    }
    /// <summary>
    /// toggle between Vacuum and Trap
    /// </summary>
    public void Toggle()
    {
        // make sure the player isnt holding the trap & that the trap is still with the player
       if (currentlyHolding == CurrentlyHolding.vacuum && trapDeployed == false)
       {
           // set game objects appropriatly
           PlayerGun.SetActive(false);
           Trap.SetActive(true);

           // change enum state
           currentlyHolding = CurrentlyHolding.trap;
           return;      // return to avoid toggle loop
       }

       if(currentlyHolding == CurrentlyHolding.trap)
       {
           PlayerGun.SetActive(true);
           Trap.SetActive(false);

           currentlyHolding = CurrentlyHolding.vacuum;
           return;  // return to avoid toggle loop
        }
    }

    /// <summary>
    /// of player is currently holding the trap allow to deploy
    ///  if conditions met, add rigidbody, make kinematic false
    /// </summary>
    public void DeployTrap()
    {
        if(currentlyHolding == CurrentlyHolding.trap)
        {
            // unassign the trap parent
            Trap.transform.parent = null;
          
            // enable the collider and allow rigidbody physics
            Trap.GetComponent<BoxCollider>().enabled = true;
            Trap.GetComponent<Rigidbody>().isKinematic = false;

            // " throw" the trap out
            _trapRigid.AddForce(Camera.main.transform.forward * TrapThrowForce, ForceMode.Impulse);

            // set currently holding to the vacuum
            currentlyHolding = CurrentlyHolding.vacuum;
            PlayerGun.SetActive(true);

            // set to true
            trapDeployed = true;

        }
    
    }

    /// <summary>
    /// if the player is in rage of the trap object and is not holding the trap
    /// then allow to pick up
    /// - disable box collider, rigidbody is kinematic, change currently holding
    /// </summary>
    /// <param name="trap"></param>
    /// 
                 
    public void pickUp()
    {
        //find the distance between the player and the trap
        float distance = Vector3.Distance(Trap.transform.position, transform.position);
        Debug.Log(distance);
        distance = Mathf.Abs(distance);
        
        // check if the player is within the range, not holding the trap and the trap is not active
        if (PickUpRange >= distance && currentlyHolding != CurrentlyHolding.trap && Bubble.activeSelf == false)
        {
           // set parent to the player
            Trap.transform.SetParent(transform);
            
            // deactovate colliders and stop rigidbody physics
            Trap.GetComponent<BoxCollider>().enabled = false;
            Trap.GetComponent<Rigidbody>().isKinematic = true;
            
            // set enum state
            currentlyHolding = CurrentlyHolding.vacuum;

            // set game objects apprpriatly
            PlayerGun.SetActive(true);
            Trap.SetActive(false);

            // set to false
            trapDeployed = false;
        }

    }



}
