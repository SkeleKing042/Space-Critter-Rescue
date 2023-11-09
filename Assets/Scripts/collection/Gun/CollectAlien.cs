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
    private VacuumGun Vac;
    private Collection collection;


    GameObject Alien;

    private void Start()
    {
        Vac = FindObjectOfType<VacuumGun>();
        collection = FindObjectOfType<Collection>();

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

        if ((creature.gameObject.tag == "alien" || creature.gameObject.tag == "bigAlien") && Vac.Pulling == true)
        {
            collection.AddAlienToCollection(creature.gameObject);

            // AlienCount++;
            Alien = creature.gameObject;
            

            Vac.UnassignAlien(creature.gameObject);
            Destroy(creature.gameObject);

            //Vac.EndPull();
            
        }
    }
}
