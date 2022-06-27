using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using NaughtyAttributes;
using System;

public class SpearAI : MonoBehaviour
{
    public bool IsLeft;
    [Required] SpearFeedbacks _spearFeedbacks;

    public Transform CclBody;
    public Spear_FSM _fsm;
    private bool _enemyInRange;

    [Required][SerializeField] Transform _spearTransformWhenAttached;

    public GameObject Cursor;
    public AttackData TravelingAttackData;
    [SerializeField] AttackData _swingAttackData;
    public readonly float TravelSpeed = 50;

    public SphereCollider Trigger { get; private set; }

    void Awake()
    {
        Trigger = GetComponent<SphereCollider>();
        _spearFeedbacks = GetComponent<SpearFeedbacks>();
        _fsm = GetComponent<Spear_FSM>();
    }

    void Start()
    {
        _enemyInRange = false;
    }

    internal void LaunchSwing()
    {
        _spearFeedbacks.PlaySwing();
        _swingAttackData.LaunchAttack();
    }

    internal void StopSwing()
    {
        _swingAttackData.StopAttack();
    }

    public void AttachSpearToPlayer()
    {
        transform.parent = CclBody;
        transform.position = _spearTransformWhenAttached.position;
        _spearFeedbacks.SetMeshRotation(_spearTransformWhenAttached.rotation.eulerAngles);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            _enemyInRange = true;
            if (_fsm.currentState.Name == Spear_StateNames.IDLE)
                _fsm.ChangeState(Spear_StateNames.ATTACKING);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            _enemyInRange = false;
            if (_fsm.currentState.Name == Spear_StateNames.ATTACKING)
                _fsm.ChangeState(Spear_StateNames.IDLE);
        }
    }

    public void AttackIfShouldAttack()
    {
        if (_enemyInRange)
            _fsm.ChangeState(Spear_StateNames.ATTACKING);
    }
}
