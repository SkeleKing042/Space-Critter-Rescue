//Created by Jackson Lucas
//Last edited by Jackson Lucas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float Radius;
    [Range(0f, 360f)]
    public float Angle;

    public GameObject PlayerRef;

    [Header("Masks"),SerializeField]
    private LayerMask _targetMask;
    [SerializeField]
    private LayerMask[] _obstructionMasks;

    public bool CanSeeTarget;

    private CreatureAI AI;

    private void Start()
    {
        PlayerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        AI = GetComponent<CreatureAI>();
    }
    private IEnumerator FOVRoutine()
    { 
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, Radius, _targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < Angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                bool obstructed = false;
                foreach(LayerMask mask in _obstructionMasks)
                {
                    obstructed = Physics.Raycast(transform.position, directionToTarget, distanceToTarget, mask);
                }

                if (!obstructed && Vector3.Distance(transform.position, target.position) < Radius)
                    CanSeeTarget = true;
                else
                    CanSeeTarget = false;
            }
            else
                CanSeeTarget = false;
        }
        else if (CanSeeTarget)
            CanSeeTarget = false;
    }
}
