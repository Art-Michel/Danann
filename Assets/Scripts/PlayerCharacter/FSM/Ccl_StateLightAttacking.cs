using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateLightAttacking : Ccl_State
{
    public Ccl_StateLightAttacking() : base(Ccl_StateNames.LIGHTATTACKING)
    {

    }

    float _t = 0;
    float _max = 0;

    public override void Begin()
    {
        //_feedbacks. Lancer l'animation
        _feedbacks.PlayPunchSfx();
        _t = 0;
        _max = _actions.GetActiveTime();
        _actions.EnableHitbox();
        _actions.SlowDownDuringAttack();
    }

    public override void StateUpdate()
    {
        _t+= Time.deltaTime;
        if (_t >= _max)
            _fsm.ChangeState(Ccl_StateNames.LIGHTATTACKRECOVERY);
    }

    public override void Exit()
    {
        _actions.DisableHitbox();
        _actions.ResetMovementSpeed();
    }
}
