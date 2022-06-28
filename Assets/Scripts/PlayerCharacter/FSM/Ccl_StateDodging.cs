using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateDodging : Ccl_State
{
    public Ccl_StateDodging() : base(Ccl_StateNames.DODGING)
    {

    }

    Vector3 _startingPosition;
    Vector3 _wantedDirection;

    private const float _dodgeDuration = 0.15f;
    private const float _dodgeSpeed = 30f;
    float _dodgeT = 0;

    public override void Begin()
    {
        _wantedDirection = _actions.PlayerMovement.GetOrientation();
        _feedbacks.SetAnimationTrigger("Dash");
        _actions.PlayerHP.IsInvulnerable = true;
        //_actions.PlayerHP.InvulnerableText();
        _dodgeT = 0;

        _feedbacks.PlayDodgeSfx();
        _feedbacks.SetTrailRenderer(true);
    }

    public override void StateUpdate()
    {
        _dodgeT += Time.deltaTime;
        _actions.Characon.Move(_wantedDirection * _dodgeSpeed * Time.deltaTime);
        if (_dodgeT >= _dodgeDuration)
            _fsm.ChangeState(Spear_StateNames.IDLE);
    }

    public override void Exit()
    {
        _feedbacks.SetTrailRenderer(false);
        _actions.PlayerHP.IsInvulnerable = false;
        _actions.StartDodgeCooldown();
    }
}
