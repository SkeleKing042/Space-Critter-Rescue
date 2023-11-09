using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Trap : MonoBehaviour
{
    // refrenced gun shoot function later after shoot function is made properly
    [Header("Script Refrences")]
    public TrapDeploy Check;
    public VacuumGun Vacuum;
    public CreatureAI AlienAI;

    

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

    
    private List<GameObject> _alienList = new List<GameObject>();

    private void Start()
    {
        //playerGun.GetComponent<Gun>().Shoot();

        //AI.GetComponent<VacuumGun>().AlienAI();
       // Suck.GetComponent<VacuumGun>().CheckMouseDown();

        offSet = new Vector3(1, 0, 1);
        lerpPathDestination = transform.position - offSet;
    }
    /// <summary>
    /// initial inputs to start trap
    /// </summary>
    public void TrapCatching()
    {
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
        if (Bubble.gameObject.activeSelf == true)
        {
            if (alien.gameObject.tag == "bigAlien" || alien.gameObject.tag == "alien")
            {
                if (!Catchable && alien.transform.position != lerpPathDestination)
                    alien.transform.position = Vector3.Lerp(alien.transform.position, transform.position - offSet, 2f * Time.deltaTime);
            }
        }
    }
    private void OnTriggerStay(Collider alien)
    {
        if (Bubble.gameObject.activeSelf == true)
        {
            if (alien.gameObject.tag == "bigAlien" || alien.gameObject.tag == "alien")
            {
                //I DONT KNOW WHY THIS ALLOWS PULLING
                // I ALSO DONT KNOW WHTY IT DOESNT LERP CORRECTLY EDSBUFDEWUIFBWEYDFB32

                // set state to stunned
                if (!_alienList.Contains(alien.gameObject)) // null refrence called
                {
                    _alienList.Add(alien.gameObject);
                    Debug.Log("alien added to list");
                }


                foreach (GameObject item in _alienList)
                {
                    AlienAI = item.GetComponent<CreatureAI>();

                    if (AlienAI != null)
                    {
                        if (AlienAI._currentState.GetType() != typeof(StunnedState))
                        {
                            StartCoroutine(AlienAI.UpdateState(new StunnedState(AlienAI), 0f));
                            Catchable = true;
                            Debug.Log("chnaging states");
                        }

                    }
                    if (Vacuum.Pulling == true)
                    {
                        if (AlienAI._currentState.GetType() != typeof(CaptureState))
                            StartCoroutine(AlienAI.UpdateState(new CaptureState(AlienAI), 0f));
                    }
                }


            }
        }
    }



    private void OnTriggerExit(Collider alien)
    {
        if (Bubble.gameObject.activeSelf == true)
        {
            if (alien.gameObject.tag == "bigAlien" || alien.gameObject.tag == "alien")
            {
                AlienAI = alien.GetComponent<CreatureAI>();
               if(_alienList.Contains(alien.gameObject))
                {
                    StartCoroutine(AlienAI.UpdateState(new PanicState(AlienAI), 0f));
                    _alienList.Remove(alien.gameObject);
                    Debug.Log("deleted alien from list");
                }
                
               // null refrenced gets called^
            }
        }
    }



    /// <summary>
    /// times when the bubble will disapear
    /// </summary>
    /// <returns></returns>
    IEnumerator BubbleTimer()
    {
        Bubble.SetActive(true);
        yield return new WaitForSeconds(5);
        Bubble.SetActive(false);
        Catchable = false;
        // re set state to panic
        if (Bubble.gameObject.activeSelf == false && _alienList.Count > 0)
        {
            foreach (GameObject item in _alienList)
            {
                if (AlienAI != null)
                {
                    StartCoroutine(AlienAI.UpdateState(new PanicState(AlienAI), 0f));
                }
            }
            _alienList.Clear();
        }

    }

    private void Update()
    {


    }

}
