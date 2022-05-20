using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

public class SpearAI : MonoBehaviour
{
    [SerializeField] bool _isLeft;
    [Required] SpearFeedbacks _spearFeedbacks;
    
    public Transform CclBody;

    public GameObject Cursor;
    public AttackData TravelingAttackData;
    public float TravelSpeed = 50;

    public SphereCollider Trigger { get; private set; }

    void Awake()
    {
        Trigger = GetComponent<SphereCollider>();
    }

    public void SetSpearWeight(int weight)
    {
        if (_isLeft)
            _spearFeedbacks.SetCameraTargetWeight(2, weight);
        else
            _spearFeedbacks.SetCameraTargetWeight(3, weight);
    }

    public void ResetTransform()
    {
        transform.parent = CclBody;
        _spearFeedbacks.ResetPositionAndRotation();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {

        }
    }
}
