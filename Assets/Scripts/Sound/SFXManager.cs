// created by Adanna Okoye
// last Edited by Adanna Okoye

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class SFXManager : MonoBehaviour
{
    public bool delay;

    public InputAction action;
    public PlayerInput input;


    public AudioSource HoldingSource;
    public AudioSource TrapSource;
    public AudioSource JetPackSource; 

    public AudioSource LegSource;
    public AudioSource ArmSource;
    public AudioSource MiscSource;

    public GameObject JetPack;

    public List<AudioClip> CollectionSounds; // ill chage this to audio clip later.s
    public List<AudioClip> VacuumSounds;
    public List<AudioClip> JetpackSounds;
    public List<AudioClip> DetonatorSounds;
    public List <AudioClip> TrapSounds;
    public List<AudioClip> TabletSounds;
    public List<AudioClip> FootStepSounds;

    public AudioSource JetpackFly;

    [SerializeField] private List<AudioClip> Aliens;
    [SerializeField] private List<AudioClip> AlienSoundSource;
    [SerializeField] private List<CreatureAI> AlienAI;

    float _footstepTimer;
    float _timer;
    bool Recharged;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        action = input.Player.Jump;
        action.Enable();

        JetpackFly.Pause();
        JetPack.SetActive(false);
        Debug.Log("input is: "+ input);
    }
    private void Update()
    {
        //testing
        if(Input.GetKeyDown(KeyCode.P))
        {
            CritterCollection();
        }

        if(action.WasReleasedThisFrame())
        {
            JetPack.SetActive(false);
        }
    }

    #region Collection
    public void CritterCollection()
    {
        // remeber to frefabe the sound manager
        int _soundplayed = UnityEngine.Random.Range(0, CollectionSounds.Count);
        MiscSource.PlayOneShot(CollectionSounds[_soundplayed], 1f);
    }
    public void SuckingSound(float TimePressed) 
    {
        HoldingSource.PlayOneShot(VacuumSounds[0],0.1f);
        float SoundTimer = Time.deltaTime;
        while(TimePressed > SoundTimer)
        {
           HoldingSource.PlayOneShot(VacuumSounds[1],0.7f);
        }
         if(SoundTimer > TimePressed)
        {
            Debug.Log("closing sound");
            HoldingSource.PlayOneShot(VacuumSounds[2], 1f);
        }
    }
    #endregion

    // jet pack/ player footsteps overlap. Need to fix using Delays. and new Audio Sources
    // possibly using a coruotine.
    #region JetPack
    public void Jetpackflying()
    {
            Recharged = false;
            JetPack.SetActive(true);

    
        //JetPack.SetActive(false);
        //Looping = false;
        //Source.PlayOneShot(JetpackSounds[2], 1f);
    }
    public void JetpackRecharge()
    {
      
        if (!Recharged)
        {
            JetPackSource.PlayOneShot(JetpackSounds[3], 0.1f);
            Recharged = true;
            _footstepTimer = 1;
        }
    }
    #endregion
    #region Player Movement

    public void Walking(float magnitude)
    {
        // remeber to frefabe the sound manager
        //  yield return new WaitForSeconds(1);

        _footstepTimer -= Time.deltaTime;

        if (_footstepTimer < 0 && !LegSource.isPlaying)
        {
            LegSource.volume = 15f / magnitude;
            int _soundplayed = UnityEngine.Random.Range(0, FootStepSounds.Count);
            LegSource.clip = FootStepSounds[_soundplayed];
            LegSource.Play();
            
            _footstepTimer = 1;
        }
       
  
     //   LegSource.Stop();

        
           // LegSource.time();
     }
       // FootStepSounds[_soundplayed].Play();

        // }
        //if(Sprinting)
        //{
        //    
        //    LegSource.PlayOneShot(FootStepSounds[_soundplayed], 1f);
        //}

    #endregion

    #region Detonator
    public void SwapDetonator()
    {
        ArmSource.PlayOneShot(DetonatorSounds[0], 1f);
    }
    public void Detonator()
    {
        //playes detornator 
        HoldingSource.PlayOneShot(DetonatorSounds[1], 1f);
        TrapSource.PlayOneShot(TrapSounds[0], 0.8f); 
    }
    #endregion

    #region Trap

    public void TrapDrop()
    {
        TrapSource.PlayOneShot(TrapSounds[1], 1f);
    }
    #endregion

    #region Tablet
    public void TabletOnOff(bool TabletActive)
    {
        if(!TabletActive)
            HoldingSource.PlayOneShot(TabletSounds[0], 1f);
        else
            HoldingSource.PlayOneShot(TabletSounds[1], 1f);
    }

    public void TabletChange()
    {
        HoldingSource.PlayOneShot(TabletSounds[2], 1f);
    }
    public void Teleport()
    {
        HoldingSource.PlayOneShot(TabletSounds[3], 1f);
    }

    #endregion

    //IDLE SOUNDS
    // creaqt a random number
    // if the number lands on 20 then platy a random critter sound

    // Check the states of the Aliens in the List
    // make sure the 
    #region Alien

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "alien")
        {
            //if(AlienAI.)
        }

    }
  #endregion
}


  