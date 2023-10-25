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
    [SerializeField, Tooltip("The speed at which the player moves forwards and backwards.")] private float _moveSpeed;
    [SerializeField, Tooltip("The speed at which the player moves side to side.")] private float _strafeSpeed;
    [SerializeField, Tooltip("The maximum speed the player can reach.")] private float _maxSpeed;
    [SerializeField, Tooltip("Modifies how agresive the breaking is at maximum speed.")] private float _breakForce = 1;
    private float _movementModifier = 1; //Unused ATM
    [Header("Jump movement")]
    [SerializeField, Tooltip("The force that the player jumps with.")] private float _jumpForce;
    [SerializeField] private Vector3 _feetPosition;
    [SerializeField, Tooltip("The force that the jetpack outputs.")] private float _jetpackForce;
    private bool doJet;
    [SerializeField, Tooltip("The rate at which the jetpack fuel is expended.")] private float _jetBurnRate;
    [SerializeField, Tooltip("The rate at which the jetpack refuels.")] private float _jetReturnRate;
    private float _jetFuel = 1;
    [SerializeField, Tooltip("The display for the jet fuel.")] private Image _fuelBar;

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
            _rb.AddForce((Vector3.Cross(_camera.transform.right, Vector3.up) * _movementInput.y * (_moveSpeed * _rb.mass) * _movementModifier + _camera.transform.right * _movementInput.x * (_strafeSpeed * _rb.mass) * _movementModifier) * Time.deltaTime);
        }

        //If we are using the jet and it has fuel...
        if(doJet && _jetFuel > 0)
        {
            //...Push the player up and reduce the fuel
            _rb.AddForce(_jetpackForce * Vector3.up, ForceMode.Acceleration);
            _jetFuel = Mathf.Clamp(_jetFuel - _jetBurnRate * Time.deltaTime, 0f , 1f);
        }
        //otherwise, if on the ground...
        if(!doJet && _jetFuel < 1 && GroundCheck())
        {
            //...refuel the jetpack
            _jetFuel = Mathf.Clamp(_jetFuel + _jetReturnRate * Time.deltaTime, 0f, 1f);
        }

        //Always update the ui
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
        if(Physics.Raycast(transform.position + _feetPosition, -Vector3.up, out hit, 1.0f))
        {
            Debug.Log("Hit object \"" + hit.collider.gameObject.name + "\" tagged as \"" + hit.collider.gameObject.tag);
            if(hit.collider.tag == "Ground")
                return true;
        }

        return false;
    }
    /// <summary>
    /// Pushes the player up while grounded
    /// </summary>
    public void Jump()
    {
        if(GroundCheck())
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    public void EnableJet()
    {
        doJet = true;
    }
    public void DisableJet()
    {
        doJet = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + _feetPosition, -Vector3.up);
    }
}