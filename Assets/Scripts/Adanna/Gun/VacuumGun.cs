using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class VacuumGun : MonoBehaviour
{
    public GameObject Alien;
    public Rigidbody _alienRigid;
    [SerializeField] 
    private bool _mouseDown;

    public CreatureAI AlienAI;

    public float SuckSpeed;
    public float OffsetFixSpeed;


    public Trap trap;

    Vector3 AlienPosition;
    Vector3 forward;

    float _centralOffset;


    void Update()
    {


        // vector dot gets the cosine value from a yje Vacume gun. 
        // if the value is 0,-1 then the other object behind the gun
        // id the value in -1,0 then the item is to the left
        // if the vale is 1,0 then the object is to the right
        // if the object is 0,1 the object is infront.

        // 
        if (Alien)
        {
            
            forward = transform.right;
            AlienPosition = Alien.transform.position - transform.position;
            
            _centralOffset = Vector3.Dot(forward, AlienPosition);
           // Debug.Log("result: " + pos);



        }
        


        CheckMouseDown();

        if (_mouseDown && Alien != null)
        {
            Pull();
        }
    }

    // add is catchable top the if statemnts

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

    void OffsetCorrection()
    {
        bool _comingLeft = true;
        if (_centralOffset < -0.01) // -1
        {

            _alienRigid.AddForce(Vector3.right * OffsetFixSpeed);

          //  Alien.transform.position += transform.right * OffsetFixSpeed ;
            _comingLeft = true;
        }
        if (_centralOffset > 0.01) // 1
        {
            _alienRigid.AddForce(-Vector3.right * OffsetFixSpeed);

         //   Alien.transform.position -= transform.right * OffsetFixSpeed;
            _comingLeft = false;
        }
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
        StartCoroutine(AlienAI.UpdateState(new CaptureState(AlienAI), 0f));

        //Alien.transform.position = Vector3.Lerp(Alien.transform.position, transform.position, SuckSpeed * Time.deltaTime);
        Vector3 dir =  transform.position - _alienRigid.transform.position;
        dir = Vector3.Normalize(dir);

        _alienRigid.AddForce(dir * SuckSpeed);

    }
    public void EndPull()
    {
        Debug.Log("pull terminated");

    }

    /*/// <summary>
    ///  if alien withing range set 'Alien" gameobject.
    ///  inrange = true
    /// </summary>
    /// <param name="alien"></param>
    void OnTriggerEnter(Collider alien)
    {
        if (alien.gameObject.tag == "alien" || (Trap.Catchable == true && alien.gameObject.tag == "bigAlien") && Alien == null)
        {

            Debug.Log("Assigned Alien");

            Alien = alien.transform.gameObject;
            AlienAI = Alien.GetComponent<CreatureAI>();
            _alienRigid = alien.GetComponent<Rigidbody>();
        }         
    }*/

    private void OnTriggerStay(Collider alien)
    {
        if (alien.gameObject.tag == "alien" || (Trap.Catchable == true && alien.gameObject.tag == "bigAlien") && Alien == null)
        {

            Debug.Log("Assigned Alien");

            Alien = alien.transform.gameObject;
            AlienAI = Alien.GetComponent<CreatureAI>();
            _alienRigid = alien.GetComponent<Rigidbody>();
        }
    }


    /// <summary>
    /// if alien tag left range then Alien gameobject = nulls
    /// in range = false
    /// </summary>
    /// <param name="alien"></param>
    private void OnTriggerExit(Collider alien)
    {
        if (alien.gameObject.tag == "alien" || alien.gameObject.tag == "bigAlien")
        {
            Debug.Log(" Alien left range");
            UnassignAlien();


            
            Trap.Catchable = false;
            Debug.Log("Big alien catchable: "+Trap.Catchable);
        }       
    }

    public void UnassignAlien()
    {
        Debug.Log("Unassign Creature");
        if (Alien != null)
        {
            StartCoroutine(AlienAI.UpdateState(new PanicState(AlienAI), 0f));
            _alienRigid.useGravity = true;
            _alienRigid = null;
            Alien = null;
            AlienAI = null;
        }
    }


    // if alien is in collision of the cone collider 
    // get th

}


