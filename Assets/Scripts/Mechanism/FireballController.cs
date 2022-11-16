using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    enum State
    {
        Shoot,
        Destroy
    }

    public WitchController witch;

    private Rigidbody2D _ridigbody;
    private Animator _animator;

    private State _state;

    void Awake()
    {
        _ridigbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _state = State.Shoot;
    }

    void Update()
    {
        switch (_state)
        {
            case State.Shoot:
                ShootMode();
                break;
            case State.Destroy:
                Destroy(this.gameObject);
                break;
        }
    }

    /// <summary>
    /// Method called when fireball has been shooted by Witch
    /// </summary>
    private void ShootMode()
    {
        Flip();
        _ridigbody.velocity = new Vector2(_speed * transform.localScale.x, _ridigbody.velocity.y);
    }

    /// <summary>
    /// Flip the fireball with changes scale
    /// </summary>
    private void Flip()
    {
        if (witch.gameObject.transform.localScale.x > 0 & transform.localScale.x < 0 || 
            witch.gameObject.transform.localScale.x < 0 & transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
        }
    }

    /// <summary>
    /// Changes animation from animation fireball_show to animation fireball_shoot
    /// </summary>
    private void AnimationChanged()
    {
        _animator.SetBool("Shoot", true);
    }

    /// <summary>
    /// Check game object trigger the fireball collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            _state = State.Destroy;
            player.TakeDamage(witch.damage, null);
        }
    }
}
