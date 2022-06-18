using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Billboard : PooledObject
{
    Camera _camera;
    [SerializeField] TextMeshPro _text;
    const float _speed = 2f;
    const float _duration = 1f;
    float _t;
    private Color _cyan = new Color(0f, 1f, 1f);

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
            _pooler.Return(this);
    }

    public void EnableAsDamage(int damage)
    {
        _text.text = (damage.ToString());
        float damageRatio = Mathf.InverseLerp(0, 15, damage);
        _text.color = Color.Lerp(_cyan, Color.red, damageRatio);
        _text.fontSize = Mathf.Lerp(4, 7, damageRatio);
        gameObject.SetActive(true);
        transform.localPosition = Random.insideUnitSphere;
        _t = 0;
    }

    public void EnableAsInfo(string text)
    {
        _text.text = text;
        _text.color = Color.white;
        gameObject.SetActive(true);
        transform.localPosition = Vector3.zero;
        _text.fontSize = 4;
    }

}
