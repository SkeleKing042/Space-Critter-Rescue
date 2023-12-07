//Created by Jackson Lucas
//Last edited by Jackson Lucas

using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*[System.Serializable]
    public class CreatureTrackers
    {
        [SerializeField, Range(0f, 1f), Tooltip("The % of Small Critters the player needs to collect to win")]
        private float _smallPercent;
        public float SmallPercent { get { return _smallPercent; } }

        [SerializeField, Tooltip("Amount of Small Critters the player has collected")]
        private float _smallCount;
        public float SmallCount { get { return _smallCount; } set { _smallCount = value; } }

        [SerializeField, Range(0f, 1f), Tooltip("The % of Small Critters the player needs to collect to win")]
        private float _largePercent;
        public float LargePercent { get { return _largePercent; } }
        [SerializeField, Tooltip("Amount of Small Critters the player has collected")]
        private float _largeCount;
        public float LargeCount { get { return _largeCount; } set { _largeCount = value; } }

        #region Sets Gets and Checks
        public void GetCounts(CreatureStats.creatureType type)
        {
            CreatureStats[] stats = FindObjectsOfType<CreatureStats>();
            foreach(CreatureStats creature in stats)
            {
                if (creature.Type == type)
                    if (creature.IsBig)
                        _largeCount++;
                    else if (!creature.IsBig)
                        _smallCount++;
            }
        }

        public void SetCounts(float largeCount, float smallCount)
        {
            _largeCount = largeCount;
            _smallCount = smallCount;
        }

        public bool PercentCheck(float larges, float smalls)
        {
            return (larges / _largeCount > _largePercent && smalls / _smallCount > _smallPercent);
        }

        #endregion
    }

    [SerializeField]
    private CreatureTrackers _crystalCreatures;
    public CreatureTrackers CrystalCreatures { get { return _crystalCreatures; } }
    [SerializeField]
    private CreatureTrackers _shroomCreatures;
    public CreatureTrackers ShroomCreatures { get { return _shroomCreatures; } }
    private Inventory _inv;
    // Start is called before the first frame update
    void Start()
    {
        _inv = GetComponent<Inventory>();

        CreatureStats[] stats = FindObjectsOfType<CreatureStats>();
        foreach(CreatureStats creature in stats)
        {
            switch (creature.Type)
            {
                case CreatureStats.creatureType.Shroom:
                    if (creature.IsBig)
                        _shroomCreatures.LargeCount++;
                    else
                        _shroomCreatures.SmallCount++;
                    break;
                case CreatureStats.creatureType.Crystal:
                    if (creature.IsBig)
                        _crystalCreatures.LargeCount++;
                    else
                        _crystalCreatures.SmallCount++;
                    break;
                        
            }
        }

    }*/

    /*void VicCheck()
    {
        _shroomCreatures.PercentCheck(_inv.PlayerShroomAliensBig, _inv.PlayerShroomAliens);
        _crystalCreatures.PercentCheck(_inv.PlayerCrystalAliensBig, _inv.PlayerCrystalAliens);
    }*/
}
