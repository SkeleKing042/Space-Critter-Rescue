//using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour
{
    public enum CritterState { 
    idle,
    roaming,
    movingToPOI,
    checking,
    drinking,
    sleeping,
    //startPanicing,
    panicing,
    capturing,
    stunned
    };

    public CritterState State;

    public float VelocityToPanic;
    public float POIDetectionRadius;
    private Transform _POITarget;
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
    [Header("Resources")]
    [Range(0f, 100f)]
    public float Hydration = 100;
    [Range(0f, 100f)]
    public float Energy = 100;
    [Header("Tags")]
    public string PlayerTag;
    private GameObject _player;
    public string DrinkableTag;

    public TextMeshProUGUI textBox;
    private void Start()
    {
        _homePoint = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag(PlayerTag);
    }
    // Update is called once per frame
    void Update()
    {
        textBox.text = State.ToString();
        switch (State)
        {
            case CritterState.idle:
                //Check for player
                if (Vector3.Distance(transform.position, _player.transform.position) < PlayerDectectionRadius)
                {
                    StartCoroutine(UpdateState(CritterState.panicing, 0));
                    break;
                }
                //Check for POIs
                if (Hydration < Thirstiness)
                    foreach (GameObject drinkable in GameObject.FindGameObjectsWithTag(DrinkableTag))
                        if (Vector3.Distance(transform.position, drinkable.transform.position) < POIDetectionRadius)
                        {
                            //Go to the edge of the water source
                            _POITarget = drinkable.transform;
                            _agent.SetDestination(_POITarget.position);
                            StartCoroutine(UpdateState(CritterState.movingToPOI, 0));
                            break;
                        }
                //Choose to move
                if (Energy > Lazyness)
                {
                    //If moving, where?
                    float n = TravelableDistanceFromHome / 2;
                    Vector3 newPos = new Vector3(_homePoint.x + Random.Range(-n, n), _homePoint.y + Random.Range(-n, n), _homePoint.z + Random.Range(-n, n));
                    Debug.DrawRay(newPos, Vector3.up * 0.1f, Color.blue, 1f);
                    //Move there
                    _agent.SetDestination(newPos);
                    //Start roaming
                    StartCoroutine(UpdateState(CritterState.roaming, 0));
                    break;
                }
                else
                {
                    StartCoroutine(UpdateState(CritterState.sleeping, 0));
                    break;
                }

            case CritterState.roaming:
                Mathf.Clamp(Energy -= Time.deltaTime * 2, 0, 100);
                //Check for player
                if (Vector3.Distance(transform.position, _player.transform.position) < PlayerDectectionRadius)
                {
                    StartCoroutine(UpdateState(CritterState.panicing, 0));
                    break;
                }
                //Check for POIs
                if (Hydration < Thirstiness)
                    foreach (GameObject drinkable in GameObject.FindGameObjectsWithTag(DrinkableTag))
                        if (Vector3.Distance(transform.position, drinkable.transform.position) < POIDetectionRadius)
                        {
                            //Go to the edge of the water source
                            _POITarget = drinkable.transform;
                            _agent.SetDestination(_POITarget.position);
                            StartCoroutine(UpdateState(CritterState.movingToPOI, 0));
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
            case CritterState.movingToPOI:
                if(Vector3.Distance(transform.position, _POITarget.position) < 1f)
                    if (_POITarget.gameObject.tag == DrinkableTag)
                    {
                        StartCoroutine(UpdateState(CritterState.drinking, 0));
                        break;
                    }
                break;
            case CritterState.checking:

                break;
            case CritterState.drinking:
                //Choose to continue drinking
                if (Hydration >= 100)
                {
                    StartCoroutine(UpdateState(CritterState.idle, 0));
                }
                else
                    Mathf.Clamp(Hydration += Time.deltaTime * 10, 0, 100);
                break;
            case CritterState.sleeping:
                //Choose to continue sleeping
                if (Energy >= 100)
                    StartCoroutine(UpdateState(CritterState.idle, 0));
                else
                    Mathf.Clamp(Energy += Time.deltaTime * 10, 0, 100);
                break;
                /*
            case CritterState.startPanicing:
                Vector3 dir = (_player.transform.position - transform.position).normalized;
                _agent.SetDestination(_player.transform.position + dir * 1.5f);
                StartCoroutine(UpdateState(CritterState.panicing, 0));
                break;
                 */
            case CritterState.panicing:
                //Choose to continue panicing
                Mathf.Clamp(Energy -= Time.deltaTime * 4, 0, 100);
                if (Vector3.Distance(transform.position, _player.transform.position) < PlayerDectectionRadius)
                {
                    Vector3 dir = (_player.transform.position - transform.position).normalized;
                    _agent.SetDestination(_player.transform.position + dir * PlayerDectectionRadius * -1.5f);
                    break;
                }
                else if (Vector3.Distance(transform.position, _agent.destination) < 2f)
                    StartCoroutine(UpdateState(CritterState.idle, 0));
                break;
            case CritterState.capturing:
                //TODO
                break;
            case CritterState.stunned:
                //Do nothing
                break;
        }
        if(State != CritterState.sleeping || State != CritterState.drinking)
            Hydration -= Time.deltaTime;

        Debug.DrawRay(_agent.destination, Vector3.up * 10, Color.blue);
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
