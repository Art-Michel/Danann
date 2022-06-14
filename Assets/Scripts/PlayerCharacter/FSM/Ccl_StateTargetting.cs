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
        UiManager.Instance.SetText(UiManager.Instance.EText, "Cancel");
        UiManager.Instance.SetText(UiManager.Instance.NText, "Cancel");
        UiManager.Instance.SetText(UiManager.Instance.WText, "Cancel");
        UiManager.Instance.SetText(UiManager.Instance.SText, "JUMP");
    }

    public override void StateUpdate()
    {
        
    }

    public override void Exit()
    {

    }
}