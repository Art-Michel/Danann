using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateRecalling : Ccl_State
{
    public Ccl_StateRecalling() : base(Ccl_StateNames.RECALLING)
    {

    }
    const float _stateDuration = 0.3f;
    float _t;

    public override void Begin()
    {
        _feedbacks.PlayRecallSfx();
        _t = 0;
    }

    public override void StateUpdate()
    {
        _t += Time.deltaTime;
        if (_t >= _stateDuration)
            _fsm.ChangeState(Ccl_StateNames.IDLE);
    }

    public override void Exit()
    {

    }
}
