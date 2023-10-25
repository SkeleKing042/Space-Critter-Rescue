//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovment : MonoBehaviour
{
    Rigidbody _rb;
    Vector2 _movementInput;
    public float MoveSpeed;
    public float StrafeSpeed;
    public float MaxSpeed;
    private float _movementModifier = 1;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_rb.velocity.x + _rb.velocity.z < MaxSpeed)
            _rb.AddForce((transform.forward * _movementInput.y * (MoveSpeed * _rb.mass) * _movementModifier + transform.right * _movementInput.x * (StrafeSpeed * _rb.mass) * _movementModifier) * Time.deltaTime);
    }
    public void UpdateMovementAxis(Vector2 v)
    {
        _movementInput = v;
    }
}