using System.Collections.Generic;
using UnityEngine;
using System;

public class Hurtbox : MonoBehaviour
{
    [SerializeField]float _radius;
    [NonSerialized]   public float HalfRadius;
    #region Hurtbox Visualization
    public Color SphereColor;
    public Color SphereWireColor;
    Mesh _mesh;
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        SOMeshes.Init();
        _mesh = SOMeshes.Instance.HitboxDebugSphere;
        Gizmos.color = SphereColor;
        Gizmos.DrawMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(_radius, _radius, _radius));
        Gizmos.color = SphereWireColor;
        Gizmos.DrawWireMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(_radius, _radius, _radius));
#endif
    }
    #endregion

    [SerializeField] private EntityHP _hurtboxOwnerHP;
    List<string> _attacksThatHitMe;

    void Awake()
    {
        _attacksThatHitMe = new List<string>();
    }

    void Start()
    {
        HalfRadius = _radius / 2;
    }

    public void TakeHit(string attackName, int attackDamage, int plasmaRegainValue)
    {
        if (_attacksThatHitMe.Contains(attackName))
            return;
        else
        {
            _hurtboxOwnerHP.TakeDamage(attackDamage, attackName, plasmaRegainValue);
            _attacksThatHitMe.Add(attackName);
        }
    }

    public void ForgetAttack(string attackName)
    {
        if (_attacksThatHitMe != null && _attacksThatHitMe.Contains(attackName))
            _attacksThatHitMe.Remove(attackName);
    }

    public void ForgetAllAttacks()
    {
        if (_attacksThatHitMe != null)
            _attacksThatHitMe.Clear();
    }
}