using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Hitbox : MonoBehaviour
{
    public float Radius;
    public Color SphereColor;
    public Color SphereHitColor;
    public Color SphereWireColor;
    [SerializeField] Mesh _mesh;

    public List<Hurtbox> Hurtboxes;
    private bool _hasEntered;

    void OnDrawGizmos()
    {
        Gizmos.color = SphereColor;

        if (CheckIntersection())
        {
            Gizmos.color = SphereHitColor;
            if (!_hasEntered && SoundManager.Instance != null)
                SoundManager.Instance.PlayBong();
            _hasEntered = true;
        }
        else _hasEntered = false;
        Gizmos.DrawMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
        Gizmos.color = SphereWireColor;
        Gizmos.DrawWireMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
    }

    bool CheckIntersection()
    {
        foreach (Hurtbox hurtbox in Hurtboxes)
        {
            //float distance = Vector3.Distance(transform.position, hurtbox.transform.position);
            float distance = (hurtbox.transform.position - transform.position).sqrMagnitude;
            if (distance < (Radius / 2 + hurtbox.Radius / 2) * (Radius / 2 + hurtbox.Radius / 2)) return true;
        }
        return false;
    }
}