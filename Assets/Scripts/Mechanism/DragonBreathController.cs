using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBreathController : MonoBehaviour
{
    enum State
    {
        Falling,
        Explode
    }

    private Vector3 _targetPosition;

    private State _state;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _state = State.Falling;
        _targetPosition = transform.position - new Vector3(10f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.Falling:
                Falling();
                break;
            case State.Explode:
                break;
        }
    }

    private void Falling()
    {
        Debug.Log(Time.deltaTime);
        _rigidbody.velocity -= new Vector2(Time.deltaTime +0.1f, Time.deltaTime);
        if (transform.position == _targetPosition)
        {
            _state = State.Explode;
        }
    }

    private void Explode()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(30, null);
        }
    }

    private void ChangesAnimation()
    {
        _animator.SetBool("Shoot", true);
    }
}
