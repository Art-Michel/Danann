using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    Transform _camera;

    void Awake()
    {
        _camera = Camera.main.transform;
    }

    void Update()
    {
        transform.forward = _camera.position - transform.position;
        //transform.up = _camera.up;
    }
}
