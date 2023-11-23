//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform _head;
    [HideInInspector]
    public Rigidbody PlayerRigidbody;
    Camera _camera;
    private Vector2 MovementInput;
    [HideInInspector]
    public bool DoMovement;
    public bool Actionable;
    private SoundPropagation _soundPropagation;
    [Header("Ground movement")]
    [SerializeField, Tooltip("The speed at which the player moves forwards and backwards.")]
    private float _moveAccel;
    [SerializeField, Tooltip("The speed at which the player moves side to side.")]
    private float _strafeAccel;
    [SerializeField, Tooltip("The maximum speed the player can reach.")]
    private float _maxSpeed;
    [SerializeField, Tooltip("Modifies how aggresive the breaking is at maximum speed.")]
    private float _breakAggresion = 1;
    private float _movementModifier = 1;
    [SerializeField, Tooltip("The maximum distance that the last ground check can reach.")]
    private float _lastGroundCheckMaxDistance;
    [SerializeField, Tooltip("The maximum angle that the player can move up smoothly.")]
    private float _maxAngle;
    private bool _flooringIt;
    [SerializeField, Tooltip("The increase in speed granted by sprinting")]
    private float _sprintScale;
    [SerializeField, Tooltip("The speed that the player stops sprinting at.")]
    private float _sprintCancelLevel;
    [SerializeField]
    private Transform _orientedForceObject;

    [Header("Jump movement")]
    [SerializeField, Tooltip("The force that the player jumps with.")]
    private float _jumpForce;
    private Vector3 _lastGroundPoint;
    public Vector3 RespawnOffset;

    [Header("Ground Checks")]
    [Min(1), Tooltip("The distance from the feet position that the ground check reaches.")]
    public float PlayerHeight;
    [SerializeField, Tooltip("The distance ahead of the player that ground will be detected")]
    public float StrideLength;
    [SerializeField, Tooltip("The distance to the side of the player that ground will be detected")]
    public float StrideWidth;
    [SerializeField]
    public bool VelocityBasedChecks;
    [SerializeField]
    private LayerMask _groundLayer;

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
    public float RefuelTime { get { return _refuelTime; } }
    private float _jetFuel = 1;
    public float JetFuel { get { return _jetFuel; } } 
    public bool _holdAfterJump;
    private bool _jetInputReady;

    [Header("Fuel Display")]
    [SerializeField, Tooltip("The display for the jet fuel.")]
    private Image _fuelBarMain;
    [SerializeField]
    private Image _delayedBar;
    [SerializeField]
    private Image _fuelBarBackground;
    [SerializeField]
    private Color[] _jetBackgroundColor;

    [Header("Crouch Settings")]
    [SerializeField, Tooltip("The amount to scale the player by.")]
    private float _crouchScale;
    private float _headHeight;
    private bool _crouched;

    [HideInInspector]
    public Vector3[] GroundPoints = new Vector3[4];
    void Start()
    {
        _headHeight = _head.localPosition.y;
        PlayerRigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;
        DoMovement = true;
        _soundPropagation = GetComponentInChildren<SoundPropagation>();
    }
    void FixedUpdate()
    {
        GetGroundNormal();
        //Get the horizontal velocity. We don't want to affect/clamp the vertial movement
        Vector2 horizontalVel = new Vector2(PlayerRigidbody.velocity.x, PlayerRigidbody.velocity.z);
        //Check the current movement speed
        if (horizontalVel.magnitude > _maxSpeed * _movementModifier)
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
            PlayerRigidbody.AddForce(-triBreakVelocity);
        }
        else if(DoMovement && Actionable)
        {
            _orientedForceObject.up = GetGroundNormal();
            _orientedForceObject.rotation = Quaternion.Euler(_orientedForceObject.rotation.eulerAngles.x, _camera.transform.rotation.eulerAngles.y, _orientedForceObject.rotation.eulerAngles.z);
            //Move the player forwards based on the camera rotation
            //Debug.DrawRay(transform.position, camForward, Color.blue);
            Vector3 forwardForce = _orientedForceObject.forward * MovementInput.y * _moveAccel * PlayerRigidbody.mass;
            Vector3 sideForce = _orientedForceObject.right * MovementInput.x * _strafeAccel * PlayerRigidbody.mass;
            Debug.Log("Ground norm is " + GetGroundNormal());
            PlayerRigidbody.AddForce((forwardForce + sideForce) * Time.deltaTime);

            if(!_crouched && horizontalVel.magnitude > _maxSpeed * 0.1f)
            {
                _soundPropagation.PropagateSound(Mathf.Clamp(horizontalVel.magnitude / _maxSpeed, 0, 1));
            }
            GroundedCheck();
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
        if (horizontalVel.magnitude < _sprintCancelLevel)
            DoSprint(false);


    }
    #region Movement
    public void UpdateMovementAxis(Vector2 v)
    {
        if(Actionable) MovementInput = v;
    }
    public void DoCrouch()
    {
        if(DoMovement && GroundedCheck() && Actionable)
        {
            if (!_crouched)
                CrouchPlayer();
            else
                UncrouchPlayer();
        }
    }
    public void DoCrouch(bool c)
    {
        _crouched = c;
        DoCrouch();
    }
    private void CrouchPlayer()
    {
        if (DoMovement && Actionable)
        {
            DoSprint(false);
            _flooringIt = false;
            _crouched = true;
            _headHeight *= _crouchScale;
            _movementModifier *= _crouchScale;
            gameObject.GetComponent<CapsuleCollider>().height *= _crouchScale;
            PlayerHeight *= _crouchScale;

            transform.position = new Vector3(transform.position.x, transform.position.y - (PlayerHeight / 2), transform.position.z);
        }
    }
    private void UncrouchPlayer()
    {
        if (DoMovement && Actionable)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (PlayerHeight / 2), transform.position.z);

            _crouched = false;
            _headHeight /= _crouchScale;
            _movementModifier /= _crouchScale;
            gameObject.GetComponent<CapsuleCollider>().height /= _crouchScale;
            PlayerHeight /= _crouchScale;
        }
    }

    public void DoSprint()
    {
        if (DoMovement && !_crouched && GroundedCheck() && Actionable)
        {
            if (!_flooringIt)
                _movementModifier = _sprintScale;
            else
                _movementModifier = 1;
            _flooringIt = !_flooringIt;
        }


    }
    public void DoSprint(bool run)
    {
        _flooringIt = !run;
        DoSprint();
    }
    #endregion
    #region Grounded
    /// <summary>
    /// Checks right below the player for objects tagged ground
    /// </summary>
    /// <returns></returns>
    public bool GroundedCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up * _lastGroundCheckMaxDistance, out hit, _lastGroundCheckMaxDistance))
        {
            //Debug.Log("Hit object \"" + hit.collider.gameObject.name + "\" tagged as \"" + hit.collider.gameObject.tag);
            if (hit.collider.tag == "Ground")
            {
                _lastGroundPoint = hit.point + new Vector3(0, PlayerHeight, 0);
            }
        }

        float camY = _camera.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        Vector3 playerForwards = new Vector3(Mathf.Sin(camY), 0, Mathf.Cos(camY));
        Vector3 playerRights = new Vector3(Mathf.Cos(camY), 0, Mathf.Sin(-camY));
        GroundPoints[0] = transform.position + playerForwards * StrideLength;
        GroundPoints[1] = transform.position + playerRights * StrideWidth;
        GroundPoints[2] = transform.position + playerForwards * -StrideLength;
        GroundPoints[3] = transform.position + playerRights * -StrideWidth;

        if(
            Physics.Raycast(GroundPoints[0], Vector3.down * PlayerHeight, out hit, PlayerHeight, _groundLayer) ||
            Physics.Raycast(GroundPoints[1], Vector3.down * PlayerHeight, out hit, PlayerHeight, _groundLayer) ||
            Physics.Raycast(GroundPoints[2], Vector3.down * PlayerHeight, out hit, PlayerHeight, _groundLayer) ||
            Physics.Raycast(GroundPoints[3], Vector3.down * PlayerHeight, out hit, PlayerHeight, _groundLayer)
            )
        {
            //Debug.Log("Hit object \"" + hit.collider.gameObject.name + "\" tagged as \"" + hit.collider.gameObject.tag);
            if (hit.collider.tag == "Ground")
            {                              
                return true;               
            }                              
        }                                  
        return false;                      
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
    public void ReturnToLastGrounedPoint() 
    {                                      
        PlayerRigidbody.velocity = Vector3.zero;       
        transform.position = _lastGroundPoint + RespawnOffset;
    }     
    #endregion
    #region Jumping
    public void Jump()
    {
        if(DoMovement && Actionable)
        //Debug.Log("Jump initiated");       
        if (GroundedCheck())
        {
            _soundPropagation.PropagateSound(0.5f);
            PlayerRigidbody.AddForce(Vector3.up * _jumpForce * PlayerRigidbody.mass, ForceMode.Impulse);
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
                        _soundPropagation.PropagateSound(1);
                    Vector3 camForward = Vector3.Cross(_camera.transform.right, Vector3.up);
                    Vector3 forwardForce = camForward * MovementInput.y * _jumpForce * _burstScale.x * PlayerRigidbody.mass * _movementModifier;
                    Vector3 sideForce = _camera.transform.right * MovementInput.x * _jumpForce * _burstScale.x * PlayerRigidbody.mass * _movementModifier;
                    Vector3 upForce = Vector3.up * _jumpForce * _burstScale.y * PlayerRigidbody.mass;
                    PlayerRigidbody.AddForce(forwardForce + sideForce + upForce, ForceMode.Impulse);
                    _jetFuel -= _burstBurn;
                }
            }
            _jetInputReady = true;
        }
    }
    public void JetPack()
    {
        if (DoMovement && Actionable)
            //If we have fuel...
            if (_jetFuel > 0)
            {
                _soundPropagation.PropagateSound(0.85f);
                //... push the player up and reduce the fuel
                if ((_holdAfterJump && _burnTime <= 0) || (_jetInputReady))
                {
                    PlayerRigidbody.AddForce(_jetForce * Vector3.up * PlayerRigidbody.mass * Time.deltaTime, ForceMode.Force);
                    _jetFuel = Mathf.Clamp(_jetFuel - _burnRate * Time.deltaTime, 0f, 1f);
                    _refuelTime = _refuelDelay;
                }
                //...otherwise reduce burn delay
                else if (_burnTime > 0 && _holdAfterJump)
                {
                    _burnTime -= Time.deltaTime;
                }
            }
    }
    #endregion
}