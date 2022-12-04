using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void setup()
    {
        gameObject.SetActive(true);
    }

    public void Menu()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
    public void Restart()
    {
        SceneManager.LoadScene("Cave Scene");
        Time.timeScale = 1;
    }
}
