using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _baseCamera;

    [SerializeField]
    private Transform _groupCamera;

    [SerializeField]
    private Transform _player;

    [SerializeField]
    private float _range;

    enum Camera
    {
        Base,
        Group
    }

    private float _distance;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.Base;
    }

    private void Update()
    {
        _distance = Vector3.Distance(new Vector2(_player.position.x, 0), new Vector2(_groupCamera.position.x, 0));
        Debug.Log(_distance);
        switch (_camera)
        {
            case Camera.Base:
                _groupCamera.gameObject.SetActive(false);
                MainCamera();
                break;
            case Camera.Group:
                _baseCamera.gameObject.SetActive(false);
                GroupCamera();
                break;
        }
    }

    private void MainCamera()
    {
        _baseCamera.gameObject.SetActive(true);

        if (_distance < _range)
        {
            _camera = Camera.Group;
        }
    }

    private void GroupCamera()
    {
        _groupCamera.gameObject.SetActive(true);
        
        if (_distance >= _range)
        {
            _camera = Camera.Base;
        }
    }
}
