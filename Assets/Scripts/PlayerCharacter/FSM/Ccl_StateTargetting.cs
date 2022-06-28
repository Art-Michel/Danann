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

        if (_actions._currentlyTargettedSpear.SpearAi.IsLeft)
            SpearRaysManager.Instance.EnableLeftRay(true);
        else
            SpearRaysManager.Instance.EnableRightRay(true);
    }

    public override void StateUpdate()
    {

    }

    public override void Exit()
    {
        UiManager.Instance.UntargetHud();
        SpearRaysManager.Instance.EnableLeftRay(false);
        SpearRaysManager.Instance.EnableRightRay(false);
    }
}