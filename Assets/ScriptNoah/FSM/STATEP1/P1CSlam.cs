using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1CSlam : Danu_State
{
    public P1CSlam() : base(StateNames.P1C_SLAM) { }
    GameObject boombox;
    float[] startup=new float[3];
    float[] active=new float[3];
    float[] end=new float[3];
    float timer;
    Vector3[] scales=new Vector3[3];
    int slamCount;
    int maxSlamCount=3;
    int index=0;
    // Start is called before the first frame update
    public override void Begin()
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
    }

    private void RescaleBoomBox()
    {
        if (slamCount<maxSlamCount)
        {
            boombox.transform.localScale=scales[slamCount];
            slamCount++;
        }
        else
        {
            slamCount=0;
            boombox.transform.localScale=scales[slamCount];
            slamCount++;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
            switch(index)
            {
                case 0:
                    timer+=Time.deltaTime;
                    if (timer>startup[slamCount-1])
                    {
                        index++;
                        timer=0;
                        ActivateHitBox();
                    }
                    else if (timer>startup[slamCount-1]/2)
                    {
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
                    }
                    break;
                    default:
                        break;
            }
    }
    private void LookTowardPlayer()
    {
        fsm.transform.LookAt(fsm.agent.GetPlayer());
        boombox.transform.rotation=fsm.transform.rotation;
        boombox.transform.position=fsm.transform.position+fsm.transform.forward*boombox.transform.localScale.z/2;
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
    }
}
