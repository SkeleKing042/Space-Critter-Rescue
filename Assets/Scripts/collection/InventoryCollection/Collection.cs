using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Collection : MonoBehaviour
{
    // fixed varibles
    [Header("Fixed Varibles")]
    [SerializeField]
    public float Collected;
    public float DropDistance;
    public float TextNumber;

    [Header("Alien Counts")]
    public int SmallAliens;
    public int BigAliens;

    [Header("bool")]
    public bool SpaceForSmall;
    public bool SpaceForBig;

    [Header("Invetory Refrence")]
    public Inventory Inventory;

    void Update()
    {
       CollectionDropOff();
    }
    public void AddAlienToCollection(GameObject alien)
    {
        if (SmallAliens <= 11)
        {
            SpaceForSmall = true;
            if (alien.tag == "alien"  && SpaceForSmall == true)
            {
                SmallAliens++;
                Collected++;

                Inventory.AddSmallShroom(alien);
                Inventory.AddSmallCrystal(alien);
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
                Collected ++;
                Inventory.AddBigShroom(alien);
                Inventory.AddBigCrystal(alien);

            }
        }
        else
        {
           SpaceForBig = false;
        }
        TextNumber = Collected;
    }
    public void CollectionDropOff()
    {
        if(Input.GetKeyDown(KeyCode.P))
        Collected = 0;

        // add a check for dfistance from ship
        // if whithing distance thenn allow to be put donw

        // use the small and bg alien ints to despences the correct type of alien 
    }
}
