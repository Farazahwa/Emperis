using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
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
        transform.position = new Vector3(
            _player.transform.position.x + _offset.x,
            transform.position.y,
            transform.position.z);
    }
}
