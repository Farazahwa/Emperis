using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirerainController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(2, null);
        }
    }

    /// <summary>
    /// Changes animation from animation firerain_show to animation firerain_shoot
    /// </summary>
    private void AnimationChanged()
    {
        _animator.SetBool("Shoot", true);
    }
}
