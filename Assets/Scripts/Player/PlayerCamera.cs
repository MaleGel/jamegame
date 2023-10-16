using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Vector3 _cameraOffset = new Vector3(0f, 0f, -1f);
    [SerializeField] private float _cameraVelocity = 3f;

    void Awake()
    {
        //_cameraOffset = new Vector3(0f, 0f, -1f);

        if (_camera is null)
            _camera = Camera.main;

        if (_gameObject is null)
            _gameObject = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        var currentCameraPosition = _camera.transform.position;
        Vector3 newCameraPosition = Vector3.Lerp(currentCameraPosition, _gameObject.transform.position, _cameraVelocity * Time.fixedDeltaTime);
        _camera.transform.position = newCameraPosition + _cameraOffset;
    }
}
