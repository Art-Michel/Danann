using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class Hitbox : MonoBehaviour
{
    public float Radius;
    Color _currentColor;
    [SerializeField] Color _sphereNormalColor;
    [SerializeField] Color _sphereActiveColor;
    [SerializeField] Color _sphereWireNormalColor;
    [SerializeField] Color _sphereWireActiveColor;
    [SerializeField] Mesh _mesh;

    public List<Hurtbox> Hurtboxes;
    private bool _isActive = false;

    void OnDrawGizmos()
    {
        if (!_isActive)
        {
            Gizmos.color = _sphereNormalColor;
            Gizmos.DrawMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
            Gizmos.color = _sphereWireNormalColor;
            Gizmos.DrawWireMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
        }
        else 
        {
            Gizmos.color = _sphereActiveColor;
            Gizmos.DrawMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
            Gizmos.color = _sphereWireActiveColor;
            Gizmos.DrawWireMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
        }

    }

    [Button]
    public bool CheckIntersection()
    {
        foreach (Hurtbox hurtbox in Hurtboxes)
        {
            if (CheckDistance(hurtbox))
            {
                VerifyHit();
            }
        }
        return false;
    }

    [Button]
    public IEnumerator CheckIntersectionDebug()
    {
        foreach (Hurtbox hurtbox in Hurtboxes)
        {
            if (CheckDistance(hurtbox))
            {
                VerifyHit();
            }
        }
        _isActive = true;
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        _isActive = false;
        yield return null;
    }

    private bool CheckDistance(Hurtbox hurtbox)
    {
        float distance = (hurtbox.transform.position - transform.position).sqrMagnitude;
        if (distance < (Radius / 2 + hurtbox.Radius / 2) * (Radius / 2 + hurtbox.Radius / 2)) return true;
        else return false;
    }

    private void VerifyHit()
    {
        throw new NotImplementedException();
    }
}