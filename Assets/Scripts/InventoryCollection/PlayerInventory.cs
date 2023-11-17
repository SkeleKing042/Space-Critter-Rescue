// Created By Adanna Okoye
// Last edited by Adanna Okoye
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // fixed varibles
    [Header("Fixed Varibles")]
    [SerializeField]
    public float Collected;
  //  public float DropDistance;

    [Header("Alien Counts")]
    public int SmallAliens;
    public int BigAliens;

    [Header("bool")]
    public bool SpaceForSmall;
    public bool SpaceForBig;

    [Header("Invetory")]

    [Header("Invetory Refrence")]
    private ShipInventory _shipInventory;


    private void Start()
    {
        //find reference of inventory
        _shipInventory = FindObjectOfType<ShipInventory>();
    }


    /// <summary>
    /// add aliens to the collection
    /// </summary>
    /// <param name="alien"></param>
    public void AddAlienToCollection(GameObject alien)
    {
        if (SmallAliens <= 11)
        {
            SpaceForSmall = true;
            if (alien.tag == "alien"  && SpaceForSmall == true)
            {
                SmallAliens++;

                _shipInventory.AddSmallShroom(alien);
                _shipInventory.AddSmallCrystal(alien);
            }
        }
        else
        {
            SpaceForSmall = false;
        }

        if(BigAliens <= 1)
        {
            SpaceForBig = true;
            if (alien.tag == "bigAlien" && SpaceForBig == true)
            {
                BigAliens++;
                _shipInventory.AddBigShroom(alien);
                _shipInventory.AddBigCrystal(alien);

            }
        }
        else
        {
           SpaceForBig = false;
        }
    }
    
}
