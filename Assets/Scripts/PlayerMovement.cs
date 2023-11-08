//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody _rb;
    Camera _camera;
    [SerializeField] public Vector2 _movementInput;
    [Header("Ground movement")]
    [SerializeField, Tooltip("The speed at which the player moves forwards and backwards.")]
    private float _moveAccel;
    [SerializeField, Tooltip("The speed at which the player moves side to side.")]
    private float _strafeAccel;
    [SerializeField, Tooltip("The maximum speed the player can reach.")]
    private float _maxSpeed;
    [SerializeField, Tooltip("Modifies how aggresive the breaking is at maximum speed.")]
    private float _breakAggresion = 1;
    private float _movementModifier = 1; //Unused ATM
    [SerializeField, Tooltip("The maximum distance that the last ground check can reach.")]
    private float _lastGroundCheckMaxDistance;
    [SerializeField, Tooltip("The maximum angle that the player can move up smoothly.")]
    private float _maxAngle;

    [Header("Jump movement")]
    [SerializeField, Tooltip("The force that the player jumps with.")]
    private float _jumpForce;
    [Tooltip("The distance from the feet position that the ground check reaches.")]
    public float PlayerHeight;
    private Vector3 _lastGroundPoint;
    public Vector3 RespawnOffset;

    [Header("Jetpack Settings")]
    [SerializeField, Tooltip("The force that the jetpack outputs.")]
    private float _jetForce;
    [SerializeField, Tooltip("The rate at which the jetpack fuel is expended.")]
    private float _burnRate;
    [SerializeField, Tooltip("The time it takes for the jetpack to start after jumping.")]
    private float _burnDelay;
    private float _burnTime;
    [SerializeField, Tooltip("The amount of fuel burnt when using the jetpack in the air.")]
    private float _burstBurn;
    [SerializeField, Tooltip("The strength of the burst along the horizontal (x) and vertial (y) axi.")]
    private Vector2 _burstScale;
    [SerializeField, Tooltip("The rate at which the jetpack refuels.")]
    private float _refuelRate;
    [SerializeField, Tooltip("The time it takes for the jetpack to begin refueling.")]
    private float _refuelDelay;
    private float _refuelTime;
    private float _jetFuel = 1;
    public bool _holdAfterJump;
    private bool _jetInputReady;
    

    [Header("Fuel Display")]
    [SerializeField] Animator _animator;
    [SerializeField, Tooltip("The display for the jet fuel.")]
    private Image _fuelBarMain;
    [SerializeField]
    private Image _delayedBar;
    [SerializeField]
    private Image _fuelBarBackground;
    [SerializeField]
    private Color[] _jetBackgroundColor;
    [SerializeField]
    private bool _jetpackUI;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        GrabGroundBelow();
        GetGroundNormal();
        //Get the horizontal velocity. We don't want to affect/clamp the vertial movement
        Vector2 horizontalVel = new Vector2(_rb.velocity.x, _rb.velocity.z);
        //Check the current movement speed
        if (horizontalVel.magnitude > _maxSpeed)
        {
            //If to high, start breaking
            //Get the amount over the max speed that the player is moving
            float breakSpeed = horizontalVel.magnitude - _maxSpeed;

            //Get the direction
            Vector2 normalisedVel = horizontalVel.normalized;
            //Calculate the force to apply
            Vector2 breakVelocity = normalisedVel * breakSpeed * _breakAggresion;
            //Lock that force to the x and z axi
            Vector3 triBreakVelocity = new Vector3(breakVelocity.x, 0, breakVelocity.y);
            //Apply the force
            _rb.AddForce(-triBreakVelocity);
        }
        else
        {
            //Move the player forwards based on the camera rotation
            Vector3 camForward = Vector3.Cross(_camera.transform.right, Vector3.up);
            Vector3 forwardForce = camForward * _movementInput.y * _moveAccel * _rb.mass * _movementModifier;
            Vector3 sideForce = _camera.transform.right * _movementInput.x * _strafeAccel * _rb.mass * _movementModifier;
            Vector3 orientedForce = Vector3.Cross(forwardForce + sideForce, GetGroundNormal());
            _rb.AddForce(orientedForce * Time.deltaTime);
        }

        //...otherwise, if on the ground & out of fuel...
        if (_jetFuel < 1 && GroundedCheck())
        {
            //...refuel the jetpack
            if (_refuelTime > 0)
                _refuelTime -= Time.deltaTime;
            else
                _jetFuel = Mathf.Clamp(_jetFuel + _refuelRate * Time.deltaTime, 0f, 1f);
        }

        //Always update the ui
        if (_refuelTime > 0)
            _fuelBarBackground.color = _jetBackgroundColor[1];
        else
            _fuelBarBackground.color = _jetBackgroundColor[0];

        _fuelBarMain.fillAmount = _jetFuel;
        if (_delayedBar.fillAmount > _fuelBarMain.fillAmount)
            _delayedBar.fillAmount = iTween.FloatUpdate(_delayedBar.fillAmount, _fuelBarMain.fillAmount, 10);
        else
            _delayedBar.fillAmount = _fuelBarMain.fillAmount;


        if(_jetpackUI && _jetFuel == 1 && !GroundedCheck())
        {
            SetJetpackUI_OFF();
        }

    }

    

    public void UpdateMovementAxis(Vector2 v)
    {
        //Movement got flipped when I added slope calcs and i don't really wanna implement a proper fix rn (2/11 - Jackson)
        _movementInput = new Vector2(v.y, -v.x);
    }
    /// <summary>
    /// Checks right below the player for objects tagged ground
    /// </summary>
    /// <returns></returns>
    private bool GroundedCheck()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -Vector3.up * PlayerHeight, out hit, PlayerHeight))
        {
            //Debug.Log("Hit object \"" + hit.collider.gameObject.name + "\" tagged as \"" + hit.collider.gameObject.tag);
            if (hit.collider.tag == "Ground")
            {                              
                return true;               
            }                              
        }                                  
        return false;                      
    }
    private void GrabGroundBelow()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -Vector3.up * _lastGroundCheckMaxDistance, out hit, _lastGroundCheckMaxDistance))
        {
            //Debug.Log("Hit object \"" + hit.collider.gameObject.name + "\" tagged as \"" + hit.collider.gameObject.tag);
            if (hit.collider.tag == "Ground")
            {
                _lastGroundPoint = hit.point + new Vector3(0, PlayerHeight, 0);
            }
        }
    }
    public Vector3 GetGroundNormal()
    {
        Vector3 dir = Vector3.up;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up * PlayerHeight, out hit, PlayerHeight))
        {
            //Debug.Log("Hit object \"" + hit.collider.gameObject.name + "\" tagged as \"" + hit.collider.gameObject.tag);
            if (hit.collider.tag == "Ground")
            {
                if ((Mathf.Acos(hit.normal.y / Vector3.up.y) * Mathf.Rad2Deg) < _maxAngle)
                {
                    dir = hit.normal;
                    Debug.DrawRay(hit.point, dir, Color.red);
                }

            }
        }

        return dir;
    }
    public void Jump()
    {
        //Debug.Log("Jump initiated");       
        if (GroundedCheck())
        {
            _rb.AddForce(Vector3.up * _jumpForce * _rb.mass, ForceMode.Impulse);
            _burnTime = _burnDelay;

            _jetInputReady = false;
        }
        else
        {

            if (_jetInputReady || _holdAfterJump)
            {
                _burnTime = 0;

                

                if (_jetFuel >= _burstBurn)
                {
                    Vector3 camForward = Vector3.Cross(_camera.transform.right, Vector3.up);
                    Vector3 forwardForce = camForward * _movementInput.y * _jumpForce * _burstScale.x * _rb.mass * _movementModifier;
                    Vector3 sideForce = _camera.transform.right * _movementInput.x * _jumpForce * _burstScale.x * _rb.mass * _movementModifier;
                    Vector3 upForce = Vector3.up * _jumpForce * _burstScale.y * _rb.mass;
                    Vector3 orientedForce = Vector3.Cross(forwardForce + sideForce, GetGroundNormal());
                    _rb.AddForce(orientedForce + upForce, ForceMode.Impulse);
                    _jetFuel -= _burstBurn;
                }
            }
            _jetInputReady = true;
        }
    }
    public void JetPack()
    {
        //If we have fuel...
        if (_jetFuel > 0)
            //... push the player up and reduce the fuel
            if((_holdAfterJump && _burnTime <= 0) || (_jetInputReady))
            {
                _rb.AddForce(_jetForce * Vector3.up * _rb.mass * Time.deltaTime, ForceMode.Force);
                _jetFuel = Mathf.Clamp(_jetFuel - _burnRate * Time.deltaTime, 0f, 1f);
                _refuelTime = _refuelDelay;
            }
            //...otherwise reduce burn delay
            else if (_burnTime > 0 && _holdAfterJump)
            {
                _burnTime -= Time.deltaTime;
            }
    }                                                         
    public void ReturnToLastGrounedPoint() 
    {                                      
        _rb.velocity = Vector3.zero;       
        transform.position = _lastGroundPoint + RespawnOffset;
    }

    public void SetJetpackUI_ON()
    {
        if (!_jetpackUI)
        {
            _animator.SetTrigger("Jetpack UI IN Trigger");
            _jetpackUI = true;
        }
    }

    public void SetJetpackUI_OFF()
    {
        if (_jetpackUI && GroundedCheck())
        {
            _animator.SetTrigger("Jetpack UI OUT Trigger");
            _jetpackUI = false;
        }
    }




}                                          