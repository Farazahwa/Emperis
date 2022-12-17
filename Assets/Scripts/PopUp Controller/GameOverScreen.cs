using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _targetScene;
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
        if (_targetScene.tag == "Forest Scene")
        {
            SceneManager.LoadScene("Game Sceme");
        }

        if (_targetScene.tag == "Cave Scene")
        {
            SceneManager.LoadScene("Cave Scene");
        }
        
        if (_targetScene.tag == "Hell Scene")
        {
            SceneManager.LoadScene("Hell Scene");
        }
        Time.timeScale = 1;
    }
}
