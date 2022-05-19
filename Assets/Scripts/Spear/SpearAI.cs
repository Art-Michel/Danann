using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpearAI : MonoBehaviour
{
    [SerializeField] bool _isLeft;

    Vector3 LeftSpearAttachedPosition;
    Vector3 LeftSpearAttachedRotation;
    Vector3 RightSpearAttachedPosition;
    Vector3 RightSpearAttachedRotation;

    public Transform CclBody;
    public CinemachineTargetGroup TargetGroup;

    public GameObject Cursor;
    public AttackData TravelingAttackData;
    public float TravelSpeed = 50;

    public SphereCollider Trigger { get; private set; }

    void Awake()
    {
        Trigger = GetComponent<SphereCollider>();
        if (_isLeft)
        {
            LeftSpearAttachedPosition = transform.localPosition;
            LeftSpearAttachedRotation = transform.localRotation.eulerAngles;
        }
        else
        {
            RightSpearAttachedPosition = transform.localPosition;
            RightSpearAttachedRotation = transform.localRotation.eulerAngles;
        }
    }

    public void SetSpearWeight(int weight)
    {
        if (_isLeft)
            TargetGroup.m_Targets[2].weight = weight;
        else
            TargetGroup.m_Targets[3].weight = weight;

    }

    void Start()
    {

    }

    public void ResetPositionAndRotation()
    {
        transform.parent = CclBody;
        if (_isLeft)
        {
            transform.localPosition = LeftSpearAttachedPosition;
            transform.localEulerAngles = LeftSpearAttachedRotation;
        }
        else
        {
            transform.localPosition = RightSpearAttachedPosition;
            transform.localEulerAngles = RightSpearAttachedRotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {

        }
    }
}
