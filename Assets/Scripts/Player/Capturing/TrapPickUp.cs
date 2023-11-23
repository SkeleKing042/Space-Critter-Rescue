using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapPickUp : MonoBehaviour
{
    public GameObject Trap;
    public GameObject PlayerGun;
    public GameObject Player;

    private Rigidbody _trapRig;

    // Start is called before the first frame update


    enum CurrentlyHolding
    {
        vacuum,
        trap
    }
    [SerializeField] CurrentlyHolding currentlyHolding;

    // Update is called once per frame
   
    void Awake()
    {
        Trap.transform.position = PlayerGun.transform.position;
        currentlyHolding = CurrentlyHolding.vacuum;

        _trapRig = Trap.GetComponent<Rigidbody>();
        if (_trapRig != null)
        {
            Destroy(_trapRig);
        }
        Trap.transform.parent = Player.transform;
        Trap.SetActive(false);

    }
    void Update()
    {

            DeployTrap();  

      
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

            if (currentlyHolding == CurrentlyHolding.trap)
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
        
        if (currentlyHolding == CurrentlyHolding.trap && Input.GetKeyUp(KeyCode.P))
        {
            Debug.Log("trap Deploying");
            Trap.gameObject.AddComponent<Rigidbody>();
            Trap.transform.parent = null;
            currentlyHolding = CurrentlyHolding.vacuum;
        }
       
    }


    private void OnTriggerStay(Collider trap)
    {
        if (trap.gameObject.tag == "trap" && Input.GetKeyDown(KeyCode.Q))
        {
            Awake();
        }
    }
}
