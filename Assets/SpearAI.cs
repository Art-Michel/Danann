using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAI : MonoBehaviour
{
    [SerializeField] bool _isLeft;

    Vector3 LeftSpearAttachedPosition;
    Quaternion LeftSpearAttachedRotation;
    Vector3 RightSpearAttachedPosition;
    Quaternion RightSpearAttachedRotation;

    public GameObject Cursor;
    public AttackData TravelingAttackData;
    public float TravelSpeed = 50;

    public SphereCollider Trigger {get; private set;}

    void Awake()
    {
        Trigger = GetComponent<SphereCollider>();
        if (_isLeft)
        {
            LeftSpearAttachedPosition = transform.position;
            LeftSpearAttachedRotation = transform.rotation;
        }
        else
        {
            RightSpearAttachedPosition = transform.position;
            RightSpearAttachedRotation = transform.rotation;
        }
    }

    void Start()
    {

    }

    public void ResetPositionAndRotation()
    {
        if (_isLeft)
            transform.SetPositionAndRotation(LeftSpearAttachedPosition, LeftSpearAttachedRotation);
        else
            transform.SetPositionAndRotation(RightSpearAttachedPosition, RightSpearAttachedRotation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            
        }
    }
}
