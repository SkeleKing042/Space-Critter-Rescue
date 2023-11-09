//Created by Jackson Lucas
//Last edited by Jackson Lucas

using UnityEngine;

public class CheckPlayerFire : MonoBehaviour
{
    private TrapDeploy trapDeploy;
    private VacuumGun gun;
    private Trap trap;

    void Start()
    {
        trapDeploy = GetComponent<TrapDeploy>();
        gun = GetComponentInChildren<VacuumGun>();
        trap = GetComponentInChildren<Trap>();
    }

    /// <summary>
    /// Checks what the player is holding out and calls the correct fire func
    /// </summary>
    public void FireCheck()
    {
        switch (trapDeploy.currentlyHolding)
        {
            case TrapDeploy.CurrentlyHolding.vacuum:
                gun.Pull();
                break;
            case TrapDeploy.CurrentlyHolding.trap:
                trapDeploy.DeployTrap();
                break;
            //case TrapDeploy.CurrentlyHolding.detinator:
              //  trap.TrapCatching();
                //break;
        }
    }
}
