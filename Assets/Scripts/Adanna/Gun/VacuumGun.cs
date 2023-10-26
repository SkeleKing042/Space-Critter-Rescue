using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static UnityEditor.PlayerSettings;

public class VacuumGun : MonoBehaviour
{
    public GameObject Alien;
    public Rigidbody _alienRigid;
    bool _mouseDown;

    public float SuckSpeed;
    public float OffsetFixSpeed;


    public Trap trap;

    Vector3 AlienPosition;
    Vector3 forward;

    float _centralOffset;

    private void Start()
    {
     

    }

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

            if( _centralOffset < 0) // -1
            {
                //Debug.Log("Alien is on the left");
            }
            if(_centralOffset > 0)
            {
                //Debug.Log("Alien is on the right");
            }

        }
        


        CheckMouseDown();
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

/*        if (_inRange == true && _mouseDown == true)
        {

           // Debug.Log("result: " + _centralOffset);
            // write succc funtions
          //  _alienRigid.useGravity = false;
          //  Pull();
          //  OffsetCorrection();
          // Alien = Nothing.gameObject;
            // Mathf.Abs(_centralOffset);

        }
        else
        {
            _alienRigid.useGravity = true;
        }*/
     


    }

    void OffsetCorrection()
    {
        bool _comingLeft = true;
        if (_centralOffset < -0.01) // -1
        {
            Alien.transform.position += transform.right * OffsetFixSpeed ;
            _comingLeft = true;
        }
        if (_centralOffset > 0.01) // 1
        {
            Alien.transform.position -= transform.right * OffsetFixSpeed;
            _comingLeft = false;
        }
        if(_centralOffset  < 0.01f && _centralOffset > -0.01)
        {
          //  float _jiggle = Random.value;

            if(_comingLeft)
            {
                for (int i = 0; i < 10; i++)
                    Alien.transform.position -= transform.right * 0.5f;
                //Debug.Log(" right ajusting...");
            }
            if(_comingLeft!)
            {
                for (int i = 0; i < 10; i++)
                    Alien.transform.position += transform.right * 0.5f;
                //Debug.Log(" left ajusting...");
            } 
        }
    }

    void Pull()
    {
        //  while(mouseDown == true)
        Alien.transform.position = Vector3.Lerp(Alien.transform.position, transform.position, SuckSpeed * Time.deltaTime);

    }

    /// <summary>
    ///  if alien withing range set 'Alien" gameobject.
    ///  inrange = true
    /// </summary>
    /// <param name="alien"></param>
    void OnTriggerStay(Collider alien)
    {
        if (alien.gameObject.tag == "alien" || (Trap.Catchable == true && alien.gameObject.tag == "bigAlien"))
        {
            Debug.Log(" Alien in range");

            if (_mouseDown)
            {
                Debug.Log("Mouse is held down");    
                
                if(Alien == null)
                {
                    Debug.Log("Assigned Alien");

                    Alien = alien.transform.gameObject;
                    _alienRigid = alien.GetComponent<Rigidbody>();
                    _alienRigid.useGravity = false;
                }

                Pull();
                OffsetCorrection();
            }
            else
            {
                UnassignAlien();

            }
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
        if (Alien != null)
        {
            _alienRigid.useGravity = true;
            _alienRigid = null;
            Alien = null;
        }
    }


    // if alien is in collision of the cone collider 
    // get th

}


