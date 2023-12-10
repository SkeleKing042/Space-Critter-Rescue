// Created By Adanna Okoye
// Last Edited by Jackson Lucas

using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Variables
    /*[Header("Drop off")]
   //[SerializeField, Tooltip("The transform of the drop off GameObject")]
    [SerializeField] private Transform _shipDropOff;
 //   [SerializeField, Tooltip("Players transform"), HideInInspector]
    private Transform _playerTransform;
    public Transform PlayerTransform { get { return _playerTransform; } set { _playerTransform = value; } }
    [SerializeField, Tooltip("Range in which the player can put critters into the ship")]
    private float _dropOffRange;

    [Header("Player Inventory")]
    [SerializeField, Tooltip("The number of small fungi critters the player has in their inventory")]
    private int _player_fungiCritter_Small;
    public int Player_Fungi_Small { get { return _player_fungiCritter_Small; } }

    [SerializeField, Tooltip("The number of small crystal critters the player has in their inventory")]
    private int _player_crystalCritter_Small;
    public int Player_Crystal_Small { get { return _player_crystalCritter_Small; } }

    [SerializeField, Tooltip("The number of large fungi critters the player has in their inventory")]
    private int _player_fungiCritter_Large;
    public int Player_Fungi_Large { get { return _player_fungiCritter_Large; } }

    [SerializeField, Tooltip("The number of small fungi critters the player has in their inventory")]
    private int _player_crystalCritter_Large;
    public int Player_Crystal_Large { get { return _player_crystalCritter_Large; } }

    [Space]

    [SerializeField, Tooltip("The number of small critters the player has in their inventory")]
    private int _player_smallCount;
    public int SmallCount { get { return _player_smallCount; } }

    [SerializeField, Tooltip("The number of large critters the player has in their inventory")]
    private int _player_largeCount;
    public int LargeCount { get { return _player_largeCount; } }

    [SerializeField, Tooltip("The maximum number of small critters the player can have in their inventory")] 
    private int _player_smallCap;
    public int SmallCap { get { return _player_smallCap; } }

    [SerializeField, Tooltip("The maximum number of large critters the player can have in their inventory")] 
    private int _player_largeCap;
    public int LargeCap { get { return _player_largeCap; } }*/

    [Header ("Ship Inventory")]
    [SerializeField, Tooltip("The number of small fungi critters on the ship")]
    private int _ship_FungiCritter_Small;
    public int Ship_Fungi_Small { get { return _ship_FungiCritter_Small; } }

    [SerializeField, Tooltip("The number of small crystal critters on the ship")]
    private int _ship_CrystalCritter_Small;
    public int ShipCrystalAliens { get { return _ship_CrystalCritter_Small; } }

    [SerializeField, Tooltip("The number of large fungi critters on the ship")]
    private int _ship_FungiCritter_Large;
    public int Ship_FungiCritter_Large { get { return _ship_FungiCritter_Large; } }

    [SerializeField, Tooltip("The number of large crystal critters on the ship")]
    private int _ship_CrystalCritter_Large;
    public int Ship_CrystalCritter_Large { get { return _ship_CrystalCritter_Large; } }

    private SFXManager _sound;

    #endregion

    private void Awake()
    {
        //find the player transform
        //_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //find drop off transform

        /*try
        {
            _shipDropOff = GameObject.FindGameObjectWithTag("DropOff").transform;
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("No transform was given for the drop off point. Using default.\nFULL ERROR\n" + e);
            _shipDropOff = transform;
        }

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;*/
    }

    #region Inventory Management Methods

    //method to update inventories when critter is sucked up
    public void AddCritterToInv(GameObject alien)
    {
        //assign critter stats
        CreatureStats alienType = alien.GetComponent<CreatureStats>();

        //used for if the critter can be sucked up
        bool added = true;
        switch (alienType.Type)
        {
            //crystal critter
            case CreatureStats.creatureType.Crystal:
                //large critter + is room
                if (alienType.IsBig)
                    _ship_CrystalCritter_Large++;
                //small critter + is room
                else if (!alienType.IsBig)
                    _ship_CrystalCritter_Small++;
                //is NOT room
                else
                    added = false;
                break;
            //fungi critter
            case CreatureStats.creatureType.Shroom:
                //large critter + is room
                if (alienType.IsBig)
                    _ship_FungiCritter_Large++;
                //small critter + is room
                else if (!alienType.IsBig)
                    _ship_FungiCritter_Small++;
                //none of the above (no room)
                else
                    added = false;
                break;
            default:
                added = false;
                Debug.Log("Typeless creature found, idk what caused it or how it happened but if you are getting this error you might wanna restart.");
                break;
        }

        //play an animation that scales the critter down to nothing as it moves towards the vaccuum gun centre
        Destroy(alien);
    }

    //moves all critter stored in player inventory to ship inventory
    /*public void MoveToShip()
    {
        //find the distance between the player and the trap
        float distance = Mathf.Abs(Vector3.Distance(_shipDropOff.position, _playerTransform.position));

        //if the distance between the drop off and the player is less than the drop off range
        if (distance < _dropOffRange)
        {
            //update ship inventory
            _ship_FungiCritter_Small += _player_fungiCritter_Small;
            _ship_CrystalCritter_Small += _player_crystalCritter_Small;
            _ship_FungiCritter_Large += _player_fungiCritter_Large;
            _ship_CrystalCritter_Large += _player_crystalCritter_Large;

            //reset player inventory
            _player_fungiCritter_Small = _player_fungiCritter_Large = _player_crystalCritter_Small = _player_crystalCritter_Large = _player_smallCount = _player_largeCount = 0;
        }
    }*/
    #endregion
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
