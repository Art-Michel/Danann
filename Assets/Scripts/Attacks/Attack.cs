using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public const string CCL = "Ccl";
    public const string DANU = "Danu";
    public const string SPEARL = "SpearL";
    public const string SPEARR = "SpearR";

    [SerializeField] List<GameObject> hitboxesParents;

    public string AttackId {get; protected set;}
    public string Sender {get; protected set;}
    public float DamageValue { get { return damageValue; } private set { damageValue = value; } }
    [SerializeField] private float damageValue = 0;
}
