using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateAiming : Ccl_State
{
    public Ccl_StateAiming() : base(Ccl_StateNames.AIMING)
    {

    }

    [SerializeField] GameObject _cursor;

    public override void Begin()
    {
        _actions.EnableCursor();
        _feedbacks.PlayZoomInSfx();
        _feedbacks.ZoomCamera();
        UiManager.Instance.SetText(UiManager.Instance.EText, "Cancel");
        UiManager.Instance.SetText(UiManager.Instance.NText, "Cancel");
        UiManager.Instance.SetText(UiManager.Instance.WText, "Cancel");
        UiManager.Instance.SetText(UiManager.Instance.SText, "Cancel");
    }

    public override void StateUpdate()
    {
        _actions.OrientateBody();
    }

    public override void Exit()
    {
        _actions.DisableCursor();
        _feedbacks.PlayZoomOutSfx();
        _feedbacks.UnzoomCamera();
    }
}
