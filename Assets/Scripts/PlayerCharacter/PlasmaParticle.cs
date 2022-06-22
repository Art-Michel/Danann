using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlasmaParticle : PooledObject
{
    RectTransform _destination;
    Transform _origin;
    RectTransform _p1;

    [SerializeField] Vector2 _minMaxSpeed;
    float _speed;

    float _t = 0;

    public override void Init(Pooler pooler, Transform player, RectTransform destination, RectTransform interm)
    {
        _pooler = pooler;
        _destination = destination;
        _p1 = interm;
        _origin = player;
        gameObject.SetActive(false);
    }
    
    public void Enable()
    {
        gameObject.SetActive(true);
        transform.position = _origin.position;
        //transform.position = Random.insideUnitSphere;
        _t = 0;
        _speed = Random.Range(_minMaxSpeed.x, _minMaxSpeed.y);
    }

    void Disable()
    {
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

        Vector2 point = uu * _origin.position;
        point += 2 * u * _t * new Vector2(Camera.main.ScreenToWorldPoint(_p1.transform.position).x, Camera.main.ScreenToWorldPoint(_p1.transform.position).z);
        point += tt * new Vector2(Camera.main.ScreenToWorldPoint(_destination.transform.position).x, Camera.main.ScreenToWorldPoint(_destination.transform.position).z);
        return point;
    }
}