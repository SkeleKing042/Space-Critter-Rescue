using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // refrenced gun shoot function later after shoot function is made properly

    [SerializeField]
    bool Catchable = false;
    public GameObject Bubble;

    private void Update()
    {

        TrapCatching();

    }


    /// <summary>
    /// initial inputs to start trap
    /// </summary>
    void TrapCatching()
    {
        // if player presses e activate trap start timer
        if(Input.GetKeyUp(KeyCode.E))
        {
            Bubble.SetActive(true);
            StartCoroutine(BubbleTimer());
        }
        // if catchable start inable shoot function
        if(Catchable == true)
        {
            // add gun refrence later so shoot function works.
            Shoot();
        }
        
    }
    /// <summary>
    /// if a item (that is an alien) is withing the trap radius then let the alein be catchable
    /// </summary>
    /// <param name="alien"></param>
    private void OnTriggerStay(Collider alien)
    {
        if (Bubble.gameObject.active == true)
        {
            if (alien.gameObject.tag == "alien")
            {
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
        yield return new WaitForSeconds(3);
        Bubble.SetActive(false);
        Catchable = false;
    }

    void Shoot()
    {

    }
}
