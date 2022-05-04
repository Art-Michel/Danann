using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AttackData : MonoBehaviour
{
    public const string CCL = "Ccl";
    public const string DANU = "Danu";
    public const string SPEARL = "SpearL";
    public const string SPEARR = "SpearR";

    public string AttackId { get; protected set; }
    public string Sender { get; protected set; }
    public float DamageValue { get { return damageValue; } private set { damageValue = value; } }
    [SerializeField] private float damageValue = 0;

    Hitbox[,] _hitboxes;

    void Start()
    {
        _hitboxes = new Hitbox[4,4];
        int index = 0;
        foreach (Transform child in transform)
        {
            int childIndex = 0;
            foreach (Hitbox hitbox in child.GetComponentsInChildren<Hitbox>())
            {
                _hitboxes[index, childIndex] = hitbox;
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
