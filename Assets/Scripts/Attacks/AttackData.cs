using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AttackData : MonoBehaviour
{
    [Dropdown("Name")]
    [SerializeField]protected string _attackSender;
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
    
    Hitbox[,] _hitboxes;

    void Start()
    {
        _hitboxes = new Hitbox[8,8];
        int index = 0;
        foreach (Transform child in transform)
        {
            int childIndex = 0;
            foreach (Hitbox hitbox in child.GetComponentsInChildren<Hitbox>())
            {
                _hitboxes[index, childIndex] = hitbox;
                hitbox.Owner = _attackSender;
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
            if(_hitboxes[index, i])_hitboxes[index, i].CheckIntersection();
        }
    }
}
