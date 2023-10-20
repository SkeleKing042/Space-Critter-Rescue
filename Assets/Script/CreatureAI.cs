//Created by Jackson Lucas
//Last Edited by Jackson Lucas
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static CreatureAI;
using static UnityEngine.EventSystems.EventTrigger;

public class CreatureAI : MonoBehaviour
{
    private State _currentState;

    [Header("Check radi")]
    [Tooltip("The radius at which POIs are detected.")]
    [SerializeField]
    private float _POIDetectionRadius;
    private Transform _POITarget;
    public Transform POITarget { get { return _POITarget; } }
    [Tooltip("The radius at which the player is detected.")]
    [SerializeField]
    private float _playerDectectionRadius;
    public float PlayerDectectionRadius { get { return _playerDectectionRadius; } }
    private Vector3 _homePoint;
    public Vector3 HomePoint { get { return _homePoint; } }
    [Tooltip("The maximum distance that a creature will travel from their initial starting position.")]
    [SerializeField]
    private float _travelableDistanceFromHome;
    public float TravelDistance { get { return _travelableDistanceFromHome; } }

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
    public float Lazyness { get { return _lazyness; }  }

    [Header("Resources")]
    [Range(0f, 100f)]
    [SerializeField]
    public float Hydration = 100;
    [Range(0f, 100f)]
    public float Energy = 100;

    [Header("Tags")]
    [SerializeField]
    private string _playerTag;
    private GameObject _player;
    public GameObject Player { get { return _player; } }
    [SerializeField]
    private string _drinkableTag;

    private NavMeshAgent _agent;
    public NavMeshAgent GetAgent { get { return _agent; } }
    private float _baseSpeed;
    public float BaseSpeed { get { return _baseSpeed; } }
    [Header("Debug")]
    [Tooltip("State display")]
    public TextMeshProUGUI _textBox;

    private void Start()
    {
        _homePoint = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _baseSpeed = _agent.speed;
        _player = GameObject.FindGameObjectWithTag(_playerTag);
        _currentState = new IdleState(this);
        _currentState.StartState();
    }
    // Update is called once per frame
    void Update()
    {
        //_textBox.text = State.ToString();
        if (_currentState != null)
        {
            _currentState.Update();
            //Always reduce hydration

            if (_currentState.GetType() != typeof(SleepState) || _currentState.GetType() != typeof(DrinkingState))
                Hydration -= Time.deltaTime;
        }

        Debug.DrawLine(_agent.destination, transform.position, Color.blue);
    }
    /// <summary>
    /// State chaging function. Will update this creature's state after a given time.
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public IEnumerator UpdateState(State newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (newState.GetType() != _currentState.GetType())
        {
            _currentState.EndState();
            _currentState = newState;
            _currentState.StartState();
        }
        Debug.Log("The state is " + _currentState.GetType());
    }
    /// <summary>
    /// Panic if player is near. Mod affects radius.
    /// </summary>
    /// <param name="mod"></param>
    /// <returns></returns>
    public bool CheckForPlayer(float mod)
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < _playerDectectionRadius * mod)
        {
            StartCoroutine(UpdateState(new PanicState(this), 0));
            return true;
        }
        return false;
    }
    /// <summary>
    /// Looks for nearby POIs. Mod affect radius.
    /// </summary>
    /// <param name="mod"></param>
    /// <returns></returns>
    public bool CheckForPOIs(float mod)
    {
        //If dehydrated
        if (Hydration < _thirstiness)
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
                StartCoroutine(UpdateState(new MovingToPOIState(this), 0));
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
                StartCoroutine(UpdateState(new StunnedState(this), 0));
                //Run from the object
                _agent.SetDestination(transform.position + otherBody.velocity.normalized * (otherBody.velocity.magnitude / 3.0f));
                StartCoroutine(UpdateState(new PanicState(this), collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude / 10));
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


public abstract class State
{
    private CreatureAI _ai;
    private NavMeshAgent _agent;
    public State(CreatureAI ai)
    {
        _ai = ai;
        _agent = _ai.GetAgent;
    }
    public State(CreatureAI ai, NavMeshAgent agent)
    {
        _ai = ai;
        _agent = agent;
    }
    public CreatureAI AI { get { return _ai; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public abstract void StartState();
    public abstract void Update();
    public abstract void EndState();
}

public class IdleState : State
{
    public IdleState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {

    }
    public override void Update()
    {
        if (AI.CheckForPlayer(1)) return;
        if (AI.CheckForPOIs(1)) return;
        //If we have enough energy, move
        if (AI.Energy > AI.Lazyness)
        {
            //Choose a random spot
            float n = AI.TravelDistance;
            Vector3 newPos = new Vector3(AI.HomePoint.x + UnityEngine.Random.Range(-n, n), AI.HomePoint.y + UnityEngine.Random.Range(-n, n), AI.HomePoint.z + UnityEngine.Random.Range(-n, n));
            //Move there
            Agent.SetDestination(newPos);
            //Start roaming
            AI.StartCoroutine(AI.UpdateState(new RoamingState(AI), 0));
            return;
        }
        else
        {
            //Otherwise, sleep
            AI.StartCoroutine(AI.UpdateState(new SleepState(AI), 0));
            return;
        }
    }
    public override void EndState()
    {
        
    }
}
public class RoamingState : State
{
    public RoamingState(CreatureAI AI) : base(AI)
    {

    }
    public override void StartState()
    {
        Agent.isStopped = false;
    }
    public override void Update()
    {
        //Lose energy over time
        Mathf.Clamp(AI.Energy -= Time.deltaTime * 2, 0, 100);
        if (AI.CheckForPlayer(1)) return;
        if (AI.CheckForPOIs(1)) return;
        //Are we at the point?
        if (Vector3.Distance(AI.transform.position, Agent.destination) <= 1f)
        {
            //If so, stop
            AI.StartCoroutine(AI.UpdateState(new IdleState(AI), 0));
            return;
        }
        //Otherwise, keep moving
    }
    public override void EndState()
    {

    }
}
public class SleepState : State
{
    public SleepState(CreatureAI AI) : base(AI)
    {

    }
    public override void StartState()
    {
        Agent.isStopped = true;
    }
    public override void Update()
    {
        //Stop moving and replenish energy
        if (AI.Energy >= 100)
            AI.StartCoroutine(AI.UpdateState(new IdleState(AI), 0));
        else
            Mathf.Clamp(AI.Energy += Time.deltaTime * 10, 0, 100);
    }
    public override void EndState()
    {
        Agent.isStopped = false;
    }
}
public class MovingToPOIState : State
{
    public MovingToPOIState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {
        Agent.isStopped = false;
    }
    public override void Update()
    {
        if (AI.CheckForPlayer(1)) return;
        //If the POI is a drinkable source, check if we are near the source...
        if (AI.POITarget.GetComponent<DrinkableSource>())
            if (Vector3.Distance(AI.transform.position, AI.POITarget.position) < AI.POITarget.GetComponent<DrinkableSource>().GetRadius())
            {
                //... if so, drink
                AI.StartCoroutine(AI.UpdateState(new DrinkingState(AI), 0));
                return;
            }
    }
    public override void EndState()
    {

    }

}
public class DrinkingState : State
{
    public DrinkingState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {
        //Stop moving
        Agent.isStopped = true;
    }
    public override void Update()
    {
        //Replenish hydration
        if (AI.CheckForPlayer(0.5f)) return;
        if (AI.Hydration >= 100)
        {
            AI.StartCoroutine(AI.UpdateState(new IdleState(AI), 0));
        }
        else
            Mathf.Clamp(AI.Hydration += Time.deltaTime * 10, 0, 100);
    }
    public override void EndState()
    {
        Agent.isStopped = false;
    }
}
public class PanicState : State
{
    public PanicState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {
        Agent.isStopped = false;
    }
    public override void Update()
    {
        //Move twice as fast and lose energy twice as fast
        Agent.speed = AI.BaseSpeed * 2f;
        Mathf.Clamp(AI.Energy -= Time.deltaTime * 4, 0, 100);
        if (Vector3.Distance(AI.transform.position, AI.Player.transform.position) < AI.PlayerDectectionRadius)
        {
            Vector3 dir = (AI.Player.transform.position - AI.transform.position).normalized;
            Agent.SetDestination(AI.Player.transform.position + dir * AI.PlayerDectectionRadius * -1.5f);
            return;
        }
        if (Vector3.Distance(AI.transform.position, Agent.destination) < 2f)
        {
            //Reset speed and go idle
            Agent.speed = AI.BaseSpeed;
            AI.StartCoroutine(AI.UpdateState(new IdleState(AI), 0));
            return;
        }

    }
    public override void EndState()
    {

    }

}
public class CaptureState : State
{
    public CaptureState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {

    }
    public override void Update()
    {

    }
    public override void EndState()
    {

    }

}
public class StunnedState : State
{
    public StunnedState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {
        Agent.isStopped = true;
    }
    public override void Update()
    {

    }
    public override void EndState()
    {
        Agent.isStopped = false;
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