// Created by Adanna Okoye
// Last edited by Adanna Okoye

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.InputSystem.HID;
//using UnityEngine.InputSystem.Android;

public class VacuumGun : MonoBehaviour
{
    #region Variables
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
    public SoundPropagation Sound;
    private SafteyCheck _obsticleCheck;

    //[Header("Gun Components")]
    //public GameObject Vacuum;

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
    public int layermask;

    // fixed varibles 
    [Header("Fixed Varibles")]
    //private float _centralOffset;
    //private bool _mouseDown;
    public bool Pulling = false;
    private float  _alienOffset;
    private bool _wasJustPulling;
    
     

    #endregion

    private void Awake()
    {
        _obsticleCheck = FindObjectOfType<SafteyCheck>();
    }
    #region Offset Correction

    void Update()
    {

        //  collect alien cosine value in relation to the player
       
         foreach (AlienData aData in aData)
         {
             forward = transform.right;
             AlienPosition = aData.gObject.transform.position - transform.position;
        
              _alienOffset = Vector3.Dot(forward, AlienPosition);
         }

    }

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

        // add jiggly/spinning when close to the vacuum catcher.
        
    }

    #endregion

    #region Pulling
    public void Pull()
    {

        // proprapgate sound

        //Sound.PropagateSound(0.00001f);
        if (gameObject.activeSelf == true)
        {
            Debug.Log("object is active");
            Pulling = true;
            foreach (AlienData aData in aData)
            {
                try
                {
                    Debug.Log("tryinggg");
                    if (!Physics.Linecast(transform.position, aData.AI.transform.position,layermask ))
                    {
                    Debug.DrawRay(transform.position, aData.AI.transform.position, Color.black);
                    Debug.Log("Nothing inbewtween the alien and the player");
                        // set alien state to captures
                        if ((aData.AI._currentState.GetType() != typeof(CaptureState)))
                        StartCoroutine(aData.AI.UpdateState(new CaptureState(aData.AI), 0f));

                         // find what direction the alien is in 
                         Vector3 dir = transform.position - aData.Rigidbody.transform.position;
                         dir = Vector3.Normalize(dir);
                         //move the alien towards the player
                         aData.Rigidbody.AddForce(dir * SuckSpeed);
                        OffsetCorrection(aData.Rigidbody, _alienOffset);
                     }
                    else if ((Physics.Linecast(transform.position, aData.AI.transform.position)))
                    {
                        Debug.Log("cannot suck something is in the way");
                    }


                }
                catch (Exception e)
                {
                    Debug.Log("catching");
                    Debug.Log(e);
                    
                }

            }
        }
        else
        {
            Debug.Log("end pull");
            EndPull();
            foreach (AlienData alien in aData)
                UnassignAlien(alien.gObject);
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
        }
  
    }

    #endregion

    #region Collect and Destroy Objects
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
       // Debug.Log("on trigger exit called");
        if (alien.gameObject.tag == "alien" || (Trap.Catchable == true && alien.gameObject.tag == "bigAlien") && aData.Count > 0)
        {
 
               // Debug.Log("tag check passed");
                UnassignAlien(alien.gameObject);

            // add state change
                  
        }

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

    public void UnassignAlien(GameObject alien)
    {
        Debug.Log("Unassign Creature");
        foreach (AlienData dData in aData)
        {
            if (dData.gObject == alien)
            {
                
                aData.Remove(dData);
                break;
            }
        }
          
    }
}

#endregion