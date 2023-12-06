// created by Adanna Okoye
// last Edited by Adanna Okoye

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public bool Looping;

    public AudioSource Source;
    public AudioSource TrapSource;

    public List<AudioClip> CollectionSounds; // ill chage this to audio clip later.s
    public List<AudioClip> VacuumSounds;
    public List<AudioClip> JetpackSounds;
    public List<AudioClip> DetonatorSounds;
    public List <AudioClip> TrapSounds;
    public List<AudioClip> TabletSounds;


    // varibkles need to mbe moved ONLY FOR TESTING


    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            CritterCollection();
        }

        //testing
    }

    #region Collection
    public void CritterCollection()
    {
        // remeber to frefabe the sound manager
        int _soundplayed = UnityEngine.Random.Range(0, CollectionSounds.Count);
        Source.PlayOneShot(CollectionSounds[_soundplayed]);
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
    #endregion

    #region JetPack
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
    #endregion


    // Implement these functions when branch is free
    #region Detonator
    public void SwapDetonator()
    {
        Source.PlayOneShot(DetonatorSounds[0], 1f);
    }
    public void Detonator(bool TrapActive)
    {
        //playes detornator 
        Source.PlayOneShot(DetonatorSounds[1], 1f);
        if (!TrapActive)
            TrapSource.PlayOneShot(TrapSounds[0], 1f); // TODO make this stop if the trap gets disabled.
        else
            TrapSource.Stop();
    }
    // may need to add a dectivate trap

    #endregion

    #region Trap

    // put this inthe trap deplot function laterr
    public void TrapDrop()
    {
        TrapSource.PlayOneShot(TrapSounds[1], 1f);
    }
    #endregion

    #region Tablet
    public void TabletOnOff(bool TabletActive)
    {
        if(!TabletActive)
            Source.PlayOneShot(TabletSounds[0], 1f);
        else
            Source.PlayOneShot(TabletSounds[1], 1f);
    }

    public void TabletChange()
    {
        Source.PlayOneShot(TabletSounds[2], 1f);
    }
    public void Teleport()
    {
        Source.PlayOneShot(TabletSounds[3], 1f);
    }

    #endregion
}
