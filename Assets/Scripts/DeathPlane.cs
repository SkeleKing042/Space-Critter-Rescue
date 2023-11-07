using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().ReturnToLastGrounedPoint();
        }
        if(other.tag.ToLower() == "trap")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<TrapDeploy>().pickUp(false);
        }
    }
}
