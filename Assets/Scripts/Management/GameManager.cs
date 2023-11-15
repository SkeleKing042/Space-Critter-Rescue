//Created by Jackson Lucas
//Last edited by Jackson Lucas

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class CreatureTrackers
    {
        [SerializeField, Range(0f, 1f)]
        private float _smallPercent;
        private float _smallCount;
        public float SmallCount { get { return _smallCount; } set { _smallCount = value; } }
        [SerializeField, Range(0f, 1f)]
        private float _largePercent;
        private float _largeCount;
        public float LargeCount { get { return _largeCount; } set { _largeCount = value; } }

        [SerializeField]
        private Image _smallBar;
        [SerializeField]
        private Image _largeBar;

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
        public void SetCounts(float bigCount, float smallCount)
        {
            _largeCount = bigCount;
            _smallCount = smallCount;
        }
        public bool PercentCheck(float bigs, float smalls)
        {
            return (bigs / _largeCount > _largePercent && smalls / _smallCount > _smallPercent);
        }
        public void UpdateBars(int largeCount, int smallCount)
        {
            _smallBar.fillAmount = smallCount / SmallCount;
            _largeBar.fillAmount = largeCount / LargeCount;
        }
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void VicCheck()
    {
        _shroomCreatures.PercentCheck(_inv.ShroomAliensBig, _inv.ShroomAliens);
        _crystalCreatures.PercentCheck(_inv.CrystalAliensBig, _inv.CrystalAliens);
    }
    public void UpdateAllBars()
    {
        _shroomCreatures.UpdateBars(_inv.InvShroomAliensBig, _inv.InvShroomAliens);
        _crystalCreatures.UpdateBars(_inv.InvCrystalAliensBig, _inv.InvCrystalAliens);
    }
}
