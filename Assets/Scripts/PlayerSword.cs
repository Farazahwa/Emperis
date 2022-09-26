using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private Goblin _goblin;

    void Start()
    {
        this.gameObject.SetActive(false);
    }

}
