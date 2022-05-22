using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateRecalling : Ccl_State
{
    public Ccl_StateRecalling() : base(Ccl_StateNames.RECALLING)
    {

    }

    public override void Begin()
    {
        _feedbacks.PlayRecallSfx();
        _fsm.ChangeState(Ccl_StateNames.IDLE);
    }

    public override void StateUpdate()
    {
        
    }

    public override void Exit()
    {

    }
}
