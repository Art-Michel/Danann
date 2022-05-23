using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateDashing : Ccl_State
{
    public Ccl_StateDashing() : base(Ccl_StateNames.DASHING)
    {

    }

    Vector3 _startingPosition;
    Vector3 _wantedPosition;

    private const float _dashSpeed = 4f;
    float _dashT = 0;

    public override void Begin()
    {
        _startingPosition = _actions.transform.position;
        _wantedPosition = new Vector3(_actions.spearDashedOn.transform.position.x, _actions.transform.position.y, _actions.spearDashedOn.transform.position.z);
        _actions.PlayerHP._isInvulnerable = true;
        _dashT = 0;

        _feedbacks.PlayDashingShoutSfx();
        _actions.EnableDashHitbox();
        _feedbacks.SetTrailRenderer(true, true);
    }

    public override void StateUpdate()
    {
        _dashT += Time.deltaTime * _dashSpeed;
        _actions.transform.position = Vector3.Lerp(_startingPosition,_wantedPosition, _dashT);
        if (_dashT >= 1)
            _fsm.ChangeState(Spear_StateNames.IDLE);
    }

    public override void Exit()
    {
        _actions.PlayerMovement.enabled = true;
        _actions.spearDashedOn.ChangeState(Spear_StateNames.ATTACHED);
        _actions.PlayerHP._isInvulnerable = false;
        _actions.DisableDashHitbox();
    }
}
