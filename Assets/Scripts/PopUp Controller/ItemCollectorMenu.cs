using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCollectorMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;

    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private PortalController _portal;

    public void PopUp()
    {
        gameObject.SetActive(true);
    }

    private void Cancel()
    {
        gameObject.SetActive(false);
    }

    private void Collect()
    {
        _player.ruby = 0;
        _player.skull = 0;
        _portal.gameObject.SetActive(true);
    }
}
