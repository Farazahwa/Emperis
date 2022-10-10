using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchController : MonoBehaviour
{

    [SerializeField]
    private float _distractRange;

    [SerializeField]
    private float _attackRange;

    [SerializeField]
    private GameObject _player;

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

    void FixedUpdate()
    {
        PlayerDetection();
        AttackRaycast();

        if (_distract)
        {
            if (_player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-2.6f, 2.6f);
            }
            if (_player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(2.6f, 2.6f);
            }
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
        _raycastPosition = transform.position - new Vector3(0, 0.3f, 0);
        if (transform.localScale.x < 0) _raycastDirection = transform.right;
        if (transform.localScale.x > 0) _raycastDirection = -transform.right;

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
        if (transform.localScale.x < 0) _raycastDirection = transform.right;
        if (transform.localScale.x > 0) _raycastDirection = -transform.right;


        DrawRay(_raycastPosition, _raycastDirection * _attackRange, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(_raycastPosition, _raycastDirection, _attackRange, _layerMask);
        if (hit)
        {
            _anim.SetTrigger("Attack");
        }
    }

    #endregion
}
