using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
  private int item = 0;
  [SerializeField] private TextMeshProUGUI SkullItem;
  [SerializeField] private TextMeshProUGUI RubyItem;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Item"))
    {
        Destroy(collision.gameObject);
        item++;
        SkullItem.text = ":  " + item;
        RubyItem.text = ": " + item;
     }
  }
}
