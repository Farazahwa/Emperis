using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirePortalController : MonoBehaviour
{
   void OnTriggerEnter2D(Collider2D col)
   {
      Debug.Log("OnTriggerEnter");
      if (col.CompareTag("Player"))
      {
         CompleteLevel();
      }
   }

   private void CompleteLevel()
   {
    SceneManager.LoadScene("Hell Scene");
    Time.timeScale = 1;
   }
}
