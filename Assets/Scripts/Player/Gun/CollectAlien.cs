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
    //public TMP_Text AlienUICount;
    private float AlienCount;
    public bool mouseDown = false;

    [Header("Script Refrences")]
    private VacuumGun _vac;
    private Collection _collection;
    private Trap _catchable;


    GameObject Alien;

    private void Start()
    {
        _vac = FindObjectOfType<VacuumGun>();
        _collection = FindObjectOfType<Collection>();
        _catchable = FindObjectOfType<Trap>();
    }


    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            mouseDown = true;
            //Vector3 position = PlayerGun.GetComponent<Vector3>;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            mouseDown = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        //AlienUICount.text = ": " + collection.TextNumber.ToString();

    }


    void OnTriggerEnter(Collider creature)
    {

        
        if ((creature.gameObject.tag == "alien" || (creature.gameObject.tag == "bigAlien" && Trap.Catchable == true)))
        {
            Debug.Log("tags passed");
            if(_vac.Pulling == true)
            {
                Debug.Log("Pulling check passed");
                _collection.AddAlienToCollection(creature.gameObject);

                _vac.UnassignAlien(creature.gameObject);

            }
        
           // Destroy(creature.gameObject);

            //Vac.EndPull();
            
        }
    }
}
