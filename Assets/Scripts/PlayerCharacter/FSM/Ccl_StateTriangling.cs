using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_StateTriangling : Ccl_State
{
    public Ccl_StateTriangling() : base(Ccl_StateNames.TRIANGLING)
    {

    }
    float _t;
    float _startup = 2.8f;

    public override void Begin()
    {
        _t = 0f;
        SoundManager.Instance.AudioSource.Play();
        TriangleGeneration.Instance.Begin();
    }

    public override void StateUpdate()
    {
        _t += Time.deltaTime;
        _actions.SlowDownDuringTriangling(Mathf.Lerp(1, 0, (Mathf.InverseLerp(0, _startup, _t))));
        if(!TriangleGeneration.Instance.CheckIsIn())
            BreakAttack();
        if (_t > _startup)
            Activate();
    }

    void Activate()
    {
        SoundManager.Instance.AudioSource.Stop();
        SoundManager.Instance.PlayTriangleActivation();
        SoundManager.Instance.PlayTriangleExplosion();
        if(TriangleGeneration.Instance.CheckIsIn())
            TriangleGeneration.Instance.BlowUpOnBoss();
        _fsm.ChangeState(Ccl_StateNames.IDLE);
        _actions.RecallSpears();
        _actions._playerPlasma.SpendPlasma(Ccl_Attacks.TRIANGLEBOOM);
    }

    public void StopAttack()
    {
        SoundManager.Instance.AudioSource.Stop();
        SoundManager.Instance.PlayTriangleCancel();
        _actions.ResetSpears();
        _actions._playerPlasma.SpendPlasma("TriangleStop");
    }

    void BreakAttack()
    {
        SoundManager.Instance.AudioSource.Stop();
        SoundManager.Instance.PlayTriangleBreak();
        _fsm.ChangeState(Ccl_StateNames.IDLE);
        _actions.RecallSpears();
        _actions._playerPlasma.SpendPlasma("TriangleBreak");
    }

    public override void Exit()
    {
        _actions.ResetMovementSpeed();
        TriangleGeneration.Instance.Stop();
    }
}
