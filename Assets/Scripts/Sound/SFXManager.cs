using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{



    public List<AudioSource> CollectionSounds;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // CritterCollection();
    }

    public void CritterCollection()
    {
        // remeber to frefabe the sound manager
        int _soundplayed = UnityEngine.Random.Range(0, CollectionSounds.Count);
        CollectionSounds[_soundplayed].Play();
    }

    public void SuckingSound()
    {

    }
}
