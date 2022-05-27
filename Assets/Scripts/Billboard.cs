using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Billboard : PooledObject
{
    Camera _camera;
    [SerializeField]TextMeshPro _text;
    const float _speed = 1f;
    const float _duration = 1f;
    float _t;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        _t += Time.deltaTime;
        transform.rotation = _camera.transform.rotation;
        transform.position += Vector3.up * Time.deltaTime * _speed;
        if (_t > _duration)
            _pooler.Return(gameObject);
    }

    public void Enable(string text)
    {
        _text.text = (text);
        gameObject.SetActive(true);
        transform.localPosition = Vector3.zero;
        _t = 0;
    }
}
