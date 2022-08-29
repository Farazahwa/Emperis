using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private Rigidbody2D _rigidbody;
    private float _movement;
    private bool _grounded = true;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var move = _movement * _speed;
        _rigidbody.velocity = new Vector3(move, _rigidbody.velocity.y);
    }
    
    void OnMove(InputValue value)
    {
        _movement = value.Get<float>();
    }

    void OnJump(InputValue value)
    {

        Jump();
    }

    private void Jump()
    {
        if (_grounded)
        {
            Debug.Log("Space Pressed");
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _speed);
            _grounded = false;
        }
    }

    private void OnCollisionEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grounded"))
        {
            Debug.Log("Collision");
            _grounded = true;
        }
    }

    public bool SetGround()
    {
        return _grounded;
    }
}
