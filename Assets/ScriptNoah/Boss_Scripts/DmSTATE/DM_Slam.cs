using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DM_Slam : Dm_State
{
   
    Arr2D[] _slamHitbox=new Arr2D[4];
    int _state = 0;
    float[] _startup = new float[3];
    float[] _active = new float[3];
    float[] _recovery = new float[3];
    float _timer;
    int _slamCount;
    int _maxSlamCount = 3;
    int _index = 0;
    int _indexer=0;
    public override void Begin()
    {
        Init();
        _index = 0;
        StartPreview();
        Debug.Log(_slamHitbox.Length);
    }
    void Init()
    {
        _slamHitbox=fsm.GetP1SlamHitBox();

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
        
        SoundManager.Instance.PlayBossRiseStart();
        for (int i=0;i<4;i++)
            _slamHitbox[i].ad[_index].gameObject.SetActive(true);
    }

    private void StartAttack()
    {
        _timer = 0;
        _state = 1;
                if (_index == _maxSlamCount-1)
            fsm.agent.m_anims.SetTrigger("RiseOver");
       
        for (int i=0;i<4;i++)
        {
            _slamHitbox[i].arr[_index].SetActive(true);
            _slamHitbox[i].ad[_index].LaunchAttack();
        }
        SoundManager.Instance.PlayBossRiseEnd();

    }

    private void StartRecovery()
    {
        _timer = 0;
        _state = 2;

        for (int i=0;i<4;i++)
        {   
            _slamHitbox[i].ad[_index].StopAttack();
            _slamHitbox[i].ad[_index].gameObject.SetActive(false);
            _slamHitbox[i].arr[_index].SetActive(false);
        }
    }

    void Exit()
    {
       fsm.Next();
    }
}