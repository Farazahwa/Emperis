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

  void Start(){
      SkullItem.text = " " + skull;
      RubyItem.text = " " + ruby;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Skull"))
    {
        Destroy(collision.gameObject);
        skull++;
        SkullItem.text = " " + skull;
     }

      if (collision.gameObject.CompareTag("Ruby"))
    {
        Destroy(collision.gameObject);
        ruby++;
        RubyItem.text = " " + ruby;
     }
  }
}
