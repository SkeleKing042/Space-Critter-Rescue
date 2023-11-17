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
    /// Propagates a soudn out from the player.
    /// Scale ranges from 1 to 0, 1 being full size and 0 being nothing (or off)
    /// </summary>
    /// <param name="scale"></param>
    public void PropagateSound(float scale)
    {
        Debug.Log("LOUD NOISE");
        foreach(GameObject goob in _goobs)
        {
            if(Vector3.Distance(transform.position, goob.transform.position) >= _proaDistance * scale)
            goob.GetComponent<CreatureAI>().RunFromPlayer(0);
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
