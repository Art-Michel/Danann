using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_StateRecalled : Spear_State
{
    public Spear_StateRecalled() : base(Spear_StateNames.RECALLED)
    {

    }

    Vector3 _startingPosition;
    Vector3 _destination;
    float _t = 0;

    public override void Begin()
    {

        _startingPosition = _ai.transform.position;
        _destination = _ai.CclBody.position;

        //_fsm.transform.forward = (_destination - _startingPosition).normalized;

        _t = 0;
        _ai.TravelingAttackData.LaunchAttack();

        SoundManager.Instance.PlayRecall();
    }

    public override void Update()
    {
        _destination = _ai.CclBody.position;
        _t += (Time.deltaTime * _ai.TravelSpeed) / Vector3.Distance(_startingPosition, _destination);
        _fsm.transform.position = Vector3.Lerp(_startingPosition, _destination, _t);
        if (_t >= 1)
            _fsm.ChangeState(Spear_StateNames.ATTACHED);
    }

    public override void Exit()
    {
        _ai.TravelingAttackData.StopAttack();

    }
}
