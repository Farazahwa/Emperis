using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Goblin"))
        {
            Destroy(other.gameObject);
        }
    }
}
