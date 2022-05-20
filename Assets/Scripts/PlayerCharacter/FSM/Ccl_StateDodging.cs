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
    private const float _dashDuration = 0.1f;
    private const float _dashSpeed = 70f;
    float _t = 0;

    public override void Begin()
    {
        _pa.PlayerMovement.enabled = false;
        _wantedDirection = (_pa.transform.position + _pa.PlayerMovement.PlayerBody.transform.forward) - _pa.transform.position;
        _pa.PlayerHP._isInvulnerable = true;
        _t = 0;
        SoundManager.Instance.PlayDash();
        _pa.SetTrailRenderer(true);
    }

    public override void StateUpdate()
    {
        _t += Time.deltaTime;
        _pa.Characon.Move(_wantedDirection * _dashSpeed * Time.deltaTime);
        if (_t >= _dashDuration)
            _fsm.ChangeState(Spear_StateNames.IDLE);
    }

    public override void Exit()
    {
        _pa.PlayerMovement.enabled = true;
        _pa.PlayerHP._isInvulnerable = false;
        _pa.StartDodgeCooldown();
    }
}
