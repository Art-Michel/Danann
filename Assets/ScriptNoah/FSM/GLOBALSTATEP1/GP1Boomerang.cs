using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP1Boomerang : GlobalStates
{
    public GP1Boomerang() : base(StateNames.P1GBOOMERANG){}

    P1CTeleportation tp;
    P1DBoomerang br;
    P1CSlam slam;
    P1DSpin spin;
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
        fsm.AddState(spin);
        fsm.AddState(shoot);
        tp.orig=slam.orig=spin.orig=shoot.orig=br.orig=this;
        stats=gFSM.GetPhaseStats();

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
        switch (progression)
        {
            case 0:
                curr=br;
                curr.Begin();
                progression++;
                break;
            case 1:
                Vector3 playerPos=gFSM.agent.transform.position;
                Vector3 agentPos=gFSM.agent.GetPlayer().position;
                float dist=Vector3.Distance(playerPos,agentPos);
                if (dist>gFSM.agent.distLimit)
                {
                    curr.End();
                    curr=shoot;
                    curr.Begin();
                    progression+=2;
                    break;
                }
                else
                {
                    curr.End();
                    curr=tp;
                    curr.Begin();
                    progression++;
                    break;
                }
            case 2:
                nextWillEnd=true;
                if (gFSM.agent.wasParried)
                {
                    curr.End();
                    curr=spin;
                    curr.Begin();

                    progression++;
                    break;
                }
                curr.End();
                curr=slam;
                curr.Begin();
                progression++;
                break;
            case 3:
                nextWillEnd=true;
                if (gFSM.agent.isRevengeHigh)
                {
                    curr.End();
                    curr=slam;
                    curr.Begin();
                    progression++;
                    break;
                }
                curr.End();
                curr=spin;
                curr.Begin();
                progression++;
                break;
        }
    }
}