using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // refrenced gun shoot function later after shoot function is made properly

    public TrapDeploy Check;

    [SerializeField]
    public static bool Catchable = false;
    public GameObject Bubble;
    // use this to refrence traps
    public GameObject playerGun;

    public VacuumGun Suck;

    Vector3 offSet;
    Vector3 lerpPathDestination;


    private void Start()
    {
        //playerGun.GetComponent<Gun>().Shoot();

        Suck.GetComponent<VacuumGun>().CheckMouseDown();

        offSet = new Vector3(1, 0, 1);

        lerpPathDestination = transform.position - offSet;
    }

    private void Update()
    {
        //Debug.Log("Catchable: " + Catchable);

        //TrapCatching();
       

    }


    /// <summary>
    /// initial inputs to start trap
    /// </summary>
    public void TrapCatching()
    {
        // if player presses e activate trap start timer

            StartCoroutine(BubbleTimer());
        

        Suck.Pull();
        // playerGun.GetComponent<Gun>().Shoot();
    }




    /// <summary>
    /// if a item (that is an alien) is withing the trap radius then let the alein be catchable
    /// </summary>
    /// <param name="alien"></param>
    private void OnTriggerStay(Collider alien)
    {
        if (Bubble.gameObject.activeSelf == true)
        {
            if (alien.gameObject.tag == "bigAlien")
            {
                //I DONT KNOW WHY THIS ALLOWS PULLING
                // I ALSO DONT KNOW WHTY IT DOESNT LERP CORRECTLY EDSBUFDEWUIFBWEYDFB32
                if (!Catchable && alien.transform.position != lerpPathDestination)
                   alien.transform.position = Vector3.Lerp(alien.transform.position, transform.position - offSet, 2f * Time.deltaTime);

                Catchable = true;

                
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
    }

}
