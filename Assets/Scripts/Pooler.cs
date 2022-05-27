using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    Queue<GameObject> _prefabs;
    int _initialCount = 10;

    void Awake()
    {
        _prefabs = new Queue<GameObject>();
    }

    void Start()
    {
        for (int i = 0; i < _initialCount; i++)
        {
            _prefabs.Enqueue(Create());
        }
    }

    public GameObject Get()
    {
        if (_prefabs.Count > 0)
            return _prefabs.Dequeue();
        else
            return Create();
    }

    public void Return(GameObject obj)
    {
        _prefabs.Enqueue(obj);
        obj.SetActive(false);
    }

    GameObject Create()
    {
        PooledObject obj = Instantiate(_prefab).GetComponent<PooledObject>();
        obj.Init(this);
        return obj.gameObject;
    }
}
