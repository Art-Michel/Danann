using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public float Radius;
    public Color SphereColor;
    public Color SphereWireColor;
    [SerializeField] private EntityHP _entityHP;
    public EntityHP HurtboxsEntityHP { get { return _entityHP; } private set { _entityHP = value; } }

    [SerializeField] Mesh _mesh;

    List<int> _hitboxIds = new List<int>();
    List<string> _hitboxAttackNames = new List<string>();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = SphereColor;
        Gizmos.DrawMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
        Gizmos.color = SphereWireColor;
        Gizmos.DrawWireMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
    }

    public void TakeAHit(Hitbox hitbox)
    {
        if (!_hitboxAttackNames.Contains(hitbox.AttackName))
        {
            if(!_hitboxIds.Contains(hitbox.HitboxId))
            {
            _hitboxIds.Add(hitbox.HitboxId);
            _hitboxAttackNames.Add(hitbox.AttackName);
            HurtboxsEntityHP.TakeDamage(hitbox.DamageValue);
            }
        }
    }

    public void ResetIds()
    {
        _hitboxIds.Clear();
        _hitboxAttackNames.Clear();
    }
}