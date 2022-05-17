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
    [SerializeField] Hitbox[] _hitboxes;

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
        if (_hitboxes.Length > 0) SetupHitboxes();
    }

    public void GetChildrenHitboxes()
    {
        _hitboxes = new Hitbox[transform.childCount];
        int j = 0;
        for(int i = 0; i < transform.childCount; i++)
        {
            Hitbox hitbox;
            transform.GetChild(i).TryGetComponent<Hitbox>(out hitbox);
            if (hitbox)
            {
                _hitboxes[j] = hitbox;
                j++;
            }
        }
    }

    public void SetupHitboxes()
    {
        foreach (Hitbox hitbox in _hitboxes)
        {
            if (shouldHitAllies)
            {
                if (shouldHitEnemies) hitbox.Setup(GameManager.Instance.Hurtboxes, _attackName, _attackDamage);
                else hitbox.Setup(GameManager.Instance.AllyHurtboxes, _attackName, _attackDamage);
            }
            else if (shouldHitEnemies) hitbox.Setup(GameManager.Instance.EnemyHurtboxes, _attackName, _attackDamage);
            else Debug.LogError("Forgot to tell if hitboxes should hit allies and/or enemies.");
        }
    }

    [Button]
    public void LaunchAttack()
    {
        foreach (Hitbox hitbox in _hitboxes)
        {
            hitbox.Enable();
        }
    }

    public void StopAttack()
    {
        foreach (Hitbox hitbox in _hitboxes)
            hitbox.Disable();

        if (shouldHitAllies)
        {
            if (shouldHitEnemies)
                foreach (Hurtbox hurtbox in GameManager.Instance.Hurtboxes)
                    hurtbox.ForgetAttack(_attackName);
            else
                foreach (Hurtbox hurtbox in GameManager.Instance.AllyHurtboxes)
                    hurtbox.ForgetAttack(_attackName);
        }
        else if (shouldHitEnemies)
            foreach (Hurtbox hurtbox in GameManager.Instance.EnemyHurtboxes)
                hurtbox.ForgetAttack(_attackName);
    }
}
