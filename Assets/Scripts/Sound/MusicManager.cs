using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static VacuumGun;

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public class IslandInfo
    {
        public GameObject Island;
        public float IslandDistance;
        public Transform IslandPosition;
        public AudioSource IslandMusic;

        [Tooltip("This is the distance where the player will start to hear the music on full volume")]
        public int _maxMusicDistance;

        [Tooltip("This is the distance where the player will start to hear the music on the lowest volume")]
        public float _minMusicDistance;

        [Tooltip("should be close the the min music distance for smooth transition form qiet to loud")]
        public float _distanceVolumeScale;

    }
    public GameObject Player;   
    public List<IslandInfo> iData;
    // space music always playing
    // when in close radius only play specific music
    private void Start()
    {

    }
    private void Update()
    {
        Volume();
    }
    void Volume()
    {
        float _volumeScale;
        foreach (IslandInfo islands in iData)
        {
            islands.IslandDistance = Vector3.Distance(islands.IslandPosition.transform.position, Player.transform.position);
            islands.IslandDistance= Mathf.Abs(islands.IslandDistance);

            if(islands.IslandDistance > islands._minMusicDistance)
            {
                islands.IslandMusic.volume = 0;
            }

            if(islands.IslandDistance > islands._maxMusicDistance && islands.IslandDistance <islands._minMusicDistance)
            {
                _volumeScale = islands.IslandDistance /islands._distanceVolumeScale;

                islands.IslandMusic.volume = 1 - _volumeScale;
                Debug.Log(islands.IslandMusic.volume);
            }


            if(islands.IslandDistance <islands._maxMusicDistance)
            {
                islands.IslandMusic.volume = 1;
                Debug.Log(islands.IslandMusic.volume);
            }


        }

    }
}
