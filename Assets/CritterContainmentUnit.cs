using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterContainmentUnit : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Transform _spawnTransform;

    [Header("Critters")]
    [SerializeField] private List<GameObject> _smallCrystals;
    [SerializeField] private List<GameObject> _smallFungi;
    [SerializeField] private List<GameObject> _largeCrystals;
    [SerializeField] private List<GameObject> _largeFungi;

    [Header("Prefabs")]
    [SerializeField] private GameObject _smallCrystal_prefab;
    [SerializeField] private GameObject _smallFungi_prefab;
    [SerializeField] private GameObject _largeCrystal_prefab;
    [SerializeField] private GameObject _largeFungi_prefab;


    private void Awake()
    {
        _inventory = FindObjectOfType<Inventory>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_smallCrystals.Count < _inventory.Ship_Crystal_Small)
        {
            _smallCrystals.Add(Instantiate(_smallCrystal_prefab, _spawnTransform.position, _spawnTransform.rotation, gameObject.transform));
        }

        if (_smallFungi.Count < _inventory.Ship_Fungi_Small)
        {
            _smallFungi.Add(Instantiate(_smallFungi_prefab, _spawnTransform.position, _spawnTransform.rotation, gameObject.transform));
        }

        if (_largeCrystals.Count < _inventory.Ship_CrystalCritter_Large)
        {
            _largeCrystals.Add(Instantiate(_largeCrystal_prefab, _spawnTransform.position, _spawnTransform.rotation, gameObject.transform));
        }

        if (_largeFungi.Count < _inventory.Ship_FungiCritter_Large)
        {
            _largeFungi.Add(Instantiate(_largeFungi_prefab, _spawnTransform.position, _spawnTransform.rotation, gameObject.transform));
        }
    }
}
