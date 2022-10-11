using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class LoadScene : MonoBehaviour
{
    public GameObject LoadingScreen;

    public Slider slider;

    public TextMeshProUGUI progressText;

    public void LoadLevel(int sceneIndex)
    {
       StartCoroutine(LoadAysnchronously(sceneIndex));
    }

    IEnumerator LoadAysnchronously(int sceneIndex)
    {
        //LoadSceneAsync
        //keeps our current scene and all of the behaviors in it running
        //while it's loading our new scene into memory. 
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        LoadingScreen.SetActive(true);
        //will continue running as long as operation
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            
            slider.value = progress;
            progressText.text = progress * 100f + "%";

            //wait until the next frame
            yield return null;
        }
    }
}
