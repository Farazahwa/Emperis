using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit : MonoBehaviour
{
    private bool _distract = false;

    private void OnTriggerEnter2D (Collider2D other)
    {        
        if (other.gameObject.CompareTag("Player"))
        {
            _distract = true;
        }
    }

    // Method to set distract variable
    // in other script
    public bool SetDistract()
    {
        return _distract;
    }
}
