// Created By Adanna Okoye
// Last edited by Jackson Lucas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
 /*   // fixed varibles
    [Header("Fixed Varibles")]
    public float Collected;
  //  public float DropDistance;
    public float TextNumber;

    [Header("Alien Counts")]
    public int SmallAliens;
    public int BigAliens;

    [Header("Size Limits")]
    [SerializeField] private int _smallCap;
    [SerializeField] private int _bigCap;
    [Header("bool")]
    //public bool SpaceForSmall;
    //public bool SpaceForBig;

    [Header("Invetory Refrence")]
    private Inventory Inventory;


    private void Start()
    {
        Inventory = FindObjectOfType<Inventory>();
    }


    /// <summary>
    /// add aliens to the collection
    /// </summary>
    /// <param name="alien"></param>
    public void AddAlienToCollection(GameObject alien)
    {
        if (SmallAliens < _smallCap && alien.tag == "alien")//  && SpaceForSmall == true)
        {
            SmallAliens++;
            Collected++;

            Inventory.AddSmallShroom(alien);
            Inventory.AddSmallCrystal(alien);
        }

        *//*else
        {
            SpaceForSmall = false;
        }*//*
        if (BigAliens < _bigCap && alien.tag == "bigAlien")// && SpaceForBig == true)
        {
            BigAliens++;
            Collected++;
            Inventory.AddBigShroom(alien);
            Inventory.AddBigCrystal(alien);

        }

        *//*else
        {
           SpaceForBig = false;
        }*//*
        TextNumber = Collected;
    }
*/    
}
