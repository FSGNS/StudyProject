using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool _invertGravity = false;
    public float _moveSpeed;
    public Key _jumpKey;
    public float jumpHeight = 37.5f;
    public float timeToApex = 0.5f;
    public ContactFilter2D _platform;

    private Rigidbody2D _rigidbody;
    private float gravity;
    private float jumpVelocity;
    private int _gravityMultiplier = 1;

    private bool _isOnPlatform => _rigidbody.IsTouching(_platform);

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
        _gravityMultiplier = _invertGravity ? -1 : 1;
        CalculateJumpPhysics();
    }

    private void CalculateJumpPhysics()
    {
        gravity = -2f * jumpHeight / (timeToApex * timeToApex);
        jumpVelocity = Mathf.Abs(gravity) * timeToApex;
    }    

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector2(_moveSpeed, _rigidbody.linearVelocity.y);
    }

    public void Jump()
    {
        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, jumpVelocity * _gravityMultiplier);
    }

    private void Update()
    {
        if (Keyboard.current[_jumpKey].wasPressedThisFrame && _isOnPlatform)
        {
            Jump();
        }
        ApplyCustomGravity();

    }
    private void ApplyCustomGravity()
    {
        _rigidbody.linearVelocity += new Vector2(0, gravity * _gravityMultiplier * Time.deltaTime);
    }    
}