using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    [SerializeField]
    private ItemCollectorMenu _itemCollector;


    private void OnTriggerEnter2D(Collider2D collision)
    {
    var player = collision.GetComponent <PlayerController> ();
        if (player !=null) 
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                _itemCollector.PopUp();
            }
        }
    }
}




