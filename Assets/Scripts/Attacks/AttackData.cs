using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AttackData : MonoBehaviour
{
    [Dropdown("Name")]
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

    [Dropdown("attackName")]
    [SerializeField] protected string _attackName;
    DropdownList<string> attackName()
    {
        return new DropdownList<string>()
        {
        {"Light Attack 1", Ccl_Attacks.LIGHTATTACK1},
        {"Light Attack 2", Ccl_Attacks.LIGHTATTACK2},
        {"Light Attack 3", Ccl_Attacks.LIGHTATTACK3},
        };
    }

    Hitbox[,] _hitboxes;

    void Start()
    {
        _hitboxes = new Hitbox[8, 8];
        int index = 0;
        foreach (Transform child in transform)
        {
            int childIndex = 0;
            foreach (Hitbox hitbox in child.GetComponentsInChildren<Hitbox>())
            {
                _hitboxes[index, childIndex] = hitbox;
                hitbox.Owner = _attackSender;
                hitbox.AttackName = _attackName;
                childIndex++;
            }
            index++;
        }
    }

    [Button]
    public void LaunchAttack(int index = 0)
    {
        for (int i = 0; i < _hitboxes.GetLength(1); i++)
        {
            if (_hitboxes[index, i]) _hitboxes[index, i].CheckForHit();
        }
    }

    public void TellHitboxToTellHurtboxesToResetIds()
    {
        _hitboxes[0,0].MakeHurtboxResetIds();
    }
}
