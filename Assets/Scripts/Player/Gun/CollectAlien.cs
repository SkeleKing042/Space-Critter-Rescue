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
    public SoundPropagation Sound;

    private void Start()
    {
        _vac = FindObjectOfType<VacuumGun>();
        _collection = FindObjectOfType<Collection>();
        _catchable = FindObjectOfType<Trap>();
    }
    void OnTriggerEnter(Collider creature)
    {
        if ((creature.gameObject.tag == "alien" || (creature.gameObject.tag == "bigAlien" && Trap.Catchable == true)))
        {
            Debug.Log("tags passed");
           // Sound.PropagateSound(0.0000001f);

            if(_vac.Pulling == true)
            {
                // add propagate sound 
                Debug.Log("Pulling check passed");
                _collection.AddAlienToCollection(creature.gameObject);
                _vac.UnassignAlien(creature.gameObject);
            }
        }
    }
}
