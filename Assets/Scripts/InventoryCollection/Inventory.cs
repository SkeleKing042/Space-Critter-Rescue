// Created By Adanna
// Last Edited by Adanna
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{

    //
    //CollectAlien AlienCollection;
    //private List<GameObject> _alienCollection = new List<GameObject>();
    //private CreatureStats _alienType;
    //private GameObject _alien;
    [SerializeField]
    private Transform _shipDropOff;
    private Transform _playerTransform;

    //private Collection _collection;

    private float _dropOffRange;
    // create enum later for the different alien types


    [Header("Inventory collection types")]
    // Change these to arrays later
    private int _shroomAliens;
    public int PlayerShroomAliens { get { return _shroomAliens; } }
    private int _crystalAliens;
    public int PlayerCrystalAliens { get { return _crystalAliens; } }
    private int _shroomAliensBig;
    public int PlayerShroomAliensBig { get { return _shroomAliensBig; } }
    private int _crystalAliensBig;
    public int PlayerCrystalAliensBig { get { return _crystalAliensBig; } }

    private int _smallCount;
    public int SmallCount { get { return _smallCount; } }
    private int _bigCount;
    public int BigCount { get { return _bigCount; } }

    [SerializeField] private int _smallCap;
    public int SmallCap { get { return _smallCap; } }
    [SerializeField] private int _bigCap;
    public int BigCap { get { return _bigCap; } }

    [Header ("Ship Inventory")]
    private int _shipShroomAliens;
    public int ShipShroomAliens { get { return _shipShroomAliens; } }
    private int _shipCrystalAliens;
    public int ShipCrystalAliens { get { return _shipCrystalAliens; } }
    private int _shipShroomAliensBig;
    public int ShipShroomAliensBig { get { return _shipShroomAliensBig; } }
    private int _shipCrystalAliensBig;
    public int ShipCrystalAliensBig { get { return _shipCrystalAliensBig; } }

    //public int TotalShipInventory;


    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //_collection = FindObjectOfType<Collection>();
    }

    public void AddCritterToInv(GameObject alien)
    {
        CreatureStats alienType = alien.GetComponent<CreatureStats>();
        bool added = true;
        switch (alienType.Type)
        {
            case CreatureStats.creatureType.Crystal:
                if (alienType.IsBig && _bigCount < _bigCap)
                    _crystalAliensBig++;
                else if (!alienType.IsBig && _smallCount < _smallCap)
                    _crystalAliens++;
                else
                    added = false;
                break;
            case CreatureStats.creatureType.Shroom:
                if (alienType.IsBig && _bigCount < _bigCap)
                    _shroomAliensBig++;
                else if (!alienType.IsBig && _smallCount < _smallCap)
                    _shroomAliens++;
                else
                    added = false;
                break;
            default:
                added = false;
                Debug.Log("Typeless creature found, idk what caused it or how it happened but if you are getting this error you might wanna restart.");
                break;
        }
        if (added)
            Destroy(alien);

        _smallCount = _shroomAliens + _crystalAliens;
        _bigCount = _crystalAliensBig + _shroomAliensBig;
    }

    /*public void AddSmallShroom(GameObject alien)
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
    }*/


    public void MoveToShip()
    {
        float distance = Mathf.Abs(Vector3.Distance(_shipDropOff.position, _playerTransform.position));
        //find the distance between the player and the trap
        //Debug.Log(distance);

        if(distance < _dropOffRange)
        {

            _shipShroomAliens += _shroomAliens;
            _shipCrystalAliens += _crystalAliens;
            _shipShroomAliensBig += _shroomAliensBig;
            _shipCrystalAliensBig += _crystalAliensBig;

            _shroomAliens = _shroomAliensBig = _crystalAliens = _crystalAliensBig = 0;

            /*TotalShipInventory =
                _shipShroomAliens +
                _shipCrystalAliens +
                _shipShroomAliensBig +
                _shipCrystalAliensBig;
*/
            //_collection.SmallAliens = 0;
            //_collection.BigAliens = 0;



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
