using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDeploy : MonoBehaviour
{
    public GameObject Trap;
    public GameObject PlayerGun;
    public GameObject Player;

    [SerializeField]
    bool holdingTrap;
    bool holdingGun;

    enum CurrentlyHolding
    {
        vacuum,
        trap
    }

    [SerializeField] CurrentlyHolding currentlyHolding;

    void Start()
    {
        currentlyHolding = CurrentlyHolding.vacuum;
    }

    // LB to toggle holding trap
    // LT to throw trap if holding

    // Update is called once per frame
    void Update()
    {
        Toggle();

        if(Input.GetMouseButtonDown(0))
        {
            DeployTrap();
            Trap.transform.parent = null;
        }
        
        
    }

    void Toggle()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentlyHolding == CurrentlyHolding.vacuum)
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
    void DeployTrap()
    {
        if(currentlyHolding == CurrentlyHolding.trap)
        {
            Trap.transform.position = Player.transform.forward *5;
            currentlyHolding = CurrentlyHolding.vacuum;
            

        }
    
    }

    private void OnTriggerStay(Collider trap)
    {
        if (trap.gameObject.tag == "trap" && Input.GetKeyDown(KeyCode.Q))
        {

            // holding trap == true 
            // set vector position
            // make trap a parent
            //Trap.transform.parent = null;
        }
    }

   
}
