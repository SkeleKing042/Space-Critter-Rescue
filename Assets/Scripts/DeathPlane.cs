using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    public List<string> AlienTags = new List<string>();
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something has hit the deathplane...");
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().ReturnToLastGrounedPoint();
        }
        if (other.tag.ToLower() == "trap")
        {
            //GameObject.FindGameObjectWithTag("Player").GetComponent<Equipment>().pickUpTrap(false);
        }
        if (AlienTags.Contains(other.tag))
        {
            Debug.Log(other.name + " fell in to the deathplane...");
            other.GetComponent<CreatureAI>().ReturnToLastGrounedPoint();
        }
    }
}
