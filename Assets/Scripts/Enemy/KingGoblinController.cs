using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KingGoblinController : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 5f;

    [SerializeField]
    private float _attackRange = 1f;

    [SerializeField]
    private float _distractRange = 1.5f;

    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private List<GameObject> _pointer;

    [SerializeField]
    private float _waitingDelay = 2f;

    enum State
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

    private float _move = 1f;

    private int _targetIndex = 0;

    private Rigidbody2D _rb;
    private Animator _anim;
    private Transform _player;

    private Vector3 _raycastPosition;
    private Vector3 _raycastDirection;

    private State _state;

    #endregion

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _state = State.Patrolling;

    }

    void FixedUpdate()
    {
        PlayerDetection();
        AttackRaycast();

        switch (_state)
        {
            case State.Patrolling:
                AnimationControl(Animation.Move);
                Patrol();
                break;
            case State.Chasing:
                AnimationControl(Animation.Move);
                Chasing();
                break;
            case State.Waiting:
                AnimationControl(Animation.Idle);
                Waiting();
                break;
            case State.Attacking:
                AnimationControl(Animation.Attack);
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
            Debug.Log("Mati");
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
        _raycastPosition = transform.position - new Vector3(0, 0.3f, 0);
        if (transform.localScale.x > 0) _raycastDirection = transform.right;
        if (transform.localScale.x < 0) _raycastDirection = -transform.right;

        DrawRay(_raycastPosition, _raycastDirection * _distractRange, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(_raycastPosition, _raycastDirection, _distractRange, _layerMask);
        if (hit)
        {
            _state = State.Chasing;
            _player = hit.transform;
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

    #region King Goblin State

    private void Patrol()
    {
        float x;
        if (transform.localScale.x < 0)
        {   
            x = _move * _speed * -1;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
            transform.localScale = new Vector3(-2.6f, 2.6f);
        }

        if (transform.localScale.x > 0)
        {
            x = _move * _speed;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
            transform.localScale = new Vector3(2.6f, 2.6f); 
        }

        if (Vector3.Distance(transform.position, _pointer[_targetIndex].transform.position) <= .9f)
        {
            _targetIndex = (_targetIndex + 1) % _pointer.Count;
            _state = State.Waiting;
        }
    }

    private void Chasing()
    {
        if (_player.transform.position.x < transform.position.x)
        {
            var x = _move * _speed * -1;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
            transform.localScale = new Vector3(-2.6f, 2.6f);
        }
        if (_player.transform.position.x > transform.position.x)
        {
            var x = _move * _speed;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
            transform.localScale = new Vector3(2.6f, 2.6f);
        }
    }

    private void Waiting()
    {
        _move = 0;
        _waitingDelay -= Time.deltaTime;
        if (_waitingDelay <= 0)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(2.6f, 2.6f);
            }
            else
            {
                transform.localScale = new Vector3(-2.6f, 2.6f);
            }
            _move = 1;
            _waitingDelay = 2f;
            _state = State.Patrolling;
        }
    }

    #endregion

    #region Etc

    private void AttackPlayer()
    {
        Debug.Log("Player Get Hit by King Goblin");
    }

    #endregion
}