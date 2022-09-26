using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _jump = 10f;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Goblin _goblin;
    private Rigidbody2D _rigidbody;
    private float _movement;
    private bool _attack;
    private bool _grounded = true;
    private Animator _animator;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        AttackRaycast();    

        var move = _movement * _speed;
        _rigidbody.velocity = new Vector3(move, _rigidbody.velocity.y);

        _animator.SetBool("Attack", _attack);
        _animator.SetFloat("Move", Mathf.Abs(move));

        if (_movement < 0)
        {
            transform.localScale = new Vector3(-2.5f, 2.2f, 1f);
        }
        else if (_movement == 0 && transform.lossyScale.x < 0)
        {
            transform.localScale = new Vector3(-2.5f, 2.2f, 1f);
        }
        else if (_movement == 0 && transform.lossyScale.x > 0)
        {
            transform.localScale = new Vector3(2.5f, 2.2f, 1f);
        }
        else 
        {
            transform.localScale = new Vector3(2.5f, 2.2f, 1f);
        }
    }
    
#region Input System Controller

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

    void OnAttack(InputValue value)
    {
        var input = value.Get<float>();
        if (input == 1)
        {
            _attack = true;
        }
        else
        {
            _attack = false;
        }
    }

#endregion

#region Helper Method

    // Helper Method for Input System Jump
    private void Jump()
    {
        if (_grounded)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jump);
            _grounded = false;
        }
    }

#endregion

#region Collision Method

    // Collision with other game object
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Grounded"))
        {
            _grounded = true;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            _movement = 0;
        }
    }

#endregion

	void DrawRay( Vector3 start, Vector3 dir, Color color )
	{
		Debug.DrawRay( start, dir, color );
	}


    private void AttackRaycast()
    {
        var raycastPosition = transform.position;
        var raycastDirection = transform.right;
        if (_movement > 0) raycastDirection = transform.right;
        if (_movement < 0) raycastDirection = -transform.right;

        DrawRay(raycastPosition, raycastDirection * _attackRange, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, raycastDirection, _attackRange, _layerMask);
        if (hit)
        {
            
        }
    }
}
