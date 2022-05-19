using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateThrowing : Ccl_State
{
    public Ccl_StateThrowing () : base(Ccl_StateNames.THROWING)
    {

    }

    public override void Begin()
    {
        _fsm.ChangeState(Ccl_StateNames.IDLE);
    }

    public override void StateUpdate()
    {
        
    }

    public override void Exit()
    {

    }
}
