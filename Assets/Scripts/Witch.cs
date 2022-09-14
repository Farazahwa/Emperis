using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    [SerializeField] private Limit _limit;
    private bool _distract = false;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Set distract from Limit script
        _distract = _limit.SetDistract();

        if (_distract)
        {
            _distract = true;
        }
    }
}
