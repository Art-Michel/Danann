using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateTargetting : Ccl_State
{
    public Ccl_StateTargetting() : base(Ccl_StateNames.TARGETTING)
    {

    }

    public override void Begin()
    {
        _actions._currentlyTargettedSpear.SpearFeedbacks.SetText("Recall");
        UiManager.Instance.TargetHud();
    }

    public override void StateUpdate()
    {
        
    }

    public override void Exit()
    {
        UiManager.Instance.UntargetHud();

    }
}