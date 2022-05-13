using System.Runtime.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1CSlam : Danu_State
{
    public P1CSlam() : base(StateNames.P1C_SLAM) { }
    AttackData[] _slamAttackDatas = new AttackData[3];
    /*GameObject boombox;
    Transform preview;
    Hitbox boom;*/
    int _state = 0;
    //0= _isPreviewing 
    //1= _isAttacking 
    //2= _isRecovering 
    float[] _startup = new float[3];
    float[] _active = new float[3];
    float[] _recovery = new float[3];
    float _timer;
    /*float maxDistance;
    float[] radius=new float[3];
    int[] damages=new int[3];
    Vector3[] scales=new Vector3[3];*/
    int _slamCount;
    int _maxSlamCount = 3;
    int _index = 0;
    //private bool _canStart;
    // float movetime;
    //bool _bb;
    //private float _maxMoveTime;

    public override void Begin()
    {
        _slamAttackDatas[0] = fsm.GetP1Slam1AttackData();
        _slamAttackDatas[1] = fsm.GetP1Slam2AttackData();
        _slamAttackDatas[2] = fsm.GetP1Slam3AttackData();
        _index = 0;
        Vector3[] frames = new Vector3[3];
        for (int i = 0; i < 3; i++)
        {
            frames[i] = fsm.GetAttackFrames(i);
            _startup[i] = frames[i].x;
            _active[i] = frames[i].y;
            _recovery[i] = frames[i].z;
        }
        StartPreview();
        /*
        boombox=fsm.Getp1SlamHitBox();
        boom=boombox.GetComponent<LingeringHitbox>();
        canStart=true;
        preview=fsm.GetBladesPreview()[0];
        index=0;
        timer=0;
        slamCount=0;
        radius=fsm.GetP1S_Radius();
        damages=fsm.GetP1S_Damage();
        scales[0] = fsm.GetP1SlamScale(0);
        scales[1] = fsm.GetP1SlamScale(1);       
        scales[2] = fsm.GetP1SlamScale(2);
        //RescaleBoomBox();
        maxDistance=boombox.transform.localScale.z*2;
        maxMoveTime=fsm.GetP1Sl_MaxMoveTime();
        //movetime=0;
        if (maxDistance<=Vector3.Distance(fsm.transform.position,fsm.agent.GetPlayer().position))
            canStart=false;        */
    }

    /*private void RescaleBoomBox()
    {

        if (slamCount<maxSlamCount)
        {
            
            boombox.transform.localScale=scales[slamCount];
            boom.CurrentRadius=radius[slamCount];
            //boom.SetDamageValue(damages[slamCount]);
      
            slamCount++;
        }
        else
        {
            slamCount=0;
            boombox.transform.localScale=scales[slamCount];
            boom.CurrentRadius=radius[slamCount];
            //boom.SetDamageValue(damages[slamCount]);

            slamCount++;
        }
    }*/


    // Update is called once per frame
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

        #region old slam
        /*if (!canStart)
        {
            float dist=Vector3.Distance(fsm.transform.position,fsm.agent.GetPlayer().position);
            Vector3 dir=-fsm.transform.position+fsm.agent.GetPlayer().position;
            dir.Normalize();
            fsm.transform.position+=dir*dist/maxMoveTime;
            if (maxDistance<=dist)
                return;
            canStart=true;
        }
            switch(index)
            {
                case 0:
                    timer+=Time.deltaTime;

                    preview.gameObject.SetActive(true);
                    if (timer>startup[slamCount-1] )
                    {
                        index++;
                        timer=0;
                        ActivateHitBox();
                        preview.gameObject.SetActive(false);

                    }
                    else if (timer>startup[slamCount-1]/2 && !bb)
                    {
                        bb=true;
                        LookTowardPlayer();
                    }
                    break;
                case 1:
                    timer+=Time.deltaTime;
                    if (timer>active[slamCount-1])
                    {
                        index++;
                        timer=0;
                        DesactivateHitBox();
                        bb=false;
                    }
                    break;
                case 2:
                    timer+=Time.deltaTime;
                    if (timer>end[slamCount-1])
                    {
                        index=0;
                        timer=0;
                        if (maxSlamCount<=slamCount)
                        {
                            Debug.Log("End");
                            ToIdle();
                        }
                        
                        RescaleBoomBox();
                            LookTowardPlayer();

                    }
                    break;
                default:
                    break;
            }*/
        #endregion
    }
    
    /*if (orig == null)
            {
                fsm.agent.ToIdle();
            }
            else
            {
                orig.AddWaitTime(2);
                orig.FlowControl();
            }*/

    private void StartPreview()
    {
        _timer = 0;
        _state = 0;
        fsm.transform.LookAt(fsm.agent.GetPlayer());
        _slamAttackDatas[_index].gameObject.SetActive(true);
    }

    private void StartAttack()
    {
        _timer = 0;
        _state = 1;
        _slamAttackDatas[_index].LaunchAttack();
    }

    private void StartRecovery()
    {
        _timer = 0;
        _state = 2;
        _slamAttackDatas[_index].StopAttack();
        _slamAttackDatas[_index].gameObject.SetActive(false);
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
    /*
    private void LookTowardPlayer()
    {
        fsm.transform.LookAt(fsm.agent.GetPlayer());
        boombox.transform.rotation=fsm.transform.rotation;
        boombox.transform.position=fsm.transform.position+fsm.transform.forward*boombox.transform.localScale.z/2;
        preview.localScale=boombox.transform.localScale;
        preview.position=boombox.transform.position;
        preview.rotation=boombox.transform.rotation;

    }
private void ToIdle()
    {
        fsm.agent.SetWaitingTime(1);
        fsm.agent.ToIdle();   
    }

    private void DesactivateHitBox()
    {
        boombox.SetActive(false);
    }

    private void ActivateHitBox()
    {
        boombox.SetActive(true);
    }
    public override void End()
    {
        boombox=fsm.Getp1SlamHitBox();
        Vector3[] frames =new Vector3[3];
        for (int i=0;i<3;i++)
        {
            frames[i]=fsm.GetAttackFrames(i);
            startup[i]=frames[i].x;
            active[i]=frames[i].y;
            end[i]=frames[i].z;
        }
        index=0;
        timer=0;
        slamCount=0;
        scales[0] = fsm.GetP1SlamScale(0);
        scales[1] = fsm.GetP1SlamScale(1);
        scales[2] = fsm.GetP1SlamScale(2);
        RescaleBoomBox();
    }*/
}
