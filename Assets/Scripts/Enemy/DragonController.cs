using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : Enemy
{
    [SerializeField]
    private DragonBreathController _dragonBreath;

    [SerializeField]
    private FirerainController _fireRain;

    [SerializeField]
    private Transform _dragonBreathSpawner;

    [SerializeField]
    private Transform _fireRainSpawner;

    [SerializeField]
    private int _maxHealth = 100;

    [SerializeField]
    private DragonHealthBar _dragonHealthBar;

    private int _currentHealth;

    private void Start()
    {
        _state = State.Wait;
        _attackTime = _attackDelay;
        _currentHealth = _maxHealth;
        _dragonHealthBar.setMaxHealth(_maxHealth);
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
        _attackDelay -= Time.deltaTime;

        if (_attackDelay <= 0)
        {
            _attackDelay = _attackTime;
        }

        if (_attackDelay == _attackTime)
        {
            var random = Random.Range(0, 11);
            Debug.Log(random);
            if (random > 9)
            {
                ShootDragonBreath();
            }
            else
            {
                StartCoroutine(FireRain());
            }
        }
        
        if (_attackDelay <= 0)
        {
            _attackDelay = _attackTime;
        }
    }

    private void ShootDragonBreath()
    {
        _anim.SetTrigger("Attack");
        var dragonBreath = Instantiate(_dragonBreath, _dragonBreathSpawner.position, Quaternion.identity);
        dragonBreath.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
    }

    IEnumerator FireRain()
    {
        _anim.SetTrigger("Attack");
        for (int i = 0; i < 15; i++)
        {
            yield return new WaitForSeconds(0.2f);
            var x = Random.Range(-10f, 11f);
            var firerain = Instantiate(_fireRain, new Vector2(_fireRainSpawner.position.x + x, _fireRainSpawner.position.y), Quaternion.identity);
            firerain.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
    }

    public void TakeDamage(int damage)
    {
        if (_currentHealth <= 0)
        {
            _anim.SetTrigger("Die");
            return;
        }

        _currentHealth -= damage;
        _dragonHealthBar.setHealth(_currentHealth);
    }

    protected override void Death()
    {
        Destroy(this.gameObject);
        Destroy(this);
        Time.timeScale = 0;
    }

}
