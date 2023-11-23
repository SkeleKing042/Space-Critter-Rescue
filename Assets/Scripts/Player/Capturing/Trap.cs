// Created By Adanna Okoye
// Last edited by Adanna Okoye
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // refrenced gun shoot function later after shoot function is made properly
    [Header("Script Refrences")]
    public Equipment Check;
    public VacuumGun Vacuum;
    private CreatureAI AlienAI;

    [Header("Tool GameObjects")]
    public GameObject Bubble;
    public GameObject playerGun;
    public GameObject Detinator;

    [Header("Movement Vectors")]
    Vector3 offSet;
    Vector3 lerpPathDestination;

    [Header("Catchable")]
    [SerializeField]
    public static bool Catchable = false;
    [SerializeField]
    private List<GameObject> _alienList = new List<GameObject>();

    [Header("Animator")]
    [SerializeField, Tooltip("Animator That controls the trap")]
    private Animator _trap_Animator;

    private void Start()
    {
        Vacuum = FindObjectOfType<VacuumGun>();
        Check = FindObjectOfType<Equipment>();

        offSet = new Vector3(1, 0, 1);
        lerpPathDestination = transform.position - offSet;
    }

    #region Trap Methods

    /// <summary>
    /// initial inputs to start trap
    /// </summary>
    public void TrapCatching()
    {
        // check the player is holding the detinator
        if(Detinator.gameObject.activeSelf ==true)
        {
            StartCoroutine(BubbleTimer());
        }
    }

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
                if (!Catchable && alien.transform.position != lerpPathDestination)
                    alien.transform.position = Vector3.Lerp(alien.transform.position, transform.position - offSet, 2f * Time.deltaTime);
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
                    AlienAI = item.GetComponent<CreatureAI>();

                    if (AlienAI != null)
                    {
                        // if the current AI state isnt already stunned then change to stun state
                        if (AlienAI._currentState.GetType() != typeof(StunnedState))
                        {
                            StartCoroutine(AlienAI.UpdateState(new StunnedState(AlienAI), 0f));
                            Catchable = true;
                            Debug.Log("chnaging states");
                        }

                    }
                    // if the player is trying to pull an alien, set the alien state to capture state
                    if (Vacuum.Pulling == true)
                    {
                        if (AlienAI._currentState.GetType() != typeof(CaptureState))
                            StartCoroutine(AlienAI.UpdateState(new CaptureState(AlienAI), 0f));
                    }
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
                AlienAI = alien.GetComponent<CreatureAI>();
                // if the list contains the alien gameobject
               if(_alienList.Contains(alien.gameObject))
                {
                    // update state
                    StartCoroutine(AlienAI.UpdateState(new PanicState(AlienAI), 0f));
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
    IEnumerator BubbleTimer()
    {
        // sey gameobjects correctly
        Bubble.SetActive(true);
        yield return new WaitForSeconds(5);
        Bubble.SetActive(false);
        Catchable = false;


        //check the list isnt empty
        if (Bubble.gameObject.activeSelf == false && _alienList.Count > 0)
        {
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
        }

    }

    #endregion

    #region Animation
    public void SetTrigger_ActivateTrap()
    {
        _trap_Animator.SetTrigger("Activate Trap");
    }


    #endregion

}
