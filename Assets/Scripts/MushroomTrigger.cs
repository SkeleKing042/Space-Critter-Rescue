//Created by Adanna Okoye
//Last Edited Adanna Okoye
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _mushroomFog;
    private Vector3 DefultMushFogPos;
    private Camera _camera;

    public float LerpSpeed;


    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        DefultMushFogPos = _mushroomFog.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(_player != null)
        {
            _mushroomFog.transform.position = Vector3.Lerp(_mushroomFog.transform.position, _player.transform.position, LerpSpeed);
            Debug.Log("lerping");
            _mushroomFog.transform.rotation = Quaternion.Euler(0, _camera.transform.rotation.y,0);
        }
        else
        {
            _mushroomFog.transform.position = DefultMushFogPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag == "Player")
        {
           
            _player = other.gameObject;
            Debug.Log(_player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.gameObject.tag == "Player")
        {
            _mushroomFog.transform.position = DefultMushFogPos; Debug.Log(_mushroomFog.transform.position);
            _player = null;
            Debug.Log("collision exit");
        }
    }
}
//TODO
// a collider is stopping the player not sure where. Isnt the fungitrigger
// make the fungi fog follow players rotation
//


