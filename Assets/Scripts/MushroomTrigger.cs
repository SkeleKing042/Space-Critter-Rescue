using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private GameObject _mushroomFog;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _mushroomFog.transform.position = Vector3.Lerp(_mushroomFog.transform.position, _player.position, 1f);
        _mushroomFog.transform.rotation = _player.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            _player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            _player = null ;
        }
    }
}
//TODO
// a collider is stopping the player not sure where. Isnt the fungitrigger
// make the fungi fog follow players rotation
//


