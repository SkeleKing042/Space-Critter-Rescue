// Created By Adanna
// Last Edited by Adanna
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    CollectAlien AlienCollection;
    private List<GameObject> _alienCollection = new List<GameObject>();
    private CreatureAI _alienType;

    private GameObject _alien;

    // create enum later for the different alien types
    public int ShroomAliens;
    public int CrystalAliens;

    public int ShroomAliensBig;
    public int CrystalAliensBig;

    public Collection SpaceCheck;


    // creat list of the collected aqliens
    // refrence the Collect Alien Script
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


    }
    public void AddToPlayerInventory(GameObject alien)
    {
        if (alien.tag == "alien" && SpaceCheck.SpaceForSmall == true)
        {
            _alien =alien;
            if (_alienCollection.Contains(_alien)!)
            {
                _alienCollection.Add(_alien);
                _alienType = _alien.GetComponent<CreatureAI>();

                if (CreatureAI.creatureType.Crystal == _alienType.type)
                {
                    // get image and display depending on type

                    ShroomAliens++;
                }
                if (CreatureAI.creatureType.Shroom == _alienType.type)
                {
                    // get image and display
                    CrystalAliens++;
                }
            }
        }

        if (alien.tag == "bigAlien" && SpaceCheck.SpaceForBig == true)
        {
            if (_alienCollection.Contains(alien)!)
            {
                _alienCollection.Add(alien);
                _alienType = alien.GetComponent<CreatureAI>();

                if (CreatureAI.creatureType.Crystal == _alienType.type)
                {
                    // get image and display depending on type
                    ShroomAliensBig++;
                }
                if (CreatureAI.creatureType.Shroom == _alienType.type)
                {
                    // get image and display
                    CrystalAliensBig++;
                }
            }
        }
    }


    public void PlayerInventory()
    {
        


        
    }

    public void ShipInventory()
    {

    }
}
