// Created By Adanna Okoye
//Last Edited by Adanna Okoye

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CollectAlien : MonoBehaviour
{
    //[Header("Script Refrences")]
    private VacuumGun _vac;
    private Inventory _inv;


    GameObject Alien;

    private void Awake()
    {
        _vac = FindObjectOfType<VacuumGun>();
        _inv = FindObjectOfType<Inventory>();
    }

    void OnTriggerEnter(Collider creature)
    {
        if ((creature.gameObject.tag == "alien" || (creature.gameObject.tag == "bigAlien" && Trap.Catchable == true)))
        {
           // Debug.Log("tags passed");
            if(_vac.Pulling == true)
            {
                Debug.Log("get stareeddd");
                //  Debug.Log("Pulling check passed");
                _inv.AddCritterToInv(creature.gameObject);
                _vac.UnassignAlien(creature.gameObject);
            }           
        }
    }
}
