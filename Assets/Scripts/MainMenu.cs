using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Debug.Log("LoadSceneStart");
    }

    public void LoadStart(string scenename)
    {
        Debug.Log("scenename to load : " + scenename);
        SceneManager.LoadScene(scenename);
    }
}
