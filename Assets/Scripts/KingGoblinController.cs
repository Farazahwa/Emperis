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
    private bool _distract = false;
    private Rigidbody2D _rb;
    private Animator _anim;

    private Vector3 _raycastPosition;
    private Vector3 _raycastDirection;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        PlayerDetection();
        AttackRaycast();



        if (_distract)
        {
            DistractByPlayer();
        }
        else
        {
            Patrol();
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
            _move = 1;
            _distract = true;
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
            _anim.SetTrigger("Attack");
        }
    }

    #endregion

    #region Helper Method

    private void Patrol()
    {
        float x;
        if (transform.position.x > _pointer[1].transform.position.x || transform.localScale.x < 0)
        {
            x = _move * _speed * -1;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
            transform.localScale = new Vector3(-2.6f, 2.6f);
        }

        if (transform.position.x < _pointer[0].transform.position.x || transform.localScale.x > 0)
        {
            x = _move * _speed;
            _rb.velocity = new Vector3(x, _rb.velocity.y);
            transform.localScale = new Vector3(2.6f, 2.6f); 
        }
    }

    private void DistractByPlayer()
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
        _distract = true;
    }

    private void Move()
    {

    }
    #endregion


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
}