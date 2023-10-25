//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovment : MonoBehaviour
{
    Rigidbody _rb;
    Camera _camera;
    Vector2 _movementInput;
    [Header("Ground movement")]
    public float MoveSpeed;
    public float StrafeSpeed;
    public float MaxSpeed;
    private float _movementModifier = 1;
    [Header("Jump movement")]
    public float JumpForce;
    public Vector3 FeetPosition;
    public float JetpackForce;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_rb.velocity.x + _rb.velocity.z < MaxSpeed)
            _rb.AddForce(
                (Vector3.Cross(_camera.transform.right, Vector3.up) * _movementInput.y * (MoveSpeed * _rb.mass) * _movementModifier +
                _camera.transform.right * _movementInput.x * (StrafeSpeed * _rb.mass) * _movementModifier) * Time.deltaTime);
    }
    public void UpdateMovementAxis(Vector2 v)
    {
        _movementInput = v;
    }
    public void Jump()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + FeetPosition, -Vector3.up, out hit, 1.0f))
        {
            Debug.Log("Hit object \"" + hit.collider.gameObject.name + "\" tagged as \"" + hit.collider.gameObject.tag);
            if(hit.collider.tag == "Ground")
                _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }
    public void Jetpack()
    {
        _rb.AddForce(JetpackForce * Vector3.up, ForceMode.Force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + FeetPosition, -Vector3.up);
    }
}