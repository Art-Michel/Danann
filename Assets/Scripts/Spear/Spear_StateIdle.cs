using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_StateIdle : Spear_State
{
    public Spear_StateIdle() : base(Spear_StateNames.IDLE)
    {

    }

    public override void Begin()
    {
        _ai.transform.forward = Vector3.up;
        _ai.Trigger.enabled = true;
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        _ai.Trigger.enabled = false;
    }
}
