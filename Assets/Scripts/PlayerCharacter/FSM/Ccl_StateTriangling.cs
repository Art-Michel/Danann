using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateTriangling : Ccl_State
{
    public Ccl_StateTriangling() : base(Ccl_StateNames.TRIANGLING)
    {

    }
    float _t;
    float _startup = 3.5f;

    public override void Begin()
    {
        _t = 0f;
        SoundManager.Instance.PlayTriangleCharge();
    }

    public override void StateUpdate()
    {
        _t += Time.deltaTime;
        _actions.SlowDownDuringTriangling(Mathf.Lerp(1, 0, (Mathf.InverseLerp(0, _startup, _t))));
        if (_t > _startup)
            Activate();
    }

    void Activate()
    {
        SoundManager.Instance.AudioSource.Stop();
        SoundManager.Instance.PlayTriangleActivation();
        SoundManager.Instance.PlayTriangleExplosion();
        _fsm.ChangeState(Ccl_StateNames.IDLE);
        _actions.RecallSpears();
    }

    public void StopAttack()
    {
        SoundManager.Instance.AudioSource.Stop();
        SoundManager.Instance.PlayTriangleCancel();
        _actions.ResetSpears();
    }

    public override void Exit()
    {
        _actions.ResetMovementSpeed();
    }
}
