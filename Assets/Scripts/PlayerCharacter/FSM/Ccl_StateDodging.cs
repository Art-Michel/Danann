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

    //private const float _dashLength = 8f;
    private const float _dashDuration = 0.15f;
    private const float _dashSpeed = 40f;
    float _dashT = 0;

    public override void Begin()
    {
        _actions.PlayerMovement.enabled = false;
        _wantedDirection = (_actions.transform.position + _actions.PlayerMovement.PlayerBody.transform.forward) - _actions.transform.position;
        _actions.PlayerHP._isInvulnerable = true;
        _dashT = 0;

        _feedbacks.PlayDodgeSfx();
        _feedbacks.SetTrailRenderer(true, true);
    }

    public override void StateUpdate()
    {
        _dashT += Time.deltaTime;
        _actions.Characon.Move(_wantedDirection * _dashSpeed * Time.deltaTime);
        if (_dashT >= _dashDuration)
            _fsm.ChangeState(Spear_StateNames.IDLE);
    }

    public override void Exit()
    {
        _actions.PlayerMovement.enabled = true;
        _actions.PlayerHP._isInvulnerable = false;
        _actions.StartDodgeCooldown();
    }
}
