using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

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

    //control how powerful the knockback(strength)
    [SerializeField]
    private float strength = 16, delay = 0.15f;

    private Rigidbody2D _rigidbody;
    private float _movement;
    private Vector3 _raycastPosition;
    private Vector3 _raycastDirection;
    private bool _attack;
    private bool _grounded = true;
    private Animator _animator;
    private Animator anim;

    public float thrust = 1.0f;

    public UnityEvent OnBegin, OnDone;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
        _playerHealthBar.setMaxHealth(_maxHealth);
        anim = GetComponent<Animator>();
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
        _rigidbody.AddForce(new Vector3(0f, 0f, thrust), ForceMode2D.Impulse);
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

        if (other.gameObject.CompareTag("Trap"))
        {
            TakeDamage(7);
        }

        if (other.gameObject.CompareTag("Lava"))
        {
            TakeDamage(100);
            Die();
        }

    }

    void Die()
    {
        anim.SetTrigger("death");
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
            var tester = hit.collider.gameObject.GetComponent<Testing>();
            if (_attack)
            {
                if (goblin != null)
                    goblin.Hit();

                if (kingGoblin != null)
                {
                    kingGoblin.Hit();
                }

                if (tester != null)
                {
                    tester.Hit();
                }

                _attack = false;
            }
        }
    }

    #endregion

    #region Feedback Enemy Object

    //sender object to know direction should apply the snug back feedback
    //if player attacks the enemy, we're going to calculate the direction from the player to the enemy
    //public void PlayFeedback(GameObject sender)
    //{
    //    StopAllCoroutines();
    //    OnBegin?.Invoke();
    //    Vector2 direction = (transform.position-sender.transform.position).normalized;
    //    _rigidbody.AddForce(direction*strength, ForceMode2D.Impulse); //will make player fly in the opposite direction
    //    StartCoroutines(Reset());
    //}

    #endregion

    #region Reset the KnockBack Feedback to Stop

    // private IEnumerator Reset()
    // {
    //     yield return new WaitForSeconds(delay);
    //     _rigidbody.velocity = Vector3.zero;
    //     OnDone?.Invoke();
    // }

    #endregion
}
