using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 5f;

     [SerializeField] 
    private float health = 2f;

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
                AnimationControl(Animation.Attack);
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

    void DrawRay( Vector3 start, Vector3 dir, Color color )
	{
		Debug.DrawRay( start, dir, color );
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
        RaycastHit2D hit = Physics2D.Raycast(_raycastPosition, _raycastDirection, _attackRange,_layerMask);
        if (hit)
        {
            _move = 0;
            _state = State.Attacking;
        }
    }

    #endregion

    #region Goblin State

    private void Chasing()
    {
        AttackRaycast();

        // Check goblin move with scale
        if (_player.transform.position.x <= transform.position.x)
        {
            var x = _move * _speed * -1;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
            transform.localScale = new Vector3(-2.5f, 2.2f);
        }
        if (_player.transform.position.x >= transform.position.x)
        {
            var x = _move * _speed;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
            transform.localScale = new Vector3(2.5f, 2.2f);
        }
    }

    private void Patrol()
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
            transform.localScale = new Vector3(-2.5f, 2.2f);
        }
        if (transform.localScale.x > 0)
        {
            x = _move * _speed;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
            transform.localScale = new Vector3(2.5f, 2.2f);
        }

        if (Vector3.Distance(transform.position, _pointer[_targetIndex].transform.position) <= .9f)
        {
            _targetIndex = (_targetIndex + 1) % 2;
            _state = State.Waiting;
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
                transform.localScale = new Vector3(2.5f, 2.2f);
            }
            else
            {
                transform.localScale = new Vector3(-2.5f, 2.2f);
            }
            _move = 1;
            _waitingDelay = 2f;
            _state = State.Patrolling;
        }
    }

    private void Attack()
    {
        var distance = Vector3.Distance(_player.position, transform.position);
        
        if (distance >= _attackRange)
        {
            _move = 1;
            _state = State.Chasing;
        }
    }

    #endregion

    public void SetDieAnimation()
    {
        health -= 1;
        if (health == 0)
        {
            _state = State.Death;
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
