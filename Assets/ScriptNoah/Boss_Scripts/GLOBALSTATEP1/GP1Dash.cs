using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP1Dash : GlobalStates
{
    public GP1Dash(): base(StateNames.P1GDASH) { }
    P1CDash dash = new P1CDash();
    P1CMixDash mdash = new P1CMixDash();
    P1DShoot shoot = new P1DShoot();
    P1CSlam slam = new P1CSlam();
    P1CTeleportation tp= new P1CTeleportation();
    bool isMix;
    bool[] test;
    Danu_State last;
    public override void Begin()
    {
        progression=0;
        nextWillEnd=false;
        fsm=gFSM.GetComponent<Danu_FSM>();
        fsm.AddState(dash);
        fsm.AddState(mdash);
        fsm.AddState(shoot);
        fsm.AddState(slam);
        fsm.AddState(tp);
        dash.orig=mdash.orig=shoot.orig=slam.orig=this;
        stats=gFSM.GetPhaseStats();
        if (gFSM.agent.isRevengeHigh)
            isMix=true;
        else
            isMix=false;
        FlowControl();
    }
    // Update is called once per frame
    public override void Update()
    {
        curr.Update();
    }
    public override void FlowControl()
    {
        if (nextWillEnd)
        {
            progression=0;
            oldProg=0;
            nextWillEnd=false;
            gFSM.agent.SetWaitingTime(combinedWaitTime);
            combinedWaitTime=0;
            gFSM.agent.ToIdle();
            return;
        }
        if (isMix)
            MixFlow();
        else
            BaseFlow();
    }
    void MixFlow()
    {
        switch (progression)
        {
            case 0 :
                curr=mdash;
                curr.Begin();
                progression++;
                break;
            case 1 :
                if (gFSM.agent.wasParried )
                {
                    curr.End();
                    curr=slam;
                    curr.Begin();
                    progression++;
                }
                else
                {
                    fsm.SetTPDest(P1CTeleportation.destPoints.CLOSE);    
                    curr.End();
                    curr=tp;
                    curr.Begin();
                    progression+=3;
                }
                break;
            case 2:
                if (gFSM.agent.wasParried)
                {
                    fsm.SetTPDest(P1CTeleportation.destPoints.FAR);
                    curr.End();
                    curr=tp;
                    curr.Begin();
                    progression++;
                    break;
                }
                    curr.End();
                    curr=shoot;
                    curr.Begin();
                    nextWillEnd=true;

                break;
            case 3:
                    curr.End();
                    curr=shoot;
                    curr.Begin();
                    nextWillEnd=true;
                    break;
            case 4:
                if (gFSM.agent.wasParried)
                {
                    fsm.SetTPDest(P1CTeleportation.destPoints.FAR);
                    curr.End();
                    curr=tp;
                    curr.Begin();
                    nextWillEnd=true;
                    break;
                }
                fsm.SetTPDest(P1CTeleportation.destPoints.CLOSE);
                curr.End();
                curr=tp;
                curr.Begin();
                nextWillEnd=true;
                break;  
        }
    }
    void BaseFlow()
    {
        Vector3 playerPos=gFSM.agent.transform.position;
        Vector3 agentPos=gFSM.agent.GetPlayer().position;
        float dist=Vector3.Distance(playerPos,agentPos);
        switch (progression)
        {
            case 0:
            {
                curr=dash;
                curr.Begin();
                progression++;
                break;
            }
            case 1:
            {
                if (!gFSM.agent.wasParried &&dist>=gFSM.agent.distLimit )
                {
                    fsm.SetTPDest(P1CTeleportation.destPoints.CLOSE);    
                    curr.End();
                    curr=tp;
                    curr.Begin();
                    progression++;
                    break;
                }
                curr.End();
                curr=slam;
                curr.Begin();
                progression+=3;
                break;
            }
            case 2:
            {
                if (gFSM.agent.wasParried)
                {
                    fsm.SetTPDest(P1CTeleportation.destPoints.FAR);
                    curr.End();
                    curr=tp;
                    curr.Begin();
                    progression++;
                    break;
                }
                curr.End();
                curr=shoot;
                curr.Begin();
                nextWillEnd=true;
                break;
            }
            case 3:
            {
                curr.End();
                curr=shoot;
                curr.Begin();
                nextWillEnd=true;
                break;
            }
            case 4:
            {
                if (gFSM.agent.wasParried)
                {
                    fsm.SetTPDest(P1CTeleportation.destPoints.FAR);
                    curr.End();
                    curr=tp;
                    curr.Begin();
                    nextWillEnd=true;
                    break;
                }
                fsm.SetTPDest(P1CTeleportation.destPoints.CLOSE);
                curr.End();
                curr=tp;
                curr.Begin();
                nextWillEnd=true;
                break;  
            }
        }
    }
}
