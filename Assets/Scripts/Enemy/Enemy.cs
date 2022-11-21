using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    protected float _speed = 5f;

    [SerializeField]
    protected float _health;

    [SerializeField]
    protected float _detectRange;

    [SerializeField]
    protected float _attackRange;

    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    protected List<GameObject> _pointer;

    [SerializeField]
    protected float _waitingDelay = 2f;

    [SerializeField]
    protected float _attackDelay = 3f;

    public int damage;

    protected enum State
    {
        Wait,
        Patrol,
        Chase,
        Attack,
        Death
    }

    protected enum Animation
    {
        Idle,
        Move,
        Attack,
        Die,
        Hitted
    }

    #region private instance variable

    protected float _move = 1f;
    protected float _waitingTime;
    protected float _attackTime;

    protected int _targetIndex = 0;

    protected Rigidbody2D _rb;
    protected Animator _anim;
    protected Transform _player;
    
    protected virtual float Tolerance { get; }

    private Vector3 _raycastPosition;
    private Vector3 _raycastDirection;

    protected State _state;

    #endregion


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        _state = State.Patrol;

        _waitingTime = _waitingDelay;
        _attackTime = _attackDelay;
    }

    void Update()
    {
        switch (_state)
        {
            case State.Wait:
                PlayerDetection();
                AnimationControl(Animation.Idle);
                Waiting();
                break;
            case State.Patrol:
                PlayerDetection();

                if (_pointer[_targetIndex] == null)
                {
                    _state = State.Wait;
                    break;
                }
                AnimationControl(Animation.Move);
                Patrol();
                break;
            case State.Chase:
                AttackRaycast();
                AnimationControl(Animation.Move);
                Chasing();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Death:
                AnimationControl(Animation.Die);
                break;
        }
    }

    protected void AnimationControl(Animation animation)
    {
        switch (animation)
        {
            case Animation.Idle:
                _anim.SetBool("Move", false);
                break;
            case Animation.Move:
                _anim.SetBool("Move", true);
                break;
            case Animation.Attack:
                _anim.SetTrigger("Attack");
                break;
            case Animation.Die:
                _anim.SetTrigger("Die");
                break;
            case Animation.Hitted:
                _anim.SetTrigger("Hit");
                break;
        }
    }

    #region Raycast

    void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
        Debug.DrawRay(start, dir, color);
    }

    protected void PlayerDetection()
    {
        _raycastPosition = transform.position;
        if (transform.localScale.x > 0) _raycastDirection = transform.right;
        if (transform.localScale.x < 0) _raycastDirection = -transform.right;


        DrawRay(_raycastPosition, _raycastDirection * _detectRange, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(_raycastPosition, _raycastDirection, _detectRange, _layerMask);
        if (hit)
        {
            _move = 1;
            _player = hit.transform;
            _state = State.Chase;
        }
    }

    protected void AttackRaycast()
    {
        _raycastPosition = transform.position - new Vector3(0, .5f, 0);
        if (transform.localScale.x > 0) _raycastDirection = transform.right;
        if (transform.localScale.x < 0) _raycastDirection = -transform.right;


        DrawRay(_raycastPosition, _raycastDirection * _attackRange, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(_raycastPosition, _raycastDirection, _attackRange, _layerMask);
        if (hit)
        {
            _move = 0;
            _state = State.Attack;
        }
    }

    #endregion

    #region State

    protected virtual void Chasing()
    {
        // Chasing player corresponding each enemy behavior

        _move = 1;
        if (_player.transform.position.x <= transform.position.x)
        {
            var x = _move * _speed * -1;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
            if (transform.localScale.x > 0)
            {
                Flip();
            }
        }
        if (_player.transform.position.x >= transform.position.x)
        {
            var x = _move * _speed;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
            if (transform.localScale.x < 0)
            {
                Flip();
            }
        }
    }

    protected virtual void Patrol()
    {
        // Patroling
        
        float x;
        if (transform.localScale.x < 0)
        {
            x = _move * _speed * -1;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
        }
        else
        {
            x = _move * _speed;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
        }

        if (Vector3.Distance(transform.position, _pointer[_targetIndex].transform.position) <= .9f)
        {
            _targetIndex = (_targetIndex + 1) % 2;
            _state = State.Wait;
        }
    }

    protected virtual void Waiting()
    {
        // Stop in certain pointer

        _move = 0;
        _waitingDelay -= Time.deltaTime;
        if (_waitingDelay <= 0)
        {
            Flip();
            _move = 1;
            _waitingDelay = _waitingTime;
            _state = State.Patrol;
        }
    }

    /// <summary>
    /// Hit the player
    /// </summary>
    protected virtual void Attack()
    {
        if (_attackDelay == _attackTime)
        {
            AnimationControl(Animation.Attack);
        }

        AnimationControl(Animation.Idle);
        var distance = Vector3.Distance(_player.position, transform.position);
        var range = _attackRange + Tolerance;
        _attackDelay -= Time.deltaTime;

        if (_attackDelay <= 0)
        {
            _attackDelay = _attackTime;
        }

        if (distance > range)
        {
            _move = 1;
            _state = State.Chase;
        }
    }

    /// <summary>
    /// Death method : called when this gameObject death
    /// </summary>
    protected virtual void Death()
    {
        Destroy(this.gameObject);
    }

    #endregion

    #region Helper Method

    protected void Flip()
    {
        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
    }

    public void Hitted()
    {
        _health -= 1;
        if (_health <= 0)
        {
            _state = State.Death;
            return;
        }

        AnimationControl(Animation.Hitted);
    }
    
    protected void Knockback()
    {
        if (_player.position.x > transform.position.x)
        {
            _rb.AddForce(transform.right * -25f, ForceMode2D.Impulse);
        }
        else
        {
            _rb.AddForce(transform.right * 25f, ForceMode2D.Impulse);
        }
    }

    protected void HitPlayer()
    {
        var player = _player.GetComponent<PlayerController>();
        player.TakeDamage(damage, transform);
    }

    #endregion

}
