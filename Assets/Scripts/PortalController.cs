using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [SerializeField]
    private GameObject _targetScene;
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
        switch (_targetScene.tag) 
        {
            case "Forest Scene":
                SceneManager.LoadScene("Cave Scene");
                Time.timeScale = 1;
                break;
            case "Cave Scene":
                SceneManager.LoadScene("Hell Scene");
                Time.timeScale = 1;
                break;
        }

    
   }
}
