// Created by Adanna Okoye
// Last edited by Jackson Lucas

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.InputSystem.HID;

public class VacuumGun : MonoBehaviour
{
    [System.Serializable]
    public class AlienData
    {
        public GameObject gObject;
        public Rigidbody Rigidbody;
        public CreatureAI AI;
    }
    // alien components 
    [Header("Alien Components")]
    public List<AlienData> aData;
    public Trap trap;
    public GameObject Bubble;

    // movement and position vectors
    [Header ("Movement and Position Vectors")]
    Vector3 AlienPosition;
    Vector3 forward;

    // ajustable variables
    [Header("Ajustable Varibles")]
    [SerializeField]
    private float SuckSpeed = 5;
    public float OffsetFixSpeed;
    public float StunTime;

    // fixed varibles 
    [Header("Fixed Varibles")]
    //private float _centralOffset;
    //private bool _mouseDown;
    public bool Pulling = false;

    void Update()
    {

        //  collect alien cosine value in relation to the player
       
         // foreach (AlienData aData in aData)
         // {
         //     forward = transform.right;
         //     AlienPosition = aData.gObject.transform.position - transform.position;
         //
         //     float offset = Vector3.Dot(forward, AlienPosition);
         //
         //     OffsetCorrection(aData.Rigidbody, offset);
         // }

        /*if(!Pulling)
        {

            foreach (AlienData aData in aData)
            {
                Debug.Log("Ending pull");
                // set the alien states
                if (aData.AI._currentState.GetType() == typeof(CaptureState))
                {
                    StartCoroutine(aData.AI.UpdateState(new StunnedState(aData.AI), 0f));
                    // stun time: wait beforethe new state being set changes
                    StartCoroutine(aData.AI.UpdateState(new PanicState(aData.AI), StunTime));
                }
                //UnassignAlien();
            }
        }*/

        // id the value in -1,0 then the item is to the left
        // if the vale is 1,0 then the object is to the right

        //CheckMouseDown();
    }
    /*public void CheckMouseDown()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            _mouseDown = true;


        }

        if (Input.GetButtonUp("Fire1"))
        {
            _mouseDown = false;
            
        }
    }*/

    /// <summary>
    /// use the sine position collected to correct the offset
    /// </summary>
    void OffsetCorrection(Rigidbody alien, float offset)
    {
        bool _comingLeft;
        // check if coming from the left
        if (offset < -0.01) // -1
        {
           // add force to the right
            alien.AddForce(Vector3.right * OffsetFixSpeed);
            // set bool
            _comingLeft = true;
        }
        // check if coming from right
        if (offset > 0.01) // 1
        {
            // add force to the left
            alien.AddForce(-Vector3.right * OffsetFixSpeed); // (-right == left)
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
        Debug.Log("starting");
        Pulling = true;
        foreach(AlienData aData in aData)
        {
            try
            {
            if ((aData.AI._currentState.GetType() != typeof(CaptureState)))
            // set alien state to captures
            StartCoroutine(aData.AI.UpdateState(new CaptureState(aData.AI), 0f));
            // find what direction the alien is in 
            Vector3 dir = transform.position - aData.Rigidbody.transform.position;
            dir = Vector3.Normalize(dir);
            //move the alien towards the player

            aData.Rigidbody.AddForce(dir * SuckSpeed);


            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
            //Need to increase force when in bubble
/*
            if (Bubble.gameObject.activeSelf == true)
                SuckSpeed = 100;
            else
                SuckSpeed = 5;
*/
        }



    }
    public void EndPull()
    {
       Pulling = false;
        foreach(AlienData aData in aData)
        {
            if ((aData.AI._currentState.GetType() == typeof(CaptureState)))
            {
                Debug.Log("Ending pull");
                // set the alien states
                StartCoroutine(aData.AI.UpdateState(new StunnedState(aData.AI), 0f));
                // stun time: wait beforethe new state being set changes
                StartCoroutine(aData.AI.UpdateState(new PanicState(aData.AI), StunTime));
            }
            
            //UnassignAlien();
        }
  
    }
    /// <summary>
    /// find the alien and setting the values when in range 
    /// </summary>
    /// <param name="alien"></param>
    private void OnTriggerEnter(Collider alien)
    {
        if (alien.gameObject.tag == "alien" || (Trap.Catchable == true && alien.gameObject.tag == "bigAlien"))
        {
            AlienData ad = new AlienData();
            ad.gObject = alien.gameObject;
            ad.AI = alien.GetComponent<CreatureAI>();
            ad.Rigidbody = alien.GetComponent<Rigidbody>();
            aData.Add(ad);
        }
    }
    private void OnTriggerExit(Collider alien)
    {
        if (alien.gameObject.tag == "alien" || (Trap.Catchable == true && alien.gameObject.tag == "bigAlien") && aData.Count < 0)
            UnassignAlien(alien.gameObject);
    }
    /*
    private void OnTriggerStay(Collider alien)
    {
        if (alien.gameObject.tag == "alien" || (Trap.Catchable == true && alien.gameObject.tag == "bigAlien") && Alien == null)
        {
            Alien = alien.transform.gameObject;
            AlienAI = Alien.GetComponent<CreatureAI>();
            _alienRigid = alien.GetComponent<Rigidbody>();
        }
    }*/


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

    public void UnassignAlien(GameObject alien)
    {
        Debug.Log("Unassign Creature");
        foreach (AlienData dData in aData)
            if (dData.gObject == alien)
            {
                aData.Remove(dData);
                break;
            }
       
        /*if (Alien != null)
        {
           // StartCoroutine(AlienAI.UpdateState(new PanicState(AlienAI), 0f));
           //_alienRigid.useGravity = true;
           _alienRigid = null;
           Alien = null;
           AlienAI = null;
            EndPull();
        }*/
    }


    // if alien is in collision of the cone collider 
    // get th

}


