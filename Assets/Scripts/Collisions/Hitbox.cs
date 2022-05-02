using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Hitbox : MonoBehaviour
{
    public float Radius;
    Color SphereColor = new Color(255, 50, 50, 0.5f);
    Color SphereHitColor = new Color(255, 0, 0, 1f);
    Color SphereWireColor = new Color(150, 0, 0, 1);
    [SerializeField] Mesh _mesh;

    public List<Hurtbox> Hurtboxes;
    private bool _hasEntered;

    void OnDrawGizmos()
    {
        if (CheckIntersection()) Gizmos.color = SphereHitColor;
        else Gizmos.color = SphereColor;

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