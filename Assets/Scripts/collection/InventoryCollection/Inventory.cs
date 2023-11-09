// Created By Adanna
// Last Edited by Adanna
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{

    //
    CollectAlien AlienCollection;
    private List<GameObject> _alienCollection = new List<GameObject>();
    private CreatureStats _alienType;
    private GameObject _alien;
    public GameObject ShipDropOff;
    public GameObject Player;

    private Collection _collection;

    private float _distance;
    public float DropOffRange;
    // create enum later for the different alien types


    [Header("Inventory collection types")]
    // Change these to arrays later
    public int ShroomAliens;
    public int CrystalAliens;
    public int ShroomAliensBig;
    public int CrystalAliensBig;

    [Header ("Ship Inventory")]
    public int InvShroomAliens;
    public int InvCrystalAliens;
    public int InvShroomAliensBig;
    public int InvCrystalAliensBig;

    public int TotalShipInventory;


    private void Start()
    {
        _collection = FindObjectOfType<Collection>();
    }



    public void AddSmallShroom(GameObject alien)
    {
       _alienType = alien.GetComponent<CreatureStats>();

       if (CreatureStats.creatureType.Shroom == _alienType.Type)
       {
           // get image and display
           ShroomAliens++;
           Destroy(alien.gameObject);

            // add function for ui desplay
        }
        _alienType = null;
        
    }

    public void AddBigShroom(GameObject alien)
    {

       _alienType = alien.GetComponent<CreatureStats>();

       if (CreatureStats.creatureType.Shroom == _alienType.Type)
       {
           // get image and display
           ShroomAliensBig++;
           Destroy(alien.gameObject);

            // add function for ui desplay
        }
        _alienType = null;
    }

    // here
    public void AddSmallCrystal(GameObject alien)
    {

        _alienType = alien.GetComponent<CreatureStats>();

        if (CreatureStats.creatureType.Crystal == _alienType.Type)
        {
            // get image and display depending on type

            CrystalAliens++;
            Destroy(alien.gameObject);

            // add function for ui desplay
        }
        _alienType = null;
    }
    public void AddBigCrystal(GameObject alien)
    {
        _alienType = alien.GetComponent<CreatureStats>();

        if (CreatureStats.creatureType.Crystal == _alienType.Type)
        {
            // get image and display depending on type

            CrystalAliensBig++;
            Destroy(alien.gameObject);

            // add function for ui desplay
        }
        _alienType = null;
    }


    public void MoveToShip()
    {
        _distance = Vector3.Distance(ShipDropOff.transform.position, Player.transform.position);
        //find the distance between the player and the trap
        Debug.Log(_distance);
        _distance = Mathf.Abs(_distance);

        if(_distance < DropOffRange)
        {

            InvShroomAliens += ShroomAliens;
            InvCrystalAliens += CrystalAliens;
            InvShroomAliensBig += ShroomAliensBig;
            InvCrystalAliensBig += CrystalAliensBig;

            

            ShroomAliens = 0;
            ShroomAliensBig = 0;
            CrystalAliens = 0;
            CrystalAliensBig = 0;

            TotalShipInventory =
                InvShroomAliens +
                InvCrystalAliens +
                InvShroomAliensBig +
                InvCrystalAliensBig;

            _collection.SmallAliens = 0;
            _collection.BigAliens = 0;



        }

    }





    public void PlayerInventory()
    {



        
    }

    public void ShipInventory()
    {
        // crreat ship UI teling player how many is in the ship

       //       TotalShipInventory =



    }
}
