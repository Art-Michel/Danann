using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class AttackData : MonoBehaviour
{
    [SerializeField] bool shouldHitEnemies;
    [SerializeField] bool shouldHitAllies;
    public int AttackDamage;
    [SerializeField] Hitbox[] _hitboxes;
    /* [Dropdown("Name")]
    [SerializeField] protected string _attackSender;
    DropdownList<string> Name()
    {
        return new DropdownList<string>()
        {
        {"Cuchulainn", Characters.CCL},
        {"Danu", Characters.DANU},
        {"Left Spear", Characters.SPEARL},
        {"Right Spear", Characters.SPEARR}
        };
    }
*/
    [Dropdown("attackName")]
    [SerializeField] public string AttackName;
    DropdownList<string> attackName()
    {
        return new DropdownList<string>()
        {
        {"Light Attack 1", Ccl_Attacks.LIGHTATTACK1},
        {"Light Attack 2", Ccl_Attacks.LIGHTATTACK2},
        {"Light Attack 3", Ccl_Attacks.LIGHTATTACK3},
        };
    }


    [Button]
    public void LaunchAttack()
    {
        foreach (Hitbox hitbox in _hitboxes)
        {
            if (shouldHitAllies)
            {
                if (shouldHitEnemies) hitbox.Enable(GameManager.Instance.Hurtboxes, this);
                else hitbox.Enable(GameManager.Instance.AllyHurtboxes, this);
            }
            else if (shouldHitEnemies) hitbox.Enable(GameManager.Instance.EnemyHurtboxes, this);
        }
    }

    public void StopAttack()
    {
        foreach (Hitbox hitbox in _hitboxes)
        {
            if (shouldHitAllies)
            {
                if (shouldHitEnemies) hitbox.Disable(GameManager.Instance.Hurtboxes, this);
                else hitbox.Disable(GameManager.Instance.AllyHurtboxes, this);
            }
            else if (shouldHitEnemies) hitbox.Disable(GameManager.Instance.EnemyHurtboxes, this);
        }
    }
}
