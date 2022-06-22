using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlasmaParticle : PooledObject
{
    PlayerPlasma _playerPlasma;
    ParticleSystem _particleSystem;

    RectTransform _destination;
    Transform _playerTransform;
    Vector3 _origin;
    RectTransform _p1;

    float _amount;

    [SerializeField] Vector2 _minMaxSpeed;
    float _speed;
    float _t = 0;

    public override void Init(Pooler pooler, Transform player, RectTransform destination, RectTransform interm, PlayerPlasma playerPlasma)
    {
        _playerPlasma = playerPlasma;
        _particleSystem = GetComponent<ParticleSystem>();
        _playerTransform = player;
        _pooler = pooler;
        _destination = destination;
        _p1 = interm;
        gameObject.SetActive(false);
    }
    
    public void Enable(float amount)
    {
        gameObject.SetActive(true);
        _origin = _playerTransform.position;
        transform.position = _origin;
        _amount = amount;
        _particleSystem.startSize = Mathf.Lerp(0.5f, 1f,Mathf.InverseLerp(1f, 4f, amount));
        _particleSystem.Clear();
        _particleSystem.Play();

        _t = 0;
        _speed = Random.Range(_minMaxSpeed.x, _minMaxSpeed.y);
    }

    void Disable()
    {
        _playerPlasma.ActuallyIncreasePlasma(_amount);
        _pooler.Return(this);
    }

    void Update()
    {
        if (_t <= 1)
        {
            _t += Time.deltaTime * _speed;
            transform.position = BezierCurve();
        }
        else
            Disable();
    }

    Vector3 BezierCurve()
    {
        float u = 1 - _t;
        float tt = _t * _t;
        float uu = u * u;

        Vector3 point = uu * _origin;
        point += 2 * u * _t * Camera.main.ScreenToWorldPoint(new Vector3(_p1.transform.position.x, _p1.transform.position.y, Camera.main.transform.position.y));
        point += tt * Camera.main.ScreenToWorldPoint(new Vector3(_destination.transform.position.x, _destination.transform.position.y, Camera.main.transform.position.y));
        return point;
    }
}