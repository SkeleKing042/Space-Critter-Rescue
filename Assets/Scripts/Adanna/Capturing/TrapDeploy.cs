using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrapDeploy : MonoBehaviour
{
    public GameObject Trap;
    public GameObject PlayerGun;
    public GameObject Player;

    public GameObject Bubble;

    [SerializeField]
    bool holdingTrap;
    bool holdingGun;

    bool trapDeployed;

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
        currentlyHolding = CurrentlyHolding.vacuum;
        Trap.transform.SetParent(transform);
        Trap.SetActive(false);

    }

    // LB to toggle holding trap
    // LT to throw trap if holding

    /// <summary>
    /// declare functions if conditions apply
    /// </summary>
    void Update()
    {
        Toggle();

        if(Input.GetMouseButtonDown(0))
        {
            DeployTrap();
            
        }
        if (currentlyHolding == CurrentlyHolding.vacuum && Trap.transform.parent !=null)
        {
            Trap.transform.position = PlayerGun.transform.position;
            Trap.transform.rotation = PlayerGun.transform.rotation;
        }

    }
    /// <summary>
    /// toggle between Vacuum and Trap, check Enum
    /// </summary>
    void Toggle()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
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


    }

    /// <summary>
    /// deploy trap if conditions met, add rigidbody, make kinematic false
    /// </summary>
    void DeployTrap()
    {
        if(currentlyHolding == CurrentlyHolding.trap)
        {
            
            Trap.transform.parent = null;
            // Trap.transform.position = Player.transform.forward *5;
            currentlyHolding = CurrentlyHolding.vacuum;
            Trap.GetComponent<BoxCollider>().enabled = true;
          
            Trap.GetComponent<Rigidbody>().isKinematic = false;
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

    private void OnTriggerStay(Collider trap)
    {
       
        if (trap.gameObject.tag == "trap" && Input.GetKey(KeyCode.Q) && currentlyHolding != CurrentlyHolding.trap )
        {
            Debug.Log("PickUp");
            Trap.transform.SetParent(transform);
            Trap.GetComponent<BoxCollider>().enabled = false;
            Trap.GetComponent<Rigidbody>().isKinematic =true;
            currentlyHolding = CurrentlyHolding.vacuum;

            PlayerGun.SetActive(true);
            Trap.SetActive(false);
            trapDeployed = false;
            
        }
       // if (trap.gameObject.tag == "trap" && Input.GetKey(KeyCode.Q) && currentlyHolding != CurrentlyHolding.trap && Bubble.activeSelf == true)
       // {
       //     Bubble.SetActive(false);
       // }
    }

   
}
