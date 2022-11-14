using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchController : Enemy
{
    [SerializeField]
    private FireballController _fireball;

    protected override float Tolerance => .4f;

    private void ShootFireball()
    {
        Vector3 fireballPosition;
        if (transform.localScale.x > 0)
        {
            fireballPosition = transform.position + new Vector3(.5f, .1f);
        }
        else
        {
            fireballPosition = transform.position + new Vector3(-.5f, .1f);
        }
        var fireball = Instantiate(_fireball, fireballPosition, Quaternion.identity);
        fireball.witch = this;
    }
}
