using System.Runtime.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1CSlam : Danu_State
{
    public P1CSlam() : base(StateNames.P1C_SLAM) { }
    AttackData[] _slamAttackDatas = new AttackData[3];
    GameObject[] _slamHitbox=new GameObject[3];
    int _state = 0;
    float[] _startup = new float[3];
    float[] _active = new float[3];
    float[] _recovery = new float[3];
    float _timer;
    int _slamCount;
    int _maxSlamCount = 3;
    int _index = 0;
    public override void Begin()
    {
        if (!isInit)
            Init();
        _index = 0;
        StartPreview();

    }
    public override void Init()
    {
        base.Init();
        _slamHitbox=fsm.GetP1SlamHitBox();
        _slamAttackDatas[0] = fsm.GetP1Slam1AttackData();
        _slamAttackDatas[1] = fsm.GetP1Slam2AttackData();
        _slamAttackDatas[2] = fsm.GetP1Slam3AttackData();
        Vector3[] frames = new Vector3[3];
        for (int i = 0; i < 3; i++)
        {
            frames[i] = fsm.GetAttackFrames(i);
            _startup[i] = frames[i].x;
            _active[i] = frames[i].y;
            _recovery[i] = frames[i].z;
        }
    }
    public override void Update()
    {
        _timer += Time.deltaTime;
        if (_state == 0 && _timer > _startup[_index])
            StartAttack();
        if (_state == 1 && _timer > _active[_index])
            StartRecovery();
        if (_state == 2 && _timer > _recovery[_index])
        {
            _index++;
            if (_index == _maxSlamCount)
                Exit();
            else
                StartPreview();
        }
    }

    private void StartPreview()
    {
        _timer = 0;
        _state = 0;
        Transform target = fsm.agent.GetPlayer();
        Vector3 straightTarget =new Vector3( target.position.x,fsm.transform.position.y,target.position.z);
        fsm.transform.LookAt(straightTarget);
        SoundManager.Instance.PlayBossRiseStart();
        _slamAttackDatas[_index].gameObject.SetActive(true);
    }

    private void StartAttack()
    {
        _timer = 0;
        _state = 1;
        _slamAttackDatas[_index].LaunchAttack();
        _slamHitbox[_index].SetActive(true);
        SoundManager.Instance.PlayBossRiseEnd();

    }

    private void StartRecovery()
    {
        _timer = 0;
        _state = 2;
        _slamAttackDatas[_index].StopAttack();
        _slamAttackDatas[_index].gameObject.SetActive(false);
        _slamHitbox[_index].SetActive(false);
    }

    void Exit()
    {
        if (orig == null)
            {
                fsm.agent.ToIdle();
            }
            else
            {
                orig.AddWaitTime(2);
                orig.FlowControl();
            }
    }
}