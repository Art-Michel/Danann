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
        _ai.AttackIfShouldAttack();
        _feedbacks.SetSpearCameraTargetWeight(_ai.IsLeft, 1);
        _feedbacks.SetMeshRotation(Vector3.zero);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}
