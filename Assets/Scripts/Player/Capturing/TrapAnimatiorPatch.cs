using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapAnimatiorPatch : MonoBehaviour
{
    Trap _trap;
    private void Awake()
    {
        _trap = transform.parent.GetComponent<Trap>();
    }
    public void Activates()
    {
        _trap.ActivateTrap();
    }
    public void Deactivate()
    {
        _trap.DeactivateTrap();
    }
}
