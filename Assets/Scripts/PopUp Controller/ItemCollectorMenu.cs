using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCollectorMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;

    [SerializeField]
    private TextMeshProUGUI _rubyText;

    [SerializeField]
    private TextMeshProUGUI _skullText;

    [SerializeField]
    private GameObject _portal;

    public void PopUp()
    {
        _rubyText.text = $"{_player.ruby}/10";
        _skullText.text = $"{_player.skull}/10";
        gameObject.SetActive(true);
        
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void Collect()
    {
        if (_player.ruby >= 10 & _player.skull >= 10)
        {
            _player.ruby = 0;
            _player.skull = 0;
            _portal.SetActive(true);
            gameObject.SetActive(false);
        }
        else {
            return;
        }
        Time.timeScale = 1;
    }
}
