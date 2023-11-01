using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Trap : MonoBehaviour
{
    // refrenced gun shoot function later after shoot function is made properly

    public TrapDeploy Check;

    [SerializeField]
    public static bool Catchable = false;
    public GameObject Bubble;
    // use this to refrence traps
    public GameObject playerGun;
    public VacuumGun Vacuum;
    public CreatureAI AlienAI;


    Vector3 offSet;
    Vector3 lerpPathDestination;

    private List<GameObject> _alienList;

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
        StartCoroutine(BubbleTimer());
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
                //I DONT KNOW WHY THIS ALLOWS PULLING
                // I ALSO DONT KNOW WHTY IT DOESNT LERP CORRECTLY EDSBUFDEWUIFBWEYDFB32

                // set state to stunned
                if(!_alienList.Contains(alien.gameObject)) _alienList.Add(alien.gameObject);

                foreach (GameObject item in _alienList)
                 {
                     if (!Catchable && alien.transform.position != lerpPathDestination)
                     {
                        item.transform.position = Vector3.Lerp(item.transform.position, transform.position - offSet, 2f * Time.deltaTime);  
                     }

                    AlienAI = item.GetComponent<CreatureAI>();

                     if (Vacuum.Pulling == false && AlienAI != null)
                     {
                         StartCoroutine(AlienAI.UpdateState(new StunnedState(AlienAI), 0f));
                     }  
                }
                 
                Catchable = true;       
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
                StartCoroutine(AlienAI.UpdateState(new PanicState(Vacuum.AlienAI), 0f));
               if(_alienList.Contains(alien.gameObject)) _alienList.Remove(alien.gameObject);
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

   
    }

    private void Update()
    {
        if(Bubble.gameObject.activeSelf == false && _alienList.Count >0)
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

}
