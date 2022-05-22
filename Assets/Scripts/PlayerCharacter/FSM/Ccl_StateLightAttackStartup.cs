using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateLightAttackStartup : Ccl_State
{
    public Ccl_StateLightAttackStartup() : base(Ccl_StateNames.LIGHTATTACKSTARTUP)
    {

    }

    float _t = 0;
    float _max = 0;

    public override void Begin()
    {
        //_feedbacks. Lancer l'animation
        _t = 0;
        _max = _actions.GetStartupTime();
        _actions.SlowDownDuringAttack();
    }

    public override void StateUpdate()
    {
        _t+= Time.deltaTime;
        if (_t >= _max)
            _fsm.ChangeState(Ccl_StateNames.LIGHTATTACKING);
    }

    public override void Exit()
    {
        _actions.ResetMovementSpeed();
    }
}
