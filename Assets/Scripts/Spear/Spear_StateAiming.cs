using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_StateAiming : Spear_State
{
    public Spear_StateAiming() : base(Spear_StateNames.AIMING)
    {

    }

    public override void Begin()
    {
        _feedbacks.SetText("Throw");
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

    }
}
