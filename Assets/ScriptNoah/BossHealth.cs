using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EntityHP
{
    void Awake()
    {
        _maxHealthPoints = 1000;
    }
    
    override protected IEnumerator PostDamage()
    {
        SoundManager.Instance.PlayBossPunched();
        yield return null;
    }
}
