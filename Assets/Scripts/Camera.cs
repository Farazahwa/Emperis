using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private Vector3 _offset;
    private bool _grounded;
    private Player _playerClass;

    void Start()
    {
        _offset = transform.position - _player.transform.position;
        _playerClass = new Player();
        _grounded = _playerClass.SetGround();
    }

    void Update()
    {
        _grounded = _playerClass.SetGround();
    }

    void LateUpdate()
    {
        if (_grounded)
        {
            transform.position = _player.transform.position + _offset;
        }    
    }
}
