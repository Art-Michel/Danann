using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public float Radius;
    public Color SphereColor;
    public Color SphereWireColor;
    [SerializeField] Mesh _mesh;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = SphereColor;
        Gizmos.DrawMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
        Gizmos.color = SphereWireColor;
        Gizmos.DrawWireMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
    }

    
}