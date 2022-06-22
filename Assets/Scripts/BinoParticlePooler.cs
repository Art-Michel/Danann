using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinoParticlePooler : Pooler
{
    [SerializeField] RectTransform _plasmaGauge;
    [SerializeField] RectTransform _interm;
    [SerializeField] Transform _player;
    [SerializeField] PlayerPlasma _playerPlasma;
    
    protected override PooledObject Create()
    {
        GameObject obj = Instantiate(_prefab);
        PooledObject pooled = obj.GetComponent<PooledObject>();
        pooled.Init(this, _player.transform, _plasmaGauge, _interm, _playerPlasma);
        return pooled;
    }
}
