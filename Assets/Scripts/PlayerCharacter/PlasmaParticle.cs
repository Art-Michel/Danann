using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlasmaParticle : PooledObject
{
    Transform _destination;
    Vector2 _vDest;
    Vector2 _origin;
    Vector2 _p1;

    [SerializeField] Vector2 _minMaxSpeed;
    float _speed;

    float _t = 0;

    public override void Init(Pooler pooler, Transform player, Transform destination)
    {
        _pooler = pooler;

        _destination = destination;
        transform.SetParent(destination);

        _origin = player.transform.position;

        _speed = Random.Range(_minMaxSpeed.x, _minMaxSpeed.y);

        _t = 0;
    }

    void Update()
    {
        if (_t <= 1)
        {
            _t += Time.deltaTime * _speed;
            transform.position = BezierCurve();
            _vDest = new Vector2(_destination.position.x, _destination.position.y);
        }
    }

    Vector2 BezierCurve()
    {
        float u = 1 - _t;
        float tt = _t * _t;
        float uu = u * u;

        Vector2 point = uu * _origin;
        point += 2 * u * _t * _p1;
        point += tt * _vDest;
        return point;
    }
}