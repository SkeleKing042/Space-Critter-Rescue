//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour
{
    public enum CritterState { 
    idle,
    roming,
    checking,
    drinking,
    sleeping,
    startPanicing,
    panicing,
    capturing,
    stunned
    };

    public CritterState State;

    public float VelocityToPanic;
    public float POIDetectionRadius;
    public float PlayerDectectionRadius;
    private Vector3 _homePoint;
    public float TravelableDistanceFromHome;
    private NavMeshAgent _agent;
    [Header("Threasholds")]
    [Range(0f, 100f)]
    public float Thirstiness;
    [Range(0f, 100f)]
    public float Lazyness;
    [Range(0f, 100f)]
    public float Fearfulness;
    [Header("Tags")]
    public string PlayerTag;
    private GameObject _player;
    public string DrinkableTag;

    private void Start()
    {
        _homePoint = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag(PlayerTag);
    }
    // Update is called once per frame
    void Update()
    {
        float rnd;
        switch (State)
        {
            case CritterState.idle:
                //Check for player
                if (Vector3.Distance(transform.position, _player.transform.position) < PlayerDectectionRadius)
                {
                    StartCoroutine(UpdateState(CritterState.panicing, 0));
                    break;
                }

                //Choose to move
                rnd = Random.Range(0f, 100f);
                if (rnd > Lazyness)
                {
                    //If moving, where?
                    float n = TravelableDistanceFromHome / 2;
                    Vector3 newPos = new Vector3(_homePoint.x + Random.Range(-n, n), _homePoint.y + Random.Range(-n, n), _homePoint.z + Random.Range(-n, n));
                    Debug.DrawRay(newPos, Vector3.up * 0.1f, Color.blue, 1f);
                    //Move there
                    _agent.SetDestination(newPos);
                    //Start roming
                    StartCoroutine(UpdateState(CritterState.roming, 0));
                    break;
                }
                else if (rnd < Lazyness / 2)
                {
                    StartCoroutine(UpdateState(CritterState.sleeping, 0));
                    break;
                }
                break;
            case CritterState.roming:
                //Check for player
                if (Vector3.Distance(transform.position, _player.transform.position) < PlayerDectectionRadius)
                {
                    StartCoroutine(UpdateState(CritterState.panicing, 0));
                    break;
                }
                //Check for POIs
                rnd = Random.Range(0f, 100f);
                if (rnd < Thirstiness)
                    foreach (GameObject drinkable in GameObject.FindGameObjectsWithTag(DrinkableTag))
                        if (Vector3.Distance(transform.position, drinkable.transform.position) < POIDetectionRadius)
                        {
                            //Go to the edge of the water source
                            _agent.SetDestination(drinkable.transform.position);
                            break;
                        }
                //Are we at the point?
                Debug.Log(Vector3.Distance(transform.position, _agent.destination));
                if (Vector3.Distance(transform.position, _agent.destination) <= 1f)
                {
                    StartCoroutine(UpdateState(CritterState.idle, 0));
                    break;
                }
                //Keep moving
                break;
            case CritterState.checking:

                break;
            case CritterState.drinking:
                //Choose to continue drinking
                rnd = Random.Range(0f, 100f);
                if (rnd > Thirstiness)
                    StartCoroutine(UpdateState(CritterState.idle, 0));
                break;
            case CritterState.sleeping:
                //Choose to continue sleeping
                rnd = Random.Range(0f, 100f);
                if (rnd > Lazyness)
                    StartCoroutine(UpdateState(CritterState.idle, 0));
                break;
            case CritterState.startPanicing:
                _agent.SetDestination(_homePoint);
                _agent.speed *= 2;
                break;
            case CritterState.panicing:
                //Choose to continue panicing
                if (Vector3.Distance(transform.position, _player.transform.position) < PlayerDectectionRadius)
                    break;
                else if (Vector3.Distance(transform.position, _agent.destination) < 2f)
                    StartCoroutine(UpdateState(CritterState.idle, Random.Range(1f, 5f)));
                break;
            case CritterState.capturing:
                //TODO
                break;
            case CritterState.stunned:
                //Do nothing
                break;
        }
    }
    IEnumerator UpdateState(CritterState newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (newState != State)
            State = newState;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > VelocityToPanic)
            {
                StartCoroutine(UpdateState(CritterState.stunned, 0));
                StartCoroutine(UpdateState(CritterState.panicing, collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude / 10));
            }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0,0,255, 0.50f);
        Gizmos.DrawSphere(transform.position, POIDetectionRadius);
        Gizmos.color = new Color(255, 0, 0, 0.50f);
        Gizmos.DrawSphere(transform.position, PlayerDectectionRadius);
        Gizmos.color = new Color(0, 255, 0, 0.50f);
        Gizmos.DrawSphere(_homePoint, TravelableDistanceFromHome);
    }
}
