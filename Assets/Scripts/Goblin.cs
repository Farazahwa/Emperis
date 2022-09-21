using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Limit _limit;
    [SerializeField] private GameObject _player;
    
    public bool died;
    private bool _distract = false;
    private Rigidbody2D _rb;
    private Animator _anim;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Set distract from Limit script
        _distract = _limit.SetDistract();

        if (_distract)
        {
            if (_player.transform.position.x < transform.position.x)
            {
                var x = _speed * -1;
                _rb.velocity = new Vector3(x, _rb.velocity.y);
                transform.localScale = new Vector3(-2.5f, 2.2f);
                _distract = true;
            }
            if (_player.transform.position.x > transform.position.x)
            {
                _rb.velocity = new Vector3(_speed, _rb.velocity.y);
                transform.localScale = new Vector3(2.5f, 2.2f);
                _distract = true;
            }
        }

        if (died)
        {
            _anim.SetTrigger("Die");
        }
    }
}
