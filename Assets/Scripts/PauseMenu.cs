using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;

    // Update is called once per frame

    public void Resume()
    {
        pauseMenu.SetActive(false);
         Time.timeScale = 1f;
    }
    public void Pause()
    {
         pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
