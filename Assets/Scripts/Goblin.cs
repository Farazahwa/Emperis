using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 5f;

    [SerializeField] 
    private GameObject _player;

    [SerializeField] 
    private float _detectRange;

    [SerializeField] 
    private float _attackRange;

    [SerializeField] 
    private LayerMask _layerMask;

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

    void FixedUpdate()
    {
        PlayerDetection();
        AttackRaycast();
        
        if (_distract)
        {
            if (_player.transform.position.x < transform.position.x)
            {
                var x = _move * _speed * -1;
                _rb.velocity = new Vector3(x, _rb.velocity.y);
                transform.localScale = new Vector3(-2.5f, 2.2f);
            }
            if (_player.transform.position.x > transform.position.x)
            {
                var x = _move * _speed;
                _rb.velocity = new Vector3(x, _rb.velocity.y);
                transform.localScale = new Vector3(2.5f, 2.2f);
            }
            _distract = true;
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
            _distract = true;
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

    public void SetDieAnimation()
    {
        _move = 0;
        StartCoroutine(Die());
    }
}
