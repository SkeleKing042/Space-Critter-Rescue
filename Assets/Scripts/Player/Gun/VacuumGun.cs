// Created by Adanna Okoye
// Last edited by Jackson Lucas

using System.Collections.Generic;
using UnityEngine;
using System;
//using UnityEngine.InputSystem.Android;

public class VacuumGun : MonoBehaviour
{
    #region Variables
    private class AlienData
    {
        public GameObject gObject;
        public Rigidbody Rigidbody;
        public CreatureAI AI;
    }
    // alien components 
    //[Header("Alien Components")]
    private List<AlienData> _aData = new List<AlienData>();
    private Trap _trap;
    //public GameObject Bubble;
    private SoundPropagation _sound;
    //private SafteyCheck _obsticleCheck;

    //[Header("Gun Components")]
    //public GameObject Vacuum;

    // ajustable variables
    [Header("Ajustable Varibles")]
    [SerializeField] private float _suckSpeed = 5;
    [SerializeField] private float _offsetFixSpeed;
    [SerializeField] private float _stunTime;
    [SerializeField] private int _layermask;

    // fixed varibles 
    [Header("Fixed Varibles")]
    //private float _centralOffset;
    //private bool _mouseDown;
    private bool _pulling = false;
    public bool Pulling { get { return _pulling;  } }
    private float  _alienOffset;
    //private bool _wasJustPulling;

    [Header("Animators")]
    [SerializeField] Animator _equipment_Animator;

    #endregion
    #region Offset Correction
    private void Awake()
    {
        _trap = FindObjectOfType<Trap>();
    }

    void Update()
    {
        //  collect alien cosine value in relation to the player
        foreach (AlienData aData in _aData)
            if (aData != null && aData.gObject != null)
            {
                Vector3 pos = aData.gObject.transform.position - transform.position;
                _alienOffset = Vector3.Dot(transform.right, pos);
            }
    }

    /// <summary>
    /// use the sine position collected to correct the offset
    /// </summary>
    void OffsetCorrection(Rigidbody alien, float offset)
    {
        // check if coming from the left
        if (offset < -0.01) // -1
        {
           // add force to the right
            alien.AddForce(Vector3.right * _offsetFixSpeed);
            // set bool
            //_comingLeft = true;
        }
        // check if coming from right
        if (offset > 0.01) // 1
        {
            // add force to the left
            alien.AddForce(-Vector3.right * _offsetFixSpeed); // (-right == left)
            // set bool
            //_comingLeft = false;
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
            _pulling = true;
            _equipment_Animator.SetBool("isSucking", true);

            List<AlienData> eraseList = new List<AlienData>();
            foreach (AlienData aData in _aData)
            {
                try
                {
                    if (!Physics.Linecast(transform.position, aData.AI.transform.position, _layermask))
                    {
                        Debug.DrawRay(transform.position, aData.AI.transform.position, Color.black);
                        //Debug.Log("Nothing inbewtween the alien and the player");
                        if ((aData.gObject.tag == "bigAlien" && aData.AI.ReadState.GetType() == typeof(TrappedState)) || aData.gObject.tag == "alien")
                        {
                            // set alien state to captures
                            if ((aData.AI.ReadState.GetType() != typeof(CaptureState)))
                                //StartCoroutine(aData.AI.UpdateState(new CaptureState(aData.AI), 0f));
                                aData.AI.PrepareUpdateState(new CaptureState(aData.AI), 0f);
                            // find what direction the alien is in 
                            Vector3 dir = transform.position - aData.Rigidbody.transform.position;
                            dir = Vector3.Normalize(dir);
                            //move the alien towards the player
                            aData.Rigidbody.AddForce(dir * _suckSpeed);
                            OffsetCorrection(aData.Rigidbody, _alienOffset);
                        }
                    }
                    else if ((Physics.Linecast(transform.position, aData.AI.transform.position)))
                    {
                        Debug.Log("cannot suck something is in the way");
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    eraseList.Add(aData);
                }
            }
            foreach (var erasable in eraseList)
            {
                _aData.Remove(erasable);
            }
        }
        else
        {
            EndPull();
            foreach (AlienData alien in _aData)
                UnassignAlien(alien.gObject);
        }
    }
    public void EndPull()
    {
        _pulling = false;
        _equipment_Animator.SetBool("isSucking", false);

        foreach (AlienData aData in _aData)
        {
            if (aData.AI.ReadState.GetType() == typeof(CaptureState))
            {
                Debug.Log("Ending pull");
                // set the alien states
                aData.AI.StunThenRun(_stunTime);
                //StartCoroutine(aData.AI.UpdateState(new StunnedState(aData.AI), 0f));
                // stun time: wait beforethe new state being set changes
                //StartCoroutine(aData.AI.UpdateState(new PanicState(aData.AI), _stunTime));
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
        if (alien.gameObject.tag == "alien" || alien.gameObject.tag == "bigAlien")
        {
            AlienData ad = new AlienData();
            ad.gObject = alien.gameObject;
            ad.AI = alien.GetComponent<CreatureAI>();
            ad.Rigidbody = alien.GetComponent<Rigidbody>();
            _aData.Add(ad);
        }
    }

    private void OnTriggerExit(Collider alien)
    {
       // Debug.Log("on trigger exit called");
        if ((alien.gameObject.tag == "alien" || alien.gameObject.tag == "bigAlien") && _aData.Count > 0)
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
        //Debug.Log("Unassign Creature");
        foreach (AlienData dData in _aData)
        {
            if (dData.gObject == alien)
            {
                
                _aData.Remove(dData);
                break;
            }
        }         
    }
}
#endregion