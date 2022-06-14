using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_StateThrown : Spear_State
{
    public Spear_StateThrown() : base(Spear_StateNames.THROWN)
    {

    }

    Vector3 _startingPosition;
    Vector3 _destination;
    float _t = 0;

    public override void Begin()
    {
        _fsm.transform.parent = null;

        _startingPosition = _ai.transform.position;
        _destination = new Vector3(_ai.Cursor.transform.position.x, _ai.transform.position.y, _ai.Cursor.transform.position.z);

        _feedbacks.SetMeshUp(_destination - _startingPosition);
        _feedbacks.SetImageTransparency(0.5f);
        _feedbacks.SetText("");

        _t = 0;
        _ai.TravelingAttackData.LaunchAttack();
    }

    public override void Update()
    {
        _t += (Time.deltaTime * _ai.TravelSpeed) / Vector3.Distance(_startingPosition, _destination);
        _fsm.transform.position = Vector3.Lerp(_startingPosition, _destination, _t);
        if (_t >= 1)
            _fsm.ChangeState(Spear_StateNames.IDLE);
    }

    public override void Exit()
    {
        _feedbacks.SetText("Focus");
        _feedbacks.SetCameraTargetWeight(4, 0);
        _ai.TravelingAttackData.StopAttack();
    }
}
