using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateIdle : Ccl_State
{
    public Ccl_StateIdle() : base(Ccl_StateNames.IDLE)
    {

    }

    public override void Begin()
    {
        UiManager.Instance.SetText(UiManager.Instance.EText, "Parry");
        UiManager.Instance.SetText(UiManager.Instance.NText, "");
        UiManager.Instance.SetText(UiManager.Instance.WText, "Punch");
        UiManager.Instance.SetText(UiManager.Instance.SText, "Dodge");
    }

    public override void StateUpdate()
    {

    }

    public override void Exit()
    {

    }
}
