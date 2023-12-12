// Created By Adanna Okoye
//Last Edited by Jackson Lucas

using UnityEngine;

public class CollectAlien : MonoBehaviour
{
    //[Header("Script Refrences")]
    private VacuumGun _vac;
    private Inventory _inv;
    private Trap _trap;

    [SerializeField] private Animator _VC_Animator;
    [SerializeField] int crittersCaught;
    [SerializeField] bool inAnimation;

    private void Awake()
    {
        _vac = FindObjectOfType<VacuumGun>();
        _inv = FindObjectOfType<Inventory>();
        _trap = FindObjectOfType<Trap>();
    }

    private void Update()
    {
        
    }


    void OnTriggerEnter(Collider critter)
    {
        if (critter.gameObject.tag == "alien")
        {
           // Debug.Log("tags passed");
            if(_vac.Pulling == true)
            {
                //  Debug.Log("Pulling check passed");
                _inv.AddCritterToInv(critter.gameObject);
                _vac.UnassignAlien(critter.gameObject);
                _VC_Animator.SetTrigger("SuckTrigger");
            }           
        }
    }

    public void EndSuckAnimation()
    {
        crittersCaught--;
    }
}
