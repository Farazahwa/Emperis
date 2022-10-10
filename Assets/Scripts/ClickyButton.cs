using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Interface to implement if you wish to receive OnPointerDown callbacks.
//Detects ongoing mouse clicks until release of the mouse button. Use IPointerUpHandler to handle the release of the mouse button.

//Antarmuka untuk diterapkan jika Anda ingin menerima panggilan balik OnPointerDown.
//Mendeteksi klik mouse yang sedang berlangsung hingga tombol mouse dilepaskan. 
//Gunakan IPointerUpHandler untuk menangani pelepasan tombol mouse.

public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default, _pressed;
    [SerializeField] private AudioClip _compressClip, _uncompressClip;
    [SerializeField] private AudioSource _source;
    private MainMenu _mainmenu;
    void Awake()
    {
        _mainmenu = new MainMenu();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _img.sprite = _pressed;
        _source.PlayOneShot(_compressClip);   
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _img.sprite = _default;
        _source.PlayOneShot(_uncompressClip);
    }

    public void WasClicked()
    {
        Debug.Log("Click!");
        _mainmenu.LoadStart("Game Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit");
    }

    public void sound_volume(float volume)
    {
        PlayerPrefs.SetFloat("volume",volume);
    }
}
