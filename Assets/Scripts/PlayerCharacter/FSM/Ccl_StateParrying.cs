using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateParrying : Ccl_State
{
    public Ccl_StateParrying() : base(Ccl_StateNames.PARRYING)
    {

    }

    private const float _parryDuration = 0.5f;
    float _parryT = 0;

    public override void Begin()
    {
        _actions.PlayerHP._isInvulnerable = true;
        _actions.PlayerHP._isParrying = true;
        _parryT = 0;

        _feedbacks.PlayParrySfx();
    }

    public override void StateUpdate()
    {
        _parryT += Time.deltaTime;
        if (_parryT >= _parryDuration)
            _fsm.ChangeState(Spear_StateNames.IDLE);
    }

    public override void Exit()
    {
        _actions.PlayerHP._isInvulnerable = false;
        _actions.PlayerHP._isParrying = true;
    }
}
