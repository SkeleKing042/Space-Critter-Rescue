//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour
{
    [Header("States")]
    [SerializeField, Tooltip("The current state of the creature.")]
    private string _theState;
    public State _currentState;
    private Rigidbody _rb;
    public Rigidbody Rb { get { return _rb; } }
    private Animator _animator;
    public Animator Animator { get { return _animator; } }

    private float _critterHeight;

    [Header("Threasholds")]
    [SerializeField, Tooltip("The amount time that should pass between thrist checks.")]
    private float _thirstCheckDelay = 1;
    private float _timeSinceLastThirstCheck;
    private float _thirstiness;

    [SerializeField, Tooltip("The amount time that should pass between lazy checks.")]
    private float _lazyCheckDelay = 1;
    private float _timeSinceLastLazyCheck;
    private float _lazyness;

    [Header("Resources")]
    [Range(0f, 100f)]
    public float Hydration = 100;
    [Range(0f, 100f)]
    public float Energy = 100;

    [Header("POIs and Drinking sources")]
    private Transform _POITarget;
    public Transform POITarget { get { return _POITarget; } }
    private bool _goingForDrink;
    public bool GoingForDrink { get { return _goingForDrink; } set { _goingForDrink = value; } }
    private DrinkenFinden _drinkingSources;

    [Header("Navigation")]
    [SerializeField, Tooltip("The offset to respawn this creature from after falling in to a death plane.")]
    private Vector3 _respawnOffset;
    [SerializeField, Tooltip("How often the ground check should be run.")]
    private float _groundCheckDelay = 1;
    private Vector3 _lastGroundPoint;

    private float _travelableDistanceFromHome;
    public float TravelDistance { get { return _travelableDistanceFromHome; } }
    private Vector3 _homePoint;
    public Vector3 HomePoint { get { return _homePoint; } }
    private NavMeshAgent _agent;
    public NavMeshAgent GetAgent { get { return _agent; } }
    private bool _naving = true;
    private float _baseSpeed;
    public float BaseSpeed { get { return _baseSpeed; } }

    [Header("Player interaction")]
    [SerializeField, Tooltip("The tag that the player uses.")]
    private string _playerTag;
    private GameObject _player;
    public GameObject Player { get { return _player; } }
    private float _velocityToPanic;
    [SerializeField, Tooltip("How quickly the creature should turn to face the player during the capture state.")]
    private float _facePlayerRate;
    public float FacePlayerRate { get { return _facePlayerRate; } }

    private void Start()
    {
        //Sets the center of the area that the AI will move around
        _homePoint = transform.position;
        //Component grabs
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag(_playerTag);
        _animator = GetComponent<Animator>();
        _drinkingSources = GetComponentInChildren<DrinkenFinden>();
        _rb = GetComponent<Rigidbody>();

        //Saftey checks
        _thirstCheckDelay = Mathf.Abs(_thirstCheckDelay);
        _lazyCheckDelay = Mathf.Abs(_lazyCheckDelay);
        _timeSinceLastThirstCheck = _thirstCheckDelay;
        _timeSinceLastLazyCheck = _lazyCheckDelay;

        //Initial state set
        _currentState = new IdleState(this);
        _currentState.StartState();

        StartCoroutine(GrabGroundBelow());
    }
    public void InitStats(float height, float thirst, float lazy, float dis, float vel, float spd)
    {
        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();

        _agent.height = _critterHeight = height;
        _thirstiness = thirst;
        _lazyness = lazy;
        _travelableDistanceFromHome = dis;
        _velocityToPanic = vel;
        _agent.speed = _baseSpeed = spd;
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
    }
    /// <summary>
    /// State chaging function. Will update this creature's state.
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public void PrepareUpdateState(State newState)
    {
        StartCoroutine(UpdateState(newState, 0f));
    }
    /// <summary>
    /// State chaging function. Will update this creature's state after a given time.
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
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
    /// Checks for any nearby POIs (Drinkable sources)
    /// </summary>
    /// <param name="mod"></param>
    /// <returns></returns>
    public bool CheckForPOIs(float mod)
    {
        //Generate a random value between the thirstiness level and half of that
        //If that random value is greater then the Hydration level, go drink
        //Eg: thirstiness = 50 // rnd returns 33 // Hydration = 40 // Won't drink
        //rnd return amny number // Hydroation below 0.5 of thirstiness // will drink
        if (_timeSinceLastThirstCheck >= _thirstCheckDelay)
        {
            _timeSinceLastThirstCheck = 0;
            float rnd = UnityEngine.Random.Range(_thirstiness * 0.5f, _thirstiness);
            if (rnd > Hydration)
            {
                if (_drinkingSources.Sources.Count > 0)
                {
                    DrinkableSource currentSource = _drinkingSources.Sources[0].GetComponent<DrinkableSource>();
                    //Find all drinkable sources
                    foreach (GameObject drinkable in _drinkingSources.Sources)
                    {
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
        }
        return false;
    }
    /// <summary>
    /// Checks if this creature should fall asleep
    /// </summary>
    /// <returns></returns>
    public bool CheckForSleep()
    {
        if (_timeSinceLastLazyCheck >= _lazyCheckDelay)
        {
            _timeSinceLastLazyCheck = 0;
            //Same as the POI check 
            float rnd = UnityEngine.Random.Range(_lazyness * 0.5f, _lazyness);
            if (rnd > Energy)
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
        if (_naving)
        {
            _agent.isStopped = true;
            _agent.enabled = false;
            _rb.isKinematic = false;
        }
        else if (!_naving)
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
    private IEnumerator GrabGroundBelow()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up * 1000f, out hit, 1000f))
            {
                Debug.Log("Hit object \"" + hit.collider.gameObject.name + "\" tagged as \"" + hit.collider.gameObject.tag);
                if (hit.collider.tag == "Ground")
                {
                    _lastGroundPoint = hit.point + new Vector3(0, _critterHeight, 0);
                }
            }
            yield return new WaitForSeconds(_groundCheckDelay);
        }
    }
    public void ReturnToLastGrounedPoint()
    {
        if (_naving)
        {
            _agent.Warp(_lastGroundPoint + _respawnOffset);
            PrepareUpdateState(new StunnedState(this));
            PrepareUpdateState(new IdleState(this), 2f);
        }
        else if (!_naving)
        {
            _rb.velocity = Vector3.zero;
            transform.position = _lastGroundPoint + _respawnOffset;
            PrepareUpdateState(new IdleState(this), 2f);
        }
    }
    public void DEBUG_CauseBrainRot()
    {
        Debug.Log("Causing brain rot");
        PrepareUpdateState(new StunnedState(this), 1.0f);
    }
}