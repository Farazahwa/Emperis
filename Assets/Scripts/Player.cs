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
    
    void OnMove(InputValue value)
    {
        _movement = value.Get<float>();
    }

    void OnJump(InputValue value)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
        
        Jump();
    }

    private void Jump()
    {
        
        if (_grounded)
        {
            Debug.Log("Space Pressed");
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jump);
            _grounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (CompareTag("Grounded"))
        {
            _grounded = true;
        }
    }

    public bool SetGround()
    {
        return _grounded;
    }
}
