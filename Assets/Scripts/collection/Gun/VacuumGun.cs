using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class VacuumGun : MonoBehaviour
{   
    // alien components 
    public GameObject Alien;
    public Rigidbody _alienRigid;
    public CreatureAI AlienAI;
    public Trap trap;
    public GameObject Bubble;

    // movement and position vectors
    Vector3 AlienPosition;
    Vector3 forward;

    // ajustable variables
    [SerializeField]
    private float SuckSpeed = 5;
    public float OffsetFixSpeed;
    public float StunTime;

    // fixed varibles 
    private float _centralOffset;
    private bool _mouseDown;
    public bool Pulling = false;

    void Update()
    {
   


        


        //  collect alien cosine value in relation to the player
        if (Alien)
        {
            forward = transform.right;
            AlienPosition = Alien.transform.position - transform.position;
            
           _centralOffset = Vector3.Dot(forward, AlienPosition);
        }


        // id the value in -1,0 then the item is to the left
        // if the vale is 1,0 then the object is to the right

        CheckMouseDown();
    }
    public void CheckMouseDown()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            _mouseDown = true;


        }

        if (Input.GetButtonUp("Fire1"))
        {
            _mouseDown = false;
            
        }
    }

    /// <summary>
    /// use the sine position collected to correct the offset
    /// </summary>
    void OffsetCorrection()
    {
        bool _comingLeft;
        // check if coming from the left
        if (_centralOffset < -0.01) // -1
        {
           // add force to the right
            _alienRigid.AddForce(Vector3.right * OffsetFixSpeed);
            // set bool
            _comingLeft = true;
        }
        // check if coming from right
        if (_centralOffset > 0.01) // 1
        {
            // add force to the left
            _alienRigid.AddForce(-Vector3.right * OffsetFixSpeed); // (-right == left)
            // set bool
            _comingLeft = false;
        }

        // RE WORK CODE

      // if(_centralOffset  < 0.01f && _centralOffset > -0.01)
      // {
      //   //  float _jiggle = Random.value;
      //
      //     if(_comingLeft)
      //     {
      //         for (int i = 0; i < 10; i++)
      //             Alien.transform.position -= transform.right * 0.5f;
      //         //Debug.Log(" right ajusting...");
      //     }
      //     if(_comingLeft!)
      //     {
      //         for (int i = 0; i < 10; i++)
      //             Alien.transform.position += transform.right * 0.5f;
      //         //Debug.Log(" left ajusting...");
      //     } 
      // }
    }

    public void Pull()
    {
     
        Pulling = true;
        if (Pulling && Alien != null)
        {
            if (Bubble.gameObject.activeSelf == true)
                SuckSpeed = 100;
            else
                SuckSpeed = 5;

            Debug.Log("starting");
            // set alien state to captures
            StartCoroutine(AlienAI.UpdateState(new CaptureState(AlienAI), 0f));
            // find what direction the alien is in 
            Vector3 dir = transform.position - _alienRigid.transform.position;
            dir = Vector3.Normalize(dir);
            //move the alien towards the player
            _alienRigid.AddForce(dir * SuckSpeed);

        }
        
    }
    public void EndPull()
    {
        Pulling = false;
        if (Alien != null)
        {
            Debug.Log("Ending pull");
            // set the alien states
            StartCoroutine(AlienAI.UpdateState(new StunnedState(AlienAI), 0f));
            // stun time: wait beforethe new state being set changes
            StartCoroutine(AlienAI.UpdateState(new PanicState(AlienAI), StunTime));
            UnassignAlien();
        }
    }
    /// <summary>
    /// find the alien and setting the values when in range 
    /// </summary>
    /// <param name="alien"></param>
    private void OnTriggerStay(Collider alien)
    {
        if (alien.gameObject.tag == "alien" || (Trap.Catchable == true && alien.gameObject.tag == "bigAlien") && Alien == null)
        {
            Alien = alien.transform.gameObject;
            AlienAI = Alien.GetComponent<CreatureAI>();
            _alienRigid = alien.GetComponent<Rigidbody>();
        }
    }


    /// <summary>
    /// i may delete this 
    /// </summary>
    /// <param name="alien"></param>
   //private void OnTriggerExit(Collider alien)
   //{
   //    if (alien.gameObject.tag == "alien" || alien.gameObject.tag == "bigAlien")
   //    {
   //        Debug.Log(" Alien left range");
   //        Trap.Catchable = false;
   //        Debug.Log("Big alien catchable: "+Trap.Catchable);
   //    }       
   //}

    public void UnassignAlien()
    {
        Debug.Log("Unassign Creature");
        if (Alien != null)
        {
           // StartCoroutine(AlienAI.UpdateState(new PanicState(AlienAI), 0f));
           //_alienRigid.useGravity = true;
           _alienRigid = null;
           Alien = null;
           AlienAI = null;
            EndPull();
        }
    }


    // if alien is in collision of the cone collider 
    // get th

}


