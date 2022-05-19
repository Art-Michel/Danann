using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_StateAttached : Spear_State
{
    public Spear_StateAttached() : base(Spear_StateNames.ATTACHED)
    {

    }

    public override void Begin()
    {
        _ai.ResetPositionAndRotation();
        _ai.SetSpearWeight(0);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

        _ai.SetSpearWeight(1);
    }
}
