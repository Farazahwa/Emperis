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
    private GameObject _player;

    [SerializeField]
    private List<GameObject> _pointer;

    private float _move = 1f;
    private Rigidbody2D _rb;
    private Animator _anim;
    private Vector3 _raycastPosition;
    private Vector3 _raycastDirection;

    enum State
    {
        Waiting,
        Patrolling,
        Chasing,
        Attacking
    }
    enum Animation
    {
        Idle,
        Move,
        Attack
    }

    State state;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        state = State.Patrolling;
    }

    void FixedUpdate()
    {
        PlayerDetection();
        AttackRaycast();

        switch (state)
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
                StartCoroutine(Waiting());
                break;
            case State.Attacking:
                AnimationControl(Animation.Attack);
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
            state = State.Chasing;
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
            state = State.Attacking;
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

        if (transform.position.x < _pointer[0].transform.position.x ||
            transform.position.x > _pointer[1].transform.position.x)
        {
            state = State.Waiting;
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

    IEnumerator Waiting()
    {
        _move = 0;
        yield return new WaitForSeconds(2);

        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(2.6f, 2.6f);
        }

        if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-2.6f, 2.6f);
        }
        _move = 1;
        state = State.Patrolling;
    }

    #endregion

    #region Etc

    IEnumerator Die()
    {
        _anim.SetTrigger("Die");
        yield return new WaitForSeconds(_anim.speed);
        Destroy(gameObject);
    }

    public void SetDieAnimation()
    {
        StartCoroutine(Die());
    }

    private void AttackPlayer()
    {
        Debug.Log("Player Get Hit by King Goblin");
    }

    #endregion
}