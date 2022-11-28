using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : Enemy
{
    [SerializeField]
    private GameObject _dragonBreath;

    [SerializeField]
    private FirerainController _fireRain;

    [SerializeField]
    private Transform _dragonBreathSpawner;

    [SerializeField]
    private Transform _fireRainSpawner;

    private void Start()
    {
        _state = State.Wait;
        _attackTime = _attackDelay;
    }

    void Update()
    {
        switch (_state)
        {
            case State.Wait:
                PlayerDetection();
                _anim.SetBool("Idle", true);
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    protected override void PlayerDetection()
    {
        _raycastPosition = transform.position - new Vector3(0, 3.2f);
        _raycastDirection = -transform.right;


        DrawRay(_raycastPosition, _raycastDirection * _detectRange, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(_raycastPosition, _raycastDirection, _detectRange, _layerMask);
        if (hit)
        {
            _player = hit.transform;
            _state = State.Attack;
        }
    }

    protected override void Attack()
    {
        if (_attackDelay == _attackTime)
        {
            var random = Random.Range(0, 11);
            Debug.Log(random);
            if (random == 10)
            {
                ShootDragonBreath();
            }
            else
            {
                StartCoroutine(FireRain());
            }
        }

        _attackDelay -= Time.deltaTime;
        if (_attackDelay <= 0)
        {
            _attackDelay = _attackTime;
        }
    }

    private void ShootDragonBreath()
    {
        Instantiate(_dragonBreath, _dragonBreathSpawner.position, Quaternion.identity);
    }

    IEnumerator FireRain()
    {
        for (int i = 0; i < 15; i++)
        {
            yield return new WaitForSeconds(0.2f);
            var x = Random.Range(-10f, 11f);
            var firerain = Instantiate(_fireRain, new Vector2(transform.position.x + x, transform.position.y), Quaternion.identity);
            firerain.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
    }
}
