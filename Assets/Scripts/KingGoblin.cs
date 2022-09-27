using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingGoblin : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 5f;

    private bool _distract = false;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_distract)
        {
            var x = _speed * -1;
            _rb.velocity = new Vector3(x, transform.position.y);
            _distract = true;
        }
    }


}
