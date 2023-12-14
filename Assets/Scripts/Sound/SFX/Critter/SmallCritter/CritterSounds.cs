// Created by Adanna Okoye
// Last edited by Adanna Okoye

using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

public class CritterSounds : MonoBehaviour
{
    [SerializeField]private AudioSource CritterAudio;
    public List<AudioClip> CritterClips;

    private float _jumpTime;
    public float JumpSpeed;

    [SerializeField]private CreatureAI _creatureAI;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

        CritterJumping();
    }
    public void CritterJumping()
    {
        
        if ((_creatureAI.ReadState.GetType() == typeof(RoamingState) || (_creatureAI.ReadState.GetType() == typeof(PanicState))))
        {
             CritterAudio.clip = CritterClips[0];
             _jumpTime -= Time.deltaTime;

             if (_jumpTime < 0 && !CritterAudio.isPlaying)
             {
                 // LegSource.volume = 15f / magnitude;
                 CritterAudio.volume = 1;

                 CritterAudio.Play();
                 _jumpTime = JumpSpeed;
             }
        }
        // remeber to frefabe the sound manager
        //  yield return new WaitForSeconds(1);
        // int _soundplayed = UnityEngine.Random.Range(0, CritterClips.Count);


        //   LegSource.Stop();


        // LegSource.time();
    }
}
