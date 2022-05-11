using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public float Radius;
    #region Hurtbox Visualization
    public Color SphereColor;
    public Color SphereWireColor;
    Mesh _mesh;
    void OnDrawGizmos()
    {
        SOMeshes.Init();
        _mesh = SOMeshes.Instance.HitboxDebugSphere;
        Gizmos.color = SphereColor;
        Gizmos.DrawMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
        Gizmos.color = SphereWireColor;
        Gizmos.DrawWireMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
    }
    #endregion

    [SerializeField] private EntityHP _hurtboxOwnerHP;
    Dictionary<string, AttackData> _attacksThatHitMe;

    void Awake()
    {
        _attacksThatHitMe = new Dictionary<string, AttackData>();
    }

    public void TakeHit(AttackData attackData)
    {
        if (_attacksThatHitMe.ContainsKey(attackData.AttackName))
            return;
        else
        {
            _hurtboxOwnerHP.TakeDamage(attackData.AttackDamage);
            _attacksThatHitMe.Add(attackData.AttackName, attackData);
        }
    }

    public void ForgetAttack(AttackData attackData)
    {
        if (_attacksThatHitMe.ContainsKey(attackData.AttackName))
        _attacksThatHitMe.Remove(attackData.AttackName);
    }

    public void ForgetAllAttacks()
    {
        _attacksThatHitMe.Clear();
    }
}