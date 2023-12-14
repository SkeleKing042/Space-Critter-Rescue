using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TrapSounds : MonoBehaviour
{
    [SerializeField] private Collider _trapCollider;
    private SFXManager _sfxManager;
    private SoundPropagation _soundPropagation;
    // Start is called before the first frame update
    void Start()
    {
       _sfxManager = FindObjectOfType<SFXManager>();  
       _soundPropagation = _trapCollider.GetComponent<SoundPropagation>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            _soundPropagation.PropagateSound(0.1f);
           // _sfxManager.TrapDrop();
        }
    }

}
