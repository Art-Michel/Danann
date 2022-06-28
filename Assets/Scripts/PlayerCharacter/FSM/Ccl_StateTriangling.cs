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
    bool _bossStartedInside;
    float _tickCooldown;
    const float _tickMaxCooldown = 0.075f;


    public override void Begin()
    {
        _t = 0f;
        SpearRaysManager.Instance.EnableLeftRay(true);
        SpearRaysManager.Instance.EnableRightRay(true);
        SpearRaysManager.Instance.EnableLRRay(true);
        _tickCooldown = 0;
        UiManager.Instance.AimHud();
        SoundManager.Instance.AudioSource.Play();
        TriangleGeneration.Instance.Begin();
        _bossStartedInside = TriangleGeneration.Instance.CheckIsIn();
    }

    public override void StateUpdate()
    {
        _t += Time.deltaTime;
        _actions.SlowDownDuringTriangling(Mathf.Lerp(1, 0, (Mathf.InverseLerp(0, _startup, _t))));

        _tickCooldown -= Time.deltaTime;

        TriangleGeneration.Instance.SetMaterialColor(_t/_startup);

        if (TriangleGeneration.Instance.CheckIsIn() && _tickCooldown <= 0)
        {
            TriangleGeneration.Instance.TickOnBoss(_t);
            _tickCooldown = _tickMaxCooldown;
        }

        if (!TriangleGeneration.Instance.CheckIsIn() && _bossStartedInside)
            BreakAttack();

        if (_t > _startup)
            Activate();
    }

    void Activate()
    {
        SoundManager.Instance.AudioSource.Stop();
        SoundManager.Instance.PlayTriangleActivation();
        SoundManager.Instance.PlayTriangleExplosion();
        if (TriangleGeneration.Instance.CheckIsIn())
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
        TriangleGeneration.Instance.BreakAttack();
        _actions._playerPlasma.SpendPlasma("TriangleBreak");
    }

    public override void Exit()
    {
        SpearRaysManager.Instance.EnableLeftRay(false);
        SpearRaysManager.Instance.EnableRightRay(false);
        SpearRaysManager.Instance.EnableLRRay(false);
        UiManager.Instance.UnaimHud();
        _actions.ResetMovementSpeed();
        TriangleGeneration.Instance.Stop();
    }
}
