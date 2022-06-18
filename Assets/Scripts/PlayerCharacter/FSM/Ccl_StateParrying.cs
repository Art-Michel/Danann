using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateShielding : Ccl_State
{
    public Ccl_StateShielding() : base(Ccl_StateNames.SHIELDING)
    {

    }

    private const float _shieldDuration = 1f;
    public float ShieldT = 0;

    public override void Begin()
    {
        _actions.PlayerHP.IsInvulnerable = true;
        _actions.PlayerHP.IsShielding = true;
        ShieldT = 0;

        _actions.PlayerHP.ShieldingText();
        _feedbacks.StartShieldFeedback();
        _feedbacks.SetAnimationTrigger("Shield");
        _actions.EnlargenHurtbox();
    }

    public override void StateUpdate()
    {
        ShieldT += Time.deltaTime;
        if (ShieldT >= _shieldDuration)
            _fsm.ChangeState(Spear_StateNames.IDLE);
    }

    public override void Exit()
    {
        _feedbacks.SetAnimationTrigger("Idle");
        _actions.ResetHurtboxSize();
        _feedbacks.StopShieldFeedback();
        _actions.PlayerHP.ResetInvulerability();
    }
}
