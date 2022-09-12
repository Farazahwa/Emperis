using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit : MonoBehaviour
{
    private bool _distract;
    private void OnTriggerEnter2D (Collider2D other)
    {
        _distract = true;
    }

    public bool SetDistract()
    {
        return _distract;
    }
}
