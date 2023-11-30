// Created By Adanna Okoye
//Last Edited by Jackson Lucas

using UnityEngine;

public class CollectAlien : MonoBehaviour
{
    //[Header("Script Refrences")]
    private VacuumGun _vac;
    private Inventory _inv;
    private Trap _trap;


    GameObject Alien;

    private void Awake()
    {
        _vac = FindObjectOfType<VacuumGun>();
        _inv = FindObjectOfType<Inventory>();
        _trap = FindObjectOfType<Trap>();
    }

    void OnTriggerEnter(Collider creature)
    {
        if ((creature.gameObject.tag == "alien" || (creature.gameObject.tag == "bigAlien" && _trap.Catchable == true)))
        {
           // Debug.Log("tags passed");
            if(_vac.Pulling == true)
            {
                Debug.Log("get stareeddd");
                //  Debug.Log("Pulling check passed");
                _inv.AddCritterToInv(creature.gameObject);
                _vac.UnassignAlien(creature.gameObject);
            }           
        }
    }
}
