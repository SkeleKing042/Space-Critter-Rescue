using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeReference]
    private AudioSource FungiMusic;
    [SerializeReference]
    private AudioSource CrystalMusic;
    public GameObject player;
    public GameObject FungiIsland;

    private Vector3 _distanceFromIsland;


    // space music always playing
    // when in close radius only play specific music

    void Volume()
    {
        _distanceFromIsland = FungiIsland.transform.position - player.transform.position;

    // if(_distanceFromIsland => 100)
    // {
    //     FungiMusic.volume = 0;
    // }
    //
    // if(_distanceFromIsland < 100)
    // {
    //
    // }


    }

    

}
