using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private Player _player;
    private bool _attack;

    void Update()
    {
        _attack = _player.SetAttack();
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.name == "Goblin" && _attack)
        {
            Destroy(other.gameObject);
        }
    }
}
