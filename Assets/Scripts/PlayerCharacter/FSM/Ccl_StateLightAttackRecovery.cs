using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateLightAttackRecovery : Ccl_State
{
   public Ccl_StateLightAttackRecovery() : base(Ccl_StateNames.LIGHTATTACKRECOVERY)
    {

    }

    float _t = 0;
    float _max = 0;

    public override void Begin()
    {
        //_feedbacks. Lancer l'animation
        _actions.ResetComboWindow();
        _t = 0;
        _max = _actions.GetRecoveryTime();
        _actions.SlowDownDuringAttack();
    }

    public override void StateUpdate()
    {
        _t+= Time.deltaTime;
        if (_t >= _max)
            _fsm.ChangeState(Ccl_StateNames.IDLE);
    }

    public override void Exit()
    {
        _feedbacks.EnablePunchVfx(_actions.CurrentLightAttackIndex, false);
        _actions.IncreaseLightAttackIndex();
        _actions.ResetMovementSpeed();
    }
}
