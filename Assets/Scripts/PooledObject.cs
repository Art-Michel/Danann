using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    protected Pooler _pooler;

    public void Init(Pooler pooler)
    {
        _pooler = pooler;
        transform.parent = pooler.transform;
    }
}
