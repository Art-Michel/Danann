using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP1Teleportation : GlobalStates
{
    public GP1Teleportation() : base(StateNames.P1GTELEPORTATION){}

    P1CTeleportation tp;
    P1DBoomerang br;
    P1CSlam slam;
    P1CMixDash mdash;
    P1CDash dash;
    P1DShoot shoot;
    bool isFar;
    // Start is called before the first frame update
    public PhaseStats GetPhaseStats(){return stats;}

    public override void Begin()
    {
        fsm=gFSM.GetComponent<Danu_FSM>();
        fsm.AddState(br);
        fsm.AddState(tp);
        fsm.AddState(slam);
        fsm.AddState(mdash);
        fsm.AddState(dash);
        fsm.AddState(shoot);
        tp.orig=slam.orig=dash.orig=mdash.orig=shoot.orig=br.orig=this;
        stats=gFSM.GetPhaseStats();
        Vector3 playerPos=gFSM.agent.transform.position;
        Vector3 agentPos=gFSM.agent.GetPlayer().position;
        float dist=Vector3.Distance(playerPos,agentPos);
        if (dist>gFSM.agent.distLimit)
            gFSM.SetTPDest(P1CTeleportation.destPoints.FAR);    
        else
            gFSM.SetTPDest(P1CTeleportation.destPoints.CLOSE);    

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
        if (isFar)
            In();
        else 
            Out();
    }
    void In()
    {
        switch (progression)
        {
            case 0:
                curr=tp;
                curr.Begin();
                progression++;
                break;
            case 1:
                if (gFSM.agent.wasParried)
                {
                    curr.End();
                    curr=dash;
                    curr.Begin();
                }
                else
                {
                    curr.End();
                    curr=slam;
                    curr.Begin();
                }
                progression++;
                break;
            case 2:
                if (!gFSM.agent.isRevengeHigh)
                {
                    nextWillEnd=true;
                    FlowControl();
                    break;
                }
                if (curr==slam)
                {
                    nextWillEnd=true;
                    curr.End();
                    curr=shoot;
                    curr.Begin();
                    break;
                }
                nextWillEnd=true;
                curr.End();
                curr=br;
                curr.Begin();
                break;
        }
    }
    void Out()
    {
        switch (progression)
        {
            case 0:
                curr=tp;
                curr.Begin();
                progression++;
                break;
            case 1:
                if (gFSM.agent.wasParried)
                {
                    curr.End();
                    curr=slam;
                    curr.Begin();
                }
                else
                {
                    curr.End();
                    curr=br;
                    curr.Begin();
                }
                progression++;
                break;
            case 2:
                if (!gFSM.agent.isRevengeHigh && curr==slam)
                {
                    curr.End();
                    curr=dash;
                    curr.Begin();
                    nextWillEnd=true;
                    FlowControl();
                    break;
                }
                if (curr==slam)
                {
                    nextWillEnd=true;
                    curr.End();
                    curr=mdash;
                    curr.Begin();
                    break;
                }
                if (gFSM.agent.isRevengeHigh)
                {
                    nextWillEnd=true;
                    curr.End();
                    curr=shoot;
                    curr.Begin();
                }
                break;
        }
    }
}
