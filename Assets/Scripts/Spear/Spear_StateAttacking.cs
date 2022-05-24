using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_StateAttacking : Spear_State
{
    public Spear_StateAttacking() : base(Spear_StateNames.ATTACKING)
    {

    }
    
    float _t;
    float _activeAttack;
    bool _isAttacking;
    const float _attackCooldown = 1f;
    const float _attackDuration = 0.1f;

    public override void Begin()
    {
        _t = 0f;
        _activeAttack = 0f;
        Attack();
    }

    public override void Update()
    {
        _t += Time.deltaTime;
        if (!_isAttacking && _t >= _attackCooldown)
            Attack();
        if (_isAttacking && _t >= _attackDuration)
            StopAttack();

    }

    void Attack()
    {
        _t = 0f;
        _isAttacking = true;
        _ai.LaunchSwing();
    }

    private void StopAttack()
    {
        _t = 0f;
        _isAttacking = false;
        _ai.StopSwing();
    }

    public override void Exit()
    {
        _isAttacking = false;
    }
}
