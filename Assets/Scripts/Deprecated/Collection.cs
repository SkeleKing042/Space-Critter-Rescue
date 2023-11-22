// Created By Adanna Okoye
// Last edited by Jackson Lucas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
 /*   // fixed varibles
    [Header("Fixed Varibles")]
    public float Collected;
  //  public float DropDistance;

    [Header("Alien Counts")]
    public int SmallAliens;
    public int BigAliens;

    [Header("Size Limits")]
    [SerializeField] private int _smallCap;
    [SerializeField] private int _bigCap;
    [Header("bool")]
    //public bool SpaceForSmall;
    //public bool SpaceForBig;

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
        if (SmallAliens < _smallCap && alien.tag == "alien")//  && SpaceForSmall == true)
        {
<<<<<<<< HEAD:Assets/Scripts/Deprecated/Collection.cs
            SmallAliens++;
            Collected++;

            Inventory.AddSmallShroom(alien);
            Inventory.AddSmallCrystal(alien);
========
            SpaceForSmall = true;
            if (alien.tag == "alien"  && SpaceForSmall == true)
            {
                SmallAliens++;

                _shipInventory.AddSmallShroom(alien);
                _shipInventory.AddSmallCrystal(alien);
            }
>>>>>>>> origin/S2_UI/UX:Assets/Scripts/InventoryCollection/PlayerInventory.cs
        }

        *//*else
        {
            SpaceForSmall = false;
<<<<<<<< HEAD:Assets/Scripts/Deprecated/Collection.cs
        }*//*
        if (BigAliens < _bigCap && alien.tag == "bigAlien")// && SpaceForBig == true)
        {
            BigAliens++;
            Collected++;
            Inventory.AddBigShroom(alien);
            Inventory.AddBigCrystal(alien);
========
        }

        if(BigAliens <= 1)
        {
            SpaceForBig = true;
            if (alien.tag == "bigAlien" && SpaceForBig == true)
            {
                BigAliens++;
                _shipInventory.AddBigShroom(alien);
                _shipInventory.AddBigCrystal(alien);
>>>>>>>> origin/S2_UI/UX:Assets/Scripts/InventoryCollection/PlayerInventory.cs

        }

        *//*else
        {
           SpaceForBig = false;
<<<<<<<< HEAD:Assets/Scripts/Deprecated/Collection.cs
        }*//*
        TextNumber = Collected;
========
        }
>>>>>>>> origin/S2_UI/UX:Assets/Scripts/InventoryCollection/PlayerInventory.cs
    }
*/    
}
