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
#if UNITY_EDITOR
        SOMeshes.Init();
        _mesh = SOMeshes.Instance.HitboxDebugSphere;
        Gizmos.color = SphereColor;
        Gizmos.DrawMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
        Gizmos.color = SphereWireColor;
        Gizmos.DrawWireMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
#endif
    }
    #endregion

    [SerializeField] private EntityHP _hurtboxOwnerHP;
    List<string> _attacksThatHitMe;

    void Awake()
    {
        _attacksThatHitMe = new List<string>();
    }

    public void TakeHit(string attackName, int attackDamage)
    {
        if (_attacksThatHitMe.Contains(attackName))
            return;
        else
        {
            _hurtboxOwnerHP.TakeDamage(attackDamage, attackName);
            Debug.Log("ouchie ouch");
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