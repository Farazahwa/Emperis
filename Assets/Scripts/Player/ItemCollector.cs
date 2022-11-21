using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    private int skull = 0;
    private int ruby = 0;
    [SerializeField] private TextMeshProUGUI SkullItem;
    [SerializeField] private TextMeshProUGUI RubyItem;


    private void OnTriggerEnter2D(Collider2D collision)
    {
    var player = collision.GetComponent <PlayerController> ();
        if (player !=null) 
        {

        }
    }
}




