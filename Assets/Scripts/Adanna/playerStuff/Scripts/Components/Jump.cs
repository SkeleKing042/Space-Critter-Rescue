using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Jump : MonoBehaviour
{
    Rigidbody rigidbody;
    public float jumpStrength = 2;
    public event System.Action Jumped;
    public TMP_Text JetpackFuelText;

    [SerializeField]
    private float _jetpackCapacity = 100;


    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;


    void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Awake()
    {
        // Get rigidbody.
        rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        JetpackFuelText.text = ("Jetpack Fuel:" +  Mathf.RoundToInt(_jetpackCapacity));
            
    }


    void LateUpdate()
    {
        // Jump when the Jump button is pressed and we are on the ground.
        if (Input.GetButtonDown("Jump") && (!groundCheck || groundCheck.isGrounded))
        {
            rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
        }
    }
    private void FixedUpdate()
    {
        // if jetpack has energy then allow to use the jet pack
        
        if (Input.GetButton("Jump") && _jetpackCapacity >0)
        {
            rigidbody.AddForce(Vector3.up * 10);
            Jumped?.Invoke();
            rigidbody.AddForce(Vector3.up * 10);
            _jetpackCapacity = _jetpackCapacity - 1.2f;
        }

        // if the jet pack isnt at 100 the increase the jetpack fuel by 1 each frame
        if ((!groundCheck || groundCheck.isGrounded) && _jetpackCapacity< 100)
            _jetpackCapacity += 0.25f;
        // if fuel isnt withing range set value
        if (_jetpackCapacity > 100)
                _jetpackCapacity = 100;
        if (_jetpackCapacity < 0)
            _jetpackCapacity = 0;
    }
}
