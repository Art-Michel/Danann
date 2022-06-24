using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    [SerializeField]Transform _camera;

    void Awake()
    {
        if(_camera == null)_camera = Camera.main.transform;
    }

    void Update()
    {
        if (_camera) transform.forward = _camera.position - transform.position;
        //transform.up = _camera.up;
    }
}
