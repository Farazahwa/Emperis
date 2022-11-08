using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
   private void CompleteLevel()
   {
    SceneManager.LoadScene("Cave Scene");
    Time.timeScale = 1;
   }
}
