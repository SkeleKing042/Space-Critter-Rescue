//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovment : MonoBehaviour
{
    Rigidbody _rb;
    Camera _camera;
    Vector2 _movementInput;
    [Header("Ground movement")]
    [SerializeField, Tooltip("The speed at which the player moves forwards and backwards.")]
    private float _moveSpeed;
    [SerializeField, Tooltip("The speed at which the player moves side to side.")]
    private float _strafeSpeed;
    [SerializeField, Tooltip("The maximum speed the player can reach.")]
    private float _maxSpeed;
    [SerializeField, Tooltip("Modifies how agresive the breaking is at maximum speed.")]
    private float _breakForce = 1;
    private float _movementModifier = 1; //Unused ATM

    [Header("Jump movement")]
    [SerializeField, Tooltip("The force that the player jumps with.")]
    private float _jumpForce;
    [SerializeField]
    private Vector3 _feetPosition;
    [SerializeField, Tooltip("The distance from the feet position that the ground check reaches.")]
    private float _legLength;

    [Header("Jetpack Settings")]
    [SerializeField, Tooltip("The force that the jetpack outputs.")]
    private float _jetForce;
    private bool _doJet;
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

    [Header("Debug")]
    [SerializeField, Tooltip("The display for the jet fuel.")]
    private Image _fuelBar;
    [SerializeField]
    private Image _fuelBarBackground;
    [SerializeField]
    private Color[] _jetColors;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    void FixedUpdate()
    {
        //Get the horizontal velocity. We don't want to affect/clamp the vertial movement
        Vector2 horizontalVel = new Vector2(_rb.velocity.x, _rb.velocity.z);
        //Check the current movement speed
        if(horizontalVel.magnitude > _maxSpeed)
        {
            //If to high, start breaking
            //Get the amount over the max speed that the player is moving
            float breakSpeed = horizontalVel.magnitude - _maxSpeed;

            //Get the direction
            Vector2 normalisedVel = horizontalVel.normalized;
            //Calculate the force to apply
            Vector2 breakVelocity = normalisedVel * breakSpeed * _breakForce;
            //Lock that force to the x and z axi
            Vector3 triBreakVelocity = new Vector3(breakVelocity.x, 0, breakVelocity.y);
            //Apply the force
            _rb.AddForce(-triBreakVelocity);
        }
        else
        {
            //Move the player forwards based on the camera rotation
            _rb.AddForce(
                (Vector3.Cross(_camera.transform.right, Vector3.up) * _movementInput.y * (_moveSpeed * _rb.mass) * _movementModifier +
                _camera.transform.right * _movementInput.x * (_strafeSpeed * _rb.mass) * _movementModifier) * Time.deltaTime);
        }

        //While doing the jet input...
        if (_doJet)
        {
            //...if we are ready to burn...
            if (_burnTime <= 0)
            {
                //...and we have fuel...
                if (_jetFuel > 0)
                {
                    //...Push the player up and reduce the fuel
                    _rb.AddForce(_jetForce * Vector3.up * _rb.mass, ForceMode.Force);
                    _jetFuel = Mathf.Clamp(_jetFuel - _burnRate * Time.deltaTime, 0f, 1f);
                    _refuelTime = _refuelDelay;
                }
            }
            //...otherwise, reduce burn delay
            else
            {
                _burnTime -= Time.deltaTime;
            }
        }
        //...otherwise, if on the ground & out of fuel...
        else if (_jetFuel < 1 && GroundCheck())
        {
            //...refuel the jetpack
            if (_refuelTime > 0)
                _refuelTime -= Time.deltaTime;
            else
                _jetFuel = Mathf.Clamp(_jetFuel + _refuelRate * Time.deltaTime, 0f, 1f);
        } 

        //Always update the ui
        if (_refuelTime > 0)
            _fuelBarBackground.color = _jetColors[1];
        else
            _fuelBarBackground.color = _jetColors[0];

        _fuelBar.fillAmount = _jetFuel;
        
    }
    public void UpdateMovementAxis(Vector2 v)
    {
        _movementInput = v;
    }
    /// <summary>
    /// Checks right below the player for objects tagged ground
    /// </summary>
    /// <returns></returns>
    private bool GroundCheck()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + _feetPosition, -Vector3.up * _legLength, out hit, 1.0f))
        {
            Debug.Log("Hit object \"" + hit.collider.gameObject.name + "\" tagged as \"" + hit.collider.gameObject.tag);
            if(hit.collider.tag == "Ground")
                return true;
        }

        return false;
    }
    public void InitializeJump()
    {
        Debug.Log("Jump initiated");
        if (GroundCheck())
        {
            _rb.AddForce(Vector3.up * _jumpForce * _rb.mass, ForceMode.Impulse);
            _burnTime = _burnDelay;
        }
        else
        {
            if (_jetFuel >= _burstBurn)
            {

                _rb.AddForce(
                    (Vector3.Cross(_camera.transform.right, Vector3.up) * _movementInput.y * _jumpForce * _burstScale.x * _rb.mass * _movementModifier +
                    _camera.transform.right * _movementInput.x * _jumpForce * _burstScale.x * _rb.mass * _movementModifier +
                    Vector3.up * _jumpForce * _burstScale.y * _rb.mass), ForceMode.Impulse);
                _jetFuel -= _burstBurn;
            }
            _burnTime = 0;
        }
        _doJet = true;
    }
    public void TerminateJump()
    {
        Debug.Log("Jump terminated");
        _doJet = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + _feetPosition, -Vector3.up * _legLength);
    }
}