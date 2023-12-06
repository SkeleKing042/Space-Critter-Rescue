//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour
{
    [Header("States")]
    [SerializeField, Tooltip("The current state of the creature.")]
    private string _theState;
    private State _currentState;
    public State ReadState { get { return _currentState; } }
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
    private float _startingWaterSearchRadius;
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
    private float _panicSpeed;
    public float PanicSpeed { get { return _panicSpeed; } }
    private FieldOfView _fovRef;
    public FieldOfView FieldOfView { get { return _fovRef; } }

    //State Management
    [System.Serializable]
    private class StateWDelay
    {
        protected State state;
        public State ReadState { get { return state; } }
        protected float delay;
        private float timer;
        
        public StateWDelay(State s, float d)
        {
            state = s;
            delay = d;
        }

        public bool increment()
        {
            if (timer >= delay)
                return true;
            else
                timer += Time.deltaTime;

            return false;
        }
    }
    private List<StateWDelay> _stateBuffer = new List<StateWDelay>();

    #region Setup
    private void Start()
    {
    }
    private void Awake()
    {
        CreatureStats stats = GetComponent<CreatureStats>();
        //Agent setup
        _agent = GetComponent<NavMeshAgent>();
        if (!_agent.isOnNavMesh)
        {
            Debug.LogWarning(gameObject.name + " is not on a navMesh. plz fix boss\nCLICK FOR MORE INFO" +
                "\nPosition: " + transform.position.ToString() +
                "\nRotation: " + transform.rotation.ToString() + 
                "\nScale: " + transform.localScale.ToString() + 
                "\nIsBig: " + stats.IsBig.ToString()+
                "\nType: " + stats.Type.ToString());
            Destroy(gameObject);
            return;
        }
        _critterHeight = _agent.height;
        _baseSpeed = _agent.speed;

        //Component grabs
        stats.GetStats();
        _player = GameObject.FindGameObjectWithTag(_playerTag);
        _animator = GetComponentInChildren<Animator>();
        _drinkingSources = GetComponentInChildren<DrinkenFinden>();
        _startingWaterSearchRadius = _drinkingSources.GetComponent<SphereCollider>().radius;
        _rb = GetComponent<Rigidbody>();
        _fovRef = GetComponentInChildren<FieldOfView>();

        //Sets the center of the area that the AI will move around
        _homePoint = transform.position;

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
    public void InitStats(float thirst, float lazy, float dis, float vel, float pSpe)
    {
        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();

        _thirstiness = thirst;
        _lazyness = lazy;
        _travelableDistanceFromHome = dis;
        _velocityToPanic = vel;
        _panicSpeed = pSpe;
    }
    #endregion
    #region BrainFunctions
    void Update()
    {
        UpdateState();
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

        if(_naving)
        {
            //transform.rotation = Quaternion.Lerp(transform.rotation, , 0.1f);
        }

        //Debuging
        _theState = _currentState.GetType().ToString();
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
                        if(_agent.isActiveAndEnabled)
                        _agent.SetDestination(_POITarget.position);
                        GoingForDrink = true;
                        PrepareUpdateState(new RoamingState(this));
                        _drinkingSources.GetComponent<SphereCollider>().radius = _startingWaterSearchRadius;
                        return true;
                    }
                }
                else
                {
                    _drinkingSources.GetComponent<SphereCollider>().radius += 10;
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
                if(_agent.isActiveAndEnabled)
                    _agent.SetDestination(transform.position + otherBody.velocity.normalized * (otherBody.velocity.magnitude / 3.0f));
                PrepareUpdateState(new PanicState(this), collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude / 10);
            }
        }
    }
    private IEnumerator GrabGroundBelow()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up * 1000f, out hit, 1000f))
            {
                //Debug.Log("Hit object \"" + hit.collider.gameObject.name + "\" tagged as \"" + hit.collider.gameObject.tag);
                if (hit.collider.tag == "Ground")
                {
                    _lastGroundPoint = hit.point + new Vector3(0, _critterHeight, 0);
                }
            }
            float randTime = 2654435769 * Time.deltaTime * UnityEngine.Random.Range(0.1f, 1.0f) / Mathf.Pow(10, 8);
            while (randTime > 10)
                randTime /= 10;
            float waitTime = _groundCheckDelay + randTime;
            //Debug.Log("Done AI ground check, waiting for " + waitTime);
            yield return new WaitForSeconds(waitTime);
            //Debug.Log("Doing AI ground check");
        }
    }
    public void ReturnToLastGrounedPoint()
    {
        if (_naving)
        {
            if(_agent.isActiveAndEnabled)
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
    #endregion
    #region StateStuff
    /// <summary>
    /// State chaging function. Will update this creature's state.
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public void PrepareUpdateState(State newState)
    {
        PrepareUpdateState(newState, 0f);
    }
    /// <summary>
    /// State chaging function. Will update this creature's state after a given time.
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public void PrepareUpdateState(State newState, float delay)
    {
        if (newState.GetType() == typeof(PanicState))
            Debug.Log("Reading a panic state");
        _stateBuffer.Add(new StateWDelay(newState, delay));
        //Debug.Log("Added " + newState.ToString() + " to the state buffer with a " + delay + " second delay.\nBuffer now has " + _stateBuffer.Count + " enties."); 
    }
    public void UpdateState()
    {
        List<StateWDelay> rDStates = new List<StateWDelay>();
        foreach (StateWDelay state in _stateBuffer)
        {
            if (state.increment())
            {
                if (state.ReadState.GetType() != _currentState.GetType())
                {
                    //Debug.Log("Appling " + state.ReadState.ToString() + " to this ai");
                    _currentState.BASE_EndState();
                    _currentState = state.ReadState;
                    _currentState.BASE_StartState();
                }
                rDStates.Add(state);
            }
        }
        foreach (StateWDelay state in rDStates)
            if (_stateBuffer.Contains(state))
                _stateBuffer.Remove(state);
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
            _naving = false;
        }
        else if (!_naving)
        {
            _rb.isKinematic = true;
            _agent.enabled = true;
            _agent.isStopped = false;
            _naving = true;
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
    public void RunFromPlayer(float panicDelay)
    {
        if (_currentState.GetType() != typeof(CaptureState) || _currentState.GetType() != typeof(TrappedState))
        {
            if (panicDelay > 0f)
                PrepareUpdateState(new AlertState(this));
            PrepareUpdateState(new PanicState(this), panicDelay);
        }
    }
    public void StunThenRun(float stunTime)
    {
            if (stunTime > 0f)
                PrepareUpdateState(new StunnedState(this));
            PrepareUpdateState(new PanicState(this), stunTime);
    }
    #endregion
    public void DEBUG_CauseBrainRot()
    {
        Debug.Log("Causing brain rot");
        PrepareUpdateState(new StunnedState(this), 1.0f);
    }
    public void DEBUG_InstilFear()
    {
        Debug.Log("Instilling Fear");
        PrepareUpdateState(new PanicState(this));
    }
}