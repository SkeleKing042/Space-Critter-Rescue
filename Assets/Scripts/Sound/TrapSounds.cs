using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TrapSounds : MonoBehaviour
{
    [SerializeField] private Collider _trapCollider;
    private SFXManager _sfxManager;

    // Start is called before the first frame update
    void Start()
    {
       _sfxManager = FindObjectOfType<SFXManager>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            _sfxManager.TrapDrop();
        }
    }

}
