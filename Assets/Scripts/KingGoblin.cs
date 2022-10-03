using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingGoblin : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 5f;

    [SerializeField]
    private float _attackRange = 1f;

    [SerializeField]
    private float _distractRange = 1.5f;

    [SerializeField]
    private LayerMask _layerMask;

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

    private void Start()
    {
        transform.localScale= new Vector3(transform.localScale.x * -1, transform.localScale.y);
        
    }

    void FixedUpdate()
    {
        PlayerDetection();


        if (_distract)
        {
            var x = _speed * -1;
            _rb.velocity = new Vector3(x, transform.position.y);
            _distract = true;
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

        DrawRay(_raycastPosition, _raycastDirection * _distractRange, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(_raycastPosition, _raycastDirection, _distractRange, _layerMask);
        if (hit)
        {
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
            _anim.SetTrigger("Attack");
        }
    }

    #endregion

    IEnumerator Die()
    {
        _anim.SetTrigger("Die");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}