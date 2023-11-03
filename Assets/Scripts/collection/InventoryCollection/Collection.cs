using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Collection : MonoBehaviour
{
    // fixed varibles
    [SerializeField]
    public float Collected;
    public float DropDistance;
    public float TextNumber;

    public int SmallAliens;
    public int BigAliens;

    public bool SpaceForSmall;
    public bool SpaceForBig;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // MAKE THEM INTS  

    // Update is called once per frame
    void Update()
    {
       CollectionDropOff();
    }
    public void AddAlienToCollection(GameObject alien)
    {
        if (SmallAliens < 12)
        {
            SpaceForSmall = true;
            if (alien.tag == "alien")
            {
                SmallAliens++;
                Collected++;
            }
        }
        else
        {
            SpaceForSmall = false;
        }
        if(BigAliens < 2)
        {
            SpaceForBig = true;
            if (alien.tag == "bigAlien")
            {
                BigAliens++;
                Collected ++;
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
