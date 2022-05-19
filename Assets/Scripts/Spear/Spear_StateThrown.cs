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

        _startingPosition = _fsm.transform.position;
        _destination = _fsm.Cursor.transform.position;
        _t = 0;

        _fsm.TravelingAttackData.LaunchAttack();
    }

    public override void Update()
    {
        _t += (Time.deltaTime * _fsm.TravelSpeed) / Vector3.Distance(_startingPosition, _destination);
        _fsm.transform.position = Vector3.Lerp(_startingPosition, _destination, _t);
        if (_t >= 1)
            _fsm.ChangeState(Spear_StateNames.IDLE);
    }

    public override void Exit()
    {
        _fsm.TravelingAttackData.StopAttack();
        
    }
}
