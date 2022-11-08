using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KingGoblinController : Enemy
{
    protected override void Attack()
    {
        AnimationControl(Animation.Idle);
        var distance = Vector3.Distance(_player.position, transform.position);
        _attackDelay -= Time.deltaTime;
        Debug.Log($"{distance}, {_attackRange}");

        if (_attackDelay <= 0)
        {
            AnimationControl(Animation.Attack);
            _attackDelay = _attackTime;
        }

        if (distance > 2.1f)
        {
            _move = 1;
            _state = State.Chasing;
        }
    }
}