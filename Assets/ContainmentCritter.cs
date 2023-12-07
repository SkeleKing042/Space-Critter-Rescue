using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainmentCritter : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float force;
    [SerializeField] private Vector3 direction;

    [SerializeField] private float timer;
    [SerializeField] private float timerMax;
    [SerializeField] private float timerLowerBound;
    [SerializeField] private float timerUpperBound;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        timerMax = Random.Range(timerUpperBound, timerLowerBound);
    }

    private void Update()
    {
        if(timer < 0)
        {
            timer = timerMax;

            direction = Random.onUnitSphere;
            _rb.AddForce(direction * force);
        }

        timer -= Time.deltaTime;
        
    }
}
