using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class AttackData : MonoBehaviour
{
    [SerializeField] bool shouldHitEnemies;
    [SerializeField] bool shouldHitAllies;
    [SerializeField] int _attackDamage;
    Hitbox[] _hitboxes;

    [Dropdown("attackName")]
    [SerializeField] string _attackName;
    DropdownList<string> attackName()
    {
        return new DropdownList<string>()
        {
        {"Light Attack 1", Ccl_Attacks.LIGHTATTACK1},
        {"Light Attack 2", Ccl_Attacks.LIGHTATTACK2},
        {"Light Attack 3", Ccl_Attacks.LIGHTATTACK3},
        {"Energy Ball", Danu_Attacks.PROJECTILE},
        {"Dash 1", Danu_Attacks.DASH},
        {"Dash 2", Danu_Attacks.DASH2},
        {"Shockwave", Danu_Attacks.SHOCKWAVE},
        {"Slam 1", Danu_Attacks.SLAM1},
        {"Slam 2", Danu_Attacks.SLAM2},
        {"Slam 3", Danu_Attacks.SLAM3},
        {"Teleportation Explosion", Danu_Attacks.TP},
        };
    }

    void Start()
    {
        _hitboxes = new Hitbox[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _hitboxes[i] = transform.GetChild(i).GetComponent<Hitbox>();
        }
    }

    [Button]
    public void LaunchAttack()
    {
        foreach (Hitbox hitbox in _hitboxes)
        {
            if (shouldHitAllies)
            {
                if (shouldHitEnemies) hitbox.Enable(GameManager.Instance.Hurtboxes, _attackName,_attackDamage);
                else hitbox.Enable(GameManager.Instance.AllyHurtboxes, _attackName, _attackDamage);
            }
            else if (shouldHitEnemies) hitbox.Enable(GameManager.Instance.EnemyHurtboxes,   _attackName, _attackDamage);
            else Debug.LogError("This hitbox has no hurtbox to check");
        }
    }

    public void StopAttack()
    {
        foreach (Hitbox hitbox in _hitboxes)
        {
            if (shouldHitAllies)
            {
                if (shouldHitEnemies) hitbox.Disable(GameManager.Instance.Hurtboxes, _attackName);
                else hitbox.Disable(GameManager.Instance.AllyHurtboxes,  _attackName);
            }
            else if (shouldHitEnemies) hitbox.Disable(GameManager.Instance.EnemyHurtboxes, _attackName);
        }
    }
}
