using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 10f;

    [SerializeField] 
    private float _jump = 10f;

    [SerializeField] 
    private float _attackRange;

    [SerializeField]
    private float _knockbackStrength;

    [SerializeField]
    private List<LayerMask> _layerMask;

    [SerializeField]
    private int _maxHealth = 100;

    [SerializeField]
    private PlayerHealthBar _playerHealthBar;

    [SerializeField]
    private TextMeshProUGUI _skullText;

    [SerializeField]
    private TextMeshProUGUI _rubyText;

    [SerializeField]
    private ItemCollectorMenu _menuItemCollecter;

    [SerializeField]
    private GameOverScreen GameOverScreen;

    private float _movement;
    private bool _attack;
    private bool _grounded = false;

    private int _currentHealth;

    public int skull = 0;
    public int ruby = 0;
    public bool chest = false;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Transform _enemyTransform;

    public float thrust = 1.0f;

    public UnityEvent OnBegin, OnDone;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
        _playerHealthBar.setMaxHealth(_maxHealth);
    }

    void FixedUpdate()
    {
        AttackRaycast();    

        var move = _movement * _speed;
        _rigidbody.velocity = new Vector3(move, _rigidbody.velocity.y);

        
        _animator.SetFloat("Move", Mathf.Abs(move));

        // Change Scale
        if (_movement < 0)
        {
            transform.localScale = new Vector3(-2.5f, 2.2f, 1f);
        }
        else if (_movement == 0 && transform.lossyScale.x < 0)
        {
            transform.localScale = new Vector3(-2.5f, 2.2f, 1f);
        }
        else if (_movement == 0 && transform.lossyScale.x > 0)
        {
            transform.localScale = new Vector3(2.5f, 2.2f, 1f);
        }
        else 
        {
            transform.localScale = new Vector3(2.5f, 2.2f, 1f);
        }
    }

    public void TakeDamage(int damage, Transform enemyTransform)
    {
        if (enemyTransform != null)
        {
            _enemyTransform = enemyTransform;
        }
        _currentHealth -= damage;
        _playerHealthBar.setHealth(_currentHealth);
        
        if (_currentHealth <= 0)
        {
            _animator.SetTrigger("Die");
            Time.timeScale = 0;
            GameOver();
        }

    }

    public void GameOver()
    {
        GameOverScreen.setup();
    }

    #region Input System Controller

    // Input System Move
    void OnMove(InputValue value)
    {

        _movement = value.Get<float>();
    }

    // Input System Jump
    void OnJump(InputValue value)
    {
        RaycastJump();

        if (_grounded)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jump);
            _grounded = false;
        }
    }

    void OnAttack(InputValue value)
    {
        var input = value.Get<float>();
        if (input == 1)
        {
            _attack = true;
            _animator.SetTrigger("Attack");
        }
        else
        {
            _attack = false;
        }
    }

    void OnTest(InputValue value)
    {
        TakeDamage(20, null);
    }

    void OnOpenChest(InputValue value)
    {
        if (chest)
        {
            Time.timeScale = 0;
            _menuItemCollecter.PopUp();
        }
    }

    #endregion

    #region Helper Method

    void Death()
    {
        Destroy(this.gameObject);
    }

    #endregion

    /// <summary>
    /// Collision with other game object
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Wall"))
        {
            _movement = 0;
        }

        if (other.gameObject.CompareTag("Trap"))
        {
            TakeDamage(7, null);
        }

        if (other.gameObject.CompareTag("Lava"))
        {
            TakeDamage(100, null);
        }

        if (other.gameObject.CompareTag("Ruby"))
        {
            Destroy(other.gameObject);
            ruby++;
            _rubyText.text = " " + ruby;
        }

        if (other.gameObject.CompareTag("Skull"))
        {
            Destroy(other.gameObject);
            skull++;
            _skullText.text = " " + skull;
        }
    }

    #region Raycast

    void DrawRay( Vector3 start, Vector3 dir, Color color )
	{
		Debug.DrawRay( start, dir, color );
	}


    private void AttackRaycast()
    {
        var pos = transform.position;
        var dir = transform.right;
        if (_movement > 0) dir = transform.right;
        if (_movement < 0) dir = -transform.right;

        DrawRay(pos, dir * _attackRange, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, _attackRange, _layerMask[0]);
        if (hit)
        {
            var enemy = hit.collider.gameObject;

            if (_attack)
            {
                switch (enemy.tag)
                {
                    case "Goblin":
                        var goblin = enemy.GetComponent<GoblinController>();
                        goblin.Hitted();
                        break;
                    case "King Goblin":
                        var kingGoblin = enemy.GetComponent<KingGoblinController>();
                        kingGoblin.Hitted();
                        break;
                    case "Witch":
                        var witch = enemy.GetComponent<WitchController>();
                        witch.Hitted();
                        break;
                    default:
                        break;
                }

                _attack = false;
            }
        }
    }

    private void RaycastJump()
    {
        var pos = transform.position;
        var dir = -transform.up;

        DrawRay(pos, dir * 2, Color.white);
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, 2, _layerMask[1]);
        if (hit)
        {
            if (hit.collider.gameObject.CompareTag("Grounded"))
            {
                _grounded = true;
            }
            else
            {
                _grounded = false;
            }   
        }
    }

    #endregion

    private void Knockback()
    {
        if (transform.position.x > _enemyTransform.position.x)
        {
            Debug.Log("Enemy Left");
            _rigidbody.AddForce(transform.right * 35f, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Enemy Right");
            _rigidbody.AddForce(transform.right * -35f, ForceMode2D.Impulse);
        }
    }

}
