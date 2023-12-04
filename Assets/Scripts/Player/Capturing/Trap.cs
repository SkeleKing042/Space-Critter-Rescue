// Created By Adanna Okoye
// Last edited by Ru

using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // refrenced gun shoot function later after shoot function is made properly
    //private Equipment Check;
    [Header("Script Refrences")]
    private VacuumGun _vacuum;
    private CreatureAI _alienAI;

    [Header("Tool GameObjects")]
    [SerializeField] private GameObject _bubble;
    public GameObject Bubble { get { return _bubble; } }


    //public GameObject playerGun;
    //public GameObject Detinator;

    [Header("Movement Vectors")]
    Vector3 _offSet;
    Vector3 _lerpPathDestination;

    //[Header("Catchable")]
    //private bool _catchable = false;
    //public bool Catchable { get { return _catchable;  } }
    private List<GameObject> _alienList = new List<GameObject>();

    //[Header("Animator")]
    //[SerializeField, Tooltip("Animator That controls the trap")]
    private Animator _trap_Animator;
    //Used by animator
    private bool _trapActivated;

    private void Awake()
    {
        _vacuum = FindObjectOfType<VacuumGun>();
        _trap_Animator = transform.GetComponentInChildren<Animator>();

        Bubble.SetActive(false);
        
        _offSet = new Vector3(1, 0, 1);
        _lerpPathDestination = transform.position - _offSet;
    }

    #region Trap Methods

    /// <summary>
    /// initial inputs to start trap
    /// </summary>
    /*public void TrapCatching()
    {
        // check the player is holding the detinator
        if(Detinator.gameObject.activeSelf ==true)
        {
            //StartCoroutine(BubbleTimer());
        }
    }*/

    /// <summary>
    /// if a item (that is an alien) is withing the trap radius then let the alein be catchable
    /// </summary>
    /// <param name="alien"></param>
    private void OnTriggerEnter(Collider alien)
    {
        // checkm if bubble is active
        if (Bubble.gameObject.activeSelf == true)
        {
            // check tags
            if (alien.gameObject.tag == "bigAlien" || alien.gameObject.tag == "alien")
            {
                // move alien slightly towards the centre of the trap
                if (alien.transform.position != _lerpPathDestination && alien.GetComponent<CreatureAI>().ReadState.GetType() == typeof(TrappedState))
                    alien.transform.position = Vector3.Lerp(alien.transform.position, transform.position - _offSet, 2f * Time.deltaTime);
            }
        }
    }


    /// <summary>
    /// When the alien stays in the trap add the alien to the list and set the state to stunned
    /// </summary>
    /// <param name="alien"></param>
    private void OnTriggerStay(Collider alien)
    {
        if (Bubble.gameObject.activeSelf == true)
        {
            if (alien.gameObject.tag == "bigAlien" || alien.gameObject.tag == "alien")
            {
                //check that the alien is not already in the list
                if (!_alienList.Contains(alien.gameObject))
                {
                    // add alien game object to list
                    _alienList.Add(alien.gameObject);
                    Debug.Log("alien added to list");
                }
                // for every alien in the list get the AI script refrence
                foreach (GameObject item in _alienList)
                {
                    _alienAI = item.GetComponent<CreatureAI>();

                    if (_alienAI != null)
                    {
                        // if the current AI state isnt already stunned then change to stun state
                        if (_alienAI.ReadState.GetType() != typeof(TrappedState))
                        {
                            //StartCoroutine(_alienAI.UpdateState(new StunnedState(_alienAI), 0f));
                            _alienAI.PrepareUpdateState(new TrappedState(_alienAI), 0f);
                            Debug.Log("chnaging states");
                        }

                    }
                    // if the player is trying to pull an alien, set the alien state to capture state
                    /*if (_vacuum.Pulling == true)
                    {
                        if (_alienAI.ReadState.GetType() != typeof(CaptureState))
                            _alienAI.PrepareUpdateState(new CaptureState(_alienAI), 0f);
                            //StartCoroutine(_alienAI.UpdateState(new CaptureState(_alienAI), 0f));
                    }*/
                }
            }
        }
    }

    /// <summary>
    /// when alien leaves collider make the alien panic and remove the alien from the list
    /// </summary>
    /// <param name="alien"></param>

    private void OnTriggerExit(Collider alien)
    {
        if (Bubble.gameObject.activeSelf == true)
        {
            if (alien.gameObject.tag == "bigAlien" || alien.gameObject.tag == "alien")
            {
                // get ai REFRENCE
                _alienAI = alien.GetComponent<CreatureAI>();
                // if the list contains the alien gameobject
               if(_alienList.Contains(alien.gameObject))
                {
                    // update state
                    //StartCoroutine(_alienAI.UpdateState(new PanicState(_alienAI), 0f));
                    _alienAI.PrepareUpdateState(new PanicState(_alienAI), 0f);
                    // remove alien from list
                    _alienList.Remove(alien.gameObject);
                    Debug.Log("deleted alien from list");
                }
            }
        }
    }



    /// <summary>
    /// times how long the bubble is active
    /// when the bubble disables put any aliens in the list to a panic state
    /// </summary>
    /// <returns></returns>
    /*IEnumerator BubbleTimer()
    {

        // sey gameobjects correctly
        _trap_Animator.SetTrigger("Activate Trap");

        Debug.Log("bubble is active");
        yield return new WaitForSeconds(5);
        _trap_Animator.SetTrigger("Deactivate Trap");
        Debug.Log("bubble is NOT active");
        Catchable = false;


            foreach (GameObject item in _alienList)
            {
                if (AlienAI != null)
                {
                    // update the state to panic
                    StartCoroutine(AlienAI.UpdateState(new PanicState(AlienAI), 0f));
                }
            }
            // completly clear the list
            _alienList.Clear();
    }*/

    #endregion

    #region Animation
   public void SetTrigger_ActivateTrap()
    {
        _trap_Animator.SetTrigger("Activate Trap");
    }




    #endregion
}
