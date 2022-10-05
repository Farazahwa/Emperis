using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 10f;

    [SerializeField] 
    private float _jump = 10f;

    [SerializeField] 
    private float _attackRange;

    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private int _maxHealth = 100;

    [SerializeField]
    private int _currentHealth;

    [SerializeField]
    private PlayerHealthBar _playerHealthBar;

    private Rigidbody2D _rigidbody;
    private float _movement;
    private Vector3 _raycastPosition;
    private Vector3 _raycastDirection;
    private bool _attack;
    private bool _grounded = true;
    private Animator _animator;

    

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
        _playerHealthBar.setMaxHealth(_maxHealth);
    }

    void FixedUpdate()
    {
        
        AttackRaycast();    

        var move = _movement * _speed;
        _rigidbody.velocity = new Vector3(move, _rigidbody.velocity.y);

        
        _animator.SetFloat("Move", Mathf.Abs(move));

        // Change Scale
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

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        _playerHealthBar.setHealth(_currentHealth);
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
            _animator.SetTrigger("Attack");
        }
        else
        {
            _attack = false;
        }
    }

    void OnTest(InputValue value)
    {
        TakeDamage(20);
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

    #region Raycast

    void DrawRay( Vector3 start, Vector3 dir, Color color )
	{
		Debug.DrawRay( start, dir, color );
	}


    private void AttackRaycast()
    {
        _raycastPosition = transform.position;
        if (_movement > 0) _raycastDirection = transform.right;
        if (_movement < 0) _raycastDirection = -transform.right;

        DrawRay(_raycastPosition, _raycastDirection * _attackRange, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(_raycastPosition, _raycastDirection, _attackRange, _layerMask);
        if (hit)
        {
            var goblin = hit.collider.gameObject.GetComponent<GoblinController>();
            var kingGoblin = hit.collider.gameObject.GetComponent<KingGoblinController>();
            if (_attack)
            {
                if (goblin != null)
                    goblin.SetDieAnimation();

                if (kingGoblin != null)
                {
                    kingGoblin.SetDieAnimation();
                }
            }
        }
    }

    #endregion

}
