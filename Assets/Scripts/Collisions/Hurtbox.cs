using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public float Radius;
    public Color SphereColor;
    public Color SphereWireColor;
    [SerializeField] private EntityHP _entityHP;
    public EntityHP HurtboxsEntityHP { get { return _entityHP; } private set { _entityHP = value; } }

    [SerializeField] Mesh _mesh;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = SphereColor;
        Gizmos.DrawMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
        Gizmos.color = SphereWireColor;
        Gizmos.DrawWireMesh(_mesh, -1, transform.position, Quaternion.identity, new Vector3(Radius, Radius, Radius));
    }

    public void TakeAHit(Hitbox hitbox)
    {
        //VerifyHit first
        HurtboxsEntityHP.TakeDamage(hitbox.DamageValue);
    }
}