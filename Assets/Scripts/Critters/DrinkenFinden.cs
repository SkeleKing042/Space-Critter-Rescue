using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkenFinden : MonoBehaviour
{
    private List<GameObject> _source = new List<GameObject>();
    public List<GameObject> Sources { get { return _source; } }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<DrinkableSource>() && !_source.Contains(other.gameObject))
            _source.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<DrinkableSource>() && _source.Contains(other.gameObject))
                _source.Remove(other.gameObject);
    }
}
