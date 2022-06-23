using System.Collections.Generic;
using UnityEngine;

public class MShower : MonoBehaviour
{
    Pool _meteorPool;
    [SerializeField] int _meteorCount;
    int _index;
    float _timer;
    float _maxTime;
    [SerializeField] float _maxInterval;
    [SerializeField] float _minInterval;
    bool _launch;
    [SerializeField] Vector3 _nextTarget;
    [SerializeField] Vector3 _arenaCenter;
    [SerializeField] Vector3 _meteorFrames;
    [SerializeField] float _radius;
    [SerializeField] int _damage;
    [SerializeField] Transform player;

    // Start is called before the first frame update
    void Start()
    {
        _meteorPool=GetComponent<Pool>();
        _index=_meteorCount;
        Setup();
    }

    private void Spawn()
    {
        GameObject go=_meteorPool.Get();
        go.SetActive(true);
        go.transform.position=_nextTarget;
        Meteor meteor=go.GetComponent<Meteor>();
        meteor.SetUp();
        meteor.SetDamage(_damage);
        meteor.SetFrames(_meteorFrames);
        meteor.SetOrig(_meteorPool);
        Setup();
    }
    private void Setup()
    {
        _timer=0;
        _maxTime=Random.Range(_minInterval,_maxInterval);
        Vector2 rand;
        if (_index%5==0)
        rand=Vector2.zero;
        else
        rand= Random.insideUnitCircle*_radius;
        Vector3 realRand=new Vector3(rand.x,0,rand.y);
        _nextTarget=player.position+realRand;
    }

    // Update is called once per frame
    void Update()
    {
        _timer+=Time.deltaTime;
        if (_timer>=_maxTime)
        {
            if (_index==1)
            {
                _index--;
                Spawn();
            }
            else
            {
                _index-=2;
                Spawn();
                Spawn();
            }
            if (_index==0){this.enabled=false;}
        }        
    }
}
