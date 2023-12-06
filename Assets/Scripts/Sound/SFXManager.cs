// created by Adanna Okoye
// last Edited by Adanna Okoye

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public bool Looping;

    public AudioSource Source;

    public List<AudioSource> CollectionSounds; // ill chage this to audio clip later.s
    public List<AudioClip> VacuumSounds;
    public List<AudioClip> JetpackSounds;
               

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Looping);
    }

    public void CritterCollection()
    {
        // remeber to frefabe the sound manager
        int _soundplayed = UnityEngine.Random.Range(0, CollectionSounds.Count);
        CollectionSounds[_soundplayed].Play();
    }

    public void SuckingSound(float TimePressed) 
    {
        Source.PlayOneShot(VacuumSounds[0],1f);
        float SoundTimer = Time.deltaTime;
        while(TimePressed > SoundTimer)
        {
           Source.PlayOneShot(VacuumSounds[1],1f);
        }
         if(SoundTimer > TimePressed)
        {
            Debug.Log("closing sound");
            Source.PlayOneShot(VacuumSounds[2], 1f);
        }
       
    }

    public void Jetpackflying(float TimePressed)
    {
        Source.PlayOneShot(JetpackSounds[0],1f);
        float SoundTimer = Time.deltaTime;
        while(TimePressed > SoundTimer)
        {
            if (!JetpackSounds[1])
            Source.PlayOneShot(JetpackSounds[1], 1f);
                // needs tweaking
        }
        //Looping = false;
      
        //Source.PlayOneShot(JetpackSounds[2], 1f);

    }
    public void JetpackRecharge()
    {
        Source.PlayOneShot(JetpackSounds[3], 0.5f);
    }
}
