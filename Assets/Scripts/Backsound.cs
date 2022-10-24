using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backsound : MonoBehaviour
{
    void Start()
    {
        if (GameObject.Find("backsound on")==null)
        {
            GetComponent<AudioSource>().Play();
            gameObject.name = "backsound on";
            PlayerPrefs.SetFloat("volume", 1);
        }
    }
    void Update()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");

    }
}
