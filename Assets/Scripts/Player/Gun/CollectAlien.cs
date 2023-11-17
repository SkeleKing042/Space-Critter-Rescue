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
    [Header("Script Refrences")]
    private VacuumGun _vac;
    private PlayerInventory _playerInventory;


    private void Start()
    {
        _vac = FindObjectOfType<VacuumGun>();
        _playerInventory = FindObjectOfType<PlayerInventory>();
    }


    void OnTriggerEnter(Collider creature)
    {
        if ((creature.gameObject.tag == "alien" || (creature.gameObject.tag == "bigAlien" && Trap.Catchable == true)))
        {
            Debug.Log("tags passed");
            if(_vac.Pulling == true)
            {
                Debug.Log("Pulling check passed");
                _playerInventory.AddAlienToCollection(creature.gameObject);

                _vac.UnassignAlien(creature.gameObject);
            }            
        }
    }
}
