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

    }

    public GameObject Player;
    public List<IslandInfo> iData;

    // space music always playing
    // when in close radius only play specific music

    private void Update()
    {
        Volume();
    }

    void Volume()
    {
        foreach (IslandInfo islands in iData)
        {
            islands.IslandDistance = Vector3.Distance(islands.IslandPosition.transform.position, Player.transform.position);
            islands.IslandDistance= Mathf.Abs(islands.IslandDistance);

            if(islands.IslandDistance > 100)
            {
                islands.IslandMusic.volume = 0;
            }
            else
            {
                islands.IslandMusic.volume = 100 / islands.IslandDistance;
            }

          

        }

    }
}
