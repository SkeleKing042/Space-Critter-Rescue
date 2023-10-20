//Created by Jackson Lucas
//Last Edited by Jackson Lucas
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
    drinking,
    sleeping,
    panicking,
    capturing,
    stunned
    };

    public CritterState State;

    [Header("Check radi")]
    [Tooltip("The radius at which POIs are detected.")]
    [SerializeField]
    private float _POIDetectionRadius;
    private Transform _POITarget;
    [Tooltip("The radius at which the player is detected.")]
    [SerializeField]
    private float _playerDectectionRadius;
    private Vector3 _homePoint;
    [Tooltip("The maximum distance that a creature will travel from their initial starting position.")]
    [SerializeField]
    private float _travelableDistanceFromHome;

    [Tooltip("The velocity that a colliding object has to have to send the creature into the panicking state.")]
    [SerializeField]
    private float _velocityToPanic;

    [Header("Threasholds")]
    [Range(0f, 100f)]
    [Tooltip("Point at which this creature will look for water.")]
    [SerializeField]
    private float _thirstiness;
    [Range(0f, 100f)]
    [Tooltip("How quickly this creature will rest.")]
    [SerializeField]
    private float _lazyness;

    [Header("Resources")]
    [Range(0f, 100f)]
    [SerializeField]
    private float _hydration = 100;
    [Range(0f, 100f)]
    [SerializeField]
    private float _energy = 100;
    [Header("Tags")]
    [SerializeField]
    private string _playerTag;
    private GameObject _player;
    [SerializeField]
    private string _drinkableTag;

    private NavMeshAgent _agent;
    private float _baseSpeed;
    [Header("Debug")]
    [Tooltip("State display")]
    public TextMeshProUGUI _textBox;

    private void Start()
    {
        _homePoint = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _baseSpeed = _agent.speed;
        _player = GameObject.FindGameObjectWithTag(_playerTag);
    }
    // Update is called once per frame
    void Update()
    {
        _textBox.text = State.ToString();
        switch (State)
        {
            case CritterState.idle:
                if (CheckForPlayer(1)) break;
                if(CheckForPOIs(1)) break;
                //If we have enough energy, move
                if (_energy > _lazyness)
                {
                    //Choose a random spot
                    float n = _travelableDistanceFromHome;
                    Vector3 newPos = new Vector3(_homePoint.x + Random.Range(-n, n), _homePoint.y + Random.Range(-n, n), _homePoint.z + Random.Range(-n, n));
                    //Move there
                    _agent.SetDestination(newPos);
                    //Start roaming
                    StartCoroutine(UpdateState(CritterState.roaming, 0));
                    break;
                }
                else
                {
                    //Otherwise, sleep
                    StartCoroutine(UpdateState(CritterState.sleeping, 0));
                    break;
                }
            case CritterState.roaming:
                //Lose energy over time
                Mathf.Clamp(_energy -= Time.deltaTime * 2, 0, 100);
                if (CheckForPlayer(1)) break;
                if (CheckForPOIs(1)) break;
                //Are we at the point?
                if (Vector3.Distance(transform.position, _agent.destination) <= 1f)
                {
                    //If so, stop
                    StartCoroutine(UpdateState(CritterState.idle, 0));
                    break;
                }
                //Otherwise, keep moving
                break;
            case CritterState.movingToPOI:
                if(CheckForPlayer(1)) break;
                //If the POI is a drinkable source, check if we are near the source...
                if (_POITarget.GetComponent<DrinkableSource>())
                    if(Vector3.Distance(transform.position, _POITarget.position) < _POITarget.GetComponent<DrinkableSource>().GetRadius())
                    {
                        //... if so, drink
                        StartCoroutine(UpdateState(CritterState.drinking, 0));
                        break;
                    }
                break;
            case CritterState.drinking:
                if (CheckForPlayer(0.5f)) break;
                //Stop moving and replenish hydration
                _agent.isStopped = true;
                if (_hydration >= 100)
                {
                    StartCoroutine(UpdateState(CritterState.idle, 0));
                    _agent.isStopped = false;
                }
                else
                    Mathf.Clamp(_hydration += Time.deltaTime * 10, 0, 100);
                break;
            case CritterState.sleeping:
                //Stop moving and replenish energy
                _agent.isStopped = true;
                if (_energy >= 100)
                {
                    StartCoroutine(UpdateState(CritterState.idle, 0));
                    _agent.isStopped = false;
                }
                else
                    Mathf.Clamp(_energy += Time.deltaTime * 10, 0, 100);
                break;
            case CritterState.panicking:
                _agent.isStopped = false;
                //Move twice as fast and lose energy twice as fast
                _agent.speed = _baseSpeed * 2f;
                Mathf.Clamp(_energy -= Time.deltaTime * 4, 0, 100);
                if (Vector3.Distance(transform.position, _player.transform.position) < _playerDectectionRadius)
                {
                    Vector3 dir = (_player.transform.position - transform.position).normalized;
                    _agent.SetDestination(_player.transform.position + dir * _playerDectectionRadius * -1.5f);
                    break;
                }
                if (Vector3.Distance(transform.position, _agent.destination) < 2f)
                {
                    //Reset speed and go idle
                    _agent.speed = _baseSpeed;
                    StartCoroutine(UpdateState(CritterState.idle, 0));
                }
                break;
            case CritterState.capturing:
                //TODO
                break;
            case CritterState.stunned:
                _agent.isStopped = true;
                //Anims can go here
                break;
        }
        //Always reduce hydration
        if(State != CritterState.sleeping || State != CritterState.drinking)
            _hydration -= Time.deltaTime;

        Debug.DrawLine(_agent.destination, transform.position, Color.blue);
    }
    /// <summary>
    /// State chaging function. Will update this creature's state after a given time.
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator UpdateState(CritterState newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (newState != State)
            State = newState;
    }
    /// <summary>
    /// Panic if player is near. Mod affects radius.
    /// </summary>
    /// <param name="mod"></param>
    /// <returns></returns>
    private bool CheckForPlayer(float mod)
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < _playerDectectionRadius * mod)
        {
            StartCoroutine(UpdateState(CritterState.panicking, 0));
            return true;
        }
        return false;
    }
    /// <summary>
    /// Looks for nearby POIs. Mod affect radius.
    /// </summary>
    /// <param name="mod"></param>
    /// <returns></returns>
    private bool CheckForPOIs(float mod)
    {
        //If dehydrated
        if (_hydration < _thirstiness)
        {
            DrinkableSource currentSource = null;
            //Find all drinkable sources
            foreach (DrinkableSource drinkable in FindObjectsOfType<DrinkableSource>())
            {
                //Check the distance to them and if they're close enough, go to that source
                if (Vector3.Distance(transform.position, drinkable.transform.position) < _POIDetectionRadius * mod)
                    if (currentSource == null)
                        currentSource = drinkable;
                    else
                        if (Vector3.Distance(transform.position, currentSource.transform.position) > Vector3.Distance(transform.position, drinkable.transform.position))
                        currentSource = drinkable;
            }
            if (currentSource != null)
            {
                //Go to the water source
                _POITarget = currentSource.transform;
                _agent.SetDestination(_POITarget.position);
                StartCoroutine(UpdateState(CritterState.movingToPOI, 0));
                return true;
            }
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Upon hitting another rigidbody moving fast enough, get stunned then, panic
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody otherBody = collision.gameObject.GetComponent<Rigidbody>();
            if (otherBody.velocity.magnitude > _velocityToPanic)
            {
                StartCoroutine(UpdateState(CritterState.stunned, 0));
                //Run from the object
                _agent.SetDestination(transform.position + otherBody.velocity.normalized * (otherBody.velocity.magnitude / 3.0f));
                StartCoroutine(UpdateState(CritterState.panicking, collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude / 10));
            }
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0,0,255, 0.50f);
        Gizmos.DrawSphere(transform.position, _POIDetectionRadius);
        Gizmos.color = new Color(255, 0, 0, 0.50f);
        Gizmos.DrawSphere(transform.position, _playerDectectionRadius);
        Gizmos.color = new Color(0, 255, 0, 0.50f);
        Gizmos.DrawSphere(_homePoint, _travelableDistanceFromHome);
    }
}


//State _currentState

//Update()
//_currentState.Update()
//
//ChangeState(State newState)
// { State.End(); _currentState = newState; _currentState.Begin(); }


//class State
//Agent agent
//
//

//class WalkState : State
//
//Constructor (Agent agent)
//
// Update()
//{
//    if (something)
//    {
//        agent.ChangeState(new IdleState(agent));
//    }
//
//
//}
//