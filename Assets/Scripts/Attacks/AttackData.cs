using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData : MonoBehaviour
{
    public const string CCL = "Ccl";
    public const string DANU = "Danu";
    public const string SPEARL = "SpearL";
    public const string SPEARR = "SpearR";

    public List<Hitbox> Hitboxes { get { return hitboxes; } private set { hitboxes = value; } }
    [SerializeField] List<Hitbox> hitboxes;

    public string AttackId {get; protected set;}
    public string Sender {get; protected set;}
    public float DamageValue { get { return damageValue; } private set { damageValue = value; } }
    [SerializeField] private float damageValue = 0;

    public void Launch()
    {
        foreach(Hitbox hitbox in Hitboxes)
        {
            hitbox.CheckIntersection();
        }
    }
}
