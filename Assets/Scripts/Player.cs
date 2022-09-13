using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _jump = 8f;
    private Rigidbody2D _rigidbody;
    private float _movement;
    private bool _grounded = true;
    private Animator _animator;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        var move = _movement * _speed;
        _rigidbody.velocity = new Vector3(move, _rigidbody.velocity.y);

        _animator.SetFloat("Move", Mathf.Abs(move));
    }
    
    // Input System Move
    void OnMove(InputValue value)
    {
        _movement = value.Get<float>();
    }

    // Input System Jump
    void OnJump(InputValue value)
    {   
        Jump();
    }

    // Helper Method
    private void Jump()
    {
        if (_grounded)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jump);
            _grounded = false;
        }
    }

    // Collision with other game object
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Grounded"))
        {
            _grounded = true;
        }
    }
}
