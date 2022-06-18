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
        _ai.AttachSpearToPlayer();
        _feedbacks.SetSpearCameraTargetWeight(_ai.IsLeft, 0);
        _feedbacks.SetText("Aim");
        UiManager.Instance.SetSpearImage(_ai.IsLeft, false);
        _feedbacks.PlayReattach();
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}
