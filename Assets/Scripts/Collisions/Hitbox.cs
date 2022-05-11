using System.Collections;
using UnityEngine;
using System;

public class Hitbox : MonoBehaviour
{
    #region Hitbox Visualization
#if UNITY_EDITOR
    Color _currentColor;
    Color _sphereColor = new Color(1f, 0.1f, 0.1f, 0.6f);
    Color _sphereWireColor = new Color(0.5f, 0f, 0f, 1f);
    Mesh _mesh;

    private void OnDrawGizmos()
    {
        SOMeshes.Init();
        _mesh = SOMeshes.Instance.HitboxDebugSphere;
        Gizmos.color = _sphereColor;
        Gizmos.DrawMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(_baseRadius, _baseRadius, _baseRadius));
        Gizmos.color = _sphereWireColor;
        Gizmos.DrawWireMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(_baseRadius, _baseRadius, _baseRadius));
    }
#endif
    #endregion

    [NonSerialized] public float CurrentRadius;
    [SerializeField] float _baseRadius;
    Hurtbox[] _hurtboxesToFocus;
    AttackData _attackData;

    void Awake()
    {
        CurrentRadius = _baseRadius;
    }

    public void Enable(Hurtbox[] hurtboxesToFocus, AttackData attackData)
    {
        CurrentRadius = _baseRadius;
        _hurtboxesToFocus = hurtboxesToFocus;
        _attackData = attackData;
        gameObject.SetActive(true);
    }

    public void Enable(Hurtbox[] hurtboxesToFocus, AttackData attackData, float radius)
    {
        Enable(hurtboxesToFocus, attackData);
        CurrentRadius = radius;
    }

    public void Disable(Hurtbox[] hurtboxesToFocus, AttackData attackData)
    {
        foreach (Hurtbox hurtbox in hurtboxesToFocus)
        {
            hurtbox.ForgetAttack(attackData);
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        CheckForHit();
    }

    private bool CheckDistance(Hurtbox hurtbox)
    {
        float distance = (hurtbox.transform.position - transform.position).sqrMagnitude;
        if (distance < (CurrentRadius / 2 + hurtbox.Radius / 2) * (CurrentRadius / 2 + hurtbox.Radius / 2))
            return true;
        return false;
    }

    public void CheckForHit()
    {
        foreach (Hurtbox hurtbox in _hurtboxesToFocus)
        {
            if (CheckDistance(hurtbox))
                hurtbox.TakeHit(_attackData);
        }
    }
}