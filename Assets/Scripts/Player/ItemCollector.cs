using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _annoucement;

    void OnTriggerEnter2D(Collider2D collision)
    {
        _annoucement.gameObject.SetActive(true);

        var player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.chest = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _annoucement.gameObject.SetActive(false);

        var player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.chest = false;
        }
    }
}




