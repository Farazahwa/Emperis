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
        _rigidbody.velocity -= new Vector2(1, 1);
        if (transform.position == _targetPosition)
        {
            Destroy(this.gameObject);
        }
    }

    private void ChangesAnimation()
    {
        _animator.SetBool("Shoot", true);
    }
}
