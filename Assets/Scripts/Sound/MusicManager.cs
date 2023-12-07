using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static VacuumGun;

public class MusicManager : MonoBehaviour
{

    // class that contains each varible for the music to scale properly
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

    // player
    public GameObject Player;   
    // list of islands
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
    /// <summary>
    /// Will get the distance from each island and play the music louder depending on hous close the player gets
    /// </summary>
    void Volume()
    {
        // chnaging float value used to change the volume
        float _volumeScale;
        foreach (IslandInfo islands in iData)
        {

            // get the distance from the player from the island
            islands.IslandDistance = Vector3.Distance(islands.IslandPosition.transform.position, Player.transform.position);
            islands.IslandDistance= Mathf.Abs(islands.IslandDistance);

            // if too far away make the volume 0;
            if(islands.IslandDistance > islands._minMusicDistance)
            {
                islands.IslandMusic.volume = 0;
            }

            // if withing range scale volume depending on distance
            if(islands.IslandDistance > islands._maxMusicDistance && islands.IslandDistance <islands._minMusicDistance)
            {
                // gets players distance and input distance to create a scale
                _volumeScale = islands.IslandDistance /islands._distanceVolumeScale;
                islands.IslandMusic.volume = 1 -_volumeScale;
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
