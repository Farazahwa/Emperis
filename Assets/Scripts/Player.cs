using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private Vector2 _movement;
    void FixedUpdate()
    {
        
    }
    
    void OnMove(InputValue value)
    {
        _movement = value.Get<Vector2>();
        Debug.Log(_movement);
    }
}
