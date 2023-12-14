//Created by Adanna Okoye
//Last Edited by Adanna Okoye

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCreatureSounds : MonoBehaviour
{
    [SerializeField] private AudioSource CritterAudio;
    public List<AudioClip> CritterPanic;
    public List<AudioClip> Idle;
    public List<AudioClip> Eating;
    public List<AudioClip> Drinking;
    public List<AudioClip> Stunned;
    public List<AudioClip> Alert;
    public List<AudioClip> Sleep;
    public List<AudioClip> Captured;

     private float _time;
    [SerializeField] private float _soundSpeed;
    [SerializeField] private float _sleepVolume;
    public float JumpSpeed;

    [SerializeField] private CreatureAI _creatureAI;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        CritterSFX();
    }
    public void CritterSFX()
    {

        if  (_creatureAI.ReadState.GetType() == typeof(PanicState))
        {
            CritterAudio.clip = CritterPanic[0];
            _time -= Time.deltaTime;

            if (_time < 0 && !CritterAudio.isPlaying)
            {
                // LegSource.volume = 15f / magnitude;
             //  CritterAudio.volume = 1;

                CritterAudio.Play();
                _time = JumpSpeed;
            }
        }
        if ((_creatureAI.ReadState.GetType() == typeof(IdleState)))
        {
            //CritterAudio.clip = CritterClips[0];
            int _soundplayed = UnityEngine.Random.Range(0, Idle.Count);
            CritterAudio.clip = Idle[_soundplayed];
            _time -= Time.deltaTime;

            if (_time < 0 && !CritterAudio.isPlaying)
            {
                // LegSource.volume = 15f / magnitude;
               // CritterAudio.volume = 1;

                CritterAudio.Play();
                _time = _soundSpeed;
            }

        }
        if ((_creatureAI.ReadState.GetType() == typeof(SleepState)))
        {
            //CritterAudio.clip = CritterClips[0];
            int _soundplayed = UnityEngine.Random.Range(0, Idle.Count);
            CritterAudio.clip = Sleep[_soundplayed];
            _time -= Time.deltaTime;

            if (_time < 0 && !CritterAudio.isPlaying)
            {
                // LegSource.volume = 15f / magnitude;
              //  CritterAudio.volume = _sleepVolume;

                CritterAudio.Play();
                _time = _soundSpeed;
            }
        }
        if ((_creatureAI.ReadState.GetType() == typeof(DrinkingState)))
        {
            //CritterAudio.clip = CritterClips[0];
            int _soundplayed = UnityEngine.Random.Range(0, Drinking.Count);
            CritterAudio.clip = Drinking[_soundplayed];
            _time -= Time.deltaTime;

            if (_time < 0 && !CritterAudio.isPlaying)
            {
                // LegSource.volume = 15f / magnitude;
                //CritterAudio.volume = 1;

                CritterAudio.Play();
                _time = _soundSpeed;
            }
        }
        if ((_creatureAI.ReadState.GetType() == typeof(CaptureState)))
        {
            int _soundplayed = UnityEngine.Random.Range(0, Captured.Count);
            CritterAudio.clip = Captured[_soundplayed];
            _time -= Time.deltaTime;

            if (_time < 0 && !CritterAudio.isPlaying)
            {
                // LegSource.volume = 15f / magnitude;
              //  CritterAudio.volume = 1;

                CritterAudio.Play();
                _time = _soundSpeed;
            }
        }
        if ((_creatureAI.ReadState.GetType() == typeof(AlertState)))
        {
            int _soundplayed = UnityEngine.Random.Range(0, Alert.Count);
            CritterAudio.clip = Alert[_soundplayed];
            _time -= Time.deltaTime;

            if (_time < 0 && !CritterAudio.isPlaying)
            {
                // LegSource.volume = 15f / magnitude;
                //CritterAudio.volume = 1;

                CritterAudio.Play();
                _time = _soundSpeed;
            }
        }
        if ((_creatureAI.ReadState.GetType() == typeof(StunnedState)))
        {
            int _soundplayed = UnityEngine.Random.Range(0, Stunned.Count);
            CritterAudio.clip = Stunned[_soundplayed];
            _time -= Time.deltaTime;

            if (_time < 0 && !CritterAudio.isPlaying)
            {
                // LegSource.volume = 15f / magnitude;
               // CritterAudio.volume = 1;

                CritterAudio.Play();
                _time = _soundSpeed;
            }
        }
    }
}