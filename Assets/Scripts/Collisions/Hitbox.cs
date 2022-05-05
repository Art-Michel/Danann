using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class Hitbox : MonoBehaviour
{
    #region debugging
#if UNITY_EDITOR
    Color _currentColor;
    Color _sphereNormalColor = new Color(1f, 0.1f, 0.1f, 0.1f);
    Color _sphereActiveColor = new Color(1f, 0f, 0f, 0.8f);
    Color _sphereWireNormalColor = new Color(0.3f, 0.1f, 0.1f, 0.1f);
    Color _sphereWireActiveColor = new Color(0.3f, 0.1f, 0.1f, 1f);
    [SerializeField] Mesh _mesh;
    private bool _showHitbox = false;
    private void OnDrawGizmosSelected()
    {
        if (!_showHitbox)
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
#endif
    #endregion

    public float Radius;

    [NonSerialized] public string Owner = "";

    public int HitboxId { get { return _hitboxID; } private set { _hitboxID = value; } }
    [Tooltip("If hitboxes have the same ID you can't get hit by both (Single Hit), otherwise you can get Multi-Hit")]
    [SerializeField] private int _hitboxID;

    [SerializeField] private float _damageValue = 0;

    [Tooltip("Lists the hurtboxes this hitbox must check.")]
    [SerializeField] List<Hurtbox> HurtboxesToFocus;


    [Button]
    public bool CheckForHit()
    {
        #if UNITY_EDITOR
        StartCoroutine("VisualizeDebug");
        #endif

        foreach (Hurtbox hurtbox in HurtboxesToFocus)
        {
            if (CheckDistance(hurtbox))
            {
                if (VerifyHit()) return true;
            }
        }
        return false;
    }

    private IEnumerator VisualizeDebug()
    {
        _showHitbox = true;
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        _showHitbox = false;
        yield return null;
    }

    private bool CheckDistance(Hurtbox hurtbox)
    {
        float distance = (hurtbox.transform.position - transform.position).sqrMagnitude;
        if (distance < (Radius / 2 + hurtbox.Radius / 2) * (Radius / 2 + hurtbox.Radius / 2))
        {
            Debug.Log("Hurtbox in Range");
            return true;
        }
        else
            return false;
    }

    private bool VerifyHit()
    {
        return true;
    }
}