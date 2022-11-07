using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    protected float _health;

    [SerializeField]
    private float _detectRange;

    [SerializeField]
    private float _attackRange;

    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private List<GameObject> _pointer;

    [SerializeField]
    private float _waitingDelay = 2f;

    [SerializeField]
    private float _attackDelay = 3f;

    protected enum State
    {
        Waiting,
        Patrolling,
        Chasing,
        Attacking,
        Death
    }

    enum Animation
    {
        Idle,
        Move,
        Attack,
        Die
    }

    #region private instance variable

    protected float _move = 1f;
    private float _waitingTime;
    private float _attackTime;

    private int _targetIndex = 0;

    private Rigidbody2D _rb;
    private Animator _anim;
    private Transform _player;
    protected virtual Vector3 Scale { get; set; }

    private Vector3 _raycastPosition;
    private Vector3 _raycastDirection;

    protected State _state;

    #endregion


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _state = State.Patrolling;

        _waitingTime = _waitingDelay;
        _attackTime = _attackDelay;
    }

    void Update()
    {
        switch (_state)
        {
            case State.Waiting:
                AnimationControl(Animation.Idle);
                Waiting();
                break;
            case State.Patrolling:
                AnimationControl(Animation.Move);
                Patrol();
                break;
            case State.Chasing:
                AnimationControl(Animation.Move);
                Chasing();
                break;
            case State.Attacking:
                Attack();
                break;
            case State.Death:
                AnimationControl(Animation.Die);
                break;
        }
    }

    private void AnimationControl(Animation animation)
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
        }
    }

    #region Raycast

    void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
        Debug.DrawRay(start, dir, color);
    }

    private void PlayerDetection()
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
            _state = State.Chasing;
        }
    }

    private void AttackRaycast()
    {
        _raycastPosition = transform.position - new Vector3(0, .5f, 0);
        if (transform.localScale.x > 0) _raycastDirection = transform.right;
        if (transform.localScale.x < 0) _raycastDirection = -transform.right;


        DrawRay(_raycastPosition, _raycastDirection * _attackRange, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(_raycastPosition, _raycastDirection, _attackRange, _layerMask);
        if (hit)
        {
            _move = 0;
            _state = State.Attacking;
        }
    }

    #endregion

    #region Goblin State

    protected virtual void Chasing()
    {
        AttackRaycast();

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
        PlayerDetection();
        float x;
        if (_pointer[_targetIndex] == null)
        {
            return;
        }

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
            _state = State.Waiting;
        }
    }

    protected virtual void Waiting()
    {
        _move = 0;
        _waitingDelay -= Time.deltaTime;
        if (_waitingDelay <= 0)
        {
            Flip();
            _move = 1;
            _waitingDelay = _waitingTime;
            _state = State.Patrolling;
        }
    }

    protected virtual void Attack()
    {
        AnimationControl(Animation.Idle);
        var distance = Vector3.Distance(_player.position, transform.position);
        _attackDelay -= Time.deltaTime;
        Debug.Log(distance);
        
        if (_attackDelay <= 0)
        {
            AnimationControl(Animation.Attack);
            _attackDelay = _attackTime;
        }

        if (distance > _attackRange)
        {
            _move = 1;
            _state = State.Chasing;
        }
    }

    protected virtual void Death()
    {
        Destroy(this.gameObject);
    }
    #endregion

    #region Helper Method

    private void Flip()
    {
        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
    }

    public void Hit()
    {
        Debug.Log(_health);
        _health -= 1;
        if (_health <= 0)
        {
            _state = State.Death;
        }
    }

    #endregion

}
