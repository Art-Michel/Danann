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

    bool _bufferTarget = false;

    public override void Begin()
    {
        _bufferTarget = false;
        _fsm.transform.parent = null;

        _startingPosition = new Vector3(_ai.transform.position.x, _ai.transform.position.y + 1, _ai.transform.position.z);
        _destination = new Vector3(_ai.Cursor.transform.position.x, _ai.transform.position.y + 1, _ai.Cursor.transform.position.z);

        _feedbacks.SetMeshUp(_destination - _startingPosition);
        UiManager.Instance.SetSpearImage(_ai.IsLeft, true);
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

    public void BufferTarget()
    {
        _bufferTarget = true;
    }

    public override void Exit()
    {
        _feedbacks.SetText("Focus");
        _feedbacks.SetCameraTargetWeight(4, 0);
        _ai.TravelingAttackData.StopAttack();
        if (_bufferTarget)
        {
            _fsm.PlayerActions.TargetSpear(_fsm);
        }
    }
}
