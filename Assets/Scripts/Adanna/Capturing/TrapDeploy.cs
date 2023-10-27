using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TrapDeploy : MonoBehaviour
{
    public PlayerInput Input;
    private InputAction _PickUp;

    public GameObject Trap;
    public GameObject PlayerGun;
    public GameObject Player;
    private Rigidbody _trapRigid;

    public GameObject Bubble;
    public float PickUpRange = 10;

    public float TrapThrowForce;

    [SerializeField]
    bool holdingTrap;
    bool holdingGun;

    bool trapDeployed;

    private bool _pickupable = false;

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
        Input = new PlayerInput();
        currentlyHolding = CurrentlyHolding.vacuum;
        Trap.transform.SetParent(transform);
        Trap.SetActive(false);

        _PickUp = Input.Player.AltFire;
        _PickUp .Enable();

        // _swap = Input.Player.Fire;
       
        _trapRigid = Trap.GetComponent<Rigidbody>();

    }

    // LB to toggle holding trap
    // LT to throw trap if holding

    /// <summary>
    /// declare functions if conditions apply
    /// </summary>
    void Update()
    {
        //Toggle();

        //// add new input
        //if(_mouse)
        //{
        //    DeployTrap();
        //    
        //}
        if (currentlyHolding == CurrentlyHolding.vacuum && Trap.transform.parent !=null)
        {
            Trap.transform.position = PlayerGun.transform.position;
            Trap.transform.rotation = PlayerGun.transform.rotation;
        }

    }
    /// <summary>
    /// toggle between Vacuum and Trap, check Enum
    /// </summary>
    public void Toggle()
    {
        //change to new input use editor
   
            if (currentlyHolding == CurrentlyHolding.vacuum && trapDeployed == false)
            {
                PlayerGun.SetActive(false);
                Trap.SetActive(true);
                currentlyHolding = CurrentlyHolding.trap;
                return;
            }

            if(currentlyHolding == CurrentlyHolding.trap)
            {
                PlayerGun.SetActive(true);
                Trap.SetActive(false);
                currentlyHolding = CurrentlyHolding.vacuum;
                return;
            }



    }

    /// <summary>
    /// deploy trap if conditions met, add rigidbody, make kinematic false
    /// </summary>
    public void DeployTrap()
    {
        if(currentlyHolding == CurrentlyHolding.trap)
        {
            
            Trap.transform.parent = null;
            // Trap.transform.position = Player.transform.forward *5;
            currentlyHolding = CurrentlyHolding.vacuum;
            Trap.GetComponent<BoxCollider>().enabled = true;
            Trap.GetComponent<Rigidbody>().isKinematic = false;

            _trapRigid.AddForce(Camera.main.transform.forward * TrapThrowForce, ForceMode.Impulse);
            Debug.Log("deployyyyy");
            PlayerGun.SetActive(true);
            trapDeployed = true;

        }
    
    }

    /// <summary>
    /// pick up trap if the tag matching trap and if pressing Q.
    /// disable box collider, rigidbody is kinematic, change currently holding
    /// </summary>
    /// <param name="trap"></param>
    /// 


    public void OnTriggerStay(Collider trap)
    {
       
        if (trap.gameObject.tag == "trap" )
        {
            _pickupable = true;
        }
        else
        {
            _pickupable = false;
        } 
    }


                       

    public void pickUp()
    {

        float distance = Vector3.Distance(Trap.transform.position, transform.position);
        Debug.Log(distance);
        distance = Mathf.Abs(distance);
        

        if (PickUpRange >= distance && currentlyHolding != CurrentlyHolding.trap && Bubble.activeSelf == false)
        {
            Debug.Log("PickUp");
            Trap.transform.SetParent(transform);
            Trap.GetComponent<BoxCollider>().enabled = false;
            Trap.GetComponent<Rigidbody>().isKinematic = true;
            
            currentlyHolding = CurrentlyHolding.vacuum;

            PlayerGun.SetActive(true);
            Trap.SetActive(false);
            trapDeployed = false;
        }

    }



}
