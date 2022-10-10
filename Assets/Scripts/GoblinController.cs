using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
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
            _move = 0;
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

    #region Helper Method

    private void DistractByPlayer()
    {
        // Check goblin move with scale
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

    private void Patrol()
    {
        // Check the goblin have pointers or not
        if (_pointer.Count == 0)
        {
            return;
        }

        float x;

        // Check if goblin have the same x position with the pointer
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

    #endregion

    public void SetDieAnimation()
    {
        _move = 0;
        StartCoroutine(Die());
    }
}
