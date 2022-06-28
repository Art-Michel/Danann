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

    const float _dashBaseSpeed = 80f;
    float _dashSpeed = 0f;
    float _dashT = 0f;

    public override void Begin()
    {
        _startingPosition = _actions.transform.position;
        _wantedPosition = new Vector3(_actions.SpearDashedOn.transform.position.x, _actions.transform.position.y, _actions.SpearDashedOn.transform.position.z);

        _feedbacks.SetAnimationTrigger("BigDash");
        Vector3 trajectory = _startingPosition - _wantedPosition;
        _actions.PlayerMovement.OrientateBodyTowards(trajectory);
        _dashSpeed = _dashBaseSpeed / trajectory.magnitude;

        _actions.PlayerHP.IsInvulnerable = true;
        _actions.PlayerHP.InvulnerableText();
        _dashT = 0;

        _feedbacks.PlayDashingShoutSfx();
        _feedbacks.EnableDashVfx(true);
        _actions.EnableDashHitbox();
    }

    public override void StateUpdate()
    {
        _dashT += Time.deltaTime * _dashSpeed;
        _actions.transform.position = Vector3.Lerp(_startingPosition, _wantedPosition, _dashT);
        if (_dashT >= 1.1f)
            _fsm.ChangeState(Spear_StateNames.IDLE);
    }

    public override void Exit()
    {
        _feedbacks.SetAnimationTrigger("Idle");
        _actions.SpearDashedOn.ChangeState(Spear_StateNames.ATTACHED);
        _actions.PlayerHP.IsInvulnerable = false;
        _actions.DisableDashHitbox();
        _feedbacks.EnableDashVfx(false);
    }
}
