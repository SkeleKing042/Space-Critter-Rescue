//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour
{
    private State _currentState;
    [SerializeField]
    private string _theState;

    [Header("Creature Info")]
    public bool isBig;
    public enum creatureType
    {
        Shroom,
        Crystal
    };
    public creatureType type;
    //[SerializeField, Tooltip("The radius at which POIs are detected.")]
    //private float _POIDetectionRadius;
    private Transform _POITarget;
    public Transform POITarget { get { return _POITarget; } }
    private bool _goingForDrink;
    private DrinkenFinden _drinkingSources;
    public bool GoingForDrink { get { return _goingForDrink; } set { _goingForDrink = value; } }
    [Header("Check radi")]
    [SerializeField, Tooltip("The radius at which the player is detected.")]
    private float _playerDectectionRadius;
    public float PlayerDectectionRadius { get { return _playerDectectionRadius; } }
    private Vector3 _homePoint;
    public Vector3 HomePoint { get { return _homePoint; } }
    [SerializeField, Tooltip("The maximum distance that a creature will travel from their initial starting position.")]
    private float _travelableDistanceFromHome;
    public float TravelDistance { get { return _travelableDistanceFromHome; } }

    [SerializeField, Tooltip("The velocity that a colliding object has to have to send the creature into the panicking state.")]
    private float _velocityToPanic;

    [Header("Threasholds")]
    [SerializeField, Range(0f, 100f), Tooltip("Point at which this creature will look for water.")]
    private float _thirstiness;
    [SerializeField, Tooltip("The amount time that should pass between thrist checks.")]
    private float _thirstCheckDelay = 1;
    private float _timeSinceLastThirstCheck;
    
    [Range(0f, 100f)]
    [SerializeField, Tooltip("How quickly this creature will rest.")]
    private float _lazyness;
    [SerializeField, Tooltip("The amount time that should pass between lazy checks.")]
    private float _lazyCheckDelay = 1;
    private float _timeSinceLastLazyCheck;

    [Header("Resources")]
    [Range(0f, 100f)]
    public float Hydration = 100;
    [Range(0f, 100f)]
    public float Energy = 100;

    [Header("Tags")]
    [SerializeField]
    private string _playerTag;
    private GameObject _player;
    public GameObject Player { get { return _player; } }

    private NavMeshAgent _agent;
    public NavMeshAgent GetAgent { get { return _agent; } }
    private bool _naving = true;
    private Rigidbody _rb;
    public Rigidbody Rigidbody { get { return _rb; } }
    private float _baseSpeed;
    public float BaseSpeed { get { return _baseSpeed; } }
    private Animator _animator;
    public Animator Animator { get { return _animator; } }
    public float FacePlayerRate;

    private void Start()
    {
        //Sets the center of the area that the AI will move around
        _homePoint = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _baseSpeed = _agent.speed;
        _player = GameObject.FindGameObjectWithTag(_playerTag);
        _animator = GetComponent<Animator>();

        _thirstCheckDelay = Mathf.Abs(_thirstCheckDelay);
        _lazyCheckDelay = Mathf.Abs(_lazyCheckDelay);
        _timeSinceLastThirstCheck = _thirstCheckDelay;
        _timeSinceLastLazyCheck = _lazyCheckDelay;

        _drinkingSources = GetComponentInChildren<DrinkenFinden>();
        _rb = GetComponent<Rigidbody>();

        _currentState = new IdleState(this);
        _currentState.StartState();
    }
    void Update()
    {
        if (_currentState != null)
        {
            //Update the state
            _currentState.Update();

            //Always reduce hydration, unless sleeping or drinking
            if (_currentState.GetType() != typeof(SleepState) || _currentState.GetType() != typeof(DrinkingState))
                Hydration -= Time.deltaTime;
        }
        //Increment time checks
        _timeSinceLastThirstCheck += Time.deltaTime;
        _timeSinceLastLazyCheck += Time.deltaTime;

        //Debuging
        _theState = _currentState.GetType().ToString();
        Debug.DrawLine(transform.position, _agent.destination, Color.blue);
    }
    /// <summary>
    /// State chaging function. Will update this creature's state after a given time.
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public void PrepareUpdateState(State newState)
    {
        StartCoroutine(UpdateState(newState, 0f));
    }
    public void PrepareUpdateState(State newState, float delay)
    {
        StartCoroutine(UpdateState(newState, delay));
    }
    public IEnumerator UpdateState(State newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (newState.GetType() != _currentState.GetType())
        {
            _currentState.EndState();
            _currentState = newState;
            _currentState.StartState();
        }
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
            PrepareUpdateState(new PanicState(this));
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
        //Generate a random value between the thirstiness level and half of that
        //If that random value is greater then the Hydration level, go drink
        //Eg: thirstiness = 50 // rnd returns 33 // Hydration = 40 // Won't drink
        //rnd return amny number // Hydroation below 0.5 of thirstiness // will drink
        if(_timeSinceLastThirstCheck >= _thirstCheckDelay)
        {
            _timeSinceLastThirstCheck = 0;
            float rnd = UnityEngine.Random.Range(_thirstiness * 0.5f, _thirstiness);
            if(rnd > Hydration)
            {
                DrinkableSource currentSource = null;
                //Find all drinkable sources
                foreach (GameObject drinkable in _drinkingSources.Sources)
                {
                    if (currentSource == null)
                        currentSource = drinkable.GetComponent<DrinkableSource>();
                    else
                        if (Vector3.Distance(transform.position, currentSource.transform.position) > Vector3.Distance(transform.position, drinkable.transform.position))
                        currentSource = drinkable.GetComponent<DrinkableSource>();
                }
                if (currentSource != null)
                {
                    //Go to the water source
                    _POITarget = currentSource.transform;
                    _agent.SetDestination(_POITarget.position);
                    GoingForDrink = true;
                    PrepareUpdateState(new RoamingState(this));
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Checks if this creature should fall asleep
    /// </summary>
    /// <returns></returns>
    public bool CheckForSleep()
    {
        if(_timeSinceLastLazyCheck >= _lazyCheckDelay)
        {
            _timeSinceLastLazyCheck = 0;
            //Same as the POI check 
            float rnd = UnityEngine.Random.Range(_lazyness * 0.5f, _lazyness);
            if(rnd > Energy)
            {
                PrepareUpdateState(new SleepState(this));
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
                PrepareUpdateState(new StunnedState(this));
                //Run from the object
                _agent.SetDestination(transform.position + otherBody.velocity.normalized * (otherBody.velocity.magnitude / 3.0f));
                PrepareUpdateState(new PanicState(this), collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude / 10);
            }
        }
    }
    /// <summary>
    /// Either enables or disables the rigidbody mode of the creature.
    /// </summary>
    public void RigidMode()
    {
        if(_naving)
        {
            _agent.isStopped = true;
            _agent.enabled = false;
            _rb.isKinematic = false;
        }
        else if(!_naving)
        {
            _rb.isKinematic = true;
            _agent.enabled = true;
            _agent.isStopped = false;
        }
    }
    /// <summary>
    /// Enables or disables the rigidbody mode of the creature depending on the incoming bool
    /// </summary>
    public void RigidMode(bool b)
    {
        _naving = b;
        RigidMode();
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 0, 0, 0.50f);
        Gizmos.DrawSphere(transform.position, _playerDectectionRadius);
        Gizmos.color = new Color(0, 255, 0, 0.50f);
        Gizmos.DrawSphere(_homePoint, _travelableDistanceFromHome);
    }
    public void DEBUG_CauseBrainRot()
    {
        Debug.Log("Causing brain rot");
        PrepareUpdateState(new StunnedState(this), 1.0f);
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
        AI.Animator.SetBool("IdleState", true);
    }
    public override void Update()
    {
        //Call check
        if (AI.CheckForPlayer(1)) return;
        if (AI.CheckForPOIs(1)) return;
        if (AI.CheckForSleep()) return;
        else
        {
            //Choose a random spot
            float n = AI.TravelDistance;
            Vector3 newPos = new Vector3(AI.HomePoint.x + UnityEngine.Random.Range(-n, n), AI.HomePoint.y + UnityEngine.Random.Range(-n, n), AI.HomePoint.z + UnityEngine.Random.Range(-n, n));
            //Move there
            Agent.SetDestination(newPos);
            //Start roaming
            AI.PrepareUpdateState(new RoamingState(AI));
            return;
        }
    }
    public override void EndState()
    {
        AI.Animator.SetBool("IdleState", false);
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
        AI.Animator.SetBool("RoamingState", true);
    }
    public override void Update()
    {
        //Lose energy over time
        Mathf.Clamp(AI.Energy -= Time.deltaTime * 2, 0, 100);
        //Call checks
        if (AI.CheckForPlayer(1)) return;
        if (AI.GoingForDrink)
            if (AI.POITarget.GetComponent<DrinkableSource>())
            //Are we at the drinking source
                if (Vector3.Distance(AI.transform.position, AI.POITarget.position) < AI.POITarget.GetComponent<DrinkableSource>().GetRadius())
                {
                    //... if so, drink
                    AI.PrepareUpdateState(new DrinkingState(AI));
                    return;
                }
        else if (AI.CheckForPOIs(1)) return;
        if (AI.CheckForSleep()) return;
        //Are we at the point?
        if (Vector3.Distance(AI.transform.position, Agent.destination) <= 1f)
        {
            //If so, stop
            AI.PrepareUpdateState(new IdleState(AI));
            return;
        }
        //Otherwise, keep moving
    }
    public override void EndState()
    {
        AI.Animator.SetBool("RoamingState", false);
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
        AI.Animator.SetBool("SleepState", true);
    }
    public override void Update()
    {
        //Stop moving and replenish energy
        if (AI.Energy >= 100)
            AI.PrepareUpdateState(new IdleState(AI));
        else
            Mathf.Clamp(AI.Energy += Time.deltaTime * 10, 0, 100);
    }
    public override void EndState()
    {
        Agent.isStopped = false;
        AI.Animator.SetBool("SleepState", false);
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
        AI.Animator.SetBool("DrinkingState", true);
    }
    public override void Update()
    {
        //Replenish hydration
        if (AI.CheckForPlayer(0.5f)) return;
        if (AI.Hydration >= 100)
        {
            AI.PrepareUpdateState(new IdleState(AI));
        }
        else
            Mathf.Clamp(AI.Hydration += Time.deltaTime * 10, 0, 100);
    }
    public override void EndState()
    {
        AI.GoingForDrink = false;
        Agent.isStopped = false;
        AI.Animator.SetBool("DrinkingState", false);
    }
}
public class PanicState : State
{
    public PanicState(CreatureAI ai) : base(ai)
    {
    }
    public override void StartState()
    {
        if(AI.gameObject.activeSelf)
        {
            AI.GetAgent.enabled = true;
            Agent.isStopped = false;
            AI.Animator.SetBool("PanicState", true);
        }
        //Double movement speed
        Agent.speed = AI.BaseSpeed * 2f;
    }
    public override void Update()
    {
        //Lose energy twice as fast
        Mathf.Clamp(AI.Energy -= Time.deltaTime * 4, 0, 100);
        //Find a point away from the player relative to our current position
        if (Vector3.Distance(AI.transform.position, AI.Player.transform.position) < AI.PlayerDectectionRadius)
        {
            Vector3 dir = (AI.Player.transform.position - AI.transform.position).normalized;
            Agent.SetDestination(AI.Player.transform.position + dir * AI.PlayerDectectionRadius * -1.5f);
            return;
        }
        //Once we're far enough away...
        if (Vector3.Distance(AI.transform.position, Agent.destination) < 2f)
        {
            //go idle
            AI.PrepareUpdateState(new IdleState(AI));
            return;
        }

    }
    public override void EndState()
    {
        //Return to normal speed
        Agent.speed = AI.BaseSpeed;
        AI.Animator.SetBool("PanicState", false);
    }

}
public class CaptureState : State
{
    public CaptureState(CreatureAI ai) : base(ai)
    {
        
    }
    public override void StartState()
    {
        AI.Animator.SetBool("CaptureState", true);
        AI.RigidMode(true);
    }
    public override void Update()
    {
        Quaternion targetRot = Quaternion.LookRotation(AI.Player.transform.position - AI.transform.position);
        AI.transform.rotation = Quaternion.Slerp(AI.transform.rotation, targetRot, AI.FacePlayerRate);
    }
    public override void EndState()
    {
        AI.Animator.SetBool("CaptureState", false);
        AI.RigidMode(false);
    }

}
public class StunnedState : State
{
    public StunnedState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {
        AI.Animator.SetBool("StunnedState", true);
        AI.RigidMode(true);
        AI.Rigidbody.useGravity = true;
    }
    public override void Update()
    {

    }
    public override void EndState()
    {
        AI.Animator.SetBool("StunnedState", false);
        AI.RigidMode(false);
    }

}
public class StruggleState : State
{
    public StruggleState(CreatureAI AI) : base(AI)
    {

    }
    public override void StartState()
    {
        AI.Animator.SetBool("StuggleState", true);
        //Double movement speed
        Agent.speed = AI.BaseSpeed * 4f;
    }
    public override void Update()
    {
        //Lose energy twice as fast
        Mathf.Clamp(AI.Energy -= Time.deltaTime * 4, 0, 100);
        //Find a point away from the player relative to our current position
        Vector3 dir = (AI.Player.transform.position - AI.transform.position).normalized;
        Agent.SetDestination(AI.Player.transform.position + dir * AI.PlayerDectectionRadius * -1.5f);

        //Once we're far enough away...
        if (Vector3.Distance(AI.transform.position, Agent.destination) < 2f)
        {
            //Go idle
            AI.PrepareUpdateState(new IdleState(AI));
            return;
        }
    }
    public override void EndState()
    {
        //Return to base speed
        Agent.speed = AI.BaseSpeed;
        AI.Animator.SetBool("StuggleState", false);
    }
}