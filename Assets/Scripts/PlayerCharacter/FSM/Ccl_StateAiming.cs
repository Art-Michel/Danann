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
        _fsm.Cursor.transform.position = _fsm.transform.position;
        _fsm.Cursor.SetActive(true);
        _feedbacks.SetCameraTargetWeight(4, 1);
        _feedbacks.PlayZoomIn();
        _feedbacks.ZoomCamera();
    }

    public override void StateUpdate()
    {
        _fsm.Body.transform.forward =
        new Vector3(_fsm.Cursor.transform.position.x, _fsm.transform.position.y, _fsm.Cursor.transform.position.z) - _fsm.Body.position;
    }

    public override void Exit()
    {
        _fsm.Cursor.SetActive(false);
        _feedbacks.PlayZoomOut();
        _feedbacks.UnzoomCamera();
    }
}
