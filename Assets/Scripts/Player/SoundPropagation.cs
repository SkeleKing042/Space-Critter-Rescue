//Created by Jackson Lucas
//Last edited by Jackson Lucas

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SoundPropagation : MonoBehaviour
{
    //private AudioSource _aSource;
    private SphereCollider _soundField;
    private float _proaDistance;
    private List<GameObject> _goobs = new List<GameObject>();
    private void Start()
    {
        _soundField = GetComponent<SphereCollider>();
        _proaDistance = _soundField.radius;
    }
    /// <summary>
    /// Propagates a sound out from the player.
    /// Scale ranges from 1 to 0, 1 being full size and 0 being nothing (or off)
    /// </summary>
    /// <param name="scale"></param>
    public void PropagateSound(float scale)
    {
        //Go through the list of creatures within earshot
        foreach(GameObject goob in _goobs)
        {
            //If they still exist
            if(goob != null)
            {
                //and are in range
                if(Vector3.Distance(transform.position, goob.transform.position) >= _proaDistance * scale)
                    //Scare them
                    goob.GetComponent<CreatureAI>().RunFromPlayer(0);
            }
            else
                //remove them from the list
                _goobs.Remove(goob);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CreatureAI>() && !_goobs.Contains(other.gameObject))
        {
            _goobs.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (_goobs.Contains(other.gameObject))
        {
            _goobs.Remove(other.gameObject);
        }
    }
}
